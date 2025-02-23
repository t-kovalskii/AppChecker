using System.Net.Sockets;
using AppAvailabilityTracker.Shared.EventBus.Abstractions;
using AppAvailabilityTracker.Shared.EventBus.Attributes;
using AppAvailabilityTracker.Shared.EventBus.Events;
using AppAvailabilityTracker.Shared.Domain.Configuration;
using AppAvailabilityTracker.Shared.EventBus.Configuration;

using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

using System.Reflection;
using System.Text;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Polly;
using Polly.Retry;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;

namespace AppAvailabilityTracker.Shared.EventBus.Services;

public class RabbitMqEventBus(
    IRabbitMqConnectionFactory connectionFactory,
    IOptions<ServiceConfiguration> serviceConfiguration,
    IOptions<EventBusOptions> eventBusOptions,
    IServiceProvider serviceProvider,
    ILogger<RabbitMqEventBus> logger) : IEventBus,
    IHostedService
{
    private const string ExchangeName = "EventBus";
    
    private readonly ResiliencePipeline _pipeline = CreateRetryPipeline();
    
    private readonly ServiceConfiguration _serviceConfiguration = serviceConfiguration.Value;
    private readonly EventBusOptions _eventBusOptions = eventBusOptions.Value;
    
    private string ServiceName => _serviceConfiguration.Name;

    private IChannel? _channel;
    private IConnection? _connection;
    
    public async Task PublishAsync(IntegrationEvent @event)
    {
        logger.LogInformation("Publishing integration event {EventName}", @event.GetType().Name);

        var routingKey = @event.GetType().GetCustomAttribute<RoutingKeyAttribute>()?.RoutingKey;
        if (routingKey is null)
        {
            logger.LogWarning("Event '{EventName}' has no routing key", @event.GetType().Name);
            return;
        }

        _connection ??= await connectionFactory.EstablishConnectionAsync();
        logger.LogInformation("Established connection to RabbitMQ");

        var channel = await _connection.CreateChannelAsync();
        logger.LogInformation("Created channel on RabbitMQ connection");

        await channel.ExchangeDeclareAsync(exchange: ExchangeName, type: ExchangeType.Direct);
        logger.LogInformation("Declared exchange '{ExchangeName}' on RabbitMQ", ExchangeName);

        var eventSerialized = JsonConvert.SerializeObject(@event);
        var body = Encoding.UTF8.GetBytes(eventSerialized);
        logger.LogInformation("Serialized integration event to JSON: {Event}", eventSerialized);

        await channel.BasicPublishAsync(exchange: ExchangeName,
            routingKey: routingKey,
            body: body,
            mandatory: false);
        logger.LogInformation("Published integration event to RabbitMQ with routing key '{RoutingKey}'", routingKey);
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _ = Task.Run(async () =>
        {
            try
            {
                await _pipeline.ExecuteAsync(async _ =>
                {
                    _connection = await connectionFactory.EstablishConnectionAsync();
                }, cancellationToken);
                
                if (_connection is null || !_connection.IsOpen)
                {
                    logger.LogError("Failed to connect to RabbitMQ");
                    return;
                }

                _channel = await _connection.CreateChannelAsync(cancellationToken: cancellationToken);

                _channel.CallbackExceptionAsync += (sender, ea) =>
                {
                    logger.LogWarning(ea.Exception, "Error occured while calling callback");
                    return Task.CompletedTask;
                };

                await _channel.ExchangeDeclareAsync(exchange: ExchangeName,
                    type: ExchangeType.Direct,
                    cancellationToken: cancellationToken);

                await _channel.QueueDeclareAsync(queue: ServiceName,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    cancellationToken: cancellationToken);

                var consumer = new AsyncEventingBasicConsumer(_channel);
                consumer.ReceivedAsync += OnMessageReceivedAsync;

                await _channel.BasicConsumeAsync(queue: ServiceName,
                    autoAck: false,
                    consumer: consumer,
                    cancellationToken: cancellationToken);

                foreach (var eventTypeName in _eventBusOptions.Subscriptions.Keys)
                {
                    await _channel.QueueBindAsync(queue: ServiceName,
                        exchange: ExchangeName,
                        routingKey: eventTypeName,
                        cancellationToken: cancellationToken);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error establishing RabbitMQ connection on service '{ServiceName}'", ServiceName);
            }
        }, cancellationToken);
        
        return Task.CompletedTask;
    }

    private async Task OnMessageReceivedAsync(object sender, BasicDeliverEventArgs @event)
    {
        var eventName = @event.RoutingKey;
        var body = Encoding.UTF8.GetString(@event.Body.Span);

        try
        {
            await ProcessEvent(eventName, body);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Error processing message on service '{ServiceName}': {body}", ServiceName, body);
        }

        if (_channel != null)
        {
            await _channel.BasicAckAsync(@event.DeliveryTag, false);
        }
    }

    private async Task ProcessEvent(string eventName, string body)
    {
        logger.LogInformation("Processing event {EventName} with body {Body}", eventName, body);

        await using var scope = serviceProvider.CreateAsyncScope();
        if (!_eventBusOptions.Subscriptions.TryGetValue(eventName, out var eventType))
        {
            logger.LogWarning("Cannot obtain event type for event name '{EventName}'", eventName);
            return;
        }

        logger.LogInformation("Deserializing event body to type {EventType}", eventType);

        if (JsonConvert.DeserializeObject(body, eventType) is not IntegrationEvent integrationEvent)
        {
            logger.LogError("Cannot deserialize event body '{Body}' to integration event '{EventName}'", body, eventName);
            return;
        }

        logger.LogInformation("Event body deserialized to {EventType}", integrationEvent.GetType().Name);

        var integrationEventHandlers = scope.ServiceProvider.GetKeyedServices<IIntegrationEventHandler>(eventType)
            .ToList();

        logger.LogInformation("Found {HandlerCount} handlers for event {EventType}",
            integrationEventHandlers.Count,
            eventType.Name);

        foreach (var integrationEventHandler in integrationEventHandlers)
        {
            try
            {
                await integrationEventHandler.Handle(integrationEvent);
                logger.LogInformation("Handled event {EventType} with handler {HandlerType}",
                    integrationEvent.GetType()
                        .Name,
                    integrationEventHandler.GetType()
                        .Name);
            }
            catch (Exception ex)
            {
                logger.LogError(ex,
                    "Error handling event {EventType} with handler {HandlerType}",
                    integrationEvent.GetType()
                        .Name,
                    integrationEventHandler.GetType()
                        .Name);
            }
        }

        logger.LogInformation("Finished processing event {EventName}", eventName);
    }
    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
    
    private static ResiliencePipeline CreateRetryPipeline(int retryCount = 3)
    {
        var retryOptions = new RetryStrategyOptions
        {
            ShouldHandle = new PredicateBuilder().Handle<BrokerUnreachableException>().Handle<SocketException>(),
            MaxRetryAttempts = retryCount,
            DelayGenerator = (context) => ValueTask.FromResult(GenerateDelay(context.AttemptNumber))
        };

        return new ResiliencePipelineBuilder()
            .AddRetry(retryOptions)
            .Build();

        static TimeSpan? GenerateDelay(int attempt)
        {
            return TimeSpan.FromSeconds(Math.Pow(2, attempt));
        }
    }
}

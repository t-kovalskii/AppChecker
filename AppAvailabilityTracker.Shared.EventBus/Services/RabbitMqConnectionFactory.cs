using AppAvailabilityTracker.Shared.EventBus.Configuration;

using Microsoft.Extensions.Options;

using RabbitMQ.Client;

namespace AppAvailabilityTracker.Shared.EventBus.Services;

public class RabbitMqConnectionFactory(
    IOptions<EventBusConfiguration> eventBusConfigurationOptions) : IRabbitMqConnectionFactory
{
    private readonly EventBusConfiguration _eventBusConfiguration = eventBusConfigurationOptions.Value;
    
    public Task<IConnection> EstablishConnectionAsync()
    {
        var connectionFactory = GetConnectionFactory();
        return connectionFactory.CreateConnectionAsync();
    }

    private IConnectionFactory GetConnectionFactory()
    {
        var connectionFactory = new ConnectionFactory
        {
            HostName = _eventBusConfiguration.Host,
            Port = _eventBusConfiguration.Port,
            UserName = _eventBusConfiguration.UserName,
            Password = _eventBusConfiguration.Password
        };

        return connectionFactory;
    }
}

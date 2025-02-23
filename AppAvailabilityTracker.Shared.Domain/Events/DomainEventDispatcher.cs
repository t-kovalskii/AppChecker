using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AppAvailabilityTracker.Shared.Domain.Events;

public class DomainEventDispatcher(
    IServiceProvider serviceProvider,
    ILogger<DomainEventDispatcher> logger) : IDomainEventDispatcher
{
    public async Task DispatchAsync(DomainEvent domainEvent, CancellationToken cancellationToken)
    {
        logger.LogInformation("Dispatching domain event {EventType}", domainEvent.GetType().Name);

        ArgumentNullException.ThrowIfNull(domainEvent);

        var eventType = domainEvent.GetType();
        
        var domainEventHandlers = serviceProvider.GetKeyedServices<IDomainEventHandler>(eventType).ToList();
        logger.LogInformation("Found {HandlerCount} handlers for event {EventType}",
            domainEventHandlers.Count,
            eventType.Name);

        foreach (var handler in domainEventHandlers)
        {
            try
            {
                await handler.Handle(domainEvent, cancellationToken);
                logger.LogInformation("Handled event {EventType} with handler {HandlerType}",
                    eventType.Name,
                    handler.GetType().Name);
            }
            catch (Exception ex)
            {
                logger.LogError(ex,
                    "Error handling event {EventType} with handler {HandlerType}",
                    eventType.Name,
                    handler.GetType()
                        .Name);
            }
        }

        logger.LogInformation("Finished dispatching domain event {EventType}", domainEvent.GetType().Name);
    }
}

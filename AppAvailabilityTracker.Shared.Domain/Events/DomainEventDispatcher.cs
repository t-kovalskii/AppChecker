using Microsoft.Extensions.DependencyInjection;

namespace AppAvailabilityTracker.Shared.Domain.Events;

public class DomainEventDispatcher(IServiceProvider serviceProvider) : IDomainEventDispatcher
{
    public async Task DispatchAsync(DomainEvent domainEvent)
    {
        ArgumentNullException.ThrowIfNull(domainEvent);

        var eventType = domainEvent.GetType();
        var handlerType = typeof(IDomainEventHandler<>).MakeGenericType(eventType);
        
        var handlers = (IEnumerable<object>)serviceProvider.GetServices(handlerType);

        foreach (var handler in handlers)
        {
            await ((dynamic)handler).HandleAsync((dynamic)domainEvent);
        }
    }
}

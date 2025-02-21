using AppAvailabilityTracker.Shared.EventBus.Events;

namespace AppAvailabilityTracker.Shared.EventBus.Abstractions;

public interface IEventBus
{
    Task PublishAsync(IntegrationEvent @event);
}
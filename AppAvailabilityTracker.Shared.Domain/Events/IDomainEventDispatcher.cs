namespace AppAvailabilityTracker.Shared.Domain.Events;

public interface IDomainEventDispatcher
{
    Task DispatchAsync(DomainEvent domainEvent);
}

namespace AppAvailabilityTracker.Shared.Domain.Events;

public interface IDomainEventHandler<in TDomainEvent> 
    where TDomainEvent : DomainEvent
{
    public Task Handle(TDomainEvent domainEvent, CancellationToken cancellationToken);
}

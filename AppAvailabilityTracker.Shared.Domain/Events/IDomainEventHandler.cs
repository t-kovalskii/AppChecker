namespace AppAvailabilityTracker.Shared.Domain.Events;

public interface IDomainEventHandler<in TDomainEvent> : IDomainEventHandler
    where TDomainEvent : DomainEvent
{
    Task Handle(TDomainEvent domainEvent, CancellationToken cancellationToken);
    
    Task IDomainEventHandler.Handle(DomainEvent @event, CancellationToken cancellationToken) =>
        Handle((TDomainEvent)@event, cancellationToken);
}

public interface IDomainEventHandler
{
    Task Handle(DomainEvent @event, CancellationToken cancellationToken);
}

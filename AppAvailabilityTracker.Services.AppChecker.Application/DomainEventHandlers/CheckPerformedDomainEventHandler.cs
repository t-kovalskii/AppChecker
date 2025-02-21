using AppAvailabilityTracker.Services.AppChecker.Application.IntegrationEvents;
using AppAvailabilityTracker.Services.AppChecker.Domain.DomainEvents;

using AppAvailabilityTracker.Shared.EventBus.Abstractions;
using AppAvailabilityTracker.Shared.Domain.Events;

namespace AppAvailabilityTracker.Services.AppChecker.Application.DomainEventHandlers;

public class CheckPerformedDomainEventHandler(IEventBus eventBus) : IDomainEventHandler<CheckPerformed>
{
    public async Task Handle(CheckPerformed domainEvent, CancellationToken cancellationToken)
    {
        var availabilityCheck = domainEvent.AvailabilityCheck;
        var checkPerformedIntegrationEvent = new CheckPerformedIntegrationEvent
        {
            ApplicationId = availabilityCheck.ApplicationId,
            CheckResult = availabilityCheck.CheckResult,
            CheckTime = availabilityCheck.CheckTime
        };

        await eventBus.PublishAsync(checkPerformedIntegrationEvent);
    }
}

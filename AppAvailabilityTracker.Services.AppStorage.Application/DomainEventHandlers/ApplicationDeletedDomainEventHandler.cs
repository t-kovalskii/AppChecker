using AppAvailabilityTracker.Services.AppStorage.Application.IntegrationEvents;
using AppAvailabilityTracker.Services.AppStorage.Domain.DomainEvents;

using AppAvailabilityTracker.Shared.Domain.Events;
using AppAvailabilityTracker.Shared.EventBus.Abstractions;

using Microsoft.Extensions.Logging;

namespace AppAvailabilityTracker.Services.AppStorage.Application.DomainEventHandlers;

public class ApplicationDeletedDomainEventHandler(
    ILogger<ApplicationDeletedDomainEventHandler> logger,
    IEventBus eventBus) : IDomainEventHandler<ApplicationDeletedDomainEvent>
{
    public async Task Handle(ApplicationDeletedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        logger.LogInformation("Application {ApplicationId} deleted domain event", domainEvent.Application.Id);
        
        var application = domainEvent.Application;
        var applicationDeletedIntegrationEvent = new ApplicationDeletedIntegrationEvent
        {
            ApplicationId = application.Id
        };

        await eventBus.PublishAsync(applicationDeletedIntegrationEvent);
        
        logger.LogInformation("Application {ApplicationId} deleted integration event published",
            domainEvent.Application.Id);
    }
}

using AppAvailabilityTracker.Services.AppChecker.Application.IntegrationEvents;
using AppAvailabilityTracker.Services.AppChecker.Application.Interfaces;
using AppAvailabilityTracker.Shared.EventBus.Events;

using Microsoft.Extensions.Logging;

namespace AppAvailabilityTracker.Services.AppChecker.Application.IntegrationEventHandlers;

public class ApplicationDeletedIntegrationEventHandler(
    ILogger<ApplicationDeletedIntegrationEventHandler> logger,
    IRecurringJobScheduler jobScheduler) : IIntegrationEventHandler<ApplicationDeletedIntegrationEvent>
{
    public Task Handle(ApplicationDeletedIntegrationEvent applicationDeletedEvent)
    {
        logger.LogInformation("Application {ApplicationId} deleted integration event",
            applicationDeletedEvent.ApplicationId);
        
        jobScheduler.RemoveRecurringJob(applicationDeletedEvent.ApplicationId);
        
        logger.LogInformation("Application {ApplicationId} deleted recurring job removed",
            applicationDeletedEvent.ApplicationId);
        
        return Task.CompletedTask;
    }
}

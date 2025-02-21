using AppAvailabilityTracker.Services.AppStorage.Application.IntegrationEvents;
using AppAvailabilityTracker.Services.AppStorage.Domain.Repositories;
using AppAvailabilityTracker.Shared.EventBus.Events;

namespace AppAvailabilityTracker.Services.AppStorage.Application.IntegrationEventHandlers;

public class CheckPerformedIntegrationEventHandler(
    IApplicationRepository applicationRepository) : IIntegrationEventHandler<CheckPerformedIntegrationEvent>
{
    public async Task Handle(CheckPerformedIntegrationEvent @event)
    {
        var application = await applicationRepository.GetByIdAsync(@event.ApplicationId);
        if (application is null)
        {
            throw new Exception($"Application '{@event.ApplicationId}' not found");
        }
        
        application.UpdateAvailability(@event.CheckResult, @event.CheckTime);
        
        await applicationRepository.UpdateAsync(application);
    }
}
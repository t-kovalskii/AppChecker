using AppAvailabilityTracker.Services.AppStorage.Application.Configuration;
using AppAvailabilityTracker.Services.AppStorage.Application.Helpers;
using AppAvailabilityTracker.Services.AppStorage.Domain.DomainEvents;

using AppAvailabilityTracker.Shared.Domain.Events;

using Microsoft.Extensions.Options;

namespace AppAvailabilityTracker.Services.AppStorage.Application.DomainEventHandlers;

public class ApplicationAddedDomainEventHandler(
    ExternalStoreServiceResolver externalStoreServiceResolver,
    IOptions<ApplicationConfiguration> applicationConfiguration) : IDomainEventHandler<ApplicationAddedDomainEvent>
{
    private readonly ApplicationConfiguration _applicationConfiguration = applicationConfiguration.Value;
    
    public async Task Handle(ApplicationAddedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var application = domainEvent.ApplicationDomain;
        var externalStoreService = externalStoreServiceResolver.Resolve(application.Store);

        await externalStoreService.PerformInitialCheckAndScheduleAsync(application.Id, application.StoreLink,
            _applicationConfiguration.RefreshIntervalMinutes);
    }
}

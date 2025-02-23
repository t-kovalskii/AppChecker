using AppAvailabilityTracker.Shared.DataAccess.Interfaces;
using Microsoft.Extensions.Logging;

namespace AppAvailabilityTracker.Services.AppStorage.Infrastructure.Context;

public partial class ApplicationStoreContext : IUnitOfWork
{
    public async Task SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Saving entities");
        
        await base.SaveChangesAsync(cancellationToken);
        await _domainEventDispatcherService.DispatchDomainEventsAsync(cancellationToken);
        
        _logger.LogInformation("Entities saved");
    }
}
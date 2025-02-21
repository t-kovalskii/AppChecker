using AppAvailabilityTracker.Shared.DataAccess.Interfaces;

using Microsoft.Extensions.Logging;

namespace AppAvailabilityTracker.Services.AppChecker.Infrastructure.Context;

public partial class AvailabilityCheckContext : IUnitOfWork
{
    public async Task SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Dispatching domain events");

        await base.SaveChangesAsync(cancellationToken);
        await _domainEventDispatcherService.DispatchDomainEventsAsync(cancellationToken);
        
        _logger.LogInformation("Dispatched domain events");
    }
}

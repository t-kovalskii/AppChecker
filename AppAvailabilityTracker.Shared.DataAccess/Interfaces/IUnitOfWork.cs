namespace AppAvailabilityTracker.Shared.DataAccess.Interfaces;

public interface IUnitOfWork
{
    Task SaveEntitiesAsync(CancellationToken cancellationToken = default);
}
namespace AppAvailabilityTracker.Services.AppStorage.Application.Interfaces;

public interface IExternalStoreService
{
    Task<bool> PerformInitialCheckAndScheduleAsync(Guid applicationId, Uri storeLink, int intervalMinutes);
}
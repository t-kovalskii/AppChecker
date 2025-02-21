namespace AppAvailabilityTracker.Services.AppChecker.Application.Interfaces;

public interface IAppStatusChecker
{
    Task<bool> CheckAvailabilityAsync(Guid applicationId, Uri appLink);
}

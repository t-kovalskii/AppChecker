using AppAvailabilityTracker.Services.AppChecker.Application.Request;

namespace AppAvailabilityTracker.Services.AppChecker.Application.Interfaces;

public interface IAppCheckerService
{
    Task<bool> ScheduleAppAvailabilityCheckAsync(ScheduleAppAvailabilityCheckRequest request);
    
    Task RemoveAppAvailabilityCheckAsync(RemoveAppAvailabilityCheckRequest request);
}

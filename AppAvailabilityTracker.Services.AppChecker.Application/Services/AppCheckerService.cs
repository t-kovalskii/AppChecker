using AppAvailabilityTracker.Services.AppChecker.Application.Interfaces;
using AppAvailabilityTracker.Services.AppChecker.Application.Request;

namespace AppAvailabilityTracker.Services.AppChecker.Application.Services;

public class AppCheckerService(
    IRecurringJobScheduler jobScheduler,
    IAppStatusChecker appStatusChecker) : IAppCheckerService
{
    public async Task<bool> ScheduleAppAvailabilityCheckAsync(ScheduleAppAvailabilityCheckRequest request)
    {
        var jobId = request.ApplicationId;
        var isApplicationAvailable = await appStatusChecker.CheckAvailabilityAsync(request.ApplicationId,
            request.StoreLink);
        
        jobScheduler.ScheduleRecurringAvailabilityCheckJob(jobId, request.StoreLink, request.Interval);

        return isApplicationAvailable;
    }

    public Task RemoveAppAvailabilityCheckAsync(RemoveAppAvailabilityCheckRequest request)
    {
        jobScheduler.RemoveRecurringJob(request.ApplicationId);
        return Task.CompletedTask;
    }
}

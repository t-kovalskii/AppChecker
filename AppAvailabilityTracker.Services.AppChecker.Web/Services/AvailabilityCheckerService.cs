using AppAvailabilityTracker.Services.AppChecker.Application.Interfaces;
using AppAvailabilityTracker.Services.AppChecker.Application.Request;
using AppAvailabilityTracker.Services.AppChecker.Web.Grpc;

using Grpc.Core;

namespace AppAvailabilityTracker.Services.AppChecker.Web.Services;

public class AvailabilityCheckerService(
    IAppCheckerService appCheckerService) : AppCheckerService.AppCheckerServiceBase
{
    public override async Task<CheckAndScheduleResponse> PerformInitialCheckAndSchedule(CheckAndScheduleRequest request,
        ServerCallContext context)
    {
        var applicationId = new Guid(request.ApplicationId);
        var storeLink = new Uri(request.StoreLink);
        var interval = TimeSpan.FromMinutes(request.IntervalMinutes);
        
        var scheduleAppAvailabilityCheckRequest = new ScheduleAppAvailabilityCheckRequest
        {
            ApplicationId = applicationId,
            Interval = interval,
            StoreLink = storeLink
        };

        var isAppAvailable =
            await appCheckerService.ScheduleAppAvailabilityCheckAsync(scheduleAppAvailabilityCheckRequest);
        
        return new CheckAndScheduleResponse
        {
            IsAvailable = isAppAvailable
        };
    }
}
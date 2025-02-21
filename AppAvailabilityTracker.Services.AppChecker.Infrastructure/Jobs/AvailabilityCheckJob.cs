using AppAvailabilityTracker.Services.AppChecker.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace AppAvailabilityTracker.Services.AppChecker.Infrastructure.Jobs;

public class AvailabilityCheckJob(IAppStatusChecker appStatusChecker, ILogger<AvailabilityCheckJob> logger)
{
    public async Task<bool> ExecuteAsync(Guid applicationId, Uri appLink)
    {
        logger.LogInformation("Executing recurring availability check for {AppLink} at {Time}",
            appLink,
            DateTime.UtcNow);
        
        var isAvailable = await appStatusChecker.CheckAvailabilityAsync(applicationId, appLink);
        
        logger.LogInformation("Availability check for {AppLink} completed: IsAvailable = {Availability}",
            appLink,
            isAvailable);
        
        return isAvailable;
    }
}
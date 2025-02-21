namespace AppAvailabilityTracker.Services.AppChecker.Application.Request;

public class ScheduleAppAvailabilityCheckRequest
{
    public required Guid ApplicationId { get; set; }
    
    public required TimeSpan Interval { get; set; }
    
    public required Uri StoreLink { get; set; }
}

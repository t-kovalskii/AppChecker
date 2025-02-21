namespace AppAvailabilityTracker.Services.AppChecker.Application.Request;

public class RemoveAppAvailabilityCheckRequest
{
    public required Guid ApplicationId { get; set; }
}

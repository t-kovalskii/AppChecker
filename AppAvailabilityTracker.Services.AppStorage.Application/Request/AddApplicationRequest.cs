namespace AppAvailabilityTracker.Services.AppStorage.Application.Request;

public class AddApplicationRequest
{
    public required string Name { get; set; }
    
    public required Uri Link { get; set; }
}

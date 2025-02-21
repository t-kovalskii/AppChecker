using AppAvailabilityTracker.Shared.EventBus.Attributes;
using AppAvailabilityTracker.Shared.EventBus.Events;

namespace AppAvailabilityTracker.Services.AppStorage.Application.IntegrationEvents;

[RoutingKey("check.performed")]
public class CheckPerformedIntegrationEvent : IntegrationEvent
{
    public Guid ApplicationId { get; set; }
    
    public bool CheckResult { get; set; }
    
    public DateTime CheckTime { get; set; }
}

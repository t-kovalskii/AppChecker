using AppAvailabilityTracker.Shared.EventBus.Attributes;
using AppAvailabilityTracker.Shared.EventBus.Events;

namespace AppAvailabilityTracker.Services.AppStorage.Application.IntegrationEvents;

[RoutingKey("application.deleted")]
public class ApplicationDeletedIntegrationEvent : IntegrationEvent
{
    public Guid ApplicationId { get; set; }
}

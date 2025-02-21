namespace AppAvailabilityTracker.Shared.EventBus.Attributes;

public class RoutingKeyAttribute(string routingKey) : Attribute
{
    public string RoutingKey { get; } = routingKey;
}

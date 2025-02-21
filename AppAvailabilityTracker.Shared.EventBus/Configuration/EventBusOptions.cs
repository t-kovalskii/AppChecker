namespace AppAvailabilityTracker.Shared.EventBus.Configuration;

public class EventBusOptions
{
    public Dictionary<string, Type> Subscriptions { get; } = new();
}

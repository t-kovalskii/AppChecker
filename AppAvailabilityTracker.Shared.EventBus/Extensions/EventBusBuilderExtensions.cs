using AppAvailabilityTracker.Shared.EventBus.Abstractions;
using AppAvailabilityTracker.Shared.EventBus.Attributes;
using AppAvailabilityTracker.Shared.EventBus.Configuration;
using AppAvailabilityTracker.Shared.EventBus.Events;

using Microsoft.Extensions.DependencyInjection;

using System.Reflection;

namespace AppAvailabilityTracker.Shared.EventBus.Extensions;

public static class EventBusBuilderExtensions
{
    public static IEventBusBuilder
        AddSubscription<TIntegrationEvent, TIntegrationEventHandler>(this IEventBusBuilder builder)
        where TIntegrationEvent : IntegrationEvent
        where TIntegrationEventHandler : class, IIntegrationEventHandler<TIntegrationEvent>
    {
        builder.Services.AddKeyedTransient<IIntegrationEventHandler, TIntegrationEventHandler>(typeof(TIntegrationEvent));
        builder.Services.Configure<EventBusOptions>(c =>
        {
            var eventType = typeof(TIntegrationEvent);
            var eventKey = eventType.GetCustomAttribute<RoutingKeyAttribute>()?.RoutingKey;
            
            c.Subscriptions.TryAdd(eventKey ?? eventType.Name, eventType);
        });
        
        return builder;
    }
}

using Microsoft.Extensions.DependencyInjection;

namespace AppAvailabilityTracker.Shared.Domain.Events;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddDomainEventsDispatching(this IServiceCollection services)
    {
        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
        services.AddScoped<DomainEventDispatcherService>();

        return services;
    }

    public static IServiceCollection
        AddDomainEventHandler<TDomainEvent, TDomainEventHandler>(this IServiceCollection services)
        where TDomainEvent : DomainEvent
        where TDomainEventHandler : class, IDomainEventHandler<TDomainEvent>
    {
        services.AddTransient<IDomainEventHandler<TDomainEvent>, TDomainEventHandler>();
        return services;
    }
}
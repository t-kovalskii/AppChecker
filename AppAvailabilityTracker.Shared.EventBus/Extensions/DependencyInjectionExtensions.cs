using AppAvailabilityTracker.Shared.EventBus.Abstractions;
using AppAvailabilityTracker.Shared.EventBus.Configuration;
using AppAvailabilityTracker.Shared.EventBus.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AppAvailabilityTracker.Shared.EventBus.Extensions;

public static class DependencyInjectionExtensions
{
    public static IEventBusBuilder AddEventBus(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IRabbitMqConnectionFactory, RabbitMqConnectionFactory>();
        
        services.Configure<EventBusConfiguration>(configuration.GetSection(nameof(EventBusConfiguration)));
        
        services.AddSingleton<IEventBus, RabbitMqEventBus>();
        services.AddHostedService(provider => provider.GetRequiredService<RabbitMqEventBus>());

        return new EventBusBuilder(services);
    }

    private class EventBusBuilder(IServiceCollection serviceCollection) : IEventBusBuilder
    {
        public IServiceCollection Services { get; } = serviceCollection;
    }
}

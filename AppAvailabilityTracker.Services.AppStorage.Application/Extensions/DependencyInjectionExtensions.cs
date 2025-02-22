using AppAvailabilityTracker.Services.AppStorage.Application.Configuration;
using AppAvailabilityTracker.Services.AppStorage.Application.DomainEventHandlers;
using AppAvailabilityTracker.Services.AppStorage.Application.Helpers;
using AppAvailabilityTracker.Services.AppStorage.Application.Interfaces;
using AppAvailabilityTracker.Services.AppStorage.Application.Services;
using AppAvailabilityTracker.Services.AppStorage.Domain.DomainEvents;

using AppAvailabilityTracker.Shared.Domain.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AppAvailabilityTracker.Services.AppStorage.Application.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDomainEventsDispatching();
        services.AddDomainEventHandler<ApplicationAddedDomainEvent, ApplicationAddedDomainEventHandler>();
        services.AddDomainEventHandler<ApplicationDeletedDomainEvent, ApplicationDeletedDomainEventHandler>();
        
        services.AddScoped<ExternalStoreServiceResolver>();

        services.Configure<ApplicationConfiguration>(configuration.GetSection(nameof(ApplicationConfiguration)));

        services.AddScoped<IAppStorageService, AppStorageService>();
        
        return services;
    }
}

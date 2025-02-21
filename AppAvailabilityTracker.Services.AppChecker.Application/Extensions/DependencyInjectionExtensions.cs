using AppAvailabilityTracker.Services.AppChecker.Application.DomainEventHandlers;
using AppAvailabilityTracker.Services.AppChecker.Application.Interfaces;
using AppAvailabilityTracker.Services.AppChecker.Application.Services;
using AppAvailabilityTracker.Services.AppChecker.Domain.DomainEvents;
using AppAvailabilityTracker.Shared.Domain.Events;
using Microsoft.Extensions.DependencyInjection;

namespace AppAvailabilityTracker.Services.AppChecker.Application.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddDomainEventsDispatching().AddDomainEventHandler<CheckPerformed, CheckPerformedDomainEventHandler>();
        
        services.AddScoped<IAppCheckerService, AppCheckerService>();
        
        return services;
    }
}

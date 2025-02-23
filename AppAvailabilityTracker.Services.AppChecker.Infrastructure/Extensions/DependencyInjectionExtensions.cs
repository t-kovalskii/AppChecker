using AppAvailabilityTracker.Services.AppChecker.Application.Interfaces;
using AppAvailabilityTracker.Services.AppChecker.Domain.Repositories;
using AppAvailabilityTracker.Services.AppChecker.Infrastructure.Context;
using AppAvailabilityTracker.Services.AppChecker.Infrastructure.Jobs;
using AppAvailabilityTracker.Services.AppChecker.Infrastructure.Mapping;
using AppAvailabilityTracker.Services.AppChecker.Infrastructure.Repositories;
using AppAvailabilityTracker.Services.AppChecker.Infrastructure.Services;

using AppAvailabilityTracker.Shared.DataAccess;
using AppAvailabilityTracker.Shared.Domain.Mapping;

using Hangfire;
using Hangfire.MemoryStorage;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Microsoft.EntityFrameworkCore;

namespace AppAvailabilityTracker.Services.AppChecker.Infrastructure.Extensions;

public static class DependencyInjectionExtensions
{
    private const string ConnectionStringName = "AppCheckerConnection";
    
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AvailabilityCheckContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString(ConnectionStringName));
        });
        
        services.AddHostedService(sp => new MigrationHostedService<AvailabilityCheckContext>(sp));

        services.AddMapper<AvailabilityCheckModelMapperFactory>();

        services.AddHangfire(config => config.UseMemoryStorage());
        services.AddHangfireServer();
        
        services.AddScoped<AvailabilityCheckJob>();
        
        services.AddScoped<IRecurringJobScheduler, HangfireRecurringJobScheduler>();
        services.AddHttpClient<IAppStatusChecker, HttpAppStatusChecker>();

        services.AddScoped<IAvailabilityChecksRepository, AvailabilityCheckRepository>();
        
        return services;
    }
}

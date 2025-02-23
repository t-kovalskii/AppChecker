using AppAvailabilityTracker.Services.AppChecker.Web.Grpc;

using AppAvailabilityTracker.Services.AppStorage.Application.Interfaces;
using AppAvailabilityTracker.Services.AppStorage.Domain.Enums;
using AppAvailabilityTracker.Services.AppStorage.Domain.Repositories;
using AppAvailabilityTracker.Services.AppStorage.Infrastructure.Configuration;
using AppAvailabilityTracker.Services.AppStorage.Infrastructure.Context;
using AppAvailabilityTracker.Services.AppStorage.Infrastructure.Mapping;
using AppAvailabilityTracker.Services.AppStorage.Infrastructure.Repository;
using AppAvailabilityTracker.Services.AppStorage.Infrastructure.Services;

using AppAvailabilityTracker.Shared.DataAccess;
using AppAvailabilityTracker.Shared.Domain.Mapping;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Microsoft.EntityFrameworkCore;

namespace AppAvailabilityTracker.Services.AppStorage.Infrastructure.Extensions;

public static class DependencyInjectionExtensions
{
    private const string ConnectionStringName = "AppStorageConnection";
    private const string UrlsConfigurationPrefix = "urls";
    
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<UrlConfiguration>(configuration.GetSection(UrlsConfigurationPrefix));
        
        services.AddDbContext<ApplicationStoreContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString(ConnectionStringName));
        });
        
        services.AddHostedService(sp => new MigrationHostedService<ApplicationStoreContext>(sp));

        services.AddGrpcClient<AppCheckerService.AppCheckerServiceClient>(StoreType.GooglePlayMarket.ToString(),
            options =>
            {
                var urlsConfiguration = configuration.GetSection(UrlsConfigurationPrefix).Get<UrlConfiguration>()!;
                options.Address = new Uri(urlsConfiguration.GooglePlayAppCheckerServiceUrl);
            });
        
        services.AddGrpcClient<AppCheckerService.AppCheckerServiceClient>(StoreType.AppStore.ToString(),
            options =>
            {
                var urlsConfiguration = configuration.GetSection(UrlsConfigurationPrefix).Get<UrlConfiguration>()!;
                options.Address = new Uri(urlsConfiguration.AppStoreAppCheckerServiceUrl);
            });

        services.AddScoped<IExternalGooglePlayService, ExternalGooglePlayService>();
        services.AddScoped<IExternalAppStoreService, ExternalAppStoreService>();

        services.AddMapper<ApplicationModelMapperFactory>();

        services.AddScoped<IApplicationRepository, ApplicationRepository>();

        return services;
    }
}

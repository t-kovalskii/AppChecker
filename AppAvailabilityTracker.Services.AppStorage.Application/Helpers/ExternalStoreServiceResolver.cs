using AppAvailabilityTracker.Services.AppStorage.Application.Interfaces;
using AppAvailabilityTracker.Services.AppStorage.Domain.Enums;

using Microsoft.Extensions.DependencyInjection;

namespace AppAvailabilityTracker.Services.AppStorage.Application.Helpers;

public class ExternalStoreServiceResolver(IServiceProvider serviceProvider)
{
    public IExternalStoreService Resolve(StoreType storeType)
    {
        return storeType switch
        {
            StoreType.GooglePlayMarket => serviceProvider.GetRequiredService<IExternalGooglePlayService>(),
            StoreType.AppStore => serviceProvider.GetRequiredService<IExternalAppStoreService>(),
            
            _ => throw new ArgumentOutOfRangeException(nameof(storeType))
        };
    }
}

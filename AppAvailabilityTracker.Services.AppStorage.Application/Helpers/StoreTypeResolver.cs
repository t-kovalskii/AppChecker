using AppAvailabilityTracker.Services.AppStorage.Application.Constants;
using AppAvailabilityTracker.Services.AppStorage.Domain.Enums;

namespace AppAvailabilityTracker.Services.AppStorage.Application.Helpers;

public static class StoreTypeResolver
{
    public static StoreType Resolve(Uri link)
    {
        return link.Host switch
        {
            StoreHosts.GooglePlayMarket => StoreType.GooglePlayMarket,
            StoreHosts.AppStore => StoreType.AppStore,
            
            _ => throw new ArgumentOutOfRangeException($"Unknown store host: {link.Host}")
        };
    }
}
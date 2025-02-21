using AppAvailabilityTracker.Services.AppStorage.Application.Interfaces;
using AppAvailabilityTracker.Services.AppStorage.Domain.Enums;

using AppAvailabilityTracker.Services.AppChecker.Web.Grpc;

using Grpc.Net.ClientFactory;

namespace AppAvailabilityTracker.Services.AppStorage.Infrastructure.Services;

public class ExternalAppStoreService(GrpcClientFactory grpcClientFactory) : IExternalAppStoreService
{
    private readonly AppCheckerService.AppCheckerServiceClient _rpcClient =
        grpcClientFactory.CreateClient<AppCheckerService.AppCheckerServiceClient>(StoreType.AppStore.ToString());
    
    public async Task<bool> PerformInitialCheckAndScheduleAsync(Guid applicationId, Uri storeLink, int intervalMinutes)
    {
        var request = new CheckAndScheduleRequest
        {
            ApplicationId = applicationId.ToString(),
            StoreLink = storeLink.ToString(),
            IntervalMinutes = intervalMinutes
        };

        var response = await _rpcClient.PerformInitialCheckAndScheduleAsync(request);

        return response.IsAvailable;
    }
}

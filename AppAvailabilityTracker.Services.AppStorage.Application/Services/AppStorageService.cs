using AppAvailabilityTracker.Services.AppStorage.Application.Helpers;
using AppAvailabilityTracker.Services.AppStorage.Application.Interfaces;
using AppAvailabilityTracker.Services.AppStorage.Application.Request;
using AppAvailabilityTracker.Services.AppStorage.Domain.Repositories;

namespace AppAvailabilityTracker.Services.AppStorage.Application.Services;

public class AppStorageService(
    IApplicationRepository applicationRepository,
    ExternalStoreServiceResolver externalStoreServiceResolver) : IAppStorageService
{
    public async Task AddApplication(AddApplicationRequest request)
    {
        var storeType = StoreTypeResolver.Resolve(request.Link);
        var newApplication = new Domain.Models.ApplicationDomain(request.Name, request.Link, storeType);
        
        await applicationRepository.AddAsync(newApplication);
        await applicationRepository.UnitOfWork.SaveEntitiesAsync();
    }

    public async Task RemoveApplication(RemoveApplicationRequest request)
    {
        var application = await applicationRepository.GetByIdAsync(request.ApplicationId);
        if (application is null)
        {
            throw new Exception($"Application '{request.ApplicationId}' not found");
        }
        
        application.Delete();
        
        await applicationRepository.DeleteAsync(request.ApplicationId);
        await applicationRepository.UnitOfWork.SaveEntitiesAsync();
    }

    public async Task<IEnumerable<Domain.Models.ApplicationDomain>> GetAllTrackedApplications()
    {
        return await applicationRepository.GetAllAsync();
    }
}
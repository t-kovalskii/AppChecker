using AppAvailabilityTracker.Services.AppStorage.Application.Request;

namespace AppAvailabilityTracker.Services.AppStorage.Application.Interfaces;

public interface IAppStorageService
{
    Task AddApplication(AddApplicationRequest request);
    
    Task RemoveApplication(RemoveApplicationRequest request);
    
    Task<IEnumerable<Domain.Models.ApplicationDomain>> GetAllTrackedApplications();
}

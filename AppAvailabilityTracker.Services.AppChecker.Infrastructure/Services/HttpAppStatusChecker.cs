using AppAvailabilityTracker.Services.AppChecker.Application.Interfaces;
using AppAvailabilityTracker.Services.AppChecker.Domain.Models;
using AppAvailabilityTracker.Services.AppChecker.Domain.Repositories;

namespace AppAvailabilityTracker.Services.AppChecker.Infrastructure.Services;

public class HttpAppStatusChecker(
    HttpClient httpClient,
    IAvailabilityChecksRepository availabilityChecksRepository) : IAppStatusChecker
{
    public async Task<bool> CheckAvailabilityAsync(Guid applicationId, Uri appLink)
    {
        var response = await httpClient.GetAsync(appLink);
        
        var availabilityCheck = new AvailabilityCheck(applicationId, appLink, response.IsSuccessStatusCode);
        
        await availabilityChecksRepository.AddAsync(availabilityCheck);
        await availabilityChecksRepository.UnitOfWork.SaveEntitiesAsync();

        return availabilityCheck.CheckResult;
    }
}

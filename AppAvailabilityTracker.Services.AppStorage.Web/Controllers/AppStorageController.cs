using AppAvailabilityTracker.Services.AppStorage.Application.Interfaces;
using AppAvailabilityTracker.Services.AppStorage.Application.Request;
using AppAvailabilityTracker.Services.AppStorage.Web.Dto;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace AppAvailabilityTracker.Services.AppStorage.Web.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class AppStorageController(IAppStorageService appStorageService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<GetAllApplicationsResponse>> GetAllTrackedApplication()
    {
        var trackedApplications = await appStorageService.GetAllTrackedApplications();
        var trackedApplicationsDto = trackedApplications.Select(a => new ApplicationResponseDto
        {
            Id = a.Id,
            Name = a.Name,
            StoreLink = a.StoreLink,
            Store = a.Store,
            IsAvailable = a.IsAvailable,
            UpdatedAt = a.UpdatedAt
        }).ToList();
        
        return Ok(new GetAllApplicationsResponse
        {
            Count = trackedApplicationsDto.Count,
            Applications = trackedApplicationsDto
        });
    }
    
    [HttpPost]
    public async Task<ActionResult<Ok>> AddApplication(AddApplicationWebRequest webRequest)
    {
        var addApplicationRequest = new AddApplicationRequest
        {
            Name = webRequest.Name,
            Link = webRequest.Link
        };

        await appStorageService.AddApplication(addApplicationRequest);
        
        return Ok(new { message = "Successfully added" });
    }
    
    [HttpDelete]
    public async Task<ActionResult<Ok>> RemoveApplication(RemoveApplicationWebRequest webRequest)
    {
        var removeApplicationRequest = new RemoveApplicationRequest
        {
            ApplicationId = webRequest.Id
        };

        await appStorageService.RemoveApplication(removeApplicationRequest);
        
        return Ok(new { message = "Successfully removed" });
    }
}

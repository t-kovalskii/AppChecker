using AppAvailabilityTracker.Services.AppStorage.Domain.Repositories;
using AppAvailabilityTracker.Services.AppStorage.Domain.Models;
using AppAvailabilityTracker.Services.AppStorage.Infrastructure.Context;
using AppAvailabilityTracker.Shared.DataAccess.Interfaces;
using AppAvailabilityTracker.Shared.Domain.Mapping;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AppAvailabilityTracker.Services.AppStorage.Infrastructure.Repository;

public class ApplicationRepository(
    ApplicationStoreContext applicationStoreContext,
    IModelMapper modelMapper, ILogger<ApplicationRepository> logger) : IApplicationRepository
{
    public IUnitOfWork UnitOfWork { get; } = applicationStoreContext;
    
    public async Task<IEnumerable<ApplicationDomain>> GetAllAsync()
    {
        var applications = await applicationStoreContext.Applications.AsNoTracking().ToListAsync();
        return modelMapper.Map<ApplicationDomain>(applications);
    }

    public async Task<ApplicationDomain?> GetByIdAsync(Guid id)
    {
        var application = await applicationStoreContext.Applications.AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == id);
        return application is not null ? modelMapper.Map<ApplicationDomain>(application) : null;
    }

    public Task<Guid> AddAsync(ApplicationDomain entity)
    {
        var applicationEntity = modelMapper.Map<Models.Application>(entity);
        applicationStoreContext.Applications.Add(applicationEntity);
        
        return Task.FromResult(applicationEntity.Id);
    }

    public Task<ApplicationDomain> UpdateAsync(ApplicationDomain entity)
    {
        var applicationEntity = modelMapper.Map<Models.Application>(entity);
        
        logger.LogInformation("Updating application {ApplicationId}", applicationEntity.Id);
        logger.LogInformation("Result domain: {ResultDomain}; Entity: {ResultEntity}", entity.IsAvailable, applicationEntity.IsAvailable);
        
        applicationStoreContext.Applications.Update(applicationEntity);
        
        logger.LogInformation("Updated application {ApplicationId}", applicationEntity.Id);
        
        return Task.FromResult(entity);
    }

    public Task<bool> DeleteAsync(Guid id)
    {
        var applicationEntity = new Models.Application { Id = id };
        applicationStoreContext.Applications.Remove(applicationEntity);
        
        return Task.FromResult(true);
    }

    public Task<bool> ExistsAsync(Guid id)
    {
        return applicationStoreContext.Applications.AnyAsync(a => a.Id == id);
    }
}
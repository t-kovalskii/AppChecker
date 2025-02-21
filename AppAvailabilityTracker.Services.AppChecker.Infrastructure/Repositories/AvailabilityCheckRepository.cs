using AppAvailabilityTracker.Services.AppChecker.Domain.Models;
using AppAvailabilityTracker.Services.AppChecker.Domain.Repositories;
using AppAvailabilityTracker.Services.AppChecker.Infrastructure.Context;

using AppAvailabilityTracker.Shared.DataAccess.Interfaces;
using AppAvailabilityTracker.Shared.Domain.Mapping;
using Microsoft.EntityFrameworkCore;

namespace AppAvailabilityTracker.Services.AppChecker.Infrastructure.Repositories;

public class AvailabilityCheckRepository(
    AvailabilityCheckContext availabilityCheckContext,
    IModelMapper modelMapper) : IAvailabilityChecksRepository
{
    public IUnitOfWork UnitOfWork { get; } = availabilityCheckContext;
    
    public async Task<IEnumerable<AvailabilityCheck>> GetAllAsync()
    {
        var entities = await availabilityCheckContext.AvailabilityChecks
            .FromSqlRaw("SELECT * FROM availability_checks")
            .ToListAsync();
        
        return modelMapper.Map<AvailabilityCheck>(entities);
    }

    public async Task<AvailabilityCheck?> GetByIdAsync(Guid id)
    {
        var entity = await availabilityCheckContext.AvailabilityChecks
            .FromSqlRaw("SELECT * FROM availability_checks WHERE id = {0}", id)
            .FirstOrDefaultAsync();
        
        return entity is null ? null : modelMapper.Map<AvailabilityCheck>(entity);
    }

    public async Task<Guid> AddAsync(AvailabilityCheck domainEntity)
    {
        const string sql = @"
            INSERT INTO availability_checks 
                (id, application_id, app_link, check_time, check_result)
            VALUES 
                ({0}, {1}, {2}, {3}, {4})";

        await availabilityCheckContext.Database.ExecuteSqlRawAsync(sql,
            domainEntity.Id,
            domainEntity.ApplicationId,
            domainEntity.AppLink,
            domainEntity.CheckTime,
            domainEntity.CheckResult);

        return domainEntity.Id;
    }

    // Updates an existing record using a raw SQL UPDATE and returns the updated domain model.
    public async Task<AvailabilityCheck> UpdateAsync(AvailabilityCheck domainEntity)
    {
        const string sql = @"
            UPDATE availability_checks 
            SET 
                application_id = {1}, 
                app_link = {2}, 
                check_time = {3}, 
                check_result = {4}
            WHERE id = {0}";

        await availabilityCheckContext.Database.ExecuteSqlRawAsync(sql,
            domainEntity.Id,
            domainEntity.ApplicationId,
            domainEntity.AppLink,
            domainEntity.CheckTime,
            domainEntity.CheckResult);

        return domainEntity;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        const string sql = "DELETE FROM availability_checks WHERE id = {0}";
        var rowsAffected = await availabilityCheckContext.Database.ExecuteSqlRawAsync(sql, id);
        
        return rowsAffected > 0;
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        var exists = await availabilityCheckContext.AvailabilityChecks
            .FromSqlRaw("SELECT * FROM availability_checks WHERE id = {0}", id)
            .AnyAsync();
        
        return exists;
    }
}

using AppAvailabilityTracker.Shared.Domain.Models;

namespace AppAvailabilityTracker.Shared.DataAccess.Interfaces;

public interface IRepository<T> where T : DomainModel
{
    IUnitOfWork UnitOfWork { get; }

    Task<IEnumerable<T>> GetAllAsync();
    
    Task<T?> GetByIdAsync(Guid id);
    
    Task<Guid> AddAsync(T entity);
    
    Task<T> UpdateAsync(T entity);
    
    Task<bool> DeleteAsync(Guid id);
    
    Task<bool> ExistsAsync(Guid id);
}

using AppAvailabilityTracker.Services.AppStorage.Domain.DomainEvents;
using AppAvailabilityTracker.Services.AppStorage.Domain.Enums;
using AppAvailabilityTracker.Shared.Domain.Models;

namespace AppAvailabilityTracker.Services.AppStorage.Domain.Models;

public class ApplicationDomain : DomainModel
{
    public ApplicationDomain(string name, Uri storeLink, StoreType storeType)
    {
        Id = Guid.NewGuid();
        Name = name;
        StoreLink = storeLink;
        Store = storeType;
        IsAvailable = false;
        UpdatedAt = DateTime.UtcNow;
        
        AddDomainEvent(new ApplicationAddedDomainEvent(this));
    }
    
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public Uri StoreLink { get; private set; }
    public StoreType Store { get; private set; }
    public bool IsAvailable { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public void Delete()
    {
        AddDomainEvent(new ApplicationDeletedDomainEvent(this));
    }
    
    public void UpdateAvailability(bool isAvailable, DateTime checkTime)
    {
        IsAvailable = isAvailable;
        UpdatedAt = checkTime;
    }
}

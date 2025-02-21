using System.ComponentModel.DataAnnotations.Schema;

using AppAvailabilityTracker.Shared.Domain.Events;

namespace AppAvailabilityTracker.Shared.Domain.Models;

public abstract class DomainModel
{
    [NotMapped]
    private readonly List<DomainEvent> _domainEvents = [];
    
    [NotMapped]
    private static readonly AsyncLocal<HashSet<DomainModel>> RegisteredAggregates = new();
    
    [NotMapped]
    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected DomainModel()
    {
        RegisteredAggregates.Value ??= new HashSet<DomainModel>();
    }

    public void AddDomainEvent(DomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
        RegisteredAggregates.Value?.Add(this);
    }
    
    public static IEnumerable<DomainModel> GetRegisteredModels() =>
        RegisteredAggregates.Value?.ToList() ?? [];
    
    public static void RemoveRegisteredModel(DomainModel domainModel) =>
        RegisteredAggregates.Value?.Remove(domainModel);
    
    public static void ClearRegisteredModels() => RegisteredAggregates.Value?.Clear();
}

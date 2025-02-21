namespace AppAvailabilityTracker.Shared.Domain.Mapping;

public interface IModelMapper
{
    T Map<T>(object source);

    IEnumerable<T> Map<T>(IEnumerable<object> sources);
    
    TDestination Map<TSource, TDestination>(TSource source, TDestination destination);
}

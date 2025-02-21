using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AppAvailabilityTracker.Shared.Domain.Mapping;

public class ModelMapper : IModelMapper
{
    private readonly IServiceProvider _serviceProvider;
    private readonly Lazy<IMapper> _mapperLazy;
    
    public ModelMapper(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _mapperLazy = new Lazy<IMapper>(CreateAutoMapper);
    }

    public T Map<T>(object source) => _mapperLazy.Value.Map<T>(source);

    public IEnumerable<T> Map<T>(IEnumerable<object> sources) => sources.Select(Map<T>);

    public TDestination Map<TSource, TDestination>(TSource source, TDestination destination) =>
        _mapperLazy.Value.Map(source, destination);

    private IMapper CreateAutoMapper()
    {
        var modelMapperFactory = _serviceProvider.GetRequiredService<IModelMapperFactory>();
        var configuration = _serviceProvider.GetRequiredService<IConfiguration>();
        
        return modelMapperFactory.CreateMapper(configuration);
    }
}

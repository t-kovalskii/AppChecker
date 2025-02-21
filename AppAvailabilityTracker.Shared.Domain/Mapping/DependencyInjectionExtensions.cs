using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace AppAvailabilityTracker.Shared.Domain.Mapping;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddMapper<TModelMapperFactory>(this IServiceCollection services)
        where TModelMapperFactory : class, IModelMapperFactory
    {
        services.AddSingleton<IModelMapper, ModelMapper>();
        services.AddSingleton<IModelMapperFactory, TModelMapperFactory>();
        
        return services;
    }
}

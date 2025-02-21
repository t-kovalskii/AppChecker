using AppAvailabilityTracker.Services.AppStorage.Domain.Enums;
using AppAvailabilityTracker.Services.AppStorage.Domain.Models;
using AppAvailabilityTracker.Shared.Domain.Mapping;

using AutoMapper;

using Microsoft.Extensions.Configuration;

namespace AppAvailabilityTracker.Services.AppStorage.Infrastructure.Mapping;

public class ApplicationModelMapperFactory : IModelMapperFactory
{
    public IMapper CreateMapper(IConfiguration configuration)
    {
        var mapperConfiguration = new MapperConfiguration(ConfigureMap);
        return mapperConfiguration.CreateMapper();
    }

    private void ConfigureMap(IMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<ApplicationDomain, Models.Application>()
            .ForMember(dest => dest.StoreLink, opt => opt.MapFrom(src => src.StoreLink.ToString()))
            .ForMember(dest => dest.Store, opt => opt.MapFrom(src => src.Store.ToString()));

        cfg.CreateMap<Models.Application, ApplicationDomain>()
            .ConstructUsing(src => new ApplicationDomain(src.Name, new Uri(src.StoreLink), Enum.Parse<StoreType>(src.Store)))
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.IsAvailable, opt => opt.MapFrom(src => src.IsAvailable))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
            .ForMember(dest => dest.DomainEvents, opt => opt.Ignore());
    }
}

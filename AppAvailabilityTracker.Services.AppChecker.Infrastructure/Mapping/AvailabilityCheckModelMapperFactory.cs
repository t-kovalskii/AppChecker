using AppAvailabilityTracker.Services.AppChecker.Domain.Models;
using AppAvailabilityTracker.Shared.Domain.Mapping;

using AutoMapper;

using Microsoft.Extensions.Configuration;

namespace AppAvailabilityTracker.Services.AppChecker.Infrastructure.Mapping;

public class AvailabilityCheckModelMapperFactory : IModelMapperFactory
{
    public IMapper CreateMapper(IConfiguration configuration)
    {
        var mapperConfiguration = new MapperConfiguration(ConfigureMap);
        return mapperConfiguration.CreateMapper();
    }

    private void ConfigureMap(IMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<AvailabilityCheck, Models.AvailabilityCheck>()
            .ForMember(dest => dest.AppLink, opt => opt.MapFrom(src => src.AppLink.ToString()))
            .ForMember(dest => dest.ApplicationId, opt => opt.MapFrom(src => src.ApplicationId))
            .ForMember(dest => dest.CheckTime, opt => opt.MapFrom(src => src.CheckTime))
            .ForMember(dest => dest.CheckResult, opt => opt.MapFrom(src => src.CheckResult));
        
        cfg.CreateMap<Models.AvailabilityCheck, AvailabilityCheck>()
            .ConstructUsing(src => new AvailabilityCheck(src.ApplicationId, new Uri(src.AppLink), src.CheckResult))
            .ForMember(dest => dest.CheckTime, opt => opt.MapFrom(src => src.CheckTime))
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
    }
}

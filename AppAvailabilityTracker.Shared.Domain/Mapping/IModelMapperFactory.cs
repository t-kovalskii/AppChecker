using AutoMapper;

using Microsoft.Extensions.Configuration;

namespace AppAvailabilityTracker.Shared.Domain.Mapping;

public interface IModelMapperFactory
{
    IMapper CreateMapper(IConfiguration configuration);
}

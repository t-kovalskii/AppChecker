using AppAvailabilityTracker.Services.AppStorage.Domain.Enums;
using Newtonsoft.Json;

namespace AppAvailabilityTracker.Services.AppStorage.Web.Dto;

public class ApplicationResponseDto
{
    [JsonProperty("id")]
    public required Guid Id { get; init; }
    
    [JsonProperty("name")]
    public required string Name { get; init; }
    
    [JsonProperty("store_link")]
    public required Uri StoreLink { get; init; }
    
    [JsonProperty("store_typr")]
    public required StoreType Store { get; init; }
    
    [JsonProperty("is_available")]
    public required bool IsAvailable { get; init; }
    
    [JsonProperty("updated_at")]
    public required DateTime UpdatedAt { get; init; }
}
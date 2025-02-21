using Newtonsoft.Json;

namespace AppAvailabilityTracker.Services.AppStorage.Web.Dto;

public class GetAllApplicationsResponse
{
    [JsonProperty("count")]
    public required int Count { get; set; }
    
    [JsonProperty("applications")]
    public required IEnumerable<ApplicationResponseDto> Applications { get; set; }
}

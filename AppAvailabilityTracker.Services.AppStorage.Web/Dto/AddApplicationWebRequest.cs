using Newtonsoft.Json;

namespace AppAvailabilityTracker.Services.AppStorage.Web.Dto;

public class AddApplicationWebRequest
{
    [JsonProperty("name")]
    public string Name { get; set; }
    
    [JsonProperty("link")]
    public Uri Link { get; set; }
}

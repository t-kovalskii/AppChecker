using Newtonsoft.Json;

namespace AppAvailabilityTracker.Services.AppStorage.Web.Dto;

public class RemoveApplicationWebRequest
{
    [JsonProperty("id")]
    public Guid Id { get; set; }
}

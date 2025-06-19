using System.Text.Json.Serialization;
using JetBrains.Annotations;

namespace TramTimes.Web.Api.Models;

public class WebScreenshot
{
    [UsedImplicitly]
    [JsonPropertyName(name: "prefix")]
    public string? Prefix { get; set; }
    
    [UsedImplicitly]
    [JsonPropertyName(name: "name")]
    public string? Name { get; set; }
    
    [UsedImplicitly]
    [JsonPropertyName(name: "endpoint")]
    public string? Endpoint { get; set; }
}
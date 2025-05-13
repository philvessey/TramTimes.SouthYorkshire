using System.Text.Json.Serialization;
using JetBrains.Annotations;

namespace TramTimes.Web.Utilities.Models;

public class WebStop
{
    [UsedImplicitly]
    [JsonPropertyName(name: "id")]
    public string? Id { get; set; }
    
    [UsedImplicitly]
    [JsonPropertyName(name: "code")]
    public string? Code { get; set; }
    
    [UsedImplicitly]
    [JsonPropertyName(name: "name")]
    public string? Name { get; set; }
    
    [UsedImplicitly]
    [JsonPropertyName(name: "longitude")]
    public double? Longitude { get; set; }
    
    [UsedImplicitly]
    [JsonPropertyName(name: "latitude")]
    public double? Latitude { get; set; }
    
    [UsedImplicitly]
    [JsonPropertyName(name: "platform")]
    public string? Platform { get; set; }
    
    [UsedImplicitly]
    [JsonPropertyName(name: "points")]
    public List<WebStopPoint>? Points { get; set; }
}
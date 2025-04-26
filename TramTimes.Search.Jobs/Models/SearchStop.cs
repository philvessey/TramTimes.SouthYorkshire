using System.Text.Json.Serialization;
using JetBrains.Annotations;

namespace TramTimes.Search.Jobs.Models;

public class SearchStop
{
    [UsedImplicitly]
    [JsonPropertyName("id")]
    public string? Id { get; set; }
    
    [UsedImplicitly]
    [JsonPropertyName("code")]
    public string? Code { get; set; }
    
    [UsedImplicitly]
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    
    [UsedImplicitly]
    [JsonPropertyName("latitude")]
    public double? Latitude { get; set; }
    
    [UsedImplicitly]
    [JsonPropertyName("longitude")]
    public double? Longitude { get; set; }
    
    [UsedImplicitly]
    [JsonPropertyName("points")]
    public List<SearchStopPoint>? Points { get; set; }
}
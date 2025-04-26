using System.Text.Json.Serialization;
using JetBrains.Annotations;

namespace TramTimes.Cache.Jobs.Models;

public class CacheStop
{
    [UsedImplicitly]
    [JsonPropertyName("id")]
    public string? Id { get; set; }
    
    [UsedImplicitly]
    [JsonPropertyName("points")]
    public List<CacheStopPoint>? Points { get; set; }
}
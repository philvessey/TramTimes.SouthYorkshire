using System.Text.Json.Serialization;
using JetBrains.Annotations;

namespace TramTimes.Database.Jobs.Models;

public class WorkerStopPoint
{
    [UsedImplicitly]
    [JsonPropertyName("departure_time")]
    public string? DepartureTime { get; set; }
    
    [UsedImplicitly]
    [JsonPropertyName("route_name")]
    public string? RouteName { get; set; }
}
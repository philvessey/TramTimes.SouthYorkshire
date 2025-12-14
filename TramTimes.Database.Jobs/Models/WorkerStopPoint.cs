using System.Text.Json.Serialization;
using JetBrains.Annotations;

namespace TramTimes.Database.Jobs.Models;

public class WorkerStopPoint
{
    [UsedImplicitly]
    [JsonPropertyName(name: "departure_time")]
    public string? DepartureTime { get; set; }

    [UsedImplicitly]
    [JsonPropertyName(name: "destination_name")]
    public string? DestinationName { get; set; }

    [UsedImplicitly]
    [JsonPropertyName(name: "route_name")]
    public string? RouteName { get; set; }
}
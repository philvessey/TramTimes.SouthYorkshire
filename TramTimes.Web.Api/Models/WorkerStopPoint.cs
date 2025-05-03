using System.Text.Json.Serialization;
using JetBrains.Annotations;

namespace TramTimes.Web.Api.Models;

public class WorkerStopPoint
{
    [UsedImplicitly]
    [JsonPropertyName("departure_date_time")]
    public string? DepartureDateTime { get; set; }
    
    [UsedImplicitly]
    [JsonPropertyName("destination_name")]
    public string? DestinationName { get; set; }
    
    [UsedImplicitly]
    [JsonPropertyName("route_name")]
    public string? RouteName { get; set; }
    
    [UsedImplicitly]
    [JsonPropertyName("stop_id")]
    public string? StopId { get; set; }
    
    [UsedImplicitly]
    [JsonPropertyName("stop_name")]
    public string? StopName { get; set; }
    
    [UsedImplicitly]
    [JsonPropertyName("trip_id")]
    public string? TripId { get; set; }
}
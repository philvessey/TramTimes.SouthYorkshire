using System.Text.Json.Serialization;
using JetBrains.Annotations;

namespace TramTimes.Web.Utilities.Models;

public class WebStopPoint
{
    [UsedImplicitly]
    [JsonPropertyName(name: "departure_date_time")]
    public string? DepartureDateTime { get; set; }
    
    [UsedImplicitly]
    [JsonPropertyName(name: "destination_name")]
    public string? DestinationName { get; set; }
    
    [UsedImplicitly]
    [JsonPropertyName(name: "route_name")]
    public string? RouteName { get; set; }
    
    [UsedImplicitly]
    [JsonPropertyName(name: "stop_id")]
    public string? StopId { get; set; }
    
    [UsedImplicitly]
    [JsonPropertyName(name: "stop_name")]
    public string? StopName { get; set; }
    
    [UsedImplicitly]
    [JsonPropertyName(name: "trip_id")]
    public string? TripId { get; set; }
}
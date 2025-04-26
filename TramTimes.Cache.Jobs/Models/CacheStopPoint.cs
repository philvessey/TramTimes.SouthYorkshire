using System.Text.Json.Serialization;
using JetBrains.Annotations;

namespace TramTimes.Cache.Jobs.Models;

public class CacheStopPoint
{
    [UsedImplicitly]
    [JsonPropertyName("departure_date_time")]
    public DateTime? DepartureDateTime { get; set; }
    
    [UsedImplicitly]
    [JsonPropertyName("destination_name")]
    public string? DestinationName { get; set; }
    
    [UsedImplicitly]
    [JsonPropertyName("route_name")]
    public string? RouteName { get; set; }
    
    [UsedImplicitly]
    [JsonPropertyName("trip_id")]
    public string? TripId { get; set; }
}
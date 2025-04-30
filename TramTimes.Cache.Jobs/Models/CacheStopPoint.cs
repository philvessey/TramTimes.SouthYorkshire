using JetBrains.Annotations;

namespace TramTimes.Cache.Jobs.Models;

public class CacheStopPoint
{
    [UsedImplicitly]
    public DateTime? DepartureDateTime { get; set; }
    
    [UsedImplicitly]
    public string? DestinationName { get; set; }
    
    [UsedImplicitly]
    public string? RouteName { get; set; }
    
    [UsedImplicitly]
    public string? TripId { get; set; }
}
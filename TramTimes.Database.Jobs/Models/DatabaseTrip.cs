using JetBrains.Annotations;

namespace TramTimes.Database.Jobs.Models;

public class DatabaseTrip
{
    [UsedImplicitly]
    public string? RouteId { get; set; }
    
    [UsedImplicitly]
    public string? ServiceId { get; set; }
    
    [UsedImplicitly]
    public string? TripId { get; set; }
    
    [UsedImplicitly]
    public string? TripHeadsign { get; set; }
    
    [UsedImplicitly]
    public string? TripShortName { get; set; }
    
    [UsedImplicitly]
    public short? DirectionId { get; set; }
    
    [UsedImplicitly]
    public string? BlockId { get; set; }
    
    [UsedImplicitly]
    public string? ShapeId { get; set; }
    
    [UsedImplicitly]
    public string? WheelchairAccessible { get; set; }
    
    [UsedImplicitly]
    public string? BikesAllowed { get; set; }
}
using JetBrains.Annotations;

namespace TramTimes.Database.Jobs.Models;

public class DatabaseStopTime
{
    [UsedImplicitly] public string? TripId { get; set; }
    [UsedImplicitly] public string? ArrivalTime { get; set; }
    [UsedImplicitly] public string? DepartureTime { get; set; }
    [UsedImplicitly] public string? StopId { get; set; }
    [UsedImplicitly] public short? StopSequence { get; set; }
    [UsedImplicitly] public string? StopHeadsign { get; set; }
    [UsedImplicitly] public string? PickupType { get; set; }
    [UsedImplicitly] public string? DropOffType { get; set; }
    [UsedImplicitly] public float? ShapeDistTraveled { get; set; }
    [UsedImplicitly] public short? Timepoint { get; set; }
}
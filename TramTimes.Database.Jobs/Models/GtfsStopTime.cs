// ReSharper disable all

using JetBrains.Annotations;

namespace TramTimes.Database.Jobs.Models;

public class GtfsStopTime
{
    [UsedImplicitly] public string? trip_id { get; set; }
    [UsedImplicitly] public string? arrival_time { get; set; }
    [UsedImplicitly] public string? departure_time { get; set; }
    [UsedImplicitly] public string? stop_id { get; set; }
    [UsedImplicitly] public string? stop_sequence { get; set; }
    [UsedImplicitly] public string? stop_headsign { get; set; }
    [UsedImplicitly] public string? pickup_type { get; set; }
    [UsedImplicitly] public string? drop_off_type { get; set; }
    [UsedImplicitly] public string? shape_dist_traveled { get; set; }
    [UsedImplicitly] public string? timepoint { get; set; }
}
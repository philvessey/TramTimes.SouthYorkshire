// ReSharper disable all

using JetBrains.Annotations;

namespace TramTimes.Database.Jobs.Models;

public class DatabaseTrip
{
    [UsedImplicitly] public string? route_id { get; set; }
    [UsedImplicitly] public string? service_id { get; set; }
    [UsedImplicitly] public string? trip_id { get; set; }
    [UsedImplicitly] public string? trip_headsign { get; set; }
    [UsedImplicitly] public string? trip_short_name { get; set; }
    [UsedImplicitly] public short? direction_id { get; set; }
    [UsedImplicitly] public string? block_id { get; set; }
    [UsedImplicitly] public string? shape_id { get; set; }
    [UsedImplicitly] public string? wheelchair_accessible { get; set; }
    [UsedImplicitly] public string? bikes_allowed { get; set; }
}
// ReSharper disable all

using JetBrains.Annotations;

namespace TramTimes.Database.Jobs.Models;

public class GtfsStop
{
    [UsedImplicitly] public string? stop_id { get; set; }
    [UsedImplicitly] public string? stop_code { get; set; }
    [UsedImplicitly] public string? stop_name { get; set; }
    [UsedImplicitly] public string? stop_desc { get; set; }
    [UsedImplicitly] public string? stop_lat { get; set; }
    [UsedImplicitly] public string? stop_lon { get; set; }
    [UsedImplicitly] public string? zone_id { get; set; }
    [UsedImplicitly] public string? stop_url { get; set; }
    [UsedImplicitly] public string? location_type { get; set; }
    [UsedImplicitly] public string? parent_station { get; set; }
    [UsedImplicitly] public string? stop_timezone { get; set; }
    [UsedImplicitly] public string? wheelchair_boarding { get; set; }
    [UsedImplicitly] public string? level_id { get; set; }
    [UsedImplicitly] public string? platform_code { get; set; }
}
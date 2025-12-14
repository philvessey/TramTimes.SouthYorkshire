using CsvHelper.Configuration.Attributes;
using JetBrains.Annotations;

namespace TramTimes.Database.Jobs.Models;

public class GtfsStop
{
    [UsedImplicitly]
    [Name(name: "stop_id")]
    public string? StopId { get; set; }

    [UsedImplicitly]
    [Name(name: "stop_code")]
    public string? StopCode { get; set; }

    [UsedImplicitly]
    [Name(name: "stop_name")]
    public string? StopName { get; set; }

    [UsedImplicitly]
    [Name(name: "stop_desc")]
    public string? StopDesc { get; set; }

    [UsedImplicitly]
    [Name(name: "stop_lat")]
    public string? StopLat { get; set; }

    [UsedImplicitly]
    [Name(name: "stop_lon")]
    public string? StopLon { get; set; }

    [UsedImplicitly]
    [Name(name: "zone_id")]
    public string? ZoneId { get; set; }

    [UsedImplicitly]
    [Name(name: "stop_url")]
    public string? StopUrl { get; set; }

    [UsedImplicitly]
    [Name(name: "location_type")]
    public string? LocationType { get; set; }

    [UsedImplicitly]
    [Name(name: "parent_station")]
    public string? ParentStation { get; set; }

    [UsedImplicitly]
    [Name(name: "stop_timezone")]
    public string? StopTimezone { get; set; }

    [UsedImplicitly]
    [Name(name: "wheelchair_boarding")]
    public string? WheelchairBoarding { get; set; }

    [UsedImplicitly]
    [Name(name: "level_id")]
    public string? LevelId { get; set; }

    [UsedImplicitly]
    [Name(name: "platform_code")]
    public string? PlatformCode { get; set; }
}
using CsvHelper.Configuration.Attributes;
using JetBrains.Annotations;

namespace TramTimes.Database.Jobs.Models;

public class GtfsTrip
{
    [UsedImplicitly]
    [Name(name: "route_id")]
    public string? RouteId { get; set; }

    [UsedImplicitly]
    [Name(name: "service_id")]
    public string? ServiceId { get; set; }

    [UsedImplicitly]
    [Name(name: "trip_id")]
    public string? TripId { get; set; }

    [UsedImplicitly]
    [Name(name: "trip_headsign")]
    public string? TripHeadsign { get; set; }

    [UsedImplicitly]
    [Name(name: "trip_short_name")]
    public string? TripShortName { get; set; }

    [UsedImplicitly]
    [Name(name: "direction_id")]
    public string? DirectionId { get; set; }

    [UsedImplicitly]
    [Name(name: "block_id")]
    public string? BlockId { get; set; }

    [UsedImplicitly]
    [Name(name: "shape_id")]
    public string? ShapeId { get; set; }

    [UsedImplicitly]
    [Name(name: "wheelchair_accessible")]
    public string? WheelchairAccessible { get; set; }

    [UsedImplicitly]
    [Name(name: "bikes_allowed")]
    public string? BikesAllowed { get; set; }
}
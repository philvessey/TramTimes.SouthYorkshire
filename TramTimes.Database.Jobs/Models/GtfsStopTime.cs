using CsvHelper.Configuration.Attributes;
using JetBrains.Annotations;

namespace TramTimes.Database.Jobs.Models;

public class GtfsStopTime
{
    [UsedImplicitly]
    [Name(name: "trip_id")]
    public string? TripId { get; set; }
    
    [UsedImplicitly]
    [Name(name: "arrival_time")]
    public string? ArrivalTime { get; set; }
    
    [UsedImplicitly]
    [Name(name: "departure_time")]
    public string? DepartureTime { get; set; }
    
    [UsedImplicitly]
    [Name(name: "stop_id")]
    public string? StopId { get; set; }
    
    [UsedImplicitly]
    [Name(name: "stop_sequence")]
    public string? StopSequence { get; set; }
    
    [UsedImplicitly]
    [Name(name: "stop_headsign")]
    public string? StopHeadsign { get; set; }
    
    [UsedImplicitly]
    [Name(name: "pickup_type")]
    public string? PickupType { get; set; }
    
    [UsedImplicitly]
    [Name(name: "drop_off_type")]
    public string? DropOffType { get; set; }
    
    [UsedImplicitly]
    [Name(name: "shape_dist_traveled")]
    public string? ShapeDistTraveled { get; set; }
    
    [UsedImplicitly]
    [Name(name: "timepoint")]
    public string? Timepoint { get; set; }
}
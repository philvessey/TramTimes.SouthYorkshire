using JetBrains.Annotations;

namespace TramTimes.Database.Jobs.Models;

public class DatabaseStop
{
    [UsedImplicitly]
    public string? StopId { get; set; }
    
    [UsedImplicitly]
    public string? StopCode { get; set; }
    
    [UsedImplicitly]
    public string? StopName { get; set; }
    
    [UsedImplicitly]
    public string? StopDesc { get; set; }
    
    [UsedImplicitly]
    public float? StopLat { get; set; }
    
    [UsedImplicitly]
    public float? StopLon { get; set; }
    
    [UsedImplicitly]
    public string? ZoneId { get; set; }
    
    [UsedImplicitly]
    public string? StopUrl { get; set; }
    
    [UsedImplicitly]
    public short? LocationType { get; set; }
    
    [UsedImplicitly]
    public string? ParentStation { get; set; }
    
    [UsedImplicitly]
    public string? StopTimezone { get; set; }
    
    [UsedImplicitly]
    public short? WheelchairBoarding { get; set; }
    
    [UsedImplicitly]
    public string? LevelId { get; set; }
    
    [UsedImplicitly]
    public string? PlatformCode { get; set; }
}
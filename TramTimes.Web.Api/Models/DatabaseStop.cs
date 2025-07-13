using JetBrains.Annotations;

namespace TramTimes.Web.Api.Models;

public class DatabaseStop
{
    [UsedImplicitly]
    public string? Id { get; set; }
    
    [UsedImplicitly]
    public string? Code { get; set; }
    
    [UsedImplicitly]
    public string? Name { get; set; }
    
    [UsedImplicitly]
    public double? Latitude { get; set; }
    
    [UsedImplicitly]
    public double? Longitude { get; set; }
    
    [UsedImplicitly]
    public string? Platform { get; set; }
    
    [UsedImplicitly]
    public List<DatabaseStopPoint>? Points { get; set; }
}
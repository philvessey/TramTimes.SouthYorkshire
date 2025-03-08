using JetBrains.Annotations;

namespace TramTimes.Database.Jobs.Models;

public class DatabaseRoute
{
    [UsedImplicitly]
    public string? RouteId { get; set; }
    
    [UsedImplicitly]
    public string? AgencyId { get; set; }
    
    [UsedImplicitly]
    public string? RouteShortName { get; set; }
    
    [UsedImplicitly]
    public string? RouteLongName { get; set; }
    
    [UsedImplicitly]
    public string? RouteDesc { get; set; }
    
    [UsedImplicitly]
    public short? RouteType { get; set; }
    
    [UsedImplicitly]
    public string? RouteUrl { get; set; }
    
    [UsedImplicitly]
    public short? RouteColor { get; set; }
    
    [UsedImplicitly]
    public short? RouteTextColor { get; set; }
    
    [UsedImplicitly]
    public short? RouteSortOrder { get; set; }
}
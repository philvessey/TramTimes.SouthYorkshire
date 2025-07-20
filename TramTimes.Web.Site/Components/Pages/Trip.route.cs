using Microsoft.AspNetCore.Components;

namespace TramTimes.Web.Site.Components.Pages;

public partial class Trip
{
    [Parameter]
    public required string TripId { get; set; }
    
    [Parameter]
    public string? StopId { get; set; }
    
    [Parameter]
    public double? Longitude { get; set; }
    
    [Parameter]
    public double? Latitude { get; set; }
    
    [Parameter]
    public double? Zoom { get; set; }
}
using Microsoft.AspNetCore.Components;

namespace TramTimes.Web.Site.Components.Shared;

public partial class NavigateToLocation
{
    [Parameter] public double? Longitude { get; set; }
    [Parameter] public double? Latitude { get; set; }
}
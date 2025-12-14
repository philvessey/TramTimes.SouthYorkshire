using Microsoft.AspNetCore.Components;

namespace TramTimes.Web.Site.Components.Pages;

public partial class Privacy
{
    [Parameter] public double? Longitude { get; set; }
    [Parameter] public double? Latitude { get; set; }
    [Parameter] public double? Zoom { get; set; }
}
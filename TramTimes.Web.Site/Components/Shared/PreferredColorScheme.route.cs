using Microsoft.AspNetCore.Components;

namespace TramTimes.Web.Site.Components.Shared;

public partial class PreferredColorScheme
{
    [Parameter] public string? Scheme { get; set; }
    [Parameter] public string? Page { get; set; }
}
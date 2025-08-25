using Microsoft.AspNetCore.Components;

namespace TramTimes.Web.Site.Components.Shared;

public partial class SourceCodeRepository
{
    [Parameter]
    public string? Url { get; set; }
}
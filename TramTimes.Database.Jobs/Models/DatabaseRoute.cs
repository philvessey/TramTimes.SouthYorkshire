// ReSharper disable all

using JetBrains.Annotations;

namespace TramTimes.Database.Jobs.Models;

public class DatabaseRoute
{
    [UsedImplicitly] public string? route_id { get; set; }
    [UsedImplicitly] public string? agency_id { get; set; }
    [UsedImplicitly] public string? route_short_name { get; set; }
    [UsedImplicitly] public string? route_long_name { get; set; }
    [UsedImplicitly] public string? route_desc { get; set; }
    [UsedImplicitly] public short? route_type { get; set; }
    [UsedImplicitly] public string? route_url { get; set; }
    [UsedImplicitly] public string? route_color { get; set; }
    [UsedImplicitly] public string? route_text_color { get; set; }
    [UsedImplicitly] public short? route_sort_order { get; set; }
}
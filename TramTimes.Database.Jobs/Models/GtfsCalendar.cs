// ReSharper disable all

using JetBrains.Annotations;

namespace TramTimes.Database.Jobs.Models;

public class GtfsCalendar
{
    [UsedImplicitly] public string? service_id { get; set; }
    [UsedImplicitly] public string? monday { get; set; }
    [UsedImplicitly] public string? tuesday { get; set; }
    [UsedImplicitly] public string? wednesday { get; set; }
    [UsedImplicitly] public string? thursday { get; set; }
    [UsedImplicitly] public string? friday { get; set; }
    [UsedImplicitly] public string? saturday { get; set; }
    [UsedImplicitly] public string? sunday { get; set; }
    [UsedImplicitly] public string? start_date { get; set; }
    [UsedImplicitly] public string? end_date { get; set; }
}
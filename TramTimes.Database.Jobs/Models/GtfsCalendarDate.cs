// ReSharper disable all

using JetBrains.Annotations;

namespace TramTimes.Database.Jobs.Models;

public class GtfsCalendarDate
{
    [UsedImplicitly] public string? service_id { get; set; }
    [UsedImplicitly] public string? date { get; set; }
    [UsedImplicitly] public string? exception_type { get; set; }
}
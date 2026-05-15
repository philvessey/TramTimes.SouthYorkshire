// ReSharper disable all

using JetBrains.Annotations;

namespace TramTimes.Database.Jobs.Models;

public class DatabaseCalendar
{
    [UsedImplicitly] public string? service_id { get; set; }
    [UsedImplicitly] public short? monday { get; set; }
    [UsedImplicitly] public short? tuesday { get; set; }
    [UsedImplicitly] public short? wednesday { get; set; }
    [UsedImplicitly] public short? thursday { get; set; }
    [UsedImplicitly] public short? friday { get; set; }
    [UsedImplicitly] public short? saturday { get; set; }
    [UsedImplicitly] public short? sunday { get; set; }
    [UsedImplicitly] public DateOnly? start_date { get; set; }
    [UsedImplicitly] public DateOnly? end_date { get; set; }
}
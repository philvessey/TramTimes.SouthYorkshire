// ReSharper disable all

using JetBrains.Annotations;

namespace TramTimes.Database.Jobs.Models;

public class DatabaseCalendarDate
{
    [UsedImplicitly] public string? service_id { get; set; }
    [UsedImplicitly] public DateOnly? exception_date { get; set; }
    [UsedImplicitly] public short? exception_type { get; set; }
}
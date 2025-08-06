using JetBrains.Annotations;

namespace TramTimes.Database.Jobs.Models;

public class DatabaseCalendarDate
{
    [UsedImplicitly]
    public string? ServiceId { get; set; }
    
    [UsedImplicitly]
    public DateOnly? ExceptionDate { get; set; }
    
    [UsedImplicitly]
    public short? ExceptionType { get; set; }
}
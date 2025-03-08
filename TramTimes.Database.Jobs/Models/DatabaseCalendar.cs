using JetBrains.Annotations;

namespace TramTimes.Database.Jobs.Models;

public class DatabaseCalendar
{
    [UsedImplicitly]
    public string? ServiceId { get; set; }
    
    [UsedImplicitly]
    public short? Monday { get; set; }
    
    [UsedImplicitly]
    public short? Tuesday { get; set; }
    
    [UsedImplicitly]
    public short? Wednesday { get; set; }
    
    [UsedImplicitly]
    public short? Thursday { get; set; }
    
    [UsedImplicitly]
    public short? Friday { get; set; }
    
    [UsedImplicitly]
    public short? Saturday { get; set; }
    
    [UsedImplicitly]
    public short? Sunday { get; set; }
    
    [UsedImplicitly]
    public DateTime? StartDate { get; set; }
    
    [UsedImplicitly]
    public DateTime? EndDate { get; set; }
}
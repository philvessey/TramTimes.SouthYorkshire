using JetBrains.Annotations;

namespace TramTimes.Database.Jobs.Models;

public class TravelineCalendar
{
    [UsedImplicitly]
    public bool? Monday { get; set; }
    
    [UsedImplicitly]
    public bool? Tuesday { get; set; }
    
    [UsedImplicitly]
    public bool? Wednesday { get; set; }
    
    [UsedImplicitly]
    public bool? Thursday { get; set; }
    
    [UsedImplicitly]
    public bool? Friday { get; set; }
    
    [UsedImplicitly]
    public bool? Saturday { get; set; }
    
    [UsedImplicitly]
    public bool? Sunday { get; set; }
    
    [UsedImplicitly]
    public DateOnly? StartDate { get; set; }
    
    [UsedImplicitly]
    public DateOnly? EndDate { get; set; }
    
    [UsedImplicitly]
    public List<DateOnly>? RunningDates { get; set; }
    
    [UsedImplicitly]
    public List<DateOnly>? SupplementRunningDates { get; set; }
    
    [UsedImplicitly]
    public List<DateOnly>? SupplementNonRunningDates { get; set; }
}
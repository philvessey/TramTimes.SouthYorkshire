namespace TramTimes.Database.Jobs.Extensions;

public static class TimeSpanExtensions
{
    public static TimeSpan ToNextDay(this TimeSpan? baseTimeSpan)
    {
        return baseTimeSpan?.Add(ts: TimeSpan.FromHours(value: 24)) ?? TimeSpan.MinValue;
    }
    
    public static TimeSpan ToPreviousDay(this TimeSpan? baseTimeSpan)
    {
        return baseTimeSpan?.Subtract(ts: TimeSpan.FromHours(value: 24)) ?? TimeSpan.MinValue;
    }
}
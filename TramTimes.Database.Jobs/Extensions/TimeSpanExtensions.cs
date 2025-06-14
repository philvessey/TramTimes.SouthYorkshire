namespace TramTimes.Database.Jobs.Extensions;

public static class TimeSpanExtensions
{
    public static TimeSpan ToNextDay(this TimeSpan? baseTimeSpan)
    {
        #region check valid input
        
        if (baseTimeSpan is null)
            return TimeSpan.MaxValue;
        
        #endregion
        
        #region build result
        
        var result = baseTimeSpan.Value.Add(ts: TimeSpan.FromHours(value: 24));
        
        #endregion
        
        return result;
    }
    
    public static TimeSpan ToPreviousDay(this TimeSpan? baseTimeSpan)
    {
        #region check valid input
        
        if (baseTimeSpan is null)
            return TimeSpan.MinValue;
        
        #endregion
        
        #region build result
        
        var result = baseTimeSpan.Value.Subtract(ts: TimeSpan.FromHours(value: 24));
        
        #endregion
        
        return result;
    }
}
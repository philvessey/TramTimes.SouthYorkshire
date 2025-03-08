namespace TramTimes.Database.Jobs.Extensions;

public static class StringExtensions
{
    public static DateTime ToDate(this string? baseString)
    {
        if (baseString == null)
            return DateTime.MinValue;
        
        var month = int.Parse(s: baseString.Substring(
            startIndex: 5,
            length: 2));
        
        var day = int.Parse(s: baseString.Substring(
            startIndex: 8,
            length: 2));
        
        return new DateTime(year: int.Parse(s: baseString[..4]),
            month: month,
            day: day);
    }
    
    public static short ToShort(this string? baseString)
    {
        return baseString == null ? short.Parse(s: "0") : short.Parse(s: baseString);
    }
    
    public static TimeSpan ToTime(this string? baseString)
    {
        if (baseString == null)
            return TimeSpan.MinValue;
        
        var minutes = int.Parse(s: baseString.Substring(
            startIndex: 3,
            length: 2));
        
        var seconds = int.Parse(s: baseString.Substring(
            startIndex: 6,
            length: 2));
        
        return new TimeSpan(hours: int.Parse(s: baseString[..2]),
            minutes: minutes,
            seconds: seconds);
    }
}
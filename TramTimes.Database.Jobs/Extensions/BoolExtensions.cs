namespace TramTimes.Database.Jobs.Extensions;

public static class BoolExtensions
{
    public static int ToInt(this bool? baseBool)
    {
        return !baseBool.HasValue ? int.Parse(s: "0") : baseBool.Value ? int.Parse(s: "1") : int.Parse(s: "0");
    }
    
    public static short ToShort(this bool? baseBool)
    {
        return !baseBool.HasValue ? short.Parse(s: "0") : baseBool.Value ? short.Parse(s: "1") : short.Parse(s: "0");
    }
}
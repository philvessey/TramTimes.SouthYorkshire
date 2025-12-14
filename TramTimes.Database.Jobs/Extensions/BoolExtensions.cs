namespace TramTimes.Database.Jobs.Extensions;

public static class BoolExtensions
{
    public static int ToInt(this bool? baseBool)
    {
        #region check valid input

        if (!baseBool.HasValue)
            return int.Parse(s: "0");

        #endregion

        #region build result

        var result = baseBool.Value
            ? int.Parse(s: "1")
            : int.Parse(s: "0");

        #endregion

        return result;
    }

    public static short ToShort(this bool? baseBool)
    {
        #region check valid input

        if (!baseBool.HasValue)
            return short.Parse(s: "0");

        #endregion

        #region build result

        var result = baseBool.Value
            ? short.Parse(s: "1")
            : short.Parse(s: "0");

        #endregion

        return result;
    }
}
namespace TramTimes.Database.Jobs.Extensions;

public static class StringExtensions
{
    public static DateOnly ToDate(this string? baseString)
    {
        #region check valid input

        if (baseString is null)
            return DateOnly.MinValue;

        #endregion

        #region build result

        var result = new DateOnly(
            year: int.Parse(s: baseString[..4]),
            month: int.Parse(s: baseString.Substring(
                startIndex: 5,
                length: 2)),
            day: int.Parse(s: baseString.Substring(
                startIndex: 8,
                length: 2)));

        #endregion

        return result;
    }

    public static short ToShort(this string? baseString)
    {
        #region check valid input

        if (baseString is null)
            return short.Parse(s: "0");

        #endregion

        #region build result

        var result = short.Parse(s: baseString);

        #endregion

        return result;
    }

    public static TimeSpan ToTime(this string? baseString)
    {
        #region check valid input

        if (baseString is null)
            return TimeSpan.MinValue;

        #endregion

        #region build result

        var result = new TimeSpan(
            hours: int.Parse(s: baseString[..2]),
            minutes: int.Parse(s: baseString.Substring(
                startIndex: 3,
                length: 2)),
            seconds: int.Parse(s: baseString.Substring(
                startIndex: 6,
                length: 2)));

        #endregion

        return result;
    }
}
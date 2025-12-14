namespace TramTimes.Web.Utilities.Extensions;

public static class ListExtensions
{
    public static bool IsNullOrEmpty<T>(this List<T>? baseList)
    {
        #region build result

        var result = baseList is null || baseList.Count is 0;

        #endregion

        return result;
    }
}
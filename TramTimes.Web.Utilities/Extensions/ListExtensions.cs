namespace TramTimes.Web.Utilities.Extensions;

public static class ListExtensions
{
    public static bool IsNullOrEmpty<T>(this List<T>? baseList)
    {
        return baseList is null || baseList.Count == 0;
    }
}
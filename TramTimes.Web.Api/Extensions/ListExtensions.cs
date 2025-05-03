namespace TramTimes.Web.Api.Extensions;

public static class ListExtensions
{
    public static bool IsNullOrEmpty<T>(this List<T>? baseList)
    {
        return baseList is { Count: 0 };
    }
}
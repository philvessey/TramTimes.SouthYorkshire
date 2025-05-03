namespace TramTimes.Web.Api.Extensions;

public static class ReadOnlyCollectionExtensions
{
    public static bool IsNullOrEmpty<T>(this IReadOnlyCollection<T>? baseCollection)
    {
        return baseCollection is { Count: 0 };
    }
}
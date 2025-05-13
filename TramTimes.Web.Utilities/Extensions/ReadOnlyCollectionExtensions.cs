namespace TramTimes.Web.Utilities.Extensions;

public static class ReadOnlyCollectionExtensions
{
    public static bool IsNullOrEmpty<T>(this IReadOnlyCollection<T>? baseCollection)
    {
        return baseCollection is null || baseCollection.Count == 0;
    }
}
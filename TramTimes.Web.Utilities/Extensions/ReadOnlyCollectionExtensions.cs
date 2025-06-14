namespace TramTimes.Web.Utilities.Extensions;

public static class ReadOnlyCollectionExtensions
{
    public static bool IsNullOrEmpty<T>(this IReadOnlyCollection<T>? baseCollection)
    {
        #region build result
        
        var result = baseCollection is null || baseCollection.Count is 0;
        
        #endregion
        
        return result;
    }
}
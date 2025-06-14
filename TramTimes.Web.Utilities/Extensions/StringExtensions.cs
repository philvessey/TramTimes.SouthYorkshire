namespace TramTimes.Web.Utilities.Extensions;

public static class StringExtensions
{
    public static bool ContainsIgnoreCase(
        this string? baseString,
        string value) {
        
        #region check valid input
        
        if (baseString is null)
            return false;
        
        #endregion
        
        #region build result
        
        var result = baseString.Contains(
            value: value,
            comparisonType: StringComparison.InvariantCultureIgnoreCase);
        
        #endregion
        
        return result;
    }
    
    public static bool ContainsRespectCase(
        this string? baseString,
        string value) {
        
        #region check valid input
        
        if (baseString is null)
            return false;
        
        #endregion
        
        #region build result
        
        var result = baseString.Contains(
            value: value,
            comparisonType: StringComparison.InvariantCulture);
        
        #endregion
        
        return result;
    }
}
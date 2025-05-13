namespace TramTimes.Web.Utilities.Extensions;

public static class StringExtensions
{
    public static bool ContainsIgnoreCase(
        this string? baseString,
        string value) {
        
        var match = baseString?.IndexOf(
            value: value,
            comparisonType: StringComparison.InvariantCultureIgnoreCase);
        
        return match >= 0;
    }
    
    public static bool ContainsRespectCase(
        this string? baseString,
        string value) {
        
        var match = baseString?.IndexOf(
            value: value,
            comparisonType: StringComparison.InvariantCulture);
        
        return match >= 0;
    }
}
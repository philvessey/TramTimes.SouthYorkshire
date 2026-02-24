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
            comparisonType: StringComparison.InvariantCultureIgnoreCase,
            value: value);

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
            comparisonType: StringComparison.InvariantCulture,
            value: value);

        #endregion

        return result;
    }

    public static bool EndsWithIgnoreCase(
        this string? baseString,
        string value) {

        #region check valid input

        if (baseString is null)
            return false;

        #endregion

        #region build result

        var result = baseString.EndsWith(
            comparisonType: StringComparison.InvariantCultureIgnoreCase,
            value: value);

        #endregion

        return result;
    }

    public static bool EndsWithRespectCase(
        this string? baseString,
        string value) {

        #region check valid input

        if (baseString is null)
            return false;

        #endregion

        #region build result

        var result = baseString.EndsWith(
            comparisonType: StringComparison.InvariantCulture,
            value: value);

        #endregion

        return result;
    }

    public static bool EqualsIgnoreCase(
        this string? baseString,
        string value) {

        #region check valid input

        if (baseString is null)
            return false;

        #endregion

        #region build result

        var result = baseString.Equals(
            comparisonType: StringComparison.InvariantCultureIgnoreCase,
            value: value);

        #endregion

        return result;
    }

    public static bool EqualsRespectCase(
        this string? baseString,
        string value) {

        #region check valid input

        if (baseString is null)
            return false;

        #endregion

        #region build result

        var result = baseString.Equals(
            comparisonType: StringComparison.InvariantCulture,
            value: value);

        #endregion

        return result;
    }

    public static bool StartsWithIgnoreCase(
        this string? baseString,
        string value) {

        #region check valid input

        if (baseString is null)
            return false;

        #endregion

        #region build result

        var result = baseString.StartsWith(
            comparisonType: StringComparison.InvariantCultureIgnoreCase,
            value: value);

        #endregion

        return result;
    }

    public static bool StartsWithRespectCase(
        this string? baseString,
        string value) {

        #region check valid input

        if (baseString is null)
            return false;

        #endregion

        #region build result

        var result = baseString.StartsWith(
            comparisonType: StringComparison.InvariantCulture,
            value: value);

        #endregion

        return result;
    }
}
using TramTimes.Web.Utilities.Builders;

namespace TramTimes.Web.Utilities.Tools;

public static class RegexTools
{
    public static string RemoveName(string input)
    {
        #region match valid input
        
        var match = RegexBuilder
            .GetName()
            .Match(input: input);
        
        if (!match.Success)
            return input;
        
        #endregion
        
        #region build result
        
        var result = input.Replace(
            oldValue: match.Value,
            newValue: string.Empty);
        
        #endregion
        
        return result.TrimStart();
    }
    
    public static string RemoveDirection(string input)
    {
        #region match valid input
        
        var match = RegexBuilder
            .GetDirection()
            .Match(input: input);
        
        if (!match.Success)
            return input;
        
        #endregion
        
        #region build result
        
        var result = input.Replace(
            oldValue: match.Value,
            newValue: string.Empty);
        
        #endregion
        
        return result.TrimEnd();
    }
}
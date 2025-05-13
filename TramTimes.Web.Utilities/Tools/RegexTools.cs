using TramTimes.Web.Utilities.Builders;

namespace TramTimes.Web.Utilities.Tools;

public static class RegexTools
{
    public static string RemoveName(string input)
    {
        var match = RegexBuilder.GetName()
            .Match(input: input);
        
        if (!match.Success)
            return input;
        
        var output = input.Replace(
            oldValue: match.Value,
            newValue: string.Empty);
        
        return output.TrimStart();
    }
    
    public static string RemoveDirection(string input)
    {
        var match = RegexBuilder.GetDirection()
            .Match(input: input);
        
        if (!match.Success)
            return input;
        
        var output = input.Replace(
            oldValue: match.Value,
            newValue: string.Empty);
        
        return output.TrimEnd();
    }
}
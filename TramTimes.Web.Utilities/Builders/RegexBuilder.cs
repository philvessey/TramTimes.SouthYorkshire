using System.Text.RegularExpressions;

namespace TramTimes.Web.Utilities.Builders;

public static partial class RegexBuilder
{
    [GeneratedRegex(pattern: @"^(.*?)(?=\s+\b(From|To|Entrance|Platform)\b)")] public static partial Regex GetName();
    [GeneratedRegex(pattern: @"\b(From|To|Entrance|Platform)\b(?:\s.*)?")] public static partial Regex GetDirection();
    [GeneratedRegex(pattern: @"/[\d.-]+/[\d.-]+(?:/[\d.-]+)?$")] public static partial Regex GetUrl();
}
using System.Text.RegularExpressions;

namespace TramTimes.Web.Utilities.Builders;

public static partial class RegexBuilder
{
    [GeneratedRegex(pattern: @"^(.*?)(?=\s+From|\s+To)")] public static partial Regex GetName();
    [GeneratedRegex(pattern: @"\b(From|To)\s.*")] public static partial Regex GetDirection();
    [GeneratedRegex(pattern: @"/[\d.-]+/[\d.-]+$")] public static partial Regex GetUrl();
}
using System.Text.RegularExpressions;

namespace TramTimes.Database.Jobs.Builders;

public static partial class GeneratedRegexBuilder
{
    [GeneratedRegex(pattern: @"\bEntrance\b(?:\s.*)?")] public static partial Regex GetEntrance();
    [GeneratedRegex(pattern: @"\bPlatform\b(?:\s.*)?")] public static partial Regex GetPlatform();
    [GeneratedRegex(pattern: @"\bFrom\b(?:\s.*)?")] public static partial Regex GetFrom();
    [GeneratedRegex(pattern: @"\bTo\b(?:\s.*)?")] public static partial Regex GetTo();
}
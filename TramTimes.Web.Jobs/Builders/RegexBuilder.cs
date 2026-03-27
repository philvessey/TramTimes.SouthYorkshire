using System.Text.RegularExpressions;

namespace TramTimes.Web.Jobs.Builders;

public static partial class RegexBuilder
{
    [GeneratedRegex(pattern: "_country-gb_session-([A-Za-z0-9]+)_lifetime-5m")] public static partial Regex GetSession();
}
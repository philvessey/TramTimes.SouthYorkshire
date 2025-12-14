using TramTimes.Database.Jobs.Models;

namespace TramTimes.Database.Jobs.Tools;

public static class TransXChangeJourneyPatternTools
{
    public static TransXChangeJourneyPattern GetJourneyPattern(
        TransXChangeServices? services,
        string? reference) {

        #region build result

        var result = services?.Service?.StandardService?.JourneyPattern?
            .FirstOrDefault(predicate: pattern => pattern.Id == reference) ?? new TransXChangeJourneyPattern();

        #endregion

        return result;
    }

    public static List<TransXChangeJourneyPatternTimingLink> GetTimingLinks(
        TransXChangeJourneyPatternSections? patternSections,
        List<string>? references) {

        #region build results

        var results = references?
            .SelectMany(selector: reference => patternSections?.JourneyPatternSection?
                .FirstOrDefault(predicate: pattern => pattern.Id == reference)?.JourneyPatternTimingLink ?? [])
            .ToList() ?? [];

        #endregion

        return results;
    }
}
using TramTimes.Database.Jobs.Models;

namespace TramTimes.Database.Jobs.Tools;

public static class TransXChangeJourneyPatternTools
{
    public static async Task<TransXChangeJourneyPattern> GetJourneyPatternAsync(
        TransXChangeServices? services,
        string? reference) {
        
        return await Task.FromResult(result: services?.Service?.StandardService?.JourneyPattern?
            .FirstOrDefault(predicate: pattern => pattern.Id == reference) ?? new TransXChangeJourneyPattern());
    }
    
    public static async Task<List<TransXChangeJourneyPatternTimingLink>> GetTimingLinksAsync(
        TransXChangeJourneyPatternSections? patternSections,
        List<string>? references) {
        
        return await Task.FromResult(result: references?
            .SelectMany(selector: reference => patternSections?.JourneyPatternSection?
                .FirstOrDefault(predicate: pattern => pattern.Id == reference)?.JourneyPatternTimingLink ?? [])
            .ToList() ?? []);
    }
}
using TramTimes.Database.Jobs.Models;

namespace TramTimes.Database.Jobs.Tools;

public static class TravelineScheduleTools
{
    public static async Task<bool> GetDuplicateMatchAsync(
        Dictionary<string, TravelineSchedule> schedules,
        List<TravelineStopPoint>? stopPoints,
        List<DateTime>? runningDates,
        List<DateTime>? supplementRunningDates,
        List<DateTime>? supplementNonRunningDates,
        string? direction,
        string? line) {
        
        var tasks = new List<Task<bool>>
        {
            TravelineCalendarRunningDateTools.GetDuplicateDatesAsync(
                schedules: schedules,
                stopPoints: stopPoints,
                dates: runningDates,
                direction: direction,
                line: line),
            
            TravelineCalendarRunningDateTools.GetDuplicateDatesAsync(
                schedules: schedules,
                stopPoints: stopPoints,
                dates: supplementRunningDates,
                direction: direction,
                line: line),
            
            TravelineCalendarRunningDateTools.GetDuplicateDatesAsync(
                schedules: schedules,
                stopPoints: stopPoints,
                dates: supplementNonRunningDates,
                direction: direction,
                line: line),
            
            TravelineCalendarSupplementRunningDateTools.GetDuplicateDatesAsync(
                schedules: schedules,
                stopPoints: stopPoints,
                dates: runningDates,
                direction: direction,
                line: line),
            
            TravelineCalendarSupplementRunningDateTools.GetDuplicateDatesAsync(
                schedules: schedules,
                stopPoints: stopPoints,
                dates: supplementRunningDates,
                direction: direction,
                line: line),
            
            TravelineCalendarSupplementRunningDateTools.GetDuplicateDatesAsync(
                schedules: schedules,
                stopPoints: stopPoints,
                dates: supplementNonRunningDates,
                direction: direction,
                line: line),
            
            TravelineCalendarSupplementNonRunningDateTools.GetDuplicateDatesAsync(
                schedules: schedules,
                stopPoints: stopPoints,
                dates: runningDates,
                direction: direction,
                line: line),
            
            TravelineCalendarSupplementNonRunningDateTools.GetDuplicateDatesAsync(
                schedules: schedules,
                stopPoints: stopPoints,
                dates: supplementRunningDates,
                direction: direction,
                line: line),
            
            TravelineCalendarSupplementNonRunningDateTools.GetDuplicateDatesAsync(
                schedules: schedules,
                stopPoints: stopPoints,
                dates: supplementNonRunningDates,
                direction: direction,
                line: line)
        };
        
        var results = await Task.WhenAll(tasks: tasks);
        
        return await Task.FromResult(result: results
            .Any(predicate: result => result));
    }
}
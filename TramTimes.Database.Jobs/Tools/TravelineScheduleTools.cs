using TramTimes.Database.Jobs.Models;

namespace TramTimes.Database.Jobs.Tools;

public static class TravelineScheduleTools
{
    public static bool GetDuplicateMatch(
        Dictionary<string, TravelineSchedule> schedules,
        List<TravelineStopPoint>? stopPoints,
        List<DateTime>? runningDates,
        List<DateTime>? supplementRunningDates,
        List<DateTime>? supplementNonRunningDates,
        string? direction,
        string? line) {
        
        #region get running date duplicates
        
        var result = TravelineCalendarRunningDateTools.GetDuplicateDates(
            schedules: schedules,
            stopPoints: stopPoints,
            dates: runningDates,
            direction: direction,
            line: line);
        
        if (result)
            return true;
        
        result = TravelineCalendarRunningDateTools.GetDuplicateDates(
            schedules: schedules,
            stopPoints: stopPoints,
            dates: supplementRunningDates,
            direction: direction,
            line: line);
        
        if (result)
            return true;
        
        result = TravelineCalendarRunningDateTools.GetDuplicateDates(
            schedules: schedules,
            stopPoints: stopPoints,
            dates: supplementNonRunningDates,
            direction: direction,
            line: line);
        
        if (result)
            return true;
        
        #endregion
        
        #region get supplement running date duplicates
        
        result = TravelineCalendarSupplementRunningDateTools.GetDuplicateDates(
            schedules: schedules,
            stopPoints: stopPoints,
            dates: runningDates,
            direction: direction,
            line: line);
        
        if (result)
            return true;
        
        result = TravelineCalendarSupplementRunningDateTools.GetDuplicateDates(
            schedules: schedules,
            stopPoints: stopPoints,
            dates: supplementRunningDates,
            direction: direction,
            line: line);
        
        if (result)
            return true;
        
        result = TravelineCalendarSupplementRunningDateTools.GetDuplicateDates(
            schedules: schedules,
            stopPoints: stopPoints,
            dates: supplementNonRunningDates,
            direction: direction,
            line: line);
        
        if (result)
            return true;
        
        #endregion
        
        #region get supplement non running date duplicates
        
        result = TravelineCalendarSupplementNonRunningDateTools.GetDuplicateDates(
            schedules: schedules,
            stopPoints: stopPoints,
            dates: runningDates,
            direction: direction,
            line: line);
        
        if (result)
            return true;
        
        result = TravelineCalendarSupplementNonRunningDateTools.GetDuplicateDates(
            schedules: schedules,
            stopPoints: stopPoints,
            dates: supplementRunningDates,
            direction: direction,
            line: line);
        
        if (result)
            return true;
        
        result = TravelineCalendarSupplementNonRunningDateTools.GetDuplicateDates(
            schedules: schedules,
            stopPoints: stopPoints,
            dates: supplementNonRunningDates,
            direction: direction,
            line: line);
        
        #endregion
        
        return result;
    }
    
    public static string GetServiceDirection(string? direction)
    {
        #region get service direction
        
        var result = direction switch
        {
            "outbound" => "0",
            "inbound" => "1",
            "inboundAndOutbound" => "2",
            "circular" => "3",
            "clockwise" => "4",
            "anticlockwise" => "5",
            _ => "6"
        };
        
        #endregion
        
        return result;
    }
}
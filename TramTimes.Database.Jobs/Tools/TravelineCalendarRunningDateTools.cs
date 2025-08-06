using TramTimes.Database.Jobs.Models;

namespace TramTimes.Database.Jobs.Tools;

public static class TravelineCalendarRunningDateTools
{
    public static List<DateOnly> GetAllDates(
        DateOnly? startDate,
        DateOnly? endDate,
        bool? monday,
        bool? tuesday,
        bool? wednesday,
        bool? thursday,
        bool? friday,
        bool? saturday,
        bool? sunday) {
        
        #region check valid input
        
        if (!startDate.HasValue || !endDate.HasValue)
            return [];
        
        #endregion
        
        #region build results
        
        var results = new List<DateOnly>();
        
        while (startDate <= endDate)
        {
            switch (startDate.Value.DayOfWeek)
            {
                case DayOfWeek.Monday:
                {
                    if (monday.HasValue && monday.Value)
                        results.Add(item: startDate.Value);
                    
                    break;
                }
                case DayOfWeek.Tuesday:
                {
                    if (tuesday.HasValue && tuesday.Value)
                        results.Add(item: startDate.Value);
                    
                    break;
                }
                case DayOfWeek.Wednesday:
                {
                    if (wednesday.HasValue && wednesday.Value)
                        results.Add(item: startDate.Value);
                    
                    break;
                }
                case DayOfWeek.Thursday:
                {
                    if (thursday.HasValue && thursday.Value)
                        results.Add(item: startDate.Value);
                    
                    break;
                }
                case DayOfWeek.Friday:
                {
                    if (friday.HasValue && friday.Value)
                        results.Add(item: startDate.Value);
                    
                    break;
                }
                case DayOfWeek.Saturday:
                {
                    if (saturday.HasValue && saturday.Value)
                        results.Add(item: startDate.Value);
                    
                    break;
                }
                case DayOfWeek.Sunday:
                {
                    if (sunday.HasValue && sunday.Value)
                        results.Add(item: startDate.Value);
                    
                    break;
                }
                default:
                {
                    if (!results.Contains(value: startDate.Value))
                        results.Add(item: startDate.Value);
                    
                    break;
                }
            }
            
            startDate = startDate.Value.AddDays(value: 1);
        }
        
        #endregion
        
        return results
            .Distinct()
            .OrderBy(keySelector: date => date)
            .ToList();
    }
    
    public static bool GetDuplicateDates(
        Dictionary<string, TravelineSchedule> schedules,
        List<TravelineStopPoint>? stopPoints,
        List<DateOnly>? dates,
        string? direction,
        string? line) {
        
        #region build results
        
        var results = schedules.Values.Where(predicate: schedule =>
            schedule.Calendar is { RunningDates: not null } && dates is not null && direction is not null && line is not null &&
            schedule.Calendar.RunningDates.Intersect(second: dates).Any() &&
            schedule.Direction == direction &&
            schedule.Line == line);
        
        #endregion
        
        return results
            .Where(predicate: schedule =>
                schedule.StopPoints?.FirstOrDefault()?.AtcoCode == stopPoints?.FirstOrDefault()?.AtcoCode &&
                schedule.StopPoints?.FirstOrDefault()?.DepartureTime == stopPoints?.FirstOrDefault()?.DepartureTime)
            .Any(predicate: schedule =>
                schedule.StopPoints?.LastOrDefault()?.AtcoCode == stopPoints?.LastOrDefault()?.AtcoCode &&
                schedule.StopPoints?.LastOrDefault()?.ArrivalTime == stopPoints?.LastOrDefault()?.ArrivalTime);
    }
}
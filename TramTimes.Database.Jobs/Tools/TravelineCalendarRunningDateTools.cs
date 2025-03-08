using TramTimes.Database.Jobs.Models;

namespace TramTimes.Database.Jobs.Tools;

public static class TravelineCalendarRunningDateTools
{
    public static async Task<List<DateTime>> GetAllDatesAsync(
        DateTime? startDate,
        DateTime? endDate,
        bool? monday,
        bool? tuesday,
        bool? wednesday,
        bool? thursday,
        bool? friday,
        bool? saturday,
        bool? sunday) {
        
        if (!startDate.HasValue || !endDate.HasValue)
            return await Task.FromResult(result: new List<DateTime>());
        
        if (!monday.HasValue || !tuesday.HasValue || !wednesday.HasValue || !thursday.HasValue || !friday.HasValue)
            return await Task.FromResult(result: new List<DateTime>());
        
        if (!saturday.HasValue || !sunday.HasValue)
            return await Task.FromResult(result: new List<DateTime>());
        
        var results = new List<DateTime>();
        
        while (startDate <= endDate)
        {
            switch (startDate.Value.DayOfWeek)
            {
                case DayOfWeek.Monday:
                {
                    if (monday.Value)
                        results.Add(item: startDate.Value);
                    
                    break;
                }
                case DayOfWeek.Tuesday:
                {
                    if (tuesday.Value)
                        results.Add(item: startDate.Value);
                    
                    break;
                }
                case DayOfWeek.Wednesday:
                {
                    if (wednesday.Value)
                        results.Add(item: startDate.Value);
                    
                    break;
                }
                case DayOfWeek.Thursday:
                {
                    if (thursday.Value)
                        results.Add(item: startDate.Value);
                    
                    break;
                }
                case DayOfWeek.Friday:
                {
                    if (friday.Value)
                        results.Add(item: startDate.Value);
                    
                    break;
                }
                case DayOfWeek.Saturday:
                {
                    if (saturday.Value)
                        results.Add(item: startDate.Value);
                    
                    break;
                }
                case DayOfWeek.Sunday:
                {
                    if (sunday.Value)
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
        
        return await Task.FromResult(result: results
            .Distinct()
            .OrderBy(keySelector: date => date)
            .ToList());
    }
    
    public static async Task<bool> GetDuplicateDatesAsync(
        Dictionary<string, TravelineSchedule> schedules,
        List<TravelineStopPoint>? stopPoints,
        List<DateTime>? dates,
        string? direction,
        string? line) {
        
        var results = schedules.Values
            .Where(predicate: schedule =>
                schedule.Calendar is { RunningDates: not null } && dates != null && direction != null && line != null &&
                schedule.Calendar.RunningDates.Intersect(second: dates).Any() &&
                schedule.Direction == direction &&
                schedule.Line == line);
        
        return await Task.FromResult(result: results
            .Where(predicate: schedule =>
                schedule.StopPoints?.FirstOrDefault()?.AtcoCode == stopPoints?.FirstOrDefault()?.AtcoCode &&
                schedule.StopPoints?.FirstOrDefault()?.DepartureTime == stopPoints?.FirstOrDefault()?.DepartureTime)
            .Any(predicate: schedule =>
                schedule.StopPoints?.LastOrDefault()?.AtcoCode == stopPoints?.LastOrDefault()?.AtcoCode &&
                schedule.StopPoints?.LastOrDefault()?.ArrivalTime == stopPoints?.LastOrDefault()?.ArrivalTime)
        );
    }
}
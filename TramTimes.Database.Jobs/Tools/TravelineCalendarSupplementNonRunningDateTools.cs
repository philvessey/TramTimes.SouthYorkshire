using TramTimes.Database.Jobs.Extensions;
using TramTimes.Database.Jobs.Models;

namespace TramTimes.Database.Jobs.Tools;

public static class TravelineCalendarSupplementNonRunningDateTools
{
    public static async Task<List<DateTime>> GetAllDatesAsync(
        DateTime scheduleDate,
        TransXChangeOperatingProfile? operatingProfile,
        DateTime? startDate,
        DateTime? endDate,
        bool? monday,
        bool? tuesday,
        bool? wednesday,
        bool? thursday,
        bool? friday,
        bool? saturday,
        bool? sunday,
        List<DateTime>? dates) {
        
        if (!startDate.HasValue ||
            !endDate.HasValue ||
            !monday.HasValue ||
            !tuesday.HasValue ||
            !wednesday.HasValue ||
            !thursday.HasValue ||
            !friday.HasValue ||
            !saturday.HasValue ||
            !sunday.HasValue) {
            
            return await Task.FromResult(result: new List<DateTime>());
        }
        
        var daysOfNonOperation = operatingProfile?.SpecialDaysOperation?.DaysOfNonOperation;
        
        startDate = await DateTimeTools.GetProfileStartDateAsync(
            scheduleDate: scheduleDate,
            startDate: daysOfNonOperation?.DateRange?.StartDate.ToDate());
        
        endDate = await DateTimeTools.GetProfileEndDateAsync(
            scheduleDate: scheduleDate,
            endDate: daysOfNonOperation?.DateRange?.EndDate.ToDate());
        
        var results = await TransXChangeDaysOfNonOperationTools.GetAllHolidaysAsync(
            daysOfNonOperation: daysOfNonOperation,
            startDate: startDate,
            endDate: endDate);
        
        while (startDate <= endDate)
        {
            switch (startDate.Value.DayOfWeek)
            {
                case DayOfWeek.Monday:
                {
                    if (dates?.Contains(value: startDate.Value) == true && monday.Value)
                        results.Add(item: startDate.Value);
                    
                    break;
                }
                case DayOfWeek.Tuesday:
                {
                    if (dates?.Contains(value: startDate.Value) == true && tuesday.Value)
                        results.Add(item: startDate.Value);
                    
                    break;
                }
                case DayOfWeek.Wednesday:
                {
                    if (dates?.Contains(value: startDate.Value) == true && wednesday.Value)
                        results.Add(item: startDate.Value);
                    
                    break;
                }
                case DayOfWeek.Thursday:
                {
                    if (dates?.Contains(value: startDate.Value) == true && thursday.Value)
                        results.Add(item: startDate.Value);
                    
                    break;
                }
                case DayOfWeek.Friday:
                {
                    if (dates?.Contains(value: startDate.Value) == true && friday.Value)
                        results.Add(item: startDate.Value);
                    
                    break;
                }
                case DayOfWeek.Saturday:
                {
                    if (dates?.Contains(value: startDate.Value) == true && saturday.Value)
                        results.Add(item: startDate.Value);
                    
                    break;
                }
                case DayOfWeek.Sunday:
                {
                    if (dates?.Contains(value: startDate.Value) == true && sunday.Value)
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
                schedule.Calendar is { SupplementNonRunningDates: not null } && dates != null && direction != null && line != null &&
                schedule.Calendar.SupplementNonRunningDates.Intersect(second: dates).Any() &&
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
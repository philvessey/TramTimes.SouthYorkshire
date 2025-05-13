using TramTimes.Database.Jobs.Extensions;
using TramTimes.Database.Jobs.Models;

namespace TramTimes.Database.Jobs.Tools;

public static class GtfsCalendarTools
{
    public static async Task<Dictionary<string, GtfsCalendar>> GetFromSchedulesAsync(Dictionary<string, TravelineSchedule> schedules)
    {
        var results = new Dictionary<string, GtfsCalendar>();
        
        foreach (var item in schedules.Values)
        {
            GtfsCalendar calendar = new()
            {
                Monday = item.Calendar is { Monday: not null } ? item.Calendar.Monday.ToInt().ToString() : "0",
                Tuesday = item.Calendar is { Tuesday: not null } ? item.Calendar.Tuesday.ToInt().ToString() : "0",
                Wednesday = item.Calendar is { Wednesday: not null } ? item.Calendar.Wednesday.ToInt().ToString() : "0",
                Thursday = item.Calendar is { Thursday: not null } ? item.Calendar.Thursday.ToInt().ToString() : "0",
                Friday = item.Calendar is { Friday: not null } ? item.Calendar.Friday.ToInt().ToString() : "0",
                Saturday = item.Calendar is { Saturday: not null } ? item.Calendar.Saturday.ToInt().ToString() : "0",
                Sunday = item.Calendar is { Sunday: not null } ? item.Calendar.Sunday.ToInt().ToString() : "0",
                
                StartDate = $"{item.Calendar?.StartDate?.ToString(format: "yyyy")}" +
                            $"{item.Calendar?.StartDate?.ToString(format: "MM")}" +
                            $"{item.Calendar?.StartDate?.ToString(format: "dd")}",
                
                EndDate = $"{item.Calendar?.EndDate?.ToString(format: "yyyy")}" +
                          $"{item.Calendar?.EndDate?.ToString(format: "MM")}" +
                          $"{item.Calendar?.EndDate?.ToString(format: "dd")}"
            };
            
            if (item.Calendar is { StartDate: not null, EndDate: not null })
            {
                calendar.ServiceId = $"{item.ServiceCode}" +
                                     $"-" +
                                     $"{item.Calendar?.StartDate:yyyy}" +
                                     $"{item.Calendar?.StartDate:MM}" +
                                     $"{item.Calendar?.StartDate:dd}" +
                                     $"-" +
                                     $"{item.Calendar?.EndDate:yyyy}" +
                                     $"{item.Calendar?.EndDate:MM}" +
                                     $"{item.Calendar?.EndDate:dd}" +
                                     $"-" +
                                     $"{item.Calendar?.Monday.ToInt().ToString()}" +
                                     $"{item.Calendar?.Tuesday.ToInt().ToString()}" +
                                     $"{item.Calendar?.Wednesday.ToInt().ToString()}" +
                                     $"{item.Calendar?.Thursday.ToInt().ToString()}" +
                                     $"{item.Calendar?.Friday.ToInt().ToString()}" +
                                     $"{item.Calendar?.Saturday.ToInt().ToString()}" +
                                     $"{item.Calendar?.Sunday.ToInt().ToString()}";
            }
            
            if (calendar.ServiceId is not null)
                results.TryAdd(
                    key: calendar.ServiceId,
                    value: calendar);
        }
        
        return await Task.FromResult(results
            .OrderBy(keySelector: calendar => calendar.Value.ServiceId)
            .ToDictionary(
                keySelector: calendar => calendar.Key,
                elementSelector: calendar => calendar.Value));
    }
}
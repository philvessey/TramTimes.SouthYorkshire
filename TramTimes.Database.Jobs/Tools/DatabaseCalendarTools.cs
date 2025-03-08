using TramTimes.Database.Jobs.Extensions;
using TramTimes.Database.Jobs.Models;

namespace TramTimes.Database.Jobs.Tools;

public static class DatabaseCalendarTools
{
    public static async Task<Dictionary<string, DatabaseCalendar>> GetFromSchedulesAsync(Dictionary<string, TravelineSchedule> schedules)
    {
        var results = new Dictionary<string, DatabaseCalendar>();
        
        foreach (var value in schedules.Values)
        {
            DatabaseCalendar calendar = new()
            {
                Monday = value.Calendar is { Monday: not null } ? value.Calendar.Monday.ToShort() : short.Parse(s: "0"),
                Tuesday = value.Calendar is { Tuesday: not null } ? value.Calendar.Tuesday.ToShort() : short.Parse(s: "0"),
                Wednesday = value.Calendar is { Wednesday: not null } ? value.Calendar.Wednesday.ToShort() : short.Parse(s: "0"),
                Thursday = value.Calendar is { Thursday: not null } ? value.Calendar.Thursday.ToShort() : short.Parse(s: "0"),
                Friday = value.Calendar is { Friday: not null } ? value.Calendar.Friday.ToShort() : short.Parse(s: "0"),
                Saturday = value.Calendar is { Saturday: not null } ? value.Calendar.Saturday.ToShort() : short.Parse(s: "0"),
                Sunday = value.Calendar is { Sunday: not null } ? value.Calendar.Sunday.ToShort() : short.Parse(s: "0"),
                StartDate = value.Calendar?.StartDate,
                EndDate = value.Calendar?.EndDate
            };
            
            if (value.Calendar is { StartDate: not null, EndDate: not null })
            {
                calendar.ServiceId = $"{value.ServiceCode}" +
                                     $"-" +
                                     $"{value.Calendar?.StartDate:yyyy}" +
                                     $"{value.Calendar?.StartDate:MM}" +
                                     $"{value.Calendar?.StartDate:dd}" +
                                     $"-" +
                                     $"{value.Calendar?.EndDate:yyyy}" +
                                     $"{value.Calendar?.EndDate:MM}" +
                                     $"{value.Calendar?.EndDate:dd}" +
                                     $"-" +
                                     $"{value.Calendar?.Monday.ToInt().ToString()}" +
                                     $"{value.Calendar?.Tuesday.ToInt().ToString()}" +
                                     $"{value.Calendar?.Wednesday.ToInt().ToString()}" +
                                     $"{value.Calendar?.Thursday.ToInt().ToString()}" +
                                     $"{value.Calendar?.Friday.ToInt().ToString()}" +
                                     $"{value.Calendar?.Saturday.ToInt().ToString()}" +
                                     $"{value.Calendar?.Sunday.ToInt().ToString()}";
            }
            
            if (calendar.ServiceId != null)
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
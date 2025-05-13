using TramTimes.Database.Jobs.Extensions;
using TramTimes.Database.Jobs.Models;

namespace TramTimes.Database.Jobs.Tools;

public static class DatabaseCalendarDateTools
{
    public static async Task<Dictionary<string, DatabaseCalendarDate>> GetFromSchedulesAsync(Dictionary<string, TravelineSchedule> schedules)
    {
        var results = new Dictionary<string, DatabaseCalendarDate>();
        
        foreach (var item in schedules.Values)
        {
            for (var i = 0; i < item.Calendar?.SupplementRunningDates?.Count; i++)
            {
                DatabaseCalendar calendar = new()
                {
                    Monday = item.Calendar is { Monday: not null } ? item.Calendar.Monday.ToShort() : short.Parse(s: "0"),
                    Tuesday = item.Calendar is { Tuesday: not null } ? item.Calendar.Tuesday.ToShort() : short.Parse(s: "0"),
                    Wednesday = item.Calendar is { Wednesday: not null } ? item.Calendar.Wednesday.ToShort() : short.Parse(s: "0"),
                    Thursday = item.Calendar is { Thursday: not null } ? item.Calendar.Thursday.ToShort() : short.Parse(s: "0"),
                    Friday = item.Calendar is { Friday: not null } ? item.Calendar.Friday.ToShort() : short.Parse(s: "0"),
                    Saturday = item.Calendar is { Saturday: not null } ? item.Calendar.Saturday.ToShort() : short.Parse(s: "0"),
                    Sunday = item.Calendar is { Sunday: not null } ? item.Calendar.Sunday.ToShort() : short.Parse(s: "0"),
                    StartDate = item.Calendar?.StartDate,
                    EndDate = item.Calendar?.EndDate
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
                
                DatabaseCalendarDate calendarDate = new()
                {
                    ServiceId = calendar.ServiceId,
                    ExceptionDate = item.Calendar?.SupplementRunningDates[i],
                    ExceptionType = short.Parse(s: "1")
                };
                
                results.TryAdd(
                    key: $"{calendarDate.ServiceId}-{calendarDate.ExceptionDate}",
                    value: calendarDate);
            }
            
            for (var i = 0; i < item.Calendar?.SupplementNonRunningDates?.Count; i++)
            {
                DatabaseCalendar calendar = new()
                {
                    Monday = item.Calendar is { Monday: not null } ? item.Calendar.Monday.ToShort() : short.Parse(s: "0"),
                    Tuesday = item.Calendar is { Tuesday: not null } ? item.Calendar.Tuesday.ToShort() : short.Parse(s: "0"),
                    Wednesday = item.Calendar is { Wednesday: not null } ? item.Calendar.Wednesday.ToShort() : short.Parse(s: "0"),
                    Thursday = item.Calendar is { Thursday: not null } ? item.Calendar.Thursday.ToShort() : short.Parse(s: "0"),
                    Friday = item.Calendar is { Friday: not null } ? item.Calendar.Friday.ToShort() : short.Parse(s: "0"),
                    Saturday = item.Calendar is { Saturday: not null } ? item.Calendar.Saturday.ToShort() : short.Parse(s: "0"),
                    Sunday = item.Calendar is { Sunday: not null } ? item.Calendar.Sunday.ToShort() : short.Parse(s: "0"),
                    StartDate = item.Calendar?.StartDate,
                    EndDate = item.Calendar?.EndDate
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
                
                DatabaseCalendarDate calendarDate = new()
                {
                    ServiceId = calendar.ServiceId,
                    ExceptionDate = item.Calendar?.SupplementNonRunningDates[i],
                    ExceptionType = short.Parse(s: "2")
                };
                
                results.TryAdd(
                    key: $"{calendarDate.ServiceId}-{calendarDate.ExceptionDate}",
                    value: calendarDate);
            }
        }
        
        return await Task.FromResult(result: results
            .OrderBy(keySelector: date => date.Value.ServiceId)
            .ToDictionary(
                keySelector: date => date.Key,
                elementSelector: date => date.Value));
    }
}
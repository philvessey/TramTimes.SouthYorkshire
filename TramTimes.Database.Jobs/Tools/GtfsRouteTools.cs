using TramTimes.Database.Jobs.Extensions;
using TramTimes.Database.Jobs.Models;

namespace TramTimes.Database.Jobs.Tools;

public static class GtfsRouteTools
{
    public static async Task<Dictionary<string, GtfsRoute>> GetFromSchedulesAsync(Dictionary<string, TravelineSchedule> schedules)
    {
        var results = new Dictionary<string, GtfsRoute>();
        
        foreach (var value in schedules.Values)
        {
            GtfsCalendar calendar = new()
            {
                Monday = value.Calendar is { Monday: not null } ? value.Calendar.Monday.ToInt().ToString() : "0",
                Tuesday = value.Calendar is { Tuesday: not null } ? value.Calendar.Tuesday.ToInt().ToString() : "0",
                Wednesday = value.Calendar is { Wednesday: not null } ? value.Calendar.Wednesday.ToInt().ToString() : "0",
                Thursday = value.Calendar is { Thursday: not null } ? value.Calendar.Thursday.ToInt().ToString() : "0",
                Friday = value.Calendar is { Friday: not null } ? value.Calendar.Friday.ToInt().ToString() : "0",
                Saturday = value.Calendar is { Saturday: not null } ? value.Calendar.Saturday.ToInt().ToString() : "0",
                Sunday = value.Calendar is { Sunday: not null } ? value.Calendar.Sunday.ToInt().ToString() : "0",
                
                StartDate = $"{value.Calendar?.StartDate?.ToString(format: "yyyy")}" +
                            $"{value.Calendar?.StartDate?.ToString(format: "MM")}" +
                            $"{value.Calendar?.StartDate?.ToString(format: "dd")}",
                
                EndDate = $"{value.Calendar?.EndDate?.ToString(format: "yyyy")}" +
                          $"{value.Calendar?.EndDate?.ToString(format: "MM")}" +
                          $"{value.Calendar?.EndDate?.ToString(format: "dd")}"
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
            
            GtfsRoute route = new()
            {
                RouteId = value.ServiceCode,
                AgencyId = value.OperatorCode,
                RouteShortName = value.Line,
                RouteLongName = value.Description,
                RouteType = value.Mode
            };
            
            if (route.RouteId != null)
                results.TryAdd(
                    key: route.RouteId,
                    value: route);
        }
        
        return await Task.FromResult(result: results
            .OrderBy(keySelector: route => route.Value.RouteId)
            .ToDictionary(
                keySelector: route => route.Key,
                elementSelector: route => route.Value));
    }
}
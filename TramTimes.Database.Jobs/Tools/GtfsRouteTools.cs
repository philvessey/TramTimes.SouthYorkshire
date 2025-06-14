using TramTimes.Database.Jobs.Extensions;
using TramTimes.Database.Jobs.Models;

namespace TramTimes.Database.Jobs.Tools;

public static class GtfsRouteTools
{
    public static Dictionary<string, GtfsRoute> GetFromSchedules(Dictionary<string, TravelineSchedule> schedules)
    {
        var results = new Dictionary<string, GtfsRoute>();
        
        foreach (var item in schedules.Values)
        {
            #region build calendar
            
            GtfsCalendar calendar = new()
            {
                Monday = item.Calendar is { Monday: not null }
                    ? item.Calendar.Monday.ToInt().ToString()
                    : "0",
                
                Tuesday = item.Calendar is { Tuesday: not null }
                    ? item.Calendar.Tuesday.ToInt().ToString()
                    : "0",
                
                Wednesday = item.Calendar is { Wednesday: not null }
                    ? item.Calendar.Wednesday.ToInt().ToString()
                    : "0",
                
                Thursday = item.Calendar is { Thursday: not null }
                    ? item.Calendar.Thursday.ToInt().ToString()
                    : "0",
                
                Friday = item.Calendar is { Friday: not null }
                    ? item.Calendar.Friday.ToInt().ToString()
                    : "0",
                
                Saturday = item.Calendar is { Saturday: not null }
                    ? item.Calendar.Saturday.ToInt().ToString()
                    : "0",
                
                Sunday = item.Calendar is { Sunday: not null }
                    ? item.Calendar.Sunday.ToInt().ToString()
                    : "0",
                
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
                                     $"{item.Calendar?.Monday.ToInt()}" +
                                     $"{item.Calendar?.Tuesday.ToInt()}" +
                                     $"{item.Calendar?.Wednesday.ToInt()}" +
                                     $"{item.Calendar?.Thursday.ToInt()}" +
                                     $"{item.Calendar?.Friday.ToInt()}" +
                                     $"{item.Calendar?.Saturday.ToInt()}" +
                                     $"{item.Calendar?.Sunday.ToInt()}";
            }
            
            #endregion
            
            #region build results
            
            GtfsRoute route = new()
            {
                RouteId = item.ServiceCode,
                AgencyId = item.OperatorCode,
                RouteShortName = item.Line,
                RouteLongName = item.Description,
                RouteType = item.Mode
            };
            
            if (route.RouteId is not null)
                results.TryAdd(
                    key: route.RouteId,
                    value: route);
            
            #endregion
        }
        
        return results
            .OrderBy(keySelector: route => route.Value.RouteId)
            .ToDictionary();
    }
}
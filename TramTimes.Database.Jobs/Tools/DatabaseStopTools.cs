using TramTimes.Database.Jobs.Extensions;
using TramTimes.Database.Jobs.Models;

namespace TramTimes.Database.Jobs.Tools;

public static class DatabaseStopTools
{
    public static Dictionary<string, DatabaseStop> GetFromSchedules(Dictionary<string, TravelineSchedule> schedules)
    {
        var results = new Dictionary<string, DatabaseStop>();
        
        foreach (var item in schedules.Values)
        {
            #region build calendar
            
            DatabaseCalendar calendar = new()
            {
                Monday = item.Calendar is { Monday: not null }
                    ? item.Calendar.Monday.ToShort()
                    : short.Parse(s: "0"),
                
                Tuesday = item.Calendar is { Tuesday: not null }
                    ? item.Calendar.Tuesday.ToShort()
                    : short.Parse(s: "0"),
                
                Wednesday = item.Calendar is { Wednesday: not null }
                    ? item.Calendar.Wednesday.ToShort()
                    : short.Parse(s: "0"),
                
                Thursday = item.Calendar is { Thursday: not null }
                    ? item.Calendar.Thursday.ToShort()
                    : short.Parse(s: "0"),
                
                Friday = item.Calendar is { Friday: not null }
                    ? item.Calendar.Friday.ToShort()
                    : short.Parse(s: "0"),
                
                Saturday = item.Calendar is { Saturday: not null }
                    ? item.Calendar.Saturday.ToShort()
                    : short.Parse(s: "0"),
                
                Sunday = item.Calendar is { Sunday: not null }
                    ? item.Calendar.Sunday.ToShort()
                    : short.Parse(s: "0"),
                
                StartDate = item.Calendar?.StartDate,
                EndDate = item.Calendar?.EndDate
            };
            
            if (item.Calendar is { StartDate: not null, EndDate: not null })
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
            
            #endregion
            
            #region build results
            
            for (var i = 0; i < item.StopPoints?.Count; i++)
            {
                DatabaseStop stop = new()
                {
                    StopId = item.StopPoints.ElementAt(index: i).NaptanStop?.AtcoCode,
                    StopCode = item.StopPoints.ElementAt(index: i).NaptanStop?.NaptanCode,
                    StopName = item.StopPoints.ElementAt(index: i).NaptanStop?.CommonName,
                    StopDesc = item.StopPoints.ElementAt(index: i).NaptanStop?.LocalityName,
                    StopLat = float.Parse(s: item.StopPoints.ElementAt(index: i).NaptanStop?.Latitude ?? string.Empty),
                    StopLon = float.Parse(s: item.StopPoints.ElementAt(index: i).NaptanStop?.Longitude ?? string.Empty),
                    LocationType = "0",
                    StopTimezone = "Europe/London",
                    WheelchairBoarding = "1",
                    PlatformCode = item.StopPoints.ElementAt(index: i).NaptanStop?.AtcoCode?[^1..]
                };
                
                if (string.IsNullOrWhiteSpace(value: stop.StopId))
                    stop.StopId = item.StopPoints.ElementAt(index: i).TravelineStop?.NaptanCode;
                
                if (string.IsNullOrWhiteSpace(value: stop.StopName))
                    stop.StopName = item.StopPoints.ElementAt(index: i).TravelineStop?.CommonName;
                
                if (string.IsNullOrWhiteSpace(value: stop.StopDesc))
                    stop.StopDesc = item.StopPoints.ElementAt(index: i).TravelineStop?.LocalityName;
                
                if (string.IsNullOrWhiteSpace(value: stop.StopLat.ToString()))
                    stop.StopLat = float.Parse(s: item.StopPoints.ElementAt(index: i).TravelineStop?.Latitude ?? string.Empty);
                
                if (string.IsNullOrWhiteSpace(value: stop.StopLon.ToString()))
                    stop.StopLon = float.Parse(s: item.StopPoints.ElementAt(index: i).TravelineStop?.Longitude ?? string.Empty);
                
                if (string.IsNullOrWhiteSpace(value: stop.PlatformCode))
                    stop.PlatformCode = item.StopPoints.ElementAt(index: i).TravelineStop?.AtcoCode?[^1..];
                
                if (stop.StopId is not null)
                    results.TryAdd(
                        key: stop.StopId,
                        value: stop);
            }
            
            #endregion
        }
        
        return results
            .OrderBy(keySelector: stop => stop.Value.StopId)
            .ToDictionary();
    }
}
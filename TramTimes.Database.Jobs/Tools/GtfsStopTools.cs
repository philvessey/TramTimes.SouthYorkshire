using TramTimes.Database.Jobs.Extensions;
using TramTimes.Database.Jobs.Models;

namespace TramTimes.Database.Jobs.Tools;

public static class GtfsStopTools
{
    public static async Task<Dictionary<string, GtfsStop>> GetFromSchedulesAsync(Dictionary<string, TravelineSchedule> schedules)
    {
        var results = new Dictionary<string, GtfsStop>();
        
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
            
            for (var i = 0; i < value.StopPoints?.Count; i++)
            {
                GtfsStop stop = new()
                {
                    StopId = value.StopPoints[i].NaptanStop?.AtcoCode,
                    StopCode = value.StopPoints[i].NaptanStop?.NaptanCode,
                    StopName = value.StopPoints[i].NaptanStop?.CommonName,
                    StopDesc = value.StopPoints[i].NaptanStop?.LocalityName,
                    StopLat = value.StopPoints[i].NaptanStop?.Latitude,
                    StopLon = value.StopPoints[i].NaptanStop?.Longitude,
                    LocationType = "0",
                    StopTimezone = "Europe/London",
                    WheelchairBoarding = "1",
                    PlatformCode = value.StopPoints[i].NaptanStop?.AtcoCode?[^1..]
                };
                
                if (string.IsNullOrWhiteSpace(value: stop.StopId))
                    stop.StopId = value.StopPoints[i].TravelineStop?.NaptanCode;
                
                if (string.IsNullOrWhiteSpace(value: stop.StopName))
                    stop.StopName = value.StopPoints[i].TravelineStop?.CommonName;
                
                if (string.IsNullOrWhiteSpace(value: stop.StopDesc))
                    stop.StopDesc = value.StopPoints[i].TravelineStop?.LocalityName;
                
                if (string.IsNullOrWhiteSpace(value: stop.StopLat))
                    stop.StopLat = value.StopPoints[i].TravelineStop?.Latitude;
                
                if (string.IsNullOrWhiteSpace(value: stop.StopLon))
                    stop.StopLon = value.StopPoints[i].TravelineStop?.Longitude;
                
                if (string.IsNullOrWhiteSpace(value: stop.PlatformCode))
                    stop.PlatformCode = value.StopPoints[i].TravelineStop?.AtcoCode?[^1..];
                
                if (stop.StopId != null)
                    results.TryAdd(
                        key: stop.StopId,
                        value: stop);
            }
        }
        
        return await Task.FromResult(results
            .OrderBy(keySelector: stop => stop.Value.StopId)
            .ToDictionary(
                keySelector: stop => stop.Key,
                elementSelector: stop => stop.Value));
    }
}
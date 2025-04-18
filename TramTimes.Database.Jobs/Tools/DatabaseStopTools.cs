using TramTimes.Database.Jobs.Extensions;
using TramTimes.Database.Jobs.Models;

namespace TramTimes.Database.Jobs.Tools;

public static class DatabaseStopTools
{
    public static async Task<Dictionary<string, DatabaseStop>> GetFromSchedulesAsync(Dictionary<string, TravelineSchedule> schedules)
    {
        var results = new Dictionary<string, DatabaseStop>();
        
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
            
            for (var i = 0; i < value.StopPoints?.Count; i++)
            {
                DatabaseStop stop = new()
                {
                    StopId = value.StopPoints[i].NaptanStop?.AtcoCode,
                    StopCode = value.StopPoints[i].NaptanStop?.NaptanCode,
                    StopName = value.StopPoints[i].NaptanStop?.CommonName,
                    StopDesc = value.StopPoints[i].NaptanStop?.LocalityName,
                    StopLat = float.Parse(s: value.StopPoints[i].NaptanStop?.Latitude ?? string.Empty),
                    StopLon = float.Parse(s: value.StopPoints[i].NaptanStop?.Longitude ?? string.Empty),
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
                
                if (string.IsNullOrWhiteSpace(value: stop.StopLat.ToString()))
                    stop.StopLat = float.Parse(s: value.StopPoints[i].TravelineStop?.Latitude ?? string.Empty);
                
                if (string.IsNullOrWhiteSpace(value: stop.StopLon.ToString()))
                    stop.StopLon = float.Parse(s: value.StopPoints[i].TravelineStop?.Longitude ?? string.Empty);
                
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
using TramTimes.Database.Jobs.Extensions;
using TramTimes.Database.Jobs.Models;

namespace TramTimes.Database.Jobs.Tools;

public static class DatabaseStopTimeTools
{
    public static async Task<Dictionary<string, DatabaseStopTime>> GetFromSchedulesAsync(Dictionary<string, TravelineSchedule> schedules)
    {
        var results = new Dictionary<string, DatabaseStopTime>();
        
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
            
            var timeSpan = TimeSpan.Zero;
            
            for (var i = 0; i < value.StopPoints?.Count; i++)
            {
                DatabaseStopTime stopTime = new()
                {
                    TripId = value.Id,
                    StopId = value.StopPoints[i].NaptanStop?.AtcoCode,
                    StopSequence = Convert.ToInt16(value: i + 1)
                };
                
                if (string.IsNullOrWhiteSpace(value: stopTime.StopId))
                    stopTime.StopId = value.StopPoints[i].TravelineStop?.AtcoCode;
                
                if (value.StopPoints[i].DepartureTime < timeSpan)
                {
                    stopTime.ArrivalTime = value.StopPoints[i].ArrivalTime.ToNextDay().ToString(format: @"hh\:mm\:ss");
                    stopTime.DepartureTime = value.StopPoints[i].DepartureTime.ToNextDay().ToString(format: @"hh\:mm\:ss");
                    
                    timeSpan = value.StopPoints[i].DepartureTime.ToNextDay();
                }
                else
                {
                    stopTime.ArrivalTime = value.StopPoints[i].ArrivalTime?.ToString(format: @"hh\:mm\:ss");
                    stopTime.DepartureTime = value.StopPoints[i].DepartureTime?.ToString(format: @"hh\:mm\:ss");
                    
                    timeSpan = value.StopPoints[i].DepartureTime ?? TimeSpan.Zero;
                }
                
                switch (value.StopPoints[i].Activity)
                {
                    case "pickUp":
                    {
                        stopTime.PickupType = "0";
                        stopTime.DropOffType = "1";
                        
                        break;
                    }
                    case "pickUpAndSetDown":
                    {
                        stopTime.PickupType = "0";
                        stopTime.DropOffType = "0";
                        
                        break;
                    }
                    case "setDown":
                    {
                        stopTime.PickupType = "1";
                        stopTime.DropOffType = "0";
                        
                        break;
                    }
                    default:
                    {
                        stopTime.PickupType = "1";
                        stopTime.DropOffType = "1";
                        
                        break;
                    }
                }
                
                results.TryAdd(
                    key: Guid.NewGuid().ToString(),
                    value: stopTime);
            }
        }
        
        return await Task.FromResult(results
            .OrderBy(keySelector: time => time.Value.TripId)
            .ToDictionary(
                keySelector: time => time.Key,
                elementSelector: time => time.Value));
    }
}
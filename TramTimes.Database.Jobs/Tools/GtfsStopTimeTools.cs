using TramTimes.Database.Jobs.Extensions;
using TramTimes.Database.Jobs.Models;

namespace TramTimes.Database.Jobs.Tools;

public static class GtfsStopTimeTools
{
    public static async Task<Dictionary<string, GtfsStopTime>> GetFromSchedulesAsync(Dictionary<string, TravelineSchedule> schedules)
    {
        var results = new Dictionary<string, GtfsStopTime>();
        
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
            
            var timeSpan = TimeSpan.Zero;
            
            for (var i = 0; i < item.StopPoints?.Count; i++)
            {
                GtfsStopTime stopTime = new()
                {
                    TripId = item.Id,
                    StopId = item.StopPoints[i].NaptanStop?.AtcoCode,
                    StopSequence = Convert.ToString(value: i + 1)
                };
                
                if (string.IsNullOrWhiteSpace(value: stopTime.StopId))
                    stopTime.StopId = item.StopPoints[i].TravelineStop?.AtcoCode;
                
                if (item.StopPoints[i].DepartureTime < timeSpan)
                {
                    stopTime.ArrivalTime = item.StopPoints[i].ArrivalTime.ToNextDay().ToString(format: @"hh\:mm\:ss");
                    stopTime.DepartureTime = item.StopPoints[i].DepartureTime.ToNextDay().ToString(format: @"hh\:mm\:ss");
                    
                    timeSpan = item.StopPoints[i].DepartureTime.ToNextDay();
                }
                else
                {
                    stopTime.ArrivalTime = item.StopPoints[i].ArrivalTime?.ToString(format: @"hh\:mm\:ss");
                    stopTime.DepartureTime = item.StopPoints[i].DepartureTime?.ToString(format: @"hh\:mm\:ss");
                    
                    timeSpan = item.StopPoints[i].DepartureTime ?? TimeSpan.Zero;
                }
                
                switch (item.StopPoints[i].Activity)
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
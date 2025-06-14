using TramTimes.Database.Jobs.Extensions;
using TramTimes.Database.Jobs.Models;

namespace TramTimes.Database.Jobs.Tools;

public static class GtfsStopTimeTools
{
    public static Dictionary<string, GtfsStopTime> GetFromSchedules(Dictionary<string, TravelineSchedule> schedules)
    {
        var results = new Dictionary<string, GtfsStopTime>();
        
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
            
            var timeSpan = TimeSpan.Zero;
            
            for (var i = 0; i < item.StopPoints?.Count; i++)
            {
                GtfsStopTime stopTime = new()
                {
                    TripId = item.Id,
                    StopId = item.StopPoints.ElementAt(index: i).NaptanStop?.AtcoCode,
                    StopSequence = Convert.ToString(value: i + 1)
                };
                
                if (string.IsNullOrWhiteSpace(value: stopTime.StopId))
                    stopTime.StopId = item.StopPoints.ElementAt(index: i).TravelineStop?.AtcoCode;
                
                if (item.StopPoints.ElementAt(index: i).DepartureTime < timeSpan)
                {
                    var arrivalTime = item.StopPoints.ElementAt(index: i).ArrivalTime.ToNextDay();
                    var departureTime = item.StopPoints.ElementAt(index: i).DepartureTime.ToNextDay();
                    
                    stopTime.ArrivalTime = arrivalTime.ToString(format: @"hh\:mm\:ss");
                    stopTime.DepartureTime = departureTime.ToString(format: @"hh\:mm\:ss");
                    
                    timeSpan = item.StopPoints.ElementAt(index: i).DepartureTime.ToNextDay();
                }
                else
                {
                    var arrivalTime = item.StopPoints.ElementAt(index: i).ArrivalTime ?? TimeSpan.Zero;
                    var departureTime = item.StopPoints.ElementAt(index: i).DepartureTime ?? TimeSpan.Zero;
                    
                    stopTime.ArrivalTime = arrivalTime.ToString(format: @"hh\:mm\:ss");
                    stopTime.DepartureTime = departureTime.ToString(format: @"hh\:mm\:ss");
                    
                    timeSpan = item.StopPoints.ElementAt(index: i).DepartureTime ?? TimeSpan.Zero;
                }
                
                switch (item.StopPoints.ElementAt(index: i).Activity)
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
                
                var guid = Guid.NewGuid();
                
                results.TryAdd(
                    key: guid.ToString(),
                    value: stopTime);
            }
            
            #endregion
        }
        
        return results
            .OrderBy(keySelector: time => time.Value.TripId)
            .ToDictionary();
    }
}
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
                monday = item.Calendar is { Monday: not null }
                    ? item.Calendar.Monday.ToInt().ToString()
                    : "0",

                tuesday = item.Calendar is { Tuesday: not null }
                    ? item.Calendar.Tuesday.ToInt().ToString()
                    : "0",

                wednesday = item.Calendar is { Wednesday: not null }
                    ? item.Calendar.Wednesday.ToInt().ToString()
                    : "0",

                thursday = item.Calendar is { Thursday: not null }
                    ? item.Calendar.Thursday.ToInt().ToString()
                    : "0",

                friday = item.Calendar is { Friday: not null }
                    ? item.Calendar.Friday.ToInt().ToString()
                    : "0",

                saturday = item.Calendar is { Saturday: not null }
                    ? item.Calendar.Saturday.ToInt().ToString()
                    : "0",

                sunday = item.Calendar is { Sunday: not null }
                    ? item.Calendar.Sunday.ToInt().ToString()
                    : "0",

                start_date = item.Calendar?.StartDate?.ToString(format: "yyyyMMdd"),
                end_date = item.Calendar?.EndDate?.ToString(format: "yyyyMMdd")
            };

            if (item.Calendar is { StartDate: not null, EndDate: not null })
                calendar.service_id = $"{item.ServiceCode}" +
                                      $"-" +
                                      $"{item.Calendar?.StartDate:yyyyMMdd}" +
                                      $"-" +
                                      $"{item.Calendar?.EndDate:yyyyMMdd}" +
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

            var timeSpan = TimeSpan.Zero;

            for (var i = 0; i < item.StopPoints?.Count; i++)
            {
                GtfsStopTime stopTime = new()
                {
                    trip_id = item.Id,
                    stop_id = item.StopPoints.ElementAt(index: i).NaptanStop?.AtcoCode,
                    stop_sequence = Convert.ToString(value: i + 1)
                };

                if (string.IsNullOrWhiteSpace(value: stopTime.stop_id))
                    stopTime.stop_id = item.StopPoints.ElementAt(index: i).TravelineStop?.AtcoCode;

                if (item.StopPoints.ElementAt(index: i).DepartureTime < timeSpan)
                {
                    var arrivalTime = item.StopPoints.ElementAt(index: i).ArrivalTime.ToNextDay();
                    var departureTime = item.StopPoints.ElementAt(index: i).DepartureTime.ToNextDay();

                    stopTime.arrival_time = arrivalTime.ToString(format: @"hh\:mm\:ss");
                    stopTime.departure_time = departureTime.ToString(format: @"hh\:mm\:ss");

                    timeSpan = item.StopPoints.ElementAt(index: i).DepartureTime.ToNextDay();
                }
                else
                {
                    var arrivalTime = item.StopPoints.ElementAt(index: i).ArrivalTime ?? TimeSpan.Zero;
                    var departureTime = item.StopPoints.ElementAt(index: i).DepartureTime ?? TimeSpan.Zero;

                    stopTime.arrival_time = arrivalTime.ToString(format: @"hh\:mm\:ss");
                    stopTime.departure_time = departureTime.ToString(format: @"hh\:mm\:ss");

                    timeSpan = item.StopPoints.ElementAt(index: i).DepartureTime ?? TimeSpan.Zero;
                }

                switch (item.StopPoints.ElementAt(index: i).Activity)
                {
                    case "pickUp":
                    {
                        stopTime.pickup_type = "0";
                        stopTime.drop_off_type = "1";

                        break;
                    }
                    case "pickUpAndSetDown":
                    {
                        stopTime.pickup_type = "0";
                        stopTime.drop_off_type = "0";

                        break;
                    }
                    case "setDown":
                    {
                        stopTime.pickup_type = "1";
                        stopTime.drop_off_type = "0";

                        break;
                    }
                    default:
                    {
                        stopTime.pickup_type = "1";
                        stopTime.drop_off_type = "1";

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
            .OrderBy(keySelector: time => time.Value.trip_id)
            .ToDictionary();
    }
}
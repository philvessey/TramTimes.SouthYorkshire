using TramTimes.Database.Jobs.Extensions;
using TramTimes.Database.Jobs.Models;

namespace TramTimes.Database.Jobs.Tools;

public static class DatabaseStopTimeTools
{
    public static Dictionary<string, DatabaseStopTime> GetFromSchedules(Dictionary<string, TravelineSchedule> schedules)
    {
        var results = new Dictionary<string, DatabaseStopTime>();

        foreach (var item in schedules.Values)
        {
            #region build calendar

            DatabaseCalendar calendar = new()
            {
                monday = item.Calendar is { Monday: not null }
                    ? item.Calendar.Monday.ToShort()
                    : short.Parse(s: "0"),

                tuesday = item.Calendar is { Tuesday: not null }
                    ? item.Calendar.Tuesday.ToShort()
                    : short.Parse(s: "0"),

                wednesday = item.Calendar is { Wednesday: not null }
                    ? item.Calendar.Wednesday.ToShort()
                    : short.Parse(s: "0"),

                thursday = item.Calendar is { Thursday: not null }
                    ? item.Calendar.Thursday.ToShort()
                    : short.Parse(s: "0"),

                friday = item.Calendar is { Friday: not null }
                    ? item.Calendar.Friday.ToShort()
                    : short.Parse(s: "0"),

                saturday = item.Calendar is { Saturday: not null }
                    ? item.Calendar.Saturday.ToShort()
                    : short.Parse(s: "0"),

                sunday = item.Calendar is { Sunday: not null }
                    ? item.Calendar.Sunday.ToShort()
                    : short.Parse(s: "0"),

                start_date = item.Calendar?.StartDate,
                end_date = item.Calendar?.EndDate
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
                DatabaseStopTime stopTime = new()
                {
                    trip_id = item.Id,
                    stop_id = item.StopPoints.ElementAt(index: i).NaptanStop?.AtcoCode,
                    stop_sequence = Convert.ToInt16(value: i + 1)
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
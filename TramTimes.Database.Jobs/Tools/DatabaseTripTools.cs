using TramTimes.Database.Jobs.Extensions;
using TramTimes.Database.Jobs.Models;

namespace TramTimes.Database.Jobs.Tools;

public static class DatabaseTripTools
{
    public static Dictionary<string, DatabaseTrip> GetFromSchedules(Dictionary<string, TravelineSchedule> schedules)
    {
        var results = new Dictionary<string, DatabaseTrip>();

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

            DatabaseTrip trip = new()
            {
                route_id = item.ServiceCode,
                service_id = calendar.service_id,
                trip_id = item.Id,
                trip_headsign = item.StopPoints?.LastOrDefault()?.NaptanStop?.CommonName,
                direction_id = short.Parse(s: item.Direction ?? string.Empty)
            };

            if (string.IsNullOrWhiteSpace(value: trip.trip_headsign))
                trip.trip_headsign = item.StopPoints?.LastOrDefault()?.TravelineStop?.CommonName;

            var guid = Guid.NewGuid();

            results.TryAdd(
                key: guid.ToString(),
                value: trip);

            #endregion
        }

        return results
            .OrderBy(keySelector: trip => trip.Value.trip_id)
            .ToDictionary();
    }
}
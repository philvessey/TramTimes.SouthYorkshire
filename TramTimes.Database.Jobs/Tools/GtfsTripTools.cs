using TramTimes.Database.Jobs.Extensions;
using TramTimes.Database.Jobs.Models;

namespace TramTimes.Database.Jobs.Tools;

public static class GtfsTripTools
{
    public static Dictionary<string, GtfsTrip> GetFromSchedules(Dictionary<string, TravelineSchedule> schedules)
    {
        var results = new Dictionary<string, GtfsTrip>();

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

                start_date = $"{item.Calendar?.StartDate?.ToString(format: "yyyy")}" +
                             $"{item.Calendar?.StartDate?.ToString(format: "MM")}" +
                             $"{item.Calendar?.StartDate?.ToString(format: "dd")}",

                end_date = $"{item.Calendar?.EndDate?.ToString(format: "yyyy")}" +
                           $"{item.Calendar?.EndDate?.ToString(format: "MM")}" +
                           $"{item.Calendar?.EndDate?.ToString(format: "dd")}"
            };

            if (item.Calendar is { StartDate: not null, EndDate: not null })
                calendar.service_id = $"{item.ServiceCode}" +
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

            GtfsTrip trip = new()
            {
                route_id = item.ServiceCode,
                service_id = calendar.service_id,
                trip_id = item.Id,
                trip_headsign = item.StopPoints?.LastOrDefault()?.NaptanStop?.CommonName,
                direction_id = item.Direction
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
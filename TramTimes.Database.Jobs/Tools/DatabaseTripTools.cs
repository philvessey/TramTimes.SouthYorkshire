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

            DatabaseTrip trip = new()
            {
                RouteId = item.ServiceCode,
                ServiceId = calendar.ServiceId,
                TripId = item.Id,
                TripHeadsign = item.StopPoints?.LastOrDefault()?.NaptanStop?.CommonName,
                DirectionId = short.Parse(s: item.Direction ?? string.Empty)
            };

            if (string.IsNullOrWhiteSpace(value: trip.TripHeadsign))
                trip.TripHeadsign = item.StopPoints?.LastOrDefault()?.TravelineStop?.CommonName;

            var guid = Guid.NewGuid();

            results.TryAdd(
                key: guid.ToString(),
                value: trip);

            #endregion
        }

        return results
            .OrderBy(keySelector: trip => trip.Value.TripId)
            .ToDictionary();
    }
}
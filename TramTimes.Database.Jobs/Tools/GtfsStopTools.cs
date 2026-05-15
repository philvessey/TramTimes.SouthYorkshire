using TramTimes.Database.Jobs.Extensions;
using TramTimes.Database.Jobs.Models;

namespace TramTimes.Database.Jobs.Tools;

public static class GtfsStopTools
{
    public static Dictionary<string, GtfsStop> GetFromSchedules(Dictionary<string, TravelineSchedule> schedules)
    {
        var results = new Dictionary<string, GtfsStop>();

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

            for (var i = 0; i < item.StopPoints?.Count; i++)
            {
                GtfsStop stop = new()
                {
                    stop_id = item.StopPoints.ElementAt(index: i).NaptanStop?.AtcoCode,
                    stop_code = item.StopPoints.ElementAt(index: i).NaptanStop?.NaptanCode,
                    stop_name = item.StopPoints.ElementAt(index: i).NaptanStop?.CommonName,
                    stop_desc = item.StopPoints.ElementAt(index: i).NaptanStop?.LocalityName,
                    stop_lat = item.StopPoints.ElementAt(index: i).NaptanStop?.Latitude,
                    stop_lon = item.StopPoints.ElementAt(index: i).NaptanStop?.Longitude,
                    location_type = "0",
                    stop_timezone = "Europe/London",
                    wheelchair_boarding = "1",
                    platform_code = item.StopPoints.ElementAt(index: i).NaptanStop?.AtcoCode?[^1..]
                };

                if (string.IsNullOrWhiteSpace(value: stop.stop_id))
                    stop.stop_id = item.StopPoints.ElementAt(index: i).TravelineStop?.NaptanCode;

                if (string.IsNullOrWhiteSpace(value: stop.stop_name))
                    stop.stop_name = item.StopPoints.ElementAt(index: i).TravelineStop?.CommonName;

                if (string.IsNullOrWhiteSpace(value: stop.stop_desc))
                    stop.stop_desc = item.StopPoints.ElementAt(index: i).TravelineStop?.LocalityName;

                if (string.IsNullOrWhiteSpace(value: stop.stop_lat))
                    stop.stop_lat = item.StopPoints.ElementAt(index: i).TravelineStop?.Latitude;

                if (string.IsNullOrWhiteSpace(value: stop.stop_lon))
                    stop.stop_lon = item.StopPoints.ElementAt(index: i).TravelineStop?.Longitude;

                if (string.IsNullOrWhiteSpace(value: stop.platform_code))
                    stop.platform_code = item.StopPoints.ElementAt(index: i).TravelineStop?.AtcoCode?[^1..];

                if (stop.stop_id is not null)
                    results.TryAdd(
                        key: stop.stop_id,
                        value: stop);
            }

            #endregion
        }

        return results
            .OrderBy(keySelector: stop => stop.Value.stop_id)
            .ToDictionary();
    }
}
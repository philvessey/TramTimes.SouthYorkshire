using TramTimes.Database.Jobs.Extensions;
using TramTimes.Database.Jobs.Models;

namespace TramTimes.Database.Jobs.Tools;

public static class GtfsCalendarDateTools
{
    public static Dictionary<string, GtfsCalendarDate> GetFromSchedules(Dictionary<string, TravelineSchedule> schedules)
    {
        var results = new Dictionary<string, GtfsCalendarDate>();

        foreach (var item in schedules.Values)
        {
            for (var i = 0; i < item.Calendar?.SupplementRunningDates?.Count; i++)
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

                GtfsCalendarDate calendarDate = new()
                {
                    service_id = calendar.service_id,
                    date = item.Calendar?.SupplementRunningDates.ElementAt(index: i).ToString(format: "yyyyMMdd"),
                    exception_type = "1"
                };

                results.TryAdd(
                    key: $"{calendarDate.service_id}-{calendarDate.date}",
                    value: calendarDate);

                #endregion
            }

            for (var i = 0; i < item.Calendar?.SupplementNonRunningDates?.Count; i++)
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

                GtfsCalendarDate calendarDate = new()
                {
                    service_id = calendar.service_id,
                    date = item.Calendar?.SupplementNonRunningDates.ElementAt(index: i).ToString(format: "yyyyMMdd"),
                    exception_type = "2"
                };

                results.TryAdd(
                    key: $"{calendarDate.service_id}-{calendarDate.date}",
                    value: calendarDate);

                #endregion
            }
        }

        return results
            .OrderBy(keySelector: date => date.Value.service_id)
            .ToDictionary();
    }
}
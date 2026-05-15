using TramTimes.Database.Jobs.Extensions;
using TramTimes.Database.Jobs.Models;

namespace TramTimes.Database.Jobs.Tools;

public static class DatabaseCalendarDateTools
{
    public static Dictionary<string, DatabaseCalendarDate> GetFromSchedules(Dictionary<string, TravelineSchedule> schedules)
    {
        var results = new Dictionary<string, DatabaseCalendarDate>();

        foreach (var item in schedules.Values)
        {
            for (var i = 0; i < item.Calendar?.SupplementRunningDates?.Count; i++)
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

                DatabaseCalendarDate calendarDate = new()
                {
                    service_id = calendar.service_id,
                    exception_date = item.Calendar?.SupplementRunningDates.ElementAt(index: i),
                    exception_type = short.Parse(s: "1")
                };

                results.TryAdd(
                    key: $"{calendarDate.service_id}-{calendarDate.exception_date}",
                    value: calendarDate);

                #endregion
            }

            for (var i = 0; i < item.Calendar?.SupplementNonRunningDates?.Count; i++)
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

                DatabaseCalendarDate calendarDate = new()
                {
                    service_id = calendar.service_id,
                    exception_date = item.Calendar?.SupplementNonRunningDates.ElementAt(index: i),
                    exception_type = short.Parse(s: "2")
                };

                results.TryAdd(
                    key: $"{calendarDate.service_id}-{calendarDate.exception_date}",
                    value: calendarDate);

                #endregion
            }
        }

        return results
            .OrderBy(keySelector: date => date.Value.service_id)
            .ToDictionary();
    }
}
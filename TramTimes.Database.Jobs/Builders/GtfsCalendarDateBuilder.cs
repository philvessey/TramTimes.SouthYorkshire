using System.Globalization;
using CsvHelper;
using TramTimes.Database.Jobs.Models;
using TramTimes.Database.Jobs.Tools;

namespace TramTimes.Database.Jobs.Builders;

public static class GtfsCalendarDateBuilder
{
    public static async Task<string> BuildAsync(
        Dictionary<string, TravelineSchedule> schedules,
        string path) {

        #region build calendar dates

        var calendarDates = GtfsCalendarDateTools.GetFromSchedules(schedules: schedules);

        #endregion

        #region build results

        StreamWriter writer = new(path: Path.Combine(
            path1: path,
            path2: "calendar_dates.txt"));

        CsvWriter csv = new(
            writer: writer,
            culture: CultureInfo.InvariantCulture);

        csv.WriteHeader<GtfsCalendarDate>();

        await csv.NextRecordAsync();
        await csv.WriteRecordsAsync(records: calendarDates.Values);
        await csv.FlushAsync();

        #endregion

        return Path.Combine(
            path1: path,
            path2: "calendar_dates.txt");
    }
}
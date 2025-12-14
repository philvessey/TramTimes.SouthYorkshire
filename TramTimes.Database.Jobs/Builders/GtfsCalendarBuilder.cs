using System.Globalization;
using CsvHelper;
using TramTimes.Database.Jobs.Models;
using TramTimes.Database.Jobs.Tools;

namespace TramTimes.Database.Jobs.Builders;

public static class GtfsCalendarBuilder
{
    public static async Task<string> BuildAsync(
        Dictionary<string, TravelineSchedule> schedules,
        string path) {

        #region build calendars

        var calendars = GtfsCalendarTools.GetFromSchedules(schedules: schedules);

        #endregion

        #region build results

        StreamWriter writer = new(path: Path.Combine(
            path1: path,
            path2: "calendar.txt"));

        CsvWriter csv = new(
            writer: writer,
            culture: CultureInfo.InvariantCulture);

        csv.WriteHeader<GtfsCalendar>();

        await csv.NextRecordAsync();
        await csv.WriteRecordsAsync(records: calendars.Values);
        await csv.FlushAsync();

        #endregion

        return Path.Combine(
            path1: path,
            path2: "calendar.txt");
    }
}
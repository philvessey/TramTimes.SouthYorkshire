using Sylvan.Data;
using Sylvan.Data.Csv;
using TramTimes.Database.Jobs.Models;
using TramTimes.Database.Jobs.Tools;

namespace TramTimes.Database.Jobs.Builders;

public static class GtfsStopBuilder
{
    public static async Task<string> BuildAsync(
        Dictionary<string, TravelineSchedule> schedules,
        string path) {

        #region build stops

        var stops = GtfsStopTools.GetFromSchedules(schedules: schedules);

        #endregion

        #region build results

        await using var streamWriter = new StreamWriter(path: Path.Combine(
            path1: path,
            path2: "stops.txt"));

        await using var dataWriter = CsvDataWriter.Create(writer: streamWriter);
        await dataWriter.WriteAsync(reader: stops.Values.AsDataReader());

        #endregion

        return Path.Combine(
            path1: path,
            path2: "stops.txt");
    }
}
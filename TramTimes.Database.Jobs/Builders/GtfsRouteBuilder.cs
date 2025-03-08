using System.Globalization;
using CsvHelper;
using TramTimes.Database.Jobs.Models;
using TramTimes.Database.Jobs.Tools;

namespace TramTimes.Database.Jobs.Builders;

public static class GtfsRouteBuilder
{
    public static async Task<string> BuildAsync(
        Dictionary<string, TravelineSchedule> schedules,
        string path) {
        
        var routes = await GtfsRouteTools.GetFromSchedulesAsync(schedules: schedules);
        
        StreamWriter writer = new(path: Path.Combine(
            path1: path,
            path2: "routes.txt"));
        
        CsvWriter csv = new(
            writer: writer,
            culture: CultureInfo.InvariantCulture);
        
        csv.WriteHeader<GtfsRoute>();
        
        await csv.NextRecordAsync();
        await csv.WriteRecordsAsync(records: routes.Values);
        await csv.FlushAsync();
        
        return await Task.FromResult(result: Path.Combine(
            path1: path,
            path2: "routes.txt"));
    }
}
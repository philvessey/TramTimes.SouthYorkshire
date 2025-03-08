using System.Globalization;
using CsvHelper;
using TramTimes.Database.Jobs.Models;
using TramTimes.Database.Jobs.Tools;

namespace TramTimes.Database.Jobs.Builders;

public static class GtfsAgencyBuilder
{
    public static async Task<string> BuildAsync(
        Dictionary<string, TravelineSchedule> schedules,
        string path) {
        
        var agencies = await GtfsAgencyTools.GetFromSchedulesAsync(schedules: schedules);
        
        StreamWriter writer = new(path: Path.Combine(
            path1: path,
            path2: "agency.txt"));
        
        CsvWriter csv = new(
            writer: writer,
            culture: CultureInfo.InvariantCulture);
        
        csv.WriteHeader<GtfsAgency>();
        
        await csv.NextRecordAsync();
        await csv.WriteRecordsAsync(records: agencies.Values);
        await csv.FlushAsync();
        
        return await Task.FromResult(result: Path.Combine(
            path1: path,
            path2: "agency.txt"));
    }
}
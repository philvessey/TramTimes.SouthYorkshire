using System.Globalization;
using CsvHelper;
using TramTimes.Database.Jobs.Models;

namespace TramTimes.Database.Jobs.Tools;

public static class NaptanLocalityTools
{
    public static async Task<Dictionary<string, NaptanLocality>> GetFromFileAsync(string path)
    {
        var results = new Dictionary<string, NaptanLocality>();
        
        var csv = new CsvReader(
            reader: new StreamReader(path: path),
            culture: CultureInfo.InvariantCulture);
        
        var records = csv.GetRecords<NaptanLocality>();
        
        foreach (var record in records)
        {
            if (record.NptgLocalityCode != null)
                results.TryAdd(
                    key: record.NptgLocalityCode,
                    value: record);
        }
        
        return await Task.FromResult(result: results);
    }
}
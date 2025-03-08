using System.Globalization;
using CsvHelper;
using TramTimes.Database.Jobs.Models;

namespace TramTimes.Database.Jobs.Tools;

public static class NaptanStopTools
{
    public static async Task <Dictionary<string, NaptanStop>> GetFromFileAsync(string path)
    {
        var results = new Dictionary<string, NaptanStop>();
        
        var csv = new CsvReader(
            reader: new StreamReader(path: path),
            culture: CultureInfo.InvariantCulture);
        
        var records = csv.GetRecords<NaptanStop>();
        
        foreach (var record in records)
        {
            if (record.AtcoCode != null)
                results.TryAdd(
                    key: record.AtcoCode,
                    value: record);
        }
        
        return await Task.FromResult(result: results);
    }
}
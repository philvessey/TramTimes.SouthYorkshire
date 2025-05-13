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
        
        foreach (var item in records)
        {
            if (item.AtcoCode is not null)
                results.TryAdd(
                    key: item.AtcoCode,
                    value: item);
        }
        
        return await Task.FromResult(result: results);
    }
}
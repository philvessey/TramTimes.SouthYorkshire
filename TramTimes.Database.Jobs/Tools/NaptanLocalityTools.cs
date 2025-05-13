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
        
        foreach (var item in records)
        {
            if (item.NptgLocalityCode is not null)
                results.TryAdd(
                    key: item.NptgLocalityCode,
                    value: item);
        }
        
        return await Task.FromResult(result: results);
    }
}
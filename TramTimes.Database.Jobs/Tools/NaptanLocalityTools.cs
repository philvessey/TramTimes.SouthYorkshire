using System.Globalization;
using CsvHelper;
using TramTimes.Database.Jobs.Models;

namespace TramTimes.Database.Jobs.Tools;

public static class NaptanLocalityTools
{
    public static Dictionary<string, NaptanLocality> GetFromFile(string path)
    {
        var results = new Dictionary<string, NaptanLocality>();
        
        #region get records
        
        var csv = new CsvReader(
            reader: new StreamReader(path: path),
            culture: CultureInfo.InvariantCulture);
        
        var records = csv.GetRecords<NaptanLocality>();
        
        #endregion
        
        #region build results
        
        foreach (var item in records)
            if (item.NptgLocalityCode is not null)
                results.TryAdd(
                    key: item.NptgLocalityCode,
                    value: item);
        
        #endregion
        
        return results;
    }
}
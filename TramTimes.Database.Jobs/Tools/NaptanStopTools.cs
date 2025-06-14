using System.Globalization;
using CsvHelper;
using TramTimes.Database.Jobs.Models;

namespace TramTimes.Database.Jobs.Tools;

public static class NaptanStopTools
{
    public static Dictionary<string, NaptanStop> GetFromFile(string path)
    {
        var results = new Dictionary<string, NaptanStop>();
        
        #region get records
        
        var csv = new CsvReader(
            reader: new StreamReader(path: path),
            culture: CultureInfo.InvariantCulture);
        
        var records = csv.GetRecords<NaptanStop>();
        
        #endregion
        
        #region build results
        
        foreach (var item in records)
            if (item.AtcoCode is not null)
                results.TryAdd(
                    key: item.AtcoCode,
                    value: item);
        
        #endregion
        
        return results;
    }
}
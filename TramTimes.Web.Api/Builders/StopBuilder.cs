namespace TramTimes.Web.Api.Builders;

public static class StopBuilder
{
    public static string[] Build(string path)
    {
        #region build stops
        
        var stops = File.ReadAllLines(path: path);
        
        #endregion
        
        #region build results
        
        var results = stops
            .Where(predicate: id => !string.IsNullOrWhiteSpace(value: id))
            .Select(selector: id => id.Trim())
            .ToArray();
        
        #endregion
        
        return results;
    }
}
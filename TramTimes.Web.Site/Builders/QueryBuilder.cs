namespace TramTimes.Web.Site.Builders;

public static class QueryBuilder
{
    private static readonly string Endpoint = Environment.GetEnvironmentVariable(variable: "API_ENDPOINT") ?? string.Empty;
    
    public static string GetIdFromDatabase(string id)
    {
        #region build result
        
        var result = $"{Endpoint}/database/stops/id/{id}";
        
        #endregion
        
        return result;
    }
    
    public static string GetIdFromSearch(string id)
    {
        #region build result
        
        var result = $"{Endpoint}/search/stops/id/{id}";
        
        #endregion
        
        return result;
    }
    
    public static string GetLocationFromDatabase(double[] extent)
    {
        #region build result
        
        var location = string.Join(
            separator: "/",
            values:
            [
                extent.ElementAt(index: 1),
                extent.ElementAt(index: 2),
                extent.ElementAt(index: 3),
                extent.ElementAt(index: 0)
            ]);
        
        var result = $"{Endpoint}/database/stops/location/{location}";
        
        #endregion
        
        return result;
    }
    
    public static string GetLocationFromSearch(double[] extent)
    {
        #region build result
        
        var location = string.Join(
            separator: "/",
            values:
            [
                extent.ElementAt(index: 1),
                extent.ElementAt(index: 2),
                extent.ElementAt(index: 3),
                extent.ElementAt(index: 0)
            ]);
        
        var result = $"{Endpoint}/search/stops/location/{location}";
        
        #endregion
        
        return result;
    }
    
    public static string GetNameFromDatabase(string name)
    {
        #region build result
        
        var result = $"{Endpoint}/database/stops/name/{name}";
        
        #endregion
        
        return result;
    }
    
    public static string GetNameFromSearch(string name)
    {
        #region build result
        
        var result = $"{Endpoint}/search/stops/name/{name}";
        
        #endregion
        
        return result;
    }
}
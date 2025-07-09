using TramTimes.Web.Site.Types;

namespace TramTimes.Web.Site.Builders;

public static class QueryBuilder
{
    private static readonly string Endpoint = Environment.GetEnvironmentVariable(variable: "API_ENDPOINT") ?? string.Empty;
    
    public static string GetServicesFromCache(
        QueryType type,
        string value) {
        
        #region build result
        
        var result = type == QueryType.TripId
            ? $"{Endpoint}/cache/services/trip/{value}"
            : $"{Endpoint}/cache/services/stop/{value}";
        
        #endregion
        
        return result;
    }
    
    public static string GetServicesFromDatabase(
        QueryType type,
        string value) {
        
        #region build result
        
        var result = type == QueryType.TripId
            ? $"{Endpoint}/database/services/trip/{value}"
            : $"{Endpoint}/database/services/stop/{value}";
        
        #endregion
        
        return result;
    }
    
    public static string GetStopsFromDatabase(
        QueryType type,
        string value) {
        
        #region build result
        
        var result = type == QueryType.StopName
            ? $"{Endpoint}/database/stops/name/{value}"
            : $"{Endpoint}/database/stops/id/{value}";
        
        #endregion
        
        return result;
    }
    
    public static string GetStopsFromDatabase(
        QueryType type,
        double[] value) {
        
        #region build result
        
        var result = type == QueryType.StopLocation
            ? $"{Endpoint}/database/stops/location/{string.Join(
                separator: "/",
                values:
                [
                    value.ElementAt(index: 1),
                    value.ElementAt(index: 2),
                    value.ElementAt(index: 3),
                    value.ElementAt(index: 0)
                ])}"
            : $"{Endpoint}/database/stops/point/{string.Join(
                separator: "/",
                values:
                [
                    value.ElementAt(index: 1),
                    value.ElementAt(index: 0)
                ])}";
        
        #endregion
        
        return result;
    }
    
    public static string GetStopsFromSearch(
        QueryType type,
        string value) {
        
        #region build result
        
        var result = type == QueryType.StopName
            ? $"{Endpoint}/search/stops/name/{value}"
            : $"{Endpoint}/search/stops/id/{value}";
        
        #endregion
        
        return result;
    }
    
    public static string GetStopsFromSearch(
        QueryType type,
        double[] value) {
        
        #region build result
        
        var result = type == QueryType.StopLocation
            ? $"{Endpoint}/search/stops/location/{string.Join(
                separator: "/",
                values:
                [
                    value.ElementAt(index: 1),
                    value.ElementAt(index: 2),
                    value.ElementAt(index: 3),
                    value.ElementAt(index: 0)
                ])}"
            : $"{Endpoint}/search/stops/point/{string.Join(
                separator: "/",
                values:
                [
                    value.ElementAt(index: 1),
                    value.ElementAt(index: 0)
                ])}";
        
        #endregion
        
        return result;
    }
}
using TramTimes.Web.Site.Types;

namespace TramTimes.Web.Site.Builders;

public static class QueryBuilder
{
    private static readonly string _endpoint = Environment.GetEnvironmentVariable(variable: "API_ENDPOINT") ?? string.Empty;

    public static string GetServicesFromCache(
        QueryType type,
        string value) {

        #region build result

        var result = type is QueryType.TripId
            ? $"{_endpoint}/cache/services/trip/{value}"
            : $"{_endpoint}/cache/services/stop/{value}";

        #endregion

        return result;
    }

    public static string GetServicesFromDatabase(
        QueryType type,
        string value) {

        #region build result

        var result = type is QueryType.TripId
            ? $"{_endpoint}/database/services/trip/{value}"
            : $"{_endpoint}/database/services/stop/{value}";

        #endregion

        return result;
    }

    public static string GetStopsFromDatabase(
        QueryType type,
        string value) {

        #region build result

        var result = type is QueryType.StopName
            ? $"{_endpoint}/database/stops/name/{value}"
            : $"{_endpoint}/database/stops/id/{value}";

        #endregion

        return result;
    }

    public static string GetStopsFromDatabase(
        QueryType type,
        double[] value) {

        #region build result

        var result = type is QueryType.StopLocation
            ? $"{_endpoint}/database/stops/location/{string.Join(
                separator: "/",
                values:
                [
                    value.ElementAt(index: 1),
                    value.ElementAt(index: 2),
                    value.ElementAt(index: 3),
                    value.ElementAt(index: 0)
                ])}"
            : $"{_endpoint}/database/stops/point/{string.Join(
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

        var result = type is QueryType.StopName
            ? $"{_endpoint}/search/stops/name/{value}"
            : $"{_endpoint}/search/stops/id/{value}";

        #endregion

        return result;
    }

    public static string GetStopsFromSearch(
        QueryType type,
        double[] value) {

        #region build result

        var result = type is QueryType.StopLocation
            ? $"{_endpoint}/search/stops/location/{string.Join(
                separator: "/",
                values:
                [
                    value.ElementAt(index: 1),
                    value.ElementAt(index: 2),
                    value.ElementAt(index: 3),
                    value.ElementAt(index: 0)
                ])}"
            : $"{_endpoint}/search/stops/point/{string.Join(
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
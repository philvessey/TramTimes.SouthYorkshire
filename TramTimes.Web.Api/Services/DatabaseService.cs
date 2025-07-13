using TramTimes.Web.Api.Handlers;

namespace TramTimes.Web.Api.Services;

public static class DatabaseService
{
    public static void MapDatabaseEndpoints(this IEndpointRouteBuilder application)
    {
        #region endpoint -> /database/services/stop/
        
        var builder = application.MapGet(
            pattern: "/database/services/stop/{id}",
            handler: DatabaseHandler.GetServicesByStopAsync);
        
        builder.WithSummary(summary: "Get Services from Postgres Database.");
        builder.WithTags(tags: "database");
        
        #endregion
        
        #region endpoint -> /database/services/trip/
        
        builder = application.MapGet(
            pattern: "/database/services/trip/{id}",
            handler: DatabaseHandler.GetServicesByTripAsync);
        
        builder.WithSummary(summary: "Get Services from Postgres Database.");
        builder.WithTags(tags: "database");
        
        #endregion
        
        #region endpoint -> /database/stops/id/
        
        builder = application.MapGet(
            pattern: "/database/stops/id/{id}",
            handler: DatabaseHandler.GetStopsByIdAsync);
        
        builder.WithSummary(summary: "Get Stops from Postgres Database.");
        builder.WithTags(tags: "database");
        
        #endregion
        
        #region endpoint -> /database/stops/code/
        
        builder = application.MapGet(
            pattern: "/database/stops/code/{code}",
            handler: DatabaseHandler.GetStopsByCodeAsync);
        
        builder.WithSummary(summary: "Get Stops from Postgres Database.");
        builder.WithTags(tags: "database");
        
        #endregion
        
        #region endpoint -> /database/stops/name/
        
        builder = application.MapGet(
            pattern: "/database/stops/name/{name}",
            handler: DatabaseHandler.GetStopsByNameAsync);
        
        builder.WithSummary(summary: "Get Stops from Postgres Database.");
        builder.WithTags(tags: "database");
        
        #endregion
        
        #region endpoint -> /database/stops/location/
        
        builder = application.MapGet(
            pattern: "/database/stops/location/{minLon:double}/{minLat:double}/{maxLon:double}/{maxLat:double}",
            handler: DatabaseHandler.GetStopsByLocationAsync);
        
        builder.WithSummary(summary: "Get Stops from Postgres Database.");
        builder.WithTags(tags: "database");
        
        #endregion
        
        #region endpoint -> /database/stops/point/
        
        builder = application.MapGet(
            pattern: "/database/stops/point/{lon:double}/{lat:double}",
            handler: DatabaseHandler.GetStopsByPointAsync);
        
        builder.WithSummary(summary: "Get Stops from Postgres Database.");
        builder.WithTags(tags: "database");
        
        #endregion
    }
}
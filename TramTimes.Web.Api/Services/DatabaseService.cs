using TramTimes.Web.Api.Handlers;

namespace TramTimes.Web.Api.Services;

public static class DatabaseService
{
    public static IEndpointRouteBuilder MapDatabaseEndpoints(this IEndpointRouteBuilder application)
    {
        var builder = application.MapGet(
            pattern: "/database/services/stop/{id}",
            handler: DatabaseHandler.GetServicesByStopAsync);
        
        builder.WithSummary(summary: "Get Services from Postgres Database");
        builder.WithTags(tags: "database");
        
        builder = application.MapGet(
            pattern: "/database/services/trip/{id}",
            handler: DatabaseHandler.GetServicesByTripAsync);
        
        builder.WithSummary(summary: "Get Services from Postgres Database");
        builder.WithTags(tags: "database");
        
        builder = application.MapGet(
            pattern: "/database/stops/id/{id}",
            handler: DatabaseHandler.GetStopsByIdAsync);
        
        builder.WithSummary(summary: "Get Stops from Postgres Database");
        builder.WithTags(tags: "database");
        
        builder = application.MapGet(
            pattern: "/database/stops/code/{code}",
            handler: DatabaseHandler.GetStopsByCodeAsync);
        
        builder.WithSummary(summary: "Get Stops from Postgres Database");
        builder.WithTags(tags: "database");
        
        builder = application.MapGet(
            pattern: "/database/stops/name/{name}",
            handler: DatabaseHandler.GetStopsByNameAsync);
        
        builder.WithSummary(summary: "Get Stops from Postgres Database");
        builder.WithTags(tags: "database");
        
        builder = application.MapGet(
            pattern: "/database/stops/location/{minLon:double}/{minLat:double}/{maxLon:double}/{maxLat:double}",
            handler: DatabaseHandler.GetStopsByLocationAsync);
        
        builder.WithSummary(summary: "Get Stops from Postgres Database");
        builder.WithTags(tags: "database");
        
        return application;
    }
}
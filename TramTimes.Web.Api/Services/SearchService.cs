using TramTimes.Web.Api.Handlers;

namespace TramTimes.Web.Api.Services;

public static class SearchService
{
    public static IEndpointRouteBuilder MapSearchEndpoints(this IEndpointRouteBuilder application)
    {
        var builder = application.MapGet(
            pattern: "/search/stops/id/{id}",
            handler: SearchHandler.GetStopsByIdAsync);
        
        builder.WithSummary(summary: "Get Stops from Elastic Search");
        builder.WithTags(tags: "search");
        
        builder = application.MapGet(
            pattern: "/search/stops/code/{code}",
            handler: SearchHandler.GetStopsByCodeAsync);
        
        builder.WithSummary(summary: "Get Stops from Elastic Search");
        builder.WithTags(tags: "search");
        
        builder = application.MapGet(
            pattern: "/search/stops/name/{name}",
            handler: SearchHandler.GetStopsByNameAsync);
        
        builder.WithSummary(summary: "Get Stops from Elastic Search");
        builder.WithTags(tags: "search");
        
        builder = application.MapGet(
            pattern: "/search/stops/location/{minLon:double}/{minLat:double}/{maxLon:double}/{maxLat:double}",
            handler: SearchHandler.GetStopsByLocationAsync);
        
        builder.WithSummary(summary: "Get Stops from Elastic Search");
        builder.WithTags(tags: "search");
        
        return application;
    }
}
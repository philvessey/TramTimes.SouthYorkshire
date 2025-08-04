using TramTimes.Web.Api.Handlers;

namespace TramTimes.Web.Api.Services;

public static class SearchService
{
    public static void MapSearchEndpoints(this IEndpointRouteBuilder application)
    {
        #region endpoint -> /search/stops/id/
        
        var builder = application.MapGet(
            pattern: "/search/stops/id/{id}",
            handler: SearchHandler.GetStopsByIdAsync);
        
        builder.WithSummary(summary: "Get Stops from Elastic Search Index by Stop ID.");
        builder.WithTags(tags: "search");
        
        #endregion
        
        #region endpoint -> /search/stops/code/
        
        builder = application.MapGet(
            pattern: "/search/stops/code/{code}",
            handler: SearchHandler.GetStopsByCodeAsync);
        
        builder.WithSummary(summary: "Get Stops from Elastic Search Index by Stop Code.");
        builder.WithTags(tags: "search");
        
        #endregion
        
        #region endpoint -> /search/stops/name/
        
        builder = application.MapGet(
            pattern: "/search/stops/name/{name}",
            handler: SearchHandler.GetStopsByNameAsync);
        
        builder.WithSummary(summary: "Get Stops from Elastic Search Index by Stop Name.");
        builder.WithTags(tags: "search");
        
        #endregion
        
        #region endpoint -> /search/stops/location/
        
        builder = application.MapGet(
            pattern: "/search/stops/location/{minLon:double}/{minLat:double}/{maxLon:double}/{maxLat:double}",
            handler: SearchHandler.GetStopsByLocationAsync);
        
        builder.WithSummary(summary: "Get Stops from Elastic Search Index by Stop Location.");
        builder.WithTags(tags: "search");
        
        #endregion
        
        #region endpoint -> /search/stops/point/
        
        builder = application.MapGet(
            pattern: "/search/stops/point/{lon:double}/{lat:double}",
            handler: SearchHandler.GetStopsByPointAsync);
        
        builder.WithSummary(summary: "Get Stops from Elastic Search Index by Stop Point.");
        builder.WithTags(tags: "search");
        
        #endregion
    }
}
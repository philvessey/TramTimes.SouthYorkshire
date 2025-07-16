using TramTimes.Web.Api.Handlers;

namespace TramTimes.Web.Api.Services;

public static class CacheService
{
    public static void MapCacheEndpoints(this IEndpointRouteBuilder application)
    {
        #region endpoint -> /cache/services/stop/
        
        var builder = application.MapGet(
            pattern: "/cache/services/stop/{id}",
            handler: CacheHandler.GetServicesByStopAsync);
        
        builder.WithSummary(summary: "Get Services from Redis Cache.");
        builder.WithTags(tags: "cache");
        
        #endregion
        
        #region endpoint -> /cache/services/trip/
        
        builder = application.MapGet(
            pattern: "/cache/services/trip/{id}",
            handler: CacheHandler.GetServicesByTripAsync);
        
        builder.WithSummary(summary: "Get Services from Redis Cache.");
        builder.WithTags(tags: "cache");
        
        #endregion
    }
}
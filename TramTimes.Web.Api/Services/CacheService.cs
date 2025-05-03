using TramTimes.Web.Api.Handlers;

namespace TramTimes.Web.Api.Services;

public static class CacheService
{
    public static IEndpointRouteBuilder MapCacheEndpoints(this IEndpointRouteBuilder application)
    {
        var builder = application.MapGet(
            pattern: "/cache/services/stop/{id}",
            handler: CacheHandler.GetServicesByStopAsync);
        
        builder.WithSummary(summary: "Get Services from Redis Cache");
        builder.WithTags(tags: "cache");
        
        return application;
    }
}
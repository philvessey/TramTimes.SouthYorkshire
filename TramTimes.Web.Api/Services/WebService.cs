using TramTimes.Web.Api.Handlers;

namespace TramTimes.Web.Api.Services;

public static class WebService
{
    public static void MapWebEndpoints(this IEndpointRouteBuilder application)
    {
        #region endpoint -> /web/cache/build/
        
        var builder = application.MapPost(
            pattern: "/web/cache/build",
            handler: WebHandler.PostCacheByBuildAsync);
        
        builder.WithSummary(summary: "Post Services to Redis Cache.");
        builder.WithTags(tags: "web");
        
        #endregion
        
        #region endpoint -> /web/cache/delete/
        
        builder = application.MapPost(
            pattern: "/web/cache/delete",
            handler: WebHandler.PostCacheByDeleteAsync);
        
        builder.WithSummary(summary: "Post Services to Redis Cache.");
        builder.WithTags(tags: "web");
        
        #endregion
        
        #region endpoint -> /web/index/build/
        
        builder = application.MapPost(
            pattern: "/web/index/build",
            handler: WebHandler.PostIndexByBuildAsync);
        
        builder.WithSummary(summary: "Post Services to Elastic Search.");
        builder.WithTags(tags: "web");
        
        #endregion
        
        #region endpoint -> /web/index/delete/
        
        builder = application.MapPost(
            pattern: "/web/index/delete",
            handler: WebHandler.PostIndexByDeleteAsync);
        
        builder.WithSummary(summary: "Post Services to Elastic Search.");
        builder.WithTags(tags: "web");
        
        #endregion
        
        #region endpoint -> /web/screenshot/file/
        
        builder = application.MapPost(
            pattern: "/web/screenshot/file",
            handler: WebHandler.PostScreenshotByFileAsync);
        
        builder.WithSummary(summary: "Post Screenshot to Azure Storage Container.");
        builder.WithTags(tags: "web");
        
        #endregion
    }
}
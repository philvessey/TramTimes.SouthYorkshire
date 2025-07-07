using TramTimes.Web.Api.Handlers;

namespace TramTimes.Web.Api.Services;

public static class WebService
{
    public static void MapWebEndpoints(this IEndpointRouteBuilder application)
    {
        #region endpoint -> /web/cache/command/
        
        var builder = application.MapPost(
            pattern: "/web/cache/command",
            handler: WebHandler.PostCacheByCommandAsync);
        
        builder.WithSummary(summary: "Post Services to Redis Cache.");
        builder.WithTags(tags: "web");
        
        #endregion
        
        #region endpoint -> /web/index/command/
        
        builder = application.MapPost(
            pattern: "/web/index/command",
            handler: WebHandler.PostIndexByCommandAsync);
        
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
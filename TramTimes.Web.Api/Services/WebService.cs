using TramTimes.Web.Api.Handlers;

namespace TramTimes.Web.Api.Services;

public static class WebService
{
    public static void MapWebEndpoints(this IEndpointRouteBuilder application)
    {
        #region endpoint -> /web/screenshot/file/
        
        var builder = application.MapPost(
            pattern: "/web/screenshot/file",
            handler: WebHandler.PostScreenshotByFileAsync);
        
        builder.WithSummary(summary: "Post Screenshot to Azure Storage Container.");
        builder.WithTags(tags: "web");
        
        #endregion
    }
}
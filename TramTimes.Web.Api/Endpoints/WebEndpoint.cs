using TramTimes.Web.Api.Handlers;

namespace TramTimes.Web.Api.Endpoints;

public static class WebEndpoint
{
    public static void MapWebEndpoints(this IEndpointRouteBuilder application)
    {
        #region endpoint -> /web/cache/build/

        var builder = application
            .MapPost(
                pattern: "/web/cache/build",
                handler: WebHandler.PostCacheByBuildAsync)
            .ExcludeFromDescription();

        builder.WithSummary(summary: "Aspire Dashboard command to build Redis Cache.");
        builder.WithTags(tags: "web");

        #endregion

        #region endpoint -> /web/cache/delete/

        builder = application
            .MapPost(
                pattern: "/web/cache/delete",
                handler: WebHandler.PostCacheByDeleteAsync)
            .ExcludeFromDescription();

        builder.WithSummary(summary: "Aspire Dashboard command to delete Redis Cache.");
        builder.WithTags(tags: "web");

        #endregion

        #region endpoint -> /web/index/build/

        builder = application
            .MapPost(
                pattern: "/web/index/build",
                handler: WebHandler.PostIndexByBuildAsync)
            .ExcludeFromDescription();

        builder.WithSummary(summary: "Aspire Dashboard command to build Elastic Search Index.");
        builder.WithTags(tags: "web");

        #endregion

        #region endpoint -> /web/index/delete/

        builder = application
            .MapPost(
                pattern: "/web/index/delete",
                handler: WebHandler.PostIndexByDeleteAsync)
            .ExcludeFromDescription();

        builder.WithSummary(summary: "Aspire Dashboard command to delete Elastic Search Index.");
        builder.WithTags(tags: "web");

        #endregion

        #region endpoint -> /web/screenshot/file/

        builder = application
            .MapPost(
                pattern: "/web/screenshot/file",
                handler: WebHandler.PostScreenshotByFileAsync)
            .ExcludeFromDescription();

        builder.WithSummary(summary: "Upload Playwright screenshots to Azure Blob Storage Container.");
        builder.WithTags(tags: "web");

        #endregion
    }
}
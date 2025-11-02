// ReSharper disable all

using TramTimes.Aspire.Host.Resources;

namespace TramTimes.Aspire.Host.Builders;

public static class WebBuilder
{
    private static readonly string Testing = Environment.GetEnvironmentVariable(variable: "ASPIRE_TESTING") ?? string.Empty;
    
    public static void BuildWeb(
        this IDistributedApplicationBuilder builder,
        StorageResources storage,
        DatabaseResources database,
        CacheResources cache,
        SearchResources search) {
        
        #region build resources
        
        var web = new WebResources();
        
        #endregion
        
        #region add api
        
        if (builder.ExecutionContext.IsRunMode)
            web.Backend = builder
                .AddProject<Projects.TramTimes_Web_Api>(name: "web-api")
                .WaitFor(dependency: cache.Builder ?? throw new InvalidOperationException(message: "Cache builder is not available."))
                .WaitFor(dependency: search.Builder ?? throw new InvalidOperationException(message: "Search builder is not available."))
                .WithExternalHttpEndpoints()
                .WithHttpCommand(
                    displayName: "Build cache",
                    path: "/web/cache/build",
                    commandOptions: new HttpCommandOptions
                    {
                        IconName = "Settings"
                    })
                .WithHttpCommand(
                    displayName: "Delete cache",
                    path: "/web/cache/delete",
                    commandOptions: new HttpCommandOptions
                    {
                        IconName = "Delete"
                    })
                .WithHttpCommand(
                    displayName: "Build index",
                    path: "/web/index/build",
                    commandOptions: new HttpCommandOptions
                    {
                        IconName = "Settings"
                    })
                .WithHttpCommand(
                    displayName: "Delete index",
                    path: "/web/index/delete",
                    commandOptions: new HttpCommandOptions
                    {
                        IconName = "Delete"
                    })
                .WithHttpHealthCheck(path: "/healthz")
                .WithReference(source: storage.Blobs ?? throw new InvalidOperationException(message: "Storage blobs are not available."))
                .WithReference(source: storage.Queues ?? throw new InvalidOperationException(message: "Storage queues are not available."))
                .WithReference(source: storage.Tables ?? throw new InvalidOperationException(message: "Storage tables are not available."))
                .WithReference(source: database.Resource ?? throw new InvalidOperationException(message: "Database resource is not available."))
                .WithReference(source: cache.Service ?? throw new InvalidOperationException(message: "Cache service is not available."))
                .WithReference(source: search.Service ?? throw new InvalidOperationException(message: "Search service is not available."))
                .WithUrlForEndpoint(
                    callback: annotation => annotation.DisplayText = "Primary",
                    endpointName: "https")
                .WithUrlForEndpoint(
                    callback: annotation => annotation.DisplayText = "Secondary",
                    endpointName: "http");
        
        if (builder.ExecutionContext.IsPublishMode)
            web.Backend = builder
                .AddProject<Projects.TramTimes_Web_Api>(name: "web-api")
                .WaitFor(dependency: cache.Builder ?? throw new InvalidOperationException(message: "Cache builder is not available."))
                .WaitFor(dependency: search.Builder ?? throw new InvalidOperationException(message: "Search builder is not available."))
                .WithExternalHttpEndpoints()
                .WithHttpHealthCheck(path: "/healthz")
                .WithReference(source: database.Connection ?? throw new InvalidOperationException(message: "Database connection is not available."))
                .WithReference(source: cache.Connection ?? throw new InvalidOperationException(message: "Cache connection is not available."))
                .WithReference(source: search.Connection ?? throw new InvalidOperationException(message: "Search connection is not available."))
                .PublishAsAzureContainerApp(configure: (infrastructure, app) =>
                {
                    app.Template.Scale.MinReplicas = 1;
                    app.Template.Scale.MaxReplicas = 5;
                });
        
        #endregion
        
        #region add site
        
        if (builder.ExecutionContext.IsRunMode)
            web.Frontend = builder
                .AddProject<Projects.TramTimes_Web_Site>(name: "web-site")
                .WaitFor(dependency: web.Backend ?? throw new InvalidOperationException(message: "Web backend is not available."))
                .WithEnvironment(
                    name: "API_ENDPOINT",
                    endpointReference: web.Backend.GetEndpoint(name: "https").Exists
                        ? web.Backend.GetEndpoint(name: "https")
                        : web.Backend.GetEndpoint(name: "http"))
                .WithExternalHttpEndpoints()
                .WithUrlForEndpoint(
                    callback: annotation => annotation.DisplayText = "Primary",
                    endpointName: "https")
                .WithUrlForEndpoint(
                    callback: annotation => annotation.DisplayText = "Secondary",
                    endpointName: "http");
        
        if (builder.ExecutionContext.IsPublishMode)
            web.Frontend = builder
                .AddProject<Projects.TramTimes_Web_Site>(name: "web-site")
                .WaitFor(dependency: web.Backend ?? throw new InvalidOperationException(message: "Web backend is not available."))
                .WithEnvironment(
                    name: "API_ENDPOINT",
                    endpointReference: web.Backend.GetEndpoint(name: "https").Exists
                        ? web.Backend.GetEndpoint(name: "https")
                        : web.Backend.GetEndpoint(name: "http"))
                .WithExternalHttpEndpoints()
                .PublishAsAzureContainerApp(configure: (infrastructure, app) =>
                {
                    app.Template.Scale.MinReplicas = 1;
                    app.Template.Scale.MaxReplicas = 5;
                });
        
        #endregion
    }
}
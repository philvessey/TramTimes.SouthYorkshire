using TramTimes.Aspire.Host.Resources;

namespace TramTimes.Aspire.Host.Builders;

public static class WebBuilder
{
    public static void BuildWeb(
        this IDistributedApplicationBuilder builder,
        StorageResources storage,
        DatabaseResources database,
        IResourceBuilder<RedisResource> cache,
        IResourceBuilder<ElasticsearchResource> search) {
        
        #region add api
        
        var api = builder
            .AddProject<Projects.TramTimes_Web_Api>(name: "web-api")
            .WaitFor(dependency: storage.Resource ?? throw new InvalidOperationException(message: "Storage resource is not available."))
            .WaitFor(dependency: database.Resource ?? throw new InvalidOperationException(message: "Database resource is not available."))
            .WaitFor(dependency: cache)
            .WaitFor(dependency: search)
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
            .WithReference(source: storage.Resource)
            .WithReference(source: database.Resource)
            .WithReference(source: cache)
            .WithReference(source: search)
            .WithUrlForEndpoint(
                callback: annotation => annotation.DisplayText = "Primary",
                endpointName: "https")
            .WithUrlForEndpoint(
                callback: annotation => annotation.DisplayText = "Secondary",
                endpointName: "http");
        
        #endregion
        
        #region add jobs
        
        builder
            .AddProject<Projects.TramTimes_Web_Jobs>(name: "web-builder")
            .WaitFor(dependency: api)
            .WithReference(source: storage.Resource);
        
        #endregion
        
        #region add site
        
        builder
            .AddProject<Projects.TramTimes_Web_Site>(name: "web-site")
            .WaitFor(dependency: api)
            .WithEnvironment(
                name: "API_ENDPOINT",
                endpointReference: api.GetEndpoint(name: "https").Exists
                    ? api.GetEndpoint(name: "https")
                    : api.GetEndpoint(name: "http"))
            .WithExternalHttpEndpoints()
            .WithUrlForEndpoint(
                callback: annotation => annotation.DisplayText = "Primary",
                endpointName: "https")
            .WithUrlForEndpoint(
                callback: annotation => annotation.DisplayText = "Secondary",
                endpointName: "http");
        
        #endregion
    }
}
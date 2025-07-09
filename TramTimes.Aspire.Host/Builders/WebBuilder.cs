using Aspire.Hosting.Azure;

namespace TramTimes.Aspire.Host.Builders;

public static class WebBuilder
{
    public static void BuildWeb(
        this IDistributedApplicationBuilder builder,
        IResourceBuilder<AzureStorageResource> storage,
        IResourceBuilder<AzureBlobStorageContainerResource> container,
        IResourceBuilder<PostgresServerResource> server,
        IResourceBuilder<PostgresDatabaseResource> database,
        IResourceBuilder<RedisResource> cache,
        IResourceBuilder<ElasticsearchResource> search) {
        
        #region add api
        
        var api = builder
            .AddProject<Projects.TramTimes_Web_Api>(name: "web-api")
            .WaitFor(dependency: storage)
            .WaitFor(dependency: server)
            .WaitFor(dependency: database)
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
            .WithReference(source: container)
            .WithReference(source: database)
            .WithReference(source: cache)
            .WithReference(source: search)
            .WithUrlForEndpoint("https", annotation => annotation.DisplayText = "Primary")
            .WithUrlForEndpoint("http", annotation => annotation.DisplayText = "Secondary");
        
        #endregion
        
        #region add jobs
        
        builder
            .AddProject<Projects.TramTimes_Web_Jobs>(name: "web-builder")
            .WaitFor(dependency: api)
            .WithReference(source: container);
        
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
            .WithUrlForEndpoint("https", annotation => annotation.DisplayText = "Primary")
            .WithUrlForEndpoint("http", annotation => annotation.DisplayText = "Secondary");
        
        #endregion
    }
}
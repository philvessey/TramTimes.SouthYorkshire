using TramTimes.Aspire.Host.Resources;

namespace TramTimes.Aspire.Host.Builders;

public static class WebBuilder
{
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
                .WaitFor(dependency: storage.Resource ?? throw new InvalidOperationException(message: "Storage resource is not available."))
                .WaitFor(dependency: database.Resource ?? throw new InvalidOperationException(message: "Database resource is not available."))
                .WaitFor(dependency: cache.Service ?? throw new InvalidOperationException(message: "Cache service is not available."))
                .WaitFor(dependency: search.Service ?? throw new InvalidOperationException(message: "Search service is not available."))
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
                .WithReference(source: cache.Service)
                .WithReference(source: search.Service)
                .WithUrlForEndpoint(
                    callback: annotation => annotation.DisplayText = "Primary",
                    endpointName: "https")
                .WithUrlForEndpoint(
                    callback: annotation => annotation.DisplayText = "Secondary",
                    endpointName: "http");
        
        if (builder.ExecutionContext.IsPublishMode)
            web.Backend = builder
                .AddProject<Projects.TramTimes_Web_Api>(name: "web-api")
                .WaitFor(dependency: storage.Connection ?? throw new InvalidOperationException(message: "Storage connection is not available."))
                .WaitFor(dependency: database.Connection ?? throw new InvalidOperationException(message: "Database connection is not available."))
                .WaitFor(dependency: cache.Connection ?? throw new InvalidOperationException(message: "Cache connection is not available."))
                .WaitFor(dependency: search.Connection ?? throw new InvalidOperationException(message: "Search connection is not available."))
                .WithEnvironment(
                    name: "ELASTIC_ENDPOINT",
                    parameter: search.Parameters?.Endpoint ?? throw new InvalidOperationException(message: "Endpoint parameter is not available."))
                .WithEnvironment(
                    name: "ELASTIC_KEY",
                    parameter: search.Parameters?.Key ?? throw new InvalidOperationException(message: "Key parameter is not available."))
                .WithExternalHttpEndpoints()
                .WithHttpHealthCheck(path: "/healthz")
                .WithReference(source: storage.Connection)
                .WithReference(source: database.Connection)
                .WithReference(source: cache.Connection)
                .WithReference(source: search.Connection);
        
        #endregion
        
        #region add jobs
        
        if (builder.ExecutionContext.IsRunMode)
            builder
                .AddProject<Projects.TramTimes_Web_Jobs>(name: "web-builder")
                .WaitFor(dependency: storage.Resource ?? throw new InvalidOperationException(message: "Storage resource is not available."))
                .WithReference(source: storage.Resource);
        
        if (builder.ExecutionContext.IsPublishMode)
            builder
                .AddProject<Projects.TramTimes_Web_Jobs>(name: "web-builder")
                .WaitFor(dependency: storage.Connection ?? throw new InvalidOperationException(message: "Storage connection is not available."))
                .WithReference(source: storage.Connection);
        
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
                .WithExternalHttpEndpoints();
        
        #endregion
    }
}
// ReSharper disable all

using TramTimes.Aspire.Host.Resources;

namespace TramTimes.Aspire.Host.Builders;

public static class CacheBuilder
{
    private static readonly string Testing = Environment.GetEnvironmentVariable(variable: "ASPIRE_TESTING") ?? string.Empty;
    
    public static CacheResources BuildCache(
        this IDistributedApplicationBuilder builder,
        StorageResources storage,
        DatabaseResources database) {
        
        #region build resources
        
        var cache = new CacheResources();
        
        #endregion
        
        #region add redis
        
        if (builder.ExecutionContext.IsRunMode)
            cache.Service = builder
                .AddRedis(name: "cache")
                .WaitFor(dependency: storage.Blobs ?? throw new InvalidOperationException(message: "Storage blobs are not available."))
                .WaitFor(dependency: storage.Queues ?? throw new InvalidOperationException(message: "Storage queues are not available."))
                .WaitFor(dependency: storage.Tables ?? throw new InvalidOperationException(message: "Storage tables are not available."))
                .WaitFor(dependency: database.Resource ?? throw new InvalidOperationException(message: "Database resource is not available."))
                .WithDataVolume()
                .WithLifetime(lifetime: ContainerLifetime.Persistent);
        
        if (builder.ExecutionContext.IsPublishMode)
            cache.Connection = builder.AddConnectionString(name: "cache");
        
        #endregion
        
        #region add tools
        
        if (builder.ExecutionContext.IsRunMode && string.IsNullOrEmpty(value: Testing))
            cache.Service?
                .WithRedisCommander(
                    containerName: "cache-commander",
                    configureContainer: resource =>
                    {
                        resource.WaitFor(dependency: cache.Service);
                        resource.WithLifetime(lifetime: ContainerLifetime.Session);
                        resource.WithParentRelationship(parent: cache.Service);
                        resource.WithUrlForEndpoint(
                            callback: annotation => annotation.DisplayText = "Administration",
                            endpointName: "http");
                    })
                .WithRedisInsight(
                    containerName: "cache-insight",
                    configureContainer: resource =>
                    {
                        resource.WaitFor(dependency: cache.Service);
                        resource.WithLifetime(lifetime: ContainerLifetime.Session);
                        resource.WithParentRelationship(parent: cache.Service);
                        resource.WithUrlForEndpoint(
                            callback: annotation => annotation.DisplayText = "Administration",
                            endpointName: "http");
                    });
        
        #endregion
        
        #region add project
        
        if (builder.ExecutionContext.IsRunMode)
            builder
                .AddProject<Projects.TramTimes_Cache_Jobs>(name: "cache-builder")
                .WaitFor(dependency: cache.Service ?? throw new InvalidOperationException(message: "Cache service is not available."))
                .WithParentRelationship(parent: cache.Service)
                .WithReference(source: storage.Blobs ?? throw new InvalidOperationException(message: "Storage blobs are not available."))
                .WithReference(source: storage.Queues ?? throw new InvalidOperationException(message: "Storage queues are not available."))
                .WithReference(source: storage.Tables ?? throw new InvalidOperationException(message: "Storage tables are not available."))
                .WithReference(source: database.Resource ?? throw new InvalidOperationException(message: "Database resource is not available."))
                .WithReference(source: cache.Service);
        
        if (builder.ExecutionContext.IsPublishMode)
            builder
                .AddProject<Projects.TramTimes_Cache_Jobs>(name: "cache-builder")
                .WaitFor(dependency: cache.Connection ?? throw new InvalidOperationException(message: "Cache connection is not available."))
                .WithReference(source: storage.Blobs ?? throw new InvalidOperationException(message: "Storage blobs are not available."))
                .WithReference(source: storage.Queues ?? throw new InvalidOperationException(message: "Storage queues are not available."))
                .WithReference(source: storage.Tables ?? throw new InvalidOperationException(message: "Storage tables are not available."))
                .WithReference(source: database.Connection ?? throw new InvalidOperationException(message: "Database connection is not available."))
                .WithReference(source: cache.Connection)
                .PublishAsAzureContainerApp(configure: (infrastructure, app) =>
                {
                    app.Template.Scale.MinReplicas = 1;
                    app.Template.Scale.MaxReplicas = 1;
                });
        
        #endregion
        
        return cache;
    }
}
using TramTimes.Aspire.Host.Resources;

namespace TramTimes.Aspire.Host.Builders;

public static class CacheBuilder
{
    private static readonly string Testing = Environment.GetEnvironmentVariable(variable: "ASPIRE_TESTING") ?? string.Empty;
    
    public static CacheResources BuildCache(
        this IDistributedApplicationBuilder builder,
        StorageResources storage,
        DatabaseResources database) {
        
        #region build result
        
        var result = new CacheResources();
        
        #endregion
        
        #region add redis
        
        result.Redis = builder
            .AddRedis(name: "cache")
            .WaitFor(dependency: storage.Resource ?? throw new InvalidOperationException(message: "Storage resource is not available."))
            .WaitFor(dependency: database.Resource ?? throw new InvalidOperationException(message: "Database resource is not available."))
            .WithDataVolume()
            .WithLifetime(lifetime: ContainerLifetime.Persistent);
        
        #endregion
        
        #region add tools
        
        if (string.IsNullOrEmpty(value: Testing))
            result.Redis
                .WithRedisCommander(
                    containerName: "cache-commander",
                    configureContainer: resource =>
                    {
                        resource.WaitFor(dependency: result.Redis);
                        resource.WithLifetime(lifetime: ContainerLifetime.Session);
                        resource.WithParentRelationship(parent: result.Redis);
                        resource.WithUrlForEndpoint(
                            callback: annotation => annotation.DisplayText = "Administration",
                            endpointName: "http");
                    })
                .WithRedisInsight(
                    containerName: "cache-insight",
                    configureContainer: resource =>
                    {
                        resource.WaitFor(dependency: result.Redis);
                        resource.WithLifetime(lifetime: ContainerLifetime.Session);
                        resource.WithParentRelationship(parent: result.Redis);
                        resource.WithUrlForEndpoint(
                            callback: annotation => annotation.DisplayText = "Administration",
                            endpointName: "http");
                    });
        
        #endregion
        
        #region add project
        
        builder
            .AddProject<Projects.TramTimes_Cache_Jobs>(name: "cache-builder")
            .WaitFor(dependency: result.Redis)
            .WithParentRelationship(parent: result.Redis)
            .WithReference(source: storage.Resource)
            .WithReference(source: database.Resource)
            .WithReference(source: result.Redis);
        
        #endregion
        
        return result;
    }
}
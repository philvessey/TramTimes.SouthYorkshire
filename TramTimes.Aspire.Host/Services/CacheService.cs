using Aspire.Hosting.Azure;

namespace TramTimes.Aspire.Host.Services;

public static class CacheService
{
    public static IDistributedApplicationBuilder AddCache(
        this IDistributedApplicationBuilder builder,
        IResourceBuilder<AzureBlobStorageResource> blobs,
        IResourceBuilder<PostgresDatabaseResource> database) {
        
        var cache = builder.AddRedis(name: "cache")
            .WaitFor(dependency: database)
            .WithDataVolume()
            .WithLifetime(lifetime: ContainerLifetime.Persistent)
            .WithRedisCommander(configureContainer: resource =>
            {
                resource.WithLifetime(lifetime: ContainerLifetime.Persistent);
                resource.WithUrlForEndpoint("http", annotation => annotation.DisplayText = "Administration");
            })
            .WithRedisInsight(configureContainer: resource =>
            {
                resource.WithLifetime(lifetime: ContainerLifetime.Persistent);
                resource.WithUrlForEndpoint("http", annotation => annotation.DisplayText = "Administration");
            });
        
        builder.AddProject<Projects.TramTimes_Cache_Jobs>(name: "cache-builder")
            .WaitFor(dependency: cache)
            .WithParentRelationship(parent: cache)
            .WithReference(source: blobs)
            .WithReference(source: cache)
            .WithReference(source: database);
        
        return builder;
    }
}
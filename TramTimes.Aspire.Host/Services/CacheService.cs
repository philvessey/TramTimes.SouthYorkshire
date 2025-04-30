using Aspire.Hosting.Azure;
using Microsoft.Extensions.Hosting;

namespace TramTimes.Aspire.Host.Services;

public static class CacheService
{
    public static IDistributedApplicationBuilder AddCache(
        this IDistributedApplicationBuilder builder,
        IResourceBuilder<AzureBlobStorageResource> blobs,
        IResourceBuilder<PostgresServerResource> server,
        IResourceBuilder<PostgresDatabaseResource> database) {
        
        var cache = builder.AddRedis(name: "cache")
            .WaitFor(dependency: server)
            .WithDataVolume()
            .WithLifetime(lifetime: ContainerLifetime.Persistent);
        
        if (builder.Environment.IsDevelopment())
        {
            cache.WithRedisCommander(configureContainer: resource =>
            {
                resource.WithLifetime(lifetime: ContainerLifetime.Persistent);
                resource.WithUrlForEndpoint("http", annotation => annotation.DisplayText = "Administration");
            });
            
            cache.WithRedisInsight(configureContainer: resource =>
            {
                resource.WithLifetime(lifetime: ContainerLifetime.Persistent);
                resource.WithUrlForEndpoint("http", annotation => annotation.DisplayText = "Administration");
            });
        }
        
        builder.AddProject<Projects.TramTimes_Cache_Jobs>(name: "cache-builder")
            .WaitFor(dependency: cache)
            .WithParentRelationship(parent: cache)
            .WithReference(source: blobs)
            .WithReference(source: database)
            .WithReference(source: cache);
        
        return builder;
    }
}
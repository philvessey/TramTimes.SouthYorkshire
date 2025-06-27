using Aspire.Hosting.Azure;
using Microsoft.Extensions.Hosting;
using TramTimes.Aspire.Host.Resources;

namespace TramTimes.Aspire.Host.Builders;

public static class CacheBuilder
{
    public static CacheResources BuildCache(
        this IDistributedApplicationBuilder builder,
        IResourceBuilder<AzureStorageResource> storage,
        IResourceBuilder<AzureBlobStorageContainerResource> container,
        IResourceBuilder<PostgresServerResource> server,
        IResourceBuilder<PostgresDatabaseResource> database) {
        
        #region build result
        
        var result = new CacheResources();
        
        #endregion
        
        #region add redis
        
        result.Redis = builder
            .AddRedis(name: "cache")
            .WaitFor(dependency: storage)
            .WaitFor(dependency: server)
            .WaitFor(dependency: database)
            .WithDataVolume()
            .WithLifetime(lifetime: ContainerLifetime.Persistent);
        
        #endregion
        
        #region add tools
        
        if (builder.Environment.IsDevelopment())
            result.Redis
                .WithRedisCommander(configureContainer: resource =>
                {
                    resource.WithLifetime(lifetime: ContainerLifetime.Session);
                    resource.WithUrlForEndpoint("http", annotation => annotation.DisplayText = "Administration");
                })
                .WithRedisInsight(configureContainer: resource =>
                {
                    resource.WithLifetime(lifetime: ContainerLifetime.Session);
                    resource.WithUrlForEndpoint("http", annotation => annotation.DisplayText = "Administration");
                });
        
        #endregion
        
        #region add project
        
        builder
            .AddProject<Projects.TramTimes_Cache_Jobs>(name: "cache-builder")
            .WaitFor(dependency: result.Redis)
            .WithParentRelationship(parent: result.Redis)
            .WithReference(source: container)
            .WithReference(source: database)
            .WithReference(source: result.Redis);
        
        #endregion
        
        return result;
    }
}
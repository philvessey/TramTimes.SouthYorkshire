// ReSharper disable all

using Azure.Provisioning.AppContainers;
using TramTimes.Aspire.Host.Resources;

namespace TramTimes.Aspire.Host.Builders;

public static class CacheBuilder
{
    private static readonly string Context = Environment.GetEnvironmentVariable(variable: "ASPIRE_CONTEXT") ?? "Development";
    
    public static CacheResources BuildCache(
        this IDistributedApplicationBuilder builder,
        DatabaseResources database) {
        
        #region build resources
        
        var cache = new CacheResources();
        
        #endregion
        
        #region add redis
        
        if (builder.ExecutionContext.IsRunMode)
            cache.Service = builder
                .AddRedis(name: "cache")
                .WaitFor(dependency: database.Builder ?? throw new InvalidOperationException(message: "Database builder is not available."))
                .WithDataVolume()
                .WithLifetime(lifetime: ContainerLifetime.Persistent)
                .WithUrlForEndpoint(
                    callback: annotation => annotation.DisplayLocation = UrlDisplayLocation.DetailsOnly,
                    endpointName: "tcp");
        
        if (builder.ExecutionContext.IsPublishMode)
            cache.Connection = builder.AddConnectionString(name: "cache");
        
        #endregion
        
        #region add project
        
        if (builder.ExecutionContext.IsRunMode)
            cache.Builder = builder
                .AddProject<Projects.TramTimes_Cache_Jobs>(name: "cache-builder")
                .WaitFor(dependency: cache.Service ?? throw new InvalidOperationException(message: "Cache service is not available."))
                .WithEnvironment(
                    name: "ASPIRE_CONTEXT",
                    value: Context)
                .WithParentRelationship(parent: cache.Service)
                .WithReference(source: database.Resource ?? throw new InvalidOperationException(message: "Database resource is not available."))
                .WithReference(source: cache.Service);
        
        if (builder.ExecutionContext.IsPublishMode)
            cache.Builder = builder
                .AddProject<Projects.TramTimes_Cache_Jobs>(name: "cache-builder")
                .WaitFor(dependency: cache.Connection ?? throw new InvalidOperationException(message: "Cache connection is not available."))
                .WithEnvironment(
                    name: "ASPIRE_CONTEXT",
                    value: "Production")
                .WithReference(source: database.Connection ?? throw new InvalidOperationException(message: "Database connection is not available."))
                .WithReference(source: cache.Connection)
                .PublishAsAzureContainerApp(configure: (infrastructure, app) =>
                {
                    var container = app.Template.Containers.Single().Value;
                    
                    if (container is not null)
                    {
                        container.Resources.Cpu = 0.25;
                        container.Resources.Memory = "0.5Gi";
                    }
                    
                    app.Template.Scale.MinReplicas = 1;
                    app.Template.Scale.MaxReplicas = 1;
                });
        
        #endregion
        
        #region check context
        
        if (Context is "Testing")
            return cache;
        
        #endregion
        
        #region add tools
        
        if (builder.ExecutionContext.IsRunMode)
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
        
        return cache;
    }
}
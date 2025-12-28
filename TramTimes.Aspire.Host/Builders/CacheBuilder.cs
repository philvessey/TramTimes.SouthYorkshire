// ReSharper disable all

using Azure.Provisioning.AppContainers;
using TramTimes.Aspire.Host.Resources;

namespace TramTimes.Aspire.Host.Builders;

public static class CacheBuilder
{
    private static readonly string _context = Environment.GetEnvironmentVariable(variable: "ASPIRE_CONTEXT") ?? "Development";

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
                .WithImageTag(tag: "8.2")
                .WithLifetime(lifetime: ContainerLifetime.Persistent)
                .WithUrlForEndpoint(
                    callback: (ResourceUrlAnnotation url) => url.DisplayLocation = UrlDisplayLocation.DetailsOnly,
                    endpointName: "tcp")
                .WithUrlForEndpoint(
                    callback: (ResourceUrlAnnotation url) => url.DisplayLocation = UrlDisplayLocation.DetailsOnly,
                    endpointName: "secondary");

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
                    value: _context)
                .WithParentRelationship(parent: cache.Service)
                .WithReference(source: database.Resource ?? throw new InvalidOperationException(message: "Database resource is not available."))
                .WithReference(source: cache.Service);

        if (builder.ExecutionContext.IsPublishMode)
            cache.Builder = builder
                .AddProject<Projects.TramTimes_Cache_Jobs>(name: "cache-builder-peak")
                .WaitFor(dependency: cache.Connection ?? throw new InvalidOperationException(message: "Cache connection is not available."))
                .WithEnvironment(
                    name: "ASPIRE_CONTEXT",
                    value: "Production")
                .WithReference(source: database.Connection ?? throw new InvalidOperationException(message: "Database connection is not available."))
                .WithReference(source: cache.Connection)
                .PublishAsAzureContainerAppJob(configure: (infrastructure, job) =>
                {
                    var container = job.Template.Containers.Single().Value;

                    if (container is not null)
                    {
                        container.Resources.Cpu = 0.25;
                        container.Resources.Memory = "0.5Gi";
                    }

                    job.Configuration.TriggerType = ContainerAppJobTriggerType.Schedule;
                    job.Configuration.ScheduleTriggerConfig.CronExpression = "0/10 6-9,16-19 * * 1,2,3,4,5";
                });

        if (builder.ExecutionContext.IsPublishMode)
            cache.Builder = builder
                .AddProject<Projects.TramTimes_Cache_Jobs>(name: "cache-builder-offpeak")
                .WaitFor(dependency: cache.Connection ?? throw new InvalidOperationException(message: "Cache connection is not available."))
                .WithEnvironment(
                    name: "ASPIRE_CONTEXT",
                    value: "Production")
                .WithReference(source: database.Connection ?? throw new InvalidOperationException(message: "Database connection is not available."))
                .WithReference(source: cache.Connection)
                .PublishAsAzureContainerAppJob(configure: (infrastructure, job) =>
                {
                    var container = job.Template.Containers.Single().Value;

                    if (container is not null)
                    {
                        container.Resources.Cpu = 0.25;
                        container.Resources.Memory = "0.5Gi";
                    }

                    job.Configuration.TriggerType = ContainerAppJobTriggerType.Schedule;
                    job.Configuration.ScheduleTriggerConfig.CronExpression = "0/20 10-15,20-23 * * 1,2,3,4,5";
                });

        if (builder.ExecutionContext.IsPublishMode)
            cache.Builder = builder
                .AddProject<Projects.TramTimes_Cache_Jobs>(name: "cache-builder-weekend")
                .WaitFor(dependency: cache.Connection ?? throw new InvalidOperationException(message: "Cache connection is not available."))
                .WithEnvironment(
                    name: "ASPIRE_CONTEXT",
                    value: "Production")
                .WithReference(source: database.Connection ?? throw new InvalidOperationException(message: "Database connection is not available."))
                .WithReference(source: cache.Connection)
                .PublishAsAzureContainerAppJob(configure: (infrastructure, job) =>
                {
                    var container = job.Template.Containers.Single().Value;

                    if (container is not null)
                    {
                        container.Resources.Cpu = 0.25;
                        container.Resources.Memory = "0.5Gi";
                    }

                    job.Configuration.TriggerType = ContainerAppJobTriggerType.Schedule;
                    job.Configuration.ScheduleTriggerConfig.CronExpression = "0/30 6-23 * * 6,0";
                });

        if (builder.ExecutionContext.IsPublishMode)
            cache.Builder = builder
                .AddProject<Projects.TramTimes_Cache_Jobs>(name: "cache-builder-night")
                .WaitFor(dependency: cache.Connection ?? throw new InvalidOperationException(message: "Cache connection is not available."))
                .WithEnvironment(
                    name: "ASPIRE_CONTEXT",
                    value: "Production")
                .WithReference(source: database.Connection ?? throw new InvalidOperationException(message: "Database connection is not available."))
                .WithReference(source: cache.Connection)
                .PublishAsAzureContainerAppJob(configure: (infrastructure, job) =>
                {
                    var container = job.Template.Containers.Single().Value;

                    if (container is not null)
                    {
                        container.Resources.Cpu = 0.25;
                        container.Resources.Memory = "0.5Gi";
                    }

                    job.Configuration.TriggerType = ContainerAppJobTriggerType.Schedule;
                    job.Configuration.ScheduleTriggerConfig.CronExpression = "0/30 0-1,4-5 * * *";
                });

        #endregion

        #region check context

        if (_context is "Testing")
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
                            callback: (ResourceUrlAnnotation url) => url.DisplayText = "Administration",
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
                            callback: (ResourceUrlAnnotation url) => url.DisplayText = "Administration",
                            endpointName: "http");
                    });

        #endregion

        return cache;
    }
}
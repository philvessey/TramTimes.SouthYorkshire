// ReSharper disable all

using TramTimes.Aspire.Host.Resources;

namespace TramTimes.Aspire.Host.Builders;

public static class StorageBuilder
{
    private static readonly string _context = Environment.GetEnvironmentVariable(variable: "ASPIRE_CONTEXT") ?? "Development";

    public static StorageResources BuildStorage(this IDistributedApplicationBuilder builder)
    {
        #region build resources

        var storage = new StorageResources();

        #endregion

        #region add storage

        if (builder.ExecutionContext.IsRunMode)
            storage.Service = builder
                .AddAzureStorage(name: "storage")
                .WithUrlForEndpoint(
                    callback: (ResourceUrlAnnotation url) => url.DisplayLocation = UrlDisplayLocation.DetailsOnly,
                    endpointName: "blob")
                .WithUrlForEndpoint(
                    callback: (ResourceUrlAnnotation url) => url.DisplayLocation = UrlDisplayLocation.DetailsOnly,
                    endpointName: "queue")
                .WithUrlForEndpoint(
                    callback: (ResourceUrlAnnotation url) => url.DisplayLocation = UrlDisplayLocation.DetailsOnly,
                    endpointName: "table")
                .RunAsEmulator(configureContainer: resource =>
                {
                    resource.WithDataVolume();
                    resource.WithLifetime(lifetime: ContainerLifetime.Persistent);
                });

        #endregion

        #region add services

        if (builder.ExecutionContext.IsRunMode)
            storage.Blobs = storage.Service?.AddBlobs(name: "storage-blobs");

        if (builder.ExecutionContext.IsRunMode)
            storage.Queues = storage.Service?.AddQueues(name: "storage-queues");

        if (builder.ExecutionContext.IsRunMode)
            storage.Tables = storage.Service?.AddTables(name: "storage-tables");

        #endregion

        return storage;
    }
}
// ReSharper disable all

using TramTimes.Aspire.Host.Resources;

namespace TramTimes.Aspire.Host.Builders;

public static class StorageBuilder
{
    public static StorageResources BuildStorage(this IDistributedApplicationBuilder builder)
    {
        #region build resources
        
        var storage = new StorageResources();
        
        #endregion
        
        #region add azure storage
        
        if (builder.ExecutionContext.IsRunMode)
            storage.Service = builder
                .AddAzureStorage(name: "storage")
                .WithUrlForEndpoint(
                    callback: annotation => annotation.DisplayText = "Administration (Blob)",
                    endpointName: "blob")
                .WithUrlForEndpoint(
                    callback: annotation => annotation.DisplayText = "Administration (Queue)",
                    endpointName: "queue")
                .WithUrlForEndpoint(
                    callback: annotation => annotation.DisplayText = "Administration (Table)",
                    endpointName: "table")
                .RunAsEmulator(configureContainer: resource =>
                {
                    resource.WithDataVolume();
                    resource.WithLifetime(lifetime: ContainerLifetime.Persistent);
                });
        
        if (builder.ExecutionContext.IsPublishMode)
            storage.Service = builder.AddAzureStorage(name: "storage");
        
        #endregion
        
        #region add storage services
        
        storage.Blobs = storage.Service?.AddBlobs(name: "storage-blobs");
        storage.Queues = storage.Service?.AddQueues(name: "storage-queues");
        storage.Tables = storage.Service?.AddTables(name: "storage-tables");
        
        #endregion
        
        return storage;
    }
}
// ReSharper disable all

using TramTimes.Aspire.Host.Parameters;
using TramTimes.Aspire.Host.Resources;

namespace TramTimes.Aspire.Host.Builders;

public static class StorageBuilder
{
    public static StorageResources BuildStorage(this IDistributedApplicationBuilder builder)
    {
        #region build resources
        
        var storage = new StorageResources();
        
        #endregion
        
        #region add parameters
        
        storage.Parameters = new StorageParameters();
        
        if (builder.ExecutionContext.IsPublishMode)
            storage.Parameters.Group = builder.AddParameter(
                name: "storage-group",
                secret: false);
        
        if (builder.ExecutionContext.IsPublishMode)
            storage.Parameters.Name = builder.AddParameter(
                name: "storage-name",
                secret: false);
        
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
            storage.Service = builder
                .AddAzureStorage(name: "storage")
                .PublishAsExisting(
                    nameParameter: storage.Parameters.Name ?? throw new InvalidOperationException(message: "Name parameter is not available."),
                    resourceGroupParameter: storage.Parameters.Group ?? throw new InvalidOperationException(message: "Group parameter is not available."));
        
        #endregion
        
        #region add blob service
        
        storage.Resource = storage.Service?.AddBlobs(name: "storage-blobs");
        
        #endregion
        
        return storage;
    }
}
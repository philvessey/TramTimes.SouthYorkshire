using TramTimes.Aspire.Host.Resources;

namespace TramTimes.Aspire.Host.Builders;

public static class StorageBuilder
{
    public static StorageResources BuildStorage(this IDistributedApplicationBuilder builder)
    {
        #region build result
        
        var result = new StorageResources();
        
        #endregion
        
        #region add azure storage
        
        result.AzureStorage = builder
            .AddAzureStorage(name: "storage")
            .RunAsEmulator(configureContainer: resource =>
            {
                resource.WithDataVolume();
                resource.WithLifetime(lifetime: ContainerLifetime.Persistent);
            })
            .WithUrlForEndpoint(
                callback: annotation => annotation.DisplayText = "Administration (Blob)",
                endpointName: "blob")
            .WithUrlForEndpoint(
                callback: annotation => annotation.DisplayText = "Administration (Queue)",
                endpointName: "queue")
            .WithUrlForEndpoint(
                callback: annotation => annotation.DisplayText = "Administration (Table)",
                endpointName: "table");
        
        #endregion
        
        #region add blob container
        
        result.AzureBlobStorageContainer = result.AzureStorage.AddBlobContainer(
            name: "storage-blobs-southyorkshire",
            blobContainerName: "southyorkshire");
        
        #endregion
        
        return result;
    }
}
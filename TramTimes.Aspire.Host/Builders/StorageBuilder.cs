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
        
        result.Azure = builder
            .AddAzureStorage(name: "storage")
            .RunAsEmulator(configureContainer: resource =>
            {
                resource.WithDataVolume();
                resource.WithLifetime(lifetime: ContainerLifetime.Persistent);
            });
        
        #endregion
        
        #region add azure blob storage
        
        var blobs = result.Azure.AddBlobs(name: "blobs");
        
        #endregion
        
        #region add azure blob storage containers
        
        result.Cache = blobs.AddBlobContainer(
            name: "cache-storage",
            blobContainerName: "cache");
        
        result.Database = blobs.AddBlobContainer(
            name: "database-storage",
            blobContainerName: "database");
        
        result.Search = blobs.AddBlobContainer(
            name: "search-storage",
            blobContainerName: "search");
        
        result.Web = blobs.AddBlobContainer(
            name: "web-storage",
            blobContainerName: "web");
        
        #endregion
        
        return result;
    }
}
using Aspire.Hosting.Azure;
using JetBrains.Annotations;

namespace TramTimes.Aspire.Host.Resources;

public class StorageResources
{
    [UsedImplicitly]
    public IResourceBuilder<AzureStorageResource>? AzureStorage { get; set; }
    
    [UsedImplicitly]
    public IResourceBuilder<AzureBlobStorageContainerResource>? AzureBlobStorageContainer { get; set; }
}
using Aspire.Hosting.Azure;
using JetBrains.Annotations;

namespace TramTimes.Aspire.Host.Resources;

public class StorageResources
{
    [UsedImplicitly]
    public IResourceBuilder<AzureStorageResource>? Azure { get; set; }
    
    [UsedImplicitly]
    public IResourceBuilder<AzureBlobStorageContainerResource>? Cache { get; set; }
    
    [UsedImplicitly]
    public IResourceBuilder<AzureBlobStorageContainerResource>? Database { get; set; }
    
    [UsedImplicitly]
    public IResourceBuilder<AzureBlobStorageContainerResource>? Search { get; set; }
    
    [UsedImplicitly]
    public IResourceBuilder<AzureBlobStorageContainerResource>? Web { get; set; }
}
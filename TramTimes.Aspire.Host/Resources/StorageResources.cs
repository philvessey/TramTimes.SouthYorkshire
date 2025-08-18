using Aspire.Hosting.Azure;
using JetBrains.Annotations;

namespace TramTimes.Aspire.Host.Resources;

public class StorageResources
{
    [UsedImplicitly]
    public IResourceBuilder<AzureStorageResource>? Service { get; set; }
    
    [UsedImplicitly]
    public IResourceBuilder<AzureBlobStorageResource>? Resource { get; set; }
    
    [UsedImplicitly]
    public IResourceBuilder<IResourceWithConnectionString>? Connection { get; set; }
}
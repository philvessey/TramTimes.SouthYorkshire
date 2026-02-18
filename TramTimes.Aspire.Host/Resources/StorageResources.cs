using Aspire.Hosting.Azure;
using JetBrains.Annotations;

namespace TramTimes.Aspire.Host.Resources;

public class StorageResources
{
    [UsedImplicitly] public IResourceBuilder<AzureStorageResource>? Service { get; set; }
    [UsedImplicitly] public IResourceBuilder<AzureBlobStorageResource>? Blobs { get; set; }
    [UsedImplicitly] public IResourceBuilder<AzureQueueStorageResource>? Queues { get; set; }
    [UsedImplicitly] public IResourceBuilder<AzureTableStorageResource>? Tables { get; set; }
}
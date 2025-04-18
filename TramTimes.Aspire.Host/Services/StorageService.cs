using Aspire.Hosting.Azure;
using Microsoft.Extensions.Hosting;

namespace TramTimes.Aspire.Host.Services;

public static class StorageService
{
    public static IDistributedApplicationBuilder AddStorage(
        this IDistributedApplicationBuilder builder,
        out IResourceBuilder<AzureStorageResource> storage,
        out IResourceBuilder<AzureBlobStorageResource> blobs) {
        
        storage = builder.AddAzureStorage(name: "storage");
        
        if (builder.Environment.IsDevelopment())
        {
            storage.RunAsEmulator(configureContainer: resource =>
            {
                resource.WithDataVolume();
                resource.WithLifetime(lifetime: ContainerLifetime.Persistent);
            });
        }
        
        blobs = storage.AddBlobs(name: "blobs");
        
        return builder;
    }
}
using TramTimes.Aspire.Host.Resources;

namespace TramTimes.Aspire.Host.Builders;

public static class StorageBuilder
{
    public static StorageResources BuildStorage(this IDistributedApplicationBuilder builder)
    {
        #region build resources
        
        var resources = new StorageResources();
        
        #endregion
        
        #region add azure storage
        
        if (builder.ExecutionContext.IsRunMode)
            resources.Service = builder
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
        
        #region add blob service
        
        if (builder.ExecutionContext.IsRunMode)
            resources.Resource = resources.Service?.AddBlobs(name: "storage-blobs");
        
        if (builder.ExecutionContext.IsPublishMode)
            resources.Connection = builder.AddConnectionString(name: "storage-blobs");
        
        #endregion
        
        return resources;
    }
}
// ReSharper disable all

using TramTimes.Aspire.Host.Resources;

namespace TramTimes.Aspire.Host.Builders;

public static class ContainerBuilder
{
    public static ContainerResources BuildContainer(this IDistributedApplicationBuilder builder)
    {
        #region build resources
        
        var container = new ContainerResources();
        
        #endregion
        
        #region add container registry
        
        if (builder.ExecutionContext.IsPublishMode)
            container.Service = builder.AddAzureContainerRegistry(name: "registry");
        
        #endregion
        
        #region add container environment
        
        if (builder.ExecutionContext.IsPublishMode)
            container.Resource = builder
                .AddAzureContainerAppEnvironment(name: "environment")
                .WithAzureContainerRegistry(registryBuilder: container.Service ?? throw new InvalidOperationException(message: "Container service is not available."));
        
        #endregion
        
        return container;
    }
}
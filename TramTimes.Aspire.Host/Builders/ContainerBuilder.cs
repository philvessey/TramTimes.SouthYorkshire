// ReSharper disable all

using TramTimes.Aspire.Host.Parameters;
using TramTimes.Aspire.Host.Resources;

namespace TramTimes.Aspire.Host.Builders;

public static class ContainerBuilder
{
    public static ContainerResources BuildContainer(this IDistributedApplicationBuilder builder)
    {
        #region build resources
        
        var container = new ContainerResources();
        
        #endregion
        
        #region add parameters
        
        container.Parameters = new ContainerParameters();
        
        if (builder.ExecutionContext.IsPublishMode)
            container.Parameters.Group = builder.AddParameter(
                name: "container-group",
                secret: false);
        
        if (builder.ExecutionContext.IsPublishMode)
            container.Parameters.Name = builder.AddParameter(
                name: "container-name",
                secret: false);
        
        #endregion
        
        #region add container registry
        
        if (builder.ExecutionContext.IsPublishMode)
            container.Service = builder
                .AddAzureContainerRegistry(name: "tramtimes")
                .PublishAsExisting(
                    nameParameter: container.Parameters.Name ?? throw new InvalidOperationException(message: "Name parameter is not available."),
                    resourceGroupParameter: container.Parameters.Group ?? throw new InvalidOperationException(message: "Group parameter is not available."));
        
        #endregion
        
        #region add container environment
        
        if (builder.ExecutionContext.IsPublishMode)
            builder
                .AddAzureContainerAppEnvironment(name: "southyorkshire")
                .WithAzureContainerRegistry(registryBuilder: container.Service ?? throw new InvalidOperationException(message: "Container service is not available."));
        
        #endregion
        
        return container;
    }
}
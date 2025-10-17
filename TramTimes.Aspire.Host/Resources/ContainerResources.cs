using Aspire.Hosting.Azure;
using Aspire.Hosting.Azure.AppContainers;
using JetBrains.Annotations;

namespace TramTimes.Aspire.Host.Resources;

public class ContainerResources
{
    [UsedImplicitly]
    public IResourceBuilder<AzureContainerRegistryResource>? Service { get; set; }
    
    [UsedImplicitly]
    public IResourceBuilder<AzureContainerAppEnvironmentResource>? Resource { get; set; }
}
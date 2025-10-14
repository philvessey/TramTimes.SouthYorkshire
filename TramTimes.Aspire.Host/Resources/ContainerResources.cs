using Aspire.Hosting.Azure;
using JetBrains.Annotations;
using TramTimes.Aspire.Host.Parameters;

namespace TramTimes.Aspire.Host.Resources;

public class ContainerResources
{
    [UsedImplicitly]
    public IResourceBuilder<AzureContainerRegistryResource>? Service { get; set; }
    
    [UsedImplicitly]
    public ContainerParameters? Parameters { get; set; }
}
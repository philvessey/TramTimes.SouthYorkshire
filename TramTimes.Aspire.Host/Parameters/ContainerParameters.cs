using JetBrains.Annotations;

namespace TramTimes.Aspire.Host.Parameters;

public class ContainerParameters
{
    [UsedImplicitly]
    public IResourceBuilder<ParameterResource>? Group { get; set; }
    
    [UsedImplicitly]
    public IResourceBuilder<ParameterResource>? Name { get; set; }
}
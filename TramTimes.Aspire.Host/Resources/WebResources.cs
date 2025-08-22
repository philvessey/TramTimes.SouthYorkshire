using JetBrains.Annotations;

namespace TramTimes.Aspire.Host.Resources;

public class WebResources
{
    [UsedImplicitly]
    public IResourceBuilder<ProjectResource>? Backend { get; set; }
    
    [UsedImplicitly]
    public IResourceBuilder<ProjectResource>? Frontend { get; set; }
}
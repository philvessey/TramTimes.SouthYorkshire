using JetBrains.Annotations;
using TramTimes.Aspire.Host.Parameters;

namespace TramTimes.Aspire.Host.Resources;

public class WebResources
{
    [UsedImplicitly]
    public WebParameters? Parameters { get; set; }
    
    [UsedImplicitly]
    public IResourceBuilder<ProjectResource>? Backend { get; set; }
    
    [UsedImplicitly]
    public IResourceBuilder<ProjectResource>? Frontend { get; set; }
}
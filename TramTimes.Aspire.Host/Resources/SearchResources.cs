using JetBrains.Annotations;
using TramTimes.Aspire.Host.Parameters;

namespace TramTimes.Aspire.Host.Resources;

public class SearchResources
{
    [UsedImplicitly]
    public IResourceBuilder<ElasticsearchResource>? Service { get; set; }
    
    [UsedImplicitly]
    public IResourceBuilder<IResourceWithConnectionString>? Connection { get; set; }
    
    [UsedImplicitly]
    public SearchParameters? Parameters { get; set; }
}
using JetBrains.Annotations;

namespace TramTimes.Aspire.Host.Resources;

public class SearchResources
{
    [UsedImplicitly]
    public IResourceBuilder<ElasticsearchResource>? Elastic { get; set; }
}
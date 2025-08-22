using JetBrains.Annotations;

namespace TramTimes.Aspire.Host.Parameters;

public class SearchParameters
{
    [UsedImplicitly]
    public IResourceBuilder<ParameterResource>? Endpoint { get; set; }
    
    [UsedImplicitly]
    public IResourceBuilder<ParameterResource>? Key { get; set; }
}
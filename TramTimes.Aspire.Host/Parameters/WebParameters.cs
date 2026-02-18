using JetBrains.Annotations;

namespace TramTimes.Aspire.Host.Parameters;

public class WebParameters
{
    [UsedImplicitly] public IResourceBuilder<ParameterResource>? Certificate { get; set; }
    [UsedImplicitly] public IResourceBuilder<ParameterResource>? Domain { get; set; }
}
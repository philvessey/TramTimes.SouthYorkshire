using JetBrains.Annotations;

namespace TramTimes.Aspire.Host.Parameters;

public class WebParameters
{
    [UsedImplicitly] public IResourceBuilder<ParameterResource>? Certificate { get; set; }
    [UsedImplicitly] public IResourceBuilder<ParameterResource>? Domain { get; set; }
    [UsedImplicitly] public IResourceBuilder<ParameterResource>? Hostname { get; set; }
    [UsedImplicitly] public IResourceBuilder<ParameterResource>? Hostport { get; set; }
    [UsedImplicitly] public IResourceBuilder<ParameterResource>? Username { get; set; }
    [UsedImplicitly] public IResourceBuilder<ParameterResource>? Userpass { get; set; }
}
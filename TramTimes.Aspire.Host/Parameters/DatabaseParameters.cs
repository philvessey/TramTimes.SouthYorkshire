using JetBrains.Annotations;

namespace TramTimes.Aspire.Host.Parameters;

public class DatabaseParameters
{
    [UsedImplicitly]
    public IResourceBuilder<ParameterResource>? Hostname { get; set; }
    
    [UsedImplicitly]
    public IResourceBuilder<ParameterResource>? Username { get; set; }
    
    [UsedImplicitly]
    public IResourceBuilder<ParameterResource>? Userpass { get; set; }
}
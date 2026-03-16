using JetBrains.Annotations;

namespace TramTimes.Aspire.Host.Parameters;

public class LicenseParameters
{
    [UsedImplicitly] public IResourceBuilder<ParameterResource>? Automapper { get; set; }
    [UsedImplicitly] public IResourceBuilder<ParameterResource>? Telerik { get; set; }
}
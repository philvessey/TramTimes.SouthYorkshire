using JetBrains.Annotations;
using TramTimes.Aspire.Host.Parameters;

namespace TramTimes.Aspire.Host.Resources;

public class LicenseResources
{
    [UsedImplicitly] public LicenseParameters? Parameters { get; set; }
}
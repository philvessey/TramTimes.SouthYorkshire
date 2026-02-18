using JetBrains.Annotations;
using TramTimes.Aspire.Host.Parameters;

namespace TramTimes.Aspire.Host.Resources;

public class RevenueResources
{
    [UsedImplicitly] public RevenueParameters? Parameters { get; set; }
}
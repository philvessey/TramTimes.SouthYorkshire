using JetBrains.Annotations;

namespace TramTimes.Cache.Jobs.Models;

public class CacheStop
{
    [UsedImplicitly] public string? Id { get; set; }
    [UsedImplicitly] public string? Code { get; set; }
    [UsedImplicitly] public string? Name { get; set; }
    [UsedImplicitly] public double? Latitude { get; set; }
    [UsedImplicitly] public double? Longitude { get; set; }
    [UsedImplicitly] public string? Platform { get; set; }
    [UsedImplicitly] public List<CacheStopPoint>? Points { get; set; }
}
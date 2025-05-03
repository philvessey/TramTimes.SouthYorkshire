using JetBrains.Annotations;

namespace TramTimes.Web.Api.Models;

public class CacheStop
{
    [UsedImplicitly]
    public string? Id { get; set; }
    
    [UsedImplicitly]
    public List<CacheStopPoint>? Points { get; set; }
}
using JetBrains.Annotations;

namespace TramTimes.Aspire.Host.Resources;

public class CacheResources
{
    [UsedImplicitly]
    public IResourceBuilder<RedisResource>? Redis { get; set; }
}
using JetBrains.Annotations;

namespace TramTimes.Aspire.Host.Resources;

public class CacheResources
{
    [UsedImplicitly] public IResourceBuilder<RedisResource>? Service { get; set; }

    [UsedImplicitly] public IResourceBuilder<IResourceWithConnectionString>? Connection { get; set; }

    [UsedImplicitly] public IResourceBuilder<ProjectResource>? Builder { get; set; }
}
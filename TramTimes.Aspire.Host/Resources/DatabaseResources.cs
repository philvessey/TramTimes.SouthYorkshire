using JetBrains.Annotations;

namespace TramTimes.Aspire.Host.Resources;

public class DatabaseResources
{
    [UsedImplicitly]
    public IResourceBuilder<PostgresServerResource>? Service { get; set; }
    
    [UsedImplicitly]
    public IResourceBuilder<PostgresDatabaseResource>? Resource { get; set; }
    
    [UsedImplicitly]
    public IResourceBuilder<IResourceWithConnectionString>? Connection { get; set; }
}
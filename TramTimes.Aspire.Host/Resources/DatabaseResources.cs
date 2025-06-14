using JetBrains.Annotations;

namespace TramTimes.Aspire.Host.Resources;

public class DatabaseResources
{
    [UsedImplicitly]
    public IResourceBuilder<PostgresServerResource>? Postgres { get; set; }
    
    [UsedImplicitly]
    public IResourceBuilder<PostgresDatabaseResource>? Database { get; set; }
}
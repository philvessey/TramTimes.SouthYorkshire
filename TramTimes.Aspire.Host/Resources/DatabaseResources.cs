using JetBrains.Annotations;

namespace TramTimes.Aspire.Host.Resources;

public class DatabaseResources
{
    [UsedImplicitly]
    public IResourceBuilder<PostgresServerResource>? PostgresServer { get; set; }
    
    [UsedImplicitly]
    public IResourceBuilder<PostgresDatabaseResource>? PostgresDatabase { get; set; }
    
    [UsedImplicitly]
    public IResourceBuilder<ParameterResource>? TravelineHostname { get; set; }
    
    [UsedImplicitly]
    public IResourceBuilder<ParameterResource>? TravelineUsername { get; set; }
    
    [UsedImplicitly]
    public IResourceBuilder<ParameterResource>? TravelinePassword { get; set; }
}
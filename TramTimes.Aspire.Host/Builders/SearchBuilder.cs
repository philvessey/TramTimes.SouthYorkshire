using Aspire.Hosting.Azure;
using TramTimes.Aspire.Host.Resources;

namespace TramTimes.Aspire.Host.Builders;

public static class SearchBuilder
{
    public static SearchResources BuildSearch(
        this IDistributedApplicationBuilder builder,
        IResourceBuilder<AzureStorageResource> storage,
        IResourceBuilder<AzureBlobStorageContainerResource> container,
        IResourceBuilder<PostgresServerResource> server,
        IResourceBuilder<PostgresDatabaseResource> database) {
        
        #region build result
        
        var result = new SearchResources();
        
        #endregion
        
        #region add elasticsearch
        
        result.Elastic = builder
            .AddElasticsearch(name: "search")
            .WaitFor(dependency: storage)
            .WaitFor(dependency: server)
            .WaitFor(dependency: database)
            .WithDataVolume()
            .WithLifetime(lifetime: ContainerLifetime.Persistent)
            .WithUrlForEndpoint("http", annotation => annotation.DisplayText = "Administration");
        
        #endregion
        
        #region add project
        
        builder
            .AddProject<Projects.TramTimes_Search_Jobs>(name: "search-builder")
            .WaitFor(dependency: result.Elastic)
            .WithParentRelationship(parent: result.Elastic)
            .WithReference(source: container)
            .WithReference(source: database)
            .WithReference(source: result.Elastic);
        
        #endregion
        
        return result;
    }
}
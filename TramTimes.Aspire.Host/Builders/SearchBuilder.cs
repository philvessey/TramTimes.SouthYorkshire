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
        
        result.Elasticsearch = builder
            .AddElasticsearch(name: "search")
            .WaitFor(dependency: storage)
            .WaitFor(dependency: server)
            .WaitFor(dependency: database)
            .WithDataVolume()
            .WithLifetime(lifetime: ContainerLifetime.Persistent)
            .WithUrlForEndpoint(
                callback: annotation => annotation.DisplayText = "Administration",
                endpointName: "http");
        
        #endregion
        
        #region add project
        
        builder
            .AddProject<Projects.TramTimes_Search_Jobs>(name: "search-builder")
            .WaitFor(dependency: result.Elasticsearch)
            .WithParentRelationship(parent: result.Elasticsearch)
            .WithReference(source: container)
            .WithReference(source: database)
            .WithReference(source: result.Elasticsearch);
        
        #endregion
        
        return result;
    }
}
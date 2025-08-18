using TramTimes.Aspire.Host.Resources;

namespace TramTimes.Aspire.Host.Builders;

public static class SearchBuilder
{
    public static SearchResources BuildSearch(
        this IDistributedApplicationBuilder builder,
        StorageResources storage,
        IResourceBuilder<PostgresServerResource> server,
        IResourceBuilder<PostgresDatabaseResource> database) {
        
        #region build result
        
        var result = new SearchResources();
        
        #endregion
        
        #region add elasticsearch
        
        result.Elasticsearch = builder
            .AddElasticsearch(name: "search")
            .WaitFor(dependency: storage.Resource ?? throw new InvalidOperationException(message: "Storage resource is not available."))
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
            .WithReference(source: storage.Resource)
            .WithReference(source: database)
            .WithReference(source: result.Elasticsearch);
        
        #endregion
        
        return result;
    }
}
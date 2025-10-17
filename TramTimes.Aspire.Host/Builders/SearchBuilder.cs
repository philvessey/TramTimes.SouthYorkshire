// ReSharper disable all

using TramTimes.Aspire.Host.Resources;

namespace TramTimes.Aspire.Host.Builders;

public static class SearchBuilder
{
    public static SearchResources BuildSearch(
        this IDistributedApplicationBuilder builder,
        StorageResources storage,
        DatabaseResources database) {
        
        #region build resources
        
        var search = new SearchResources();
        
        #endregion
        
        #region add elasticsearch
        
        if (builder.ExecutionContext.IsRunMode)
            search.Service = builder
                .AddElasticsearch(name: "search")
                .WaitFor(dependency: storage.Blobs ?? throw new InvalidOperationException(message: "Storage blobs are not available."))
                .WaitFor(dependency: storage.Queues ?? throw new InvalidOperationException(message: "Storage queues are not available."))
                .WaitFor(dependency: storage.Tables ?? throw new InvalidOperationException(message: "Storage tables are not available."))
                .WaitFor(dependency: database.Resource ?? throw new InvalidOperationException(message: "Database resource is not available."))
                .WithDataVolume()
                .WithLifetime(lifetime: ContainerLifetime.Persistent)
                .WithUrlForEndpoint(
                    callback: annotation => annotation.DisplayText = "Administration",
                    endpointName: "http");
        
        if (builder.ExecutionContext.IsPublishMode)
            search.Connection = builder.AddConnectionString(name: "search");
        
        #endregion
        
        #region add project
        
        if (builder.ExecutionContext.IsRunMode)
            builder
                .AddProject<Projects.TramTimes_Search_Jobs>(name: "search-builder")
                .WaitFor(dependency: search.Service ?? throw new InvalidOperationException(message: "Search service is not available."))
                .WithParentRelationship(parent: search.Service)
                .WithReference(source: storage.Blobs ?? throw new InvalidOperationException(message: "Storage blobs are not available."))
                .WithReference(source: storage.Queues ?? throw new InvalidOperationException(message: "Storage queues are not available."))
                .WithReference(source: storage.Tables ?? throw new InvalidOperationException(message: "Storage tables are not available."))
                .WithReference(source: database.Resource ?? throw new InvalidOperationException(message: "Database resource is not available."))
                .WithReference(source: search.Service);
        
        if (builder.ExecutionContext.IsPublishMode)
            builder
                .AddProject<Projects.TramTimes_Search_Jobs>(name: "search-builder")
                .WaitFor(dependency: search.Connection ?? throw new InvalidOperationException(message: "Search connection is not available."))
                .WithReference(source: storage.Blobs ?? throw new InvalidOperationException(message: "Storage blobs are not available."))
                .WithReference(source: storage.Queues ?? throw new InvalidOperationException(message: "Storage queues are not available."))
                .WithReference(source: storage.Tables ?? throw new InvalidOperationException(message: "Storage tables are not available."))
                .WithReference(source: database.Connection ?? throw new InvalidOperationException(message: "Database connection is not available."))
                .WithReference(source: search.Connection)
                .PublishAsAzureContainerApp(configure: (infrastructure, app) =>
                {
                    app.Template.Scale.MinReplicas = 1;
                    app.Template.Scale.MaxReplicas = 1;
                });
        
        #endregion
        
        return search;
    }
}
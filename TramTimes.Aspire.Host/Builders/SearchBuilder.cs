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
                .WaitFor(dependency: storage.Resource ?? throw new InvalidOperationException(message: "Storage resource is not available."))
                .WaitFor(dependency: database.Resource ?? throw new InvalidOperationException(message: "Database resource is not available."))
                .WithDataVolume()
                .WithLifetime(lifetime: ContainerLifetime.Persistent)
                .WithUrlForEndpoint(
                    callback: annotation => annotation.DisplayText = "Administration",
                    endpointName: "http");
        
        if (builder.ExecutionContext.IsPublishMode)
            search.Connection = builder.AddConnectionString(name: "search");
        
        #endregion
        
        #region add parameters
        
        IResourceBuilder<ParameterResource>? endpoint = null;
        
        if (builder.ExecutionContext.IsPublishMode)
            endpoint = builder.AddParameter(
                name: "search-endpoint",
                secret: false);
        
        IResourceBuilder<ParameterResource>? key = null;
        
        if (builder.ExecutionContext.IsPublishMode)
            key = builder.AddParameter(
                name: "search-key",
                secret: true);
        
        #endregion
        
        #region add project
        
        if (builder.ExecutionContext.IsRunMode)
            builder
                .AddProject<Projects.TramTimes_Search_Jobs>(name: "search-builder")
                .WaitFor(dependency: search.Service ?? throw new InvalidOperationException(message: "Search service is not available."))
                .WithParentRelationship(parent: search.Service)
                .WithReference(source: storage.Resource ?? throw new InvalidOperationException(message: "Storage resource is not available."))
                .WithReference(source: database.Resource ?? throw new InvalidOperationException(message: "Database resource is not available."))
                .WithReference(source: search.Service);
        
        if (builder.ExecutionContext.IsPublishMode)
            builder
                .AddProject<Projects.TramTimes_Search_Jobs>(name: "search-builder")
                .WaitFor(dependency: search.Connection ?? throw new InvalidOperationException(message: "Search connection is not available."))
                .WaitFor(dependency: endpoint ?? throw new InvalidOperationException(message: "Endpoint parameter is not available."))
                .WaitFor(dependency: key ?? throw new InvalidOperationException(message: "Key parameter is not available."))
                .WithEnvironment(
                    name: "ELASTIC_ENDPOINT",
                    parameter: endpoint)
                .WithEnvironment(
                    name: "ELASTIC_KEY",
                    parameter: key)
                .WithReference(source: storage.Connection ?? throw new InvalidOperationException(message: "Storage connection is not available."))
                .WithReference(source: database.Connection ?? throw new InvalidOperationException(message: "Database connection is not available."))
                .WithReference(source: search.Connection);
        
        #endregion
        
        return search;
    }
}
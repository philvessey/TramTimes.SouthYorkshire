// ReSharper disable all

using TramTimes.Aspire.Host.Extensions;
using TramTimes.Aspire.Host.Resources;

namespace TramTimes.Aspire.Host.Builders;

public static class SearchBuilder
{
    private static readonly string Context = Environment.GetEnvironmentVariable(variable: "ASPIRE_CONTEXT") ?? "Development";
    
    public static SearchResources BuildSearch(
        this IDistributedApplicationBuilder builder,
        DatabaseResources database) {
        
        #region build resources
        
        var search = new SearchResources();
        
        #endregion
        
        #region add elasticsearch
        
        if (builder.ExecutionContext.IsRunMode)
            search.Service = builder
                .AddElasticsearch(name: "search")
                .WaitFor(dependency: database.Builder ?? throw new InvalidOperationException(message: "Database builder is not available."))
                .WithDataVolume()
                .WithEnvironment(
                    name: "xpack.security.enabled",
                    value: "false")
                .WithImageTag(tag: "8.17.3")
                .WithLifetime(lifetime: ContainerLifetime.Persistent)
                .WithUrlForEndpoint(
                    callback: annotation => annotation.DisplayLocation = UrlDisplayLocation.DetailsOnly,
                    endpointName: "http")
                .WithUrlForEndpoint(
                    callback: annotation => annotation.DisplayLocation = UrlDisplayLocation.DetailsOnly,
                    endpointName: "internal");
        
        if (builder.ExecutionContext.IsPublishMode)
            search.Connection = builder.AddConnectionString(name: "search");
        
        #endregion
        
        #region add project
        
        if (builder.ExecutionContext.IsRunMode)
            search.Builder = builder
                .AddProject<Projects.TramTimes_Search_Jobs>(name: "search-builder")
                .WaitFor(dependency: search.Service ?? throw new InvalidOperationException(message: "Search service is not available."))
                .WithEnvironment(
                    name: "ASPIRE_CONTEXT",
                    value: Context)
                .WithParentRelationship(parent: search.Service)
                .WithReference(source: database.Resource ?? throw new InvalidOperationException(message: "Database resource is not available."))
                .WithReference(source: search.Service);
        
        if (builder.ExecutionContext.IsPublishMode)
            search.Builder = builder
                .AddProject<Projects.TramTimes_Search_Jobs>(name: "search-builder")
                .WaitFor(dependency: search.Connection ?? throw new InvalidOperationException(message: "Search connection is not available."))
                .WithEnvironment(
                    name: "ASPIRE_CONTEXT",
                    value: "Production")
                .WithReference(source: database.Connection ?? throw new InvalidOperationException(message: "Database connection is not available."))
                .WithReference(source: search.Connection)
                .PublishAsAzureContainerApp(configure: (infrastructure, app) =>
                {
                    var container = app.Template.Containers.Single().Value;
                    
                    if (container is not null)
                    {
                        container.Resources.Cpu = 0.25;
                        container.Resources.Memory = "0.5Gi";
                    }
                    
                    app.Template.Scale.MinReplicas = 1;
                    app.Template.Scale.MaxReplicas = 1;
                });
        
        #endregion
        
        #region check context
        
        if (Context is "Testing")
            return search;
        
        #endregion
        
        #region add tools
        
        if (builder.ExecutionContext.IsRunMode)
            search.Service?.WithKibana(
                containerName: "search-kibana",
                configureContainer: resource =>
                {
                    resource.WaitFor(dependency: search.Service);
                    resource.WithBuildCommand();
                    resource.WithDeleteCommand();
                    resource.WithEnvironment(
                        name: "xpack.security.enabled",
                        value: "false");
                    resource.WithImageTag(tag: "8.17.3");
                    resource.WithLifetime(lifetime: ContainerLifetime.Session);
                    resource.WithParentRelationship(parent: search.Service);
                    resource.WithUrlForEndpoint(
                        callback: annotation => annotation.DisplayText = "Administration",
                        endpointName: "http");
                });
        
        #endregion
        
        return search;
    }
}
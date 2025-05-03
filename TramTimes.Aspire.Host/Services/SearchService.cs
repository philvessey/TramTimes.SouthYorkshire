using Aspire.Hosting.Azure;

namespace TramTimes.Aspire.Host.Services;

public static class SearchService
{
    public static IDistributedApplicationBuilder AddSearch(
        this IDistributedApplicationBuilder builder,
        IResourceBuilder<AzureBlobStorageResource> blobs,
        IResourceBuilder<PostgresServerResource> server,
        IResourceBuilder<PostgresDatabaseResource> database,
        out IResourceBuilder<ElasticsearchResource> search) {
        
        search = builder.AddElasticsearch(name: "search")
            .WaitFor(dependency: server)
            .WithDataVolume()
            .WithLifetime(lifetime: ContainerLifetime.Persistent)
            .WithUrlForEndpoint("http", annotation => annotation.DisplayText = "Administration");
        
        builder.AddProject<Projects.TramTimes_Search_Jobs>(name: "search-builder")
            .WaitFor(dependency: search)
            .WithParentRelationship(parent: search)
            .WithReference(source: blobs)
            .WithReference(source: database)
            .WithReference(source: search);
        
        return builder;
    }
}
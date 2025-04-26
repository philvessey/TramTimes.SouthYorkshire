using Aspire.Hosting.Azure;

namespace TramTimes.Aspire.Host.Services;

public static class SearchService
{
    public static IDistributedApplicationBuilder AddSearch(
        this IDistributedApplicationBuilder builder,
        IResourceBuilder<AzureBlobStorageResource> blobs,
        IResourceBuilder<PostgresDatabaseResource> database) {
        
        var search = builder.AddElasticsearch(name: "search")
            .WaitFor(dependency: database)
            .WithDataVolume()
            .WithLifetime(lifetime: ContainerLifetime.Persistent);
        
        builder.AddProject<Projects.TramTimes_Search_Jobs>(name: "search-builder")
            .WaitFor(dependency: search)
            .WithParentRelationship(parent: search)
            .WithReference(source: blobs)
            .WithReference(source: database)
            .WithReference(source: search);
        
        return builder;
    }
}
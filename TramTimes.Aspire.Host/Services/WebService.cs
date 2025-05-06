namespace TramTimes.Aspire.Host.Services;

public static class WebService
{
    public static IDistributedApplicationBuilder AddWeb(
        this IDistributedApplicationBuilder builder,
        IResourceBuilder<PostgresDatabaseResource> database,
        IResourceBuilder<RedisResource> cache,
        IResourceBuilder<ElasticsearchResource> search) {
        
        var api = builder.AddProject<Projects.TramTimes_Web_Api>(name: "web-api")
            .WaitFor(dependency: cache)
            .WaitFor(dependency: search)
            .WithExternalHttpEndpoints()
            .WithReference(source: database)
            .WithReference(source: cache)
            .WithReference(source: search)
            .WithUrlForEndpoint("https", annotation => annotation.DisplayText = "Primary")
            .WithUrlForEndpoint("http", annotation => annotation.DisplayText = "Secondary");
        
        builder.AddProject<Projects.TramTimes_Web_Site>(name: "web-site")
            .WaitFor(dependency: api)
            .WithExternalHttpEndpoints()
            .WithReference(source: api)
            .WithUrlForEndpoint("https", annotation => annotation.DisplayText = "Primary")
            .WithUrlForEndpoint("http", annotation => annotation.DisplayText = "Secondary");
        
        return builder;
    }
}
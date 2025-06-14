namespace TramTimes.Aspire.Host.Builders;

public static class WebBuilder
{
    public static void BuildWeb(
        this IDistributedApplicationBuilder builder,
        IResourceBuilder<PostgresServerResource> server,
        IResourceBuilder<PostgresDatabaseResource> database,
        IResourceBuilder<RedisResource> cache,
        IResourceBuilder<ElasticsearchResource> search) {
        
        #region add api
        
        var api = builder
            .AddProject<Projects.TramTimes_Web_Api>(name: "web-api")
            .WaitFor(dependency: server)
            .WaitFor(dependency: database)
            .WaitFor(dependency: cache)
            .WaitFor(dependency: search)
            .WithExternalHttpEndpoints()
            .WithReference(source: database)
            .WithReference(source: cache)
            .WithReference(source: search)
            .WithUrlForEndpoint("https", annotation => annotation.DisplayText = "Primary")
            .WithUrlForEndpoint("http", annotation => annotation.DisplayText = "Secondary");
        
        #endregion
        
        #region add site
        
        builder
            .AddProject<Projects.TramTimes_Web_Site>(name: "web-site")
            .WaitFor(dependency: api)
            .WithEnvironment(
                name: "API_ENDPOINT",
                endpointReference: api.GetEndpoint(name: "https").Exists
                    ? api.GetEndpoint(name: "https")
                    : api.GetEndpoint(name: "http"))
            .WithExternalHttpEndpoints()
            .WithUrlForEndpoint("https", annotation => annotation.DisplayText = "Primary")
            .WithUrlForEndpoint("http", annotation => annotation.DisplayText = "Secondary");
        
        #endregion
    }
}
using Microsoft.Extensions.Hosting;

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
        
        var endpoint = api.GetEndpoint(name: "https");
        
        if (builder.Environment.IsDevelopment())
            endpoint = api.GetEndpoint(name: "http");
        
        builder.AddProject<Projects.TramTimes_Web_Site>(name: "web-site")
            .WaitFor(dependency: api)
            .WithEnvironment(
                name: "API_ENDPOINT",
                endpointReference: endpoint)
            .WithExternalHttpEndpoints()
            .WithUrlForEndpoint("https", annotation => annotation.DisplayText = "Primary")
            .WithUrlForEndpoint("http", annotation => annotation.DisplayText = "Secondary");
        
        return builder;
    }
}
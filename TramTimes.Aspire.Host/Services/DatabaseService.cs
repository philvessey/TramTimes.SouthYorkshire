using Aspire.Hosting.Azure;
using Microsoft.Extensions.Hosting;

namespace TramTimes.Aspire.Host.Services;

public static class DatabaseService
{
    public static IDistributedApplicationBuilder AddDatabase(
        this IDistributedApplicationBuilder builder,
        IResourceBuilder<AzureStorageResource> storage,
        IResourceBuilder<AzureBlobStorageResource> blobs,
        out IResourceBuilder<PostgresDatabaseResource> database) {
        
        var postgres = builder.AddPostgres(name: "postgres")
            .WaitFor(dependency: storage)
            .WithBindMount(
                source: "Scripts",
                target: "/docker-entrypoint-initdb.d")
            .WithDataVolume()
            .WithEnvironment(
                name: "POSTGRES_DB",
                value: "database")
            .WithLifetime(lifetime: ContainerLifetime.Persistent);
        
        if (builder.Environment.IsDevelopment())
        {
            postgres.WithPgAdmin(configureContainer: resource =>
            {
                resource.WithLifetime(lifetime: ContainerLifetime.Persistent);
                resource.WithUrlForEndpoint("http", annotation => annotation.DisplayText = "Administration");
            });
            
            postgres.WithPgWeb(configureContainer: resource =>
            {
                resource.WithLifetime(lifetime: ContainerLifetime.Persistent);
                resource.WithUrlForEndpoint("http", annotation => annotation.DisplayText = "Administration");
            });
        }
        
        database = postgres.AddDatabase(name: "database");
        
        builder.AddProject<Projects.TramTimes_Database_Jobs>(name: "database-builder")
            .WaitFor(dependency: database)
            .WithEnvironment(
                name: "FTP_HOSTNAME",
                parameter: builder.AddParameter(
                    name: "traveline-hostname",
                    secret: true))
            .WithEnvironment(
                name: "FTP_USERNAME",
                parameter: builder.AddParameter(
                    name: "traveline-username",
                    secret: true))
            .WithEnvironment(
                name: "FTP_PASSWORD",
                parameter: builder.AddParameter(
                    name: "traveline-password",
                    secret: true))
            .WithEnvironment(
                name: "LICENSE_KEY",
                parameter: builder.AddParameter(
                    name: "nager-key",
                    secret: true))
            .WithParentRelationship(parent: database)
            .WithReference(source: blobs)
            .WithReference(source: database);
        
        return builder;
    }
}
using Aspire.Hosting.Azure;
using TramTimes.Aspire.Host.Resources;

namespace TramTimes.Aspire.Host.Builders;

public static class DatabaseBuilder
{
    public static DatabaseResources BuildDatabase(
        this IDistributedApplicationBuilder builder,
        IResourceBuilder<AzureStorageResource> storage,
        IResourceBuilder<AzureBlobStorageContainerResource> container) {
        
        #region build result
        
        var result = new DatabaseResources();
        
        #endregion
        
        #region add postgres
        
        result.Postgres = builder
            .AddPostgres(name: "server")
            .WaitFor(dependency: storage)
            .WithBindMount(
                source: "Scripts",
                target: "/docker-entrypoint-initdb.d")
            .WithDataVolume()
            .WithEnvironment(
                name: "POSTGRES_DB",
                value: "database")
            .WithLifetime(lifetime: ContainerLifetime.Persistent)
            .WithPgAdmin(configureContainer: resource =>
            {
                resource.WithLifetime(lifetime: ContainerLifetime.Session);
                resource.WithUrlForEndpoint("http", annotation => annotation.DisplayText = "Administration");
            })
            .WithPgWeb(configureContainer: resource =>
            {
                resource.WithLifetime(lifetime: ContainerLifetime.Session);
                resource.WithUrlForEndpoint("http", annotation => annotation.DisplayText = "Administration");
            });
        
        result.Database = result.Postgres.AddDatabase(name: "database");
        
        #endregion
        
        #region add project
        
        builder
            .AddProject<Projects.TramTimes_Database_Jobs>(name: "database-builder")
            .WaitFor(dependency: result.Postgres)
            .WaitFor(dependency: result.Database)
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
            .WithParentRelationship(parent: result.Database)
            .WithReference(source: container)
            .WithReference(source: result.Database);
        
        #endregion
        
        return result;
    }
}
using Aspire.Hosting.Azure;
using TramTimes.Aspire.Host.Resources;

namespace TramTimes.Aspire.Host.Builders;

public static class DatabaseBuilder
{
    private static readonly string Testing = Environment.GetEnvironmentVariable(variable: "ASPIRE_TESTING") ?? string.Empty;
    
    public static DatabaseResources BuildDatabase(
        this IDistributedApplicationBuilder builder,
        IResourceBuilder<AzureStorageResource> storage,
        IResourceBuilder<AzureBlobStorageContainerResource> container) {
        
        #region build result
        
        var result = new DatabaseResources();
        
        #endregion
        
        #region add server
        
        result.PostgresServer = builder
            .AddPostgres(name: "server")
            .WaitFor(dependency: storage)
            .WithDataVolume()
            .WithInitFiles(source: "./Scripts")
            .WithLifetime(lifetime: ContainerLifetime.Persistent);
        
        #endregion
        
        #region add database
        
        result.PostgresDatabase = result.PostgresServer.AddDatabase(name: "southyorkshire");
        
        #endregion
        
        #region add tools
        
        if (string.IsNullOrEmpty(value: Testing))
            result.PostgresServer
                .WithPgAdmin(
                    containerName: "server-admin",
                    configureContainer: resource =>
                    {
                        resource.WaitFor(result.PostgresServer);
                        resource.WithLifetime(lifetime: ContainerLifetime.Session);
                        resource.WithParentRelationship(parent: result.PostgresServer);
                        resource.WithUrlForEndpoint(
                            callback: annotation => annotation.DisplayText = "Administration",
                            endpointName: "http");
                    })
                .WithPgWeb(
                    containerName: "server-web",
                    configureContainer: resource =>
                    {
                        resource.WaitFor(result.PostgresServer);
                        resource.WithLifetime(lifetime: ContainerLifetime.Session);
                        resource.WithParentRelationship(parent: result.PostgresServer);
                        resource.WithUrlForEndpoint(
                            callback: annotation => annotation.DisplayText = "Administration",
                            endpointName: "http");
                    });
        
        #endregion
        
        #region add parameters
        
        result.TravelineHostname = builder
            .AddParameter(
                name: "transxchange-hostname",
                secret: false)
            .WithParentRelationship(parent: result.PostgresDatabase);
        
        result.NagerKey = builder
            .AddParameter(
                name: "transxchange-userkey",
                secret: true)
            .WithParentRelationship(parent: result.PostgresDatabase);
        
        result.TravelineUsername = builder
            .AddParameter(
                name: "transxchange-username",
                secret: false)
            .WithParentRelationship(parent: result.PostgresDatabase);
        
        result.TravelinePassword = builder
            .AddParameter(
                name: "transxchange-userpass",
                secret: true)
            .WithParentRelationship(parent: result.PostgresDatabase);
        
        #endregion
        
        #region add project
        
        builder
            .AddProject<Projects.TramTimes_Database_Jobs>(name: "transxchange-builder")
            .WaitFor(dependency: result.PostgresServer)
            .WaitFor(dependency: result.PostgresDatabase)
            .WaitFor(dependency: result.TravelineHostname)
            .WaitFor(dependency: result.TravelineUsername)
            .WaitFor(dependency: result.TravelinePassword)
            .WaitFor(dependency: result.NagerKey)
            .WithEnvironment(
                name: "FTP_HOSTNAME",
                parameter: result.TravelineHostname)
            .WithEnvironment(
                name: "FTP_USERNAME",
                parameter: result.TravelineUsername)
            .WithEnvironment(
                name: "FTP_PASSWORD",
                parameter: result.TravelinePassword)
            .WithEnvironment(
                name: "LICENSE_KEY",
                parameter: result.NagerKey)
            .WithParentRelationship(parent: result.PostgresDatabase)
            .WithReference(source: container)
            .WithReference(source: result.PostgresDatabase);
        
        #endregion
        
        return result;
    }
}
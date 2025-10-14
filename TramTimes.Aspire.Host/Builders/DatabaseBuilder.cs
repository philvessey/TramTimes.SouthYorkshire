// ReSharper disable all

using TramTimes.Aspire.Host.Parameters;
using TramTimes.Aspire.Host.Resources;

namespace TramTimes.Aspire.Host.Builders;

public static class DatabaseBuilder
{
    private static readonly string Testing = Environment.GetEnvironmentVariable(variable: "ASPIRE_TESTING") ?? string.Empty;
    
    public static DatabaseResources BuildDatabase(
        this IDistributedApplicationBuilder builder,
        StorageResources storage) {
        
        #region build resources
        
        var database = new DatabaseResources();
        
        #endregion
        
        #region add parameters
        
        database.Parameters = new DatabaseParameters();
        
        if (builder.ExecutionContext.IsPublishMode)
            database.Parameters.Hostname = builder.AddParameter(
                name: "transxchange-hostname",
                secret: false);
        
        if (builder.ExecutionContext.IsPublishMode)
            database.Parameters.Username = builder.AddParameter(
                name: "transxchange-username",
                secret: false);
        
        if (builder.ExecutionContext.IsPublishMode)
            database.Parameters.Userpass = builder.AddParameter(
                name: "transxchange-userpass",
                secret: true);
        
        #endregion
        
        #region add server
        
        if (builder.ExecutionContext.IsRunMode)
            database.Service = builder
                .AddPostgres(name: "server")
                .WaitFor(dependency: storage.Resource ?? throw new InvalidOperationException(message: "Storage resource is not available."))
                .WithDataVolume()
                .WithLifetime(lifetime: ContainerLifetime.Persistent);
        
        #endregion
        
        #region add database
        
        if (builder.ExecutionContext.IsRunMode)
            database.Resource = database.Service?.AddDatabase(name: "southyorkshire"); 
        
        if (builder.ExecutionContext.IsPublishMode)
            database.Connection = builder.AddConnectionString(name: "southyorkshire");
        
        #endregion
        
        #region add tools
        
        if (builder.ExecutionContext.IsRunMode && string.IsNullOrEmpty(value: Testing))
            database.Service?
                .WithPgAdmin(
                    containerName: "server-admin",
                    configureContainer: resource =>
                    {
                        resource.WaitFor(dependency: database.Service);
                        resource.WithLifetime(lifetime: ContainerLifetime.Session);
                        resource.WithParentRelationship(parent: database.Service);
                        resource.WithUrlForEndpoint(
                            callback: annotation => annotation.DisplayText = "Administration",
                            endpointName: "http");
                    })
                .WithPgWeb(
                    containerName: "server-web",
                    configureContainer: resource =>
                    {
                        resource.WaitFor(dependency: database.Service);
                        resource.WithLifetime(lifetime: ContainerLifetime.Session);
                        resource.WithParentRelationship(parent: database.Service);
                        resource.WithUrlForEndpoint(
                            callback: annotation => annotation.DisplayText = "Administration",
                            endpointName: "http");
                    });
        
        #endregion
        
        #region add parameters
        
        if (builder.ExecutionContext.IsRunMode)
            database.Parameters.Hostname = builder
                .AddParameter(
                    name: "transxchange-hostname",
                    secret: false)
                .WithDescription(description: "Hostname for the Traveline FTP server.")
                .WithParentRelationship(parent: database.Resource ?? throw new InvalidOperationException(message: "Database resource is not available."));
        
        if (builder.ExecutionContext.IsRunMode)
            database.Parameters.Username = builder
                .AddParameter(
                    name: "transxchange-username",
                    secret: false)
                .WithDescription(description: "Username for the Traveline FTP server.")
                .WithParentRelationship(parent: database.Resource ?? throw new InvalidOperationException(message: "Database resource is not available."));
        
        if (builder.ExecutionContext.IsRunMode)
            database.Parameters.Userpass = builder
                .AddParameter(
                    name: "transxchange-userpass",
                    secret: true)
                .WithDescription(description: "Password for the Traveline FTP server.")
                .WithParentRelationship(parent: database.Resource ?? throw new InvalidOperationException(message: "Database resource is not available."));
        
        #endregion
        
        #region add project
        
        if (builder.ExecutionContext.IsRunMode)
            builder
                .AddProject<Projects.TramTimes_Database_Jobs>(name: "transxchange-builder")
                .WaitFor(dependency: database.Resource ?? throw new InvalidOperationException(message: "Database resource is not available."))
                .WaitFor(dependency: database.Parameters.Hostname ?? throw new InvalidOperationException(message: "Hostname parameter is not available."))
                .WaitFor(dependency: database.Parameters.Username ?? throw new InvalidOperationException(message: "Username parameter is not available."))
                .WaitFor(dependency: database.Parameters.Userpass ?? throw new InvalidOperationException(message: "Password parameter is not available."))
                .WithEnvironment(
                    name: "FTP_HOSTNAME",
                    parameter: database.Parameters.Hostname)
                .WithEnvironment(
                    name: "FTP_USERNAME",
                    parameter: database.Parameters.Username)
                .WithEnvironment(
                    name: "FTP_PASSWORD",
                    parameter: database.Parameters.Userpass)
                .WithParentRelationship(parent: database.Resource)
                .WithReference(source: storage.Resource ?? throw new InvalidOperationException(message: "Storage resource is not available."))
                .WithReference(source: database.Resource);
        
        if (builder.ExecutionContext.IsPublishMode)
            builder
                .AddProject<Projects.TramTimes_Database_Jobs>(name: "transxchange-builder")
                .WaitFor(dependency: database.Connection ?? throw new InvalidOperationException(message: "Database connection is not available."))
                .WaitFor(dependency: database.Parameters.Hostname ?? throw new InvalidOperationException(message: "Hostname parameter is not available."))
                .WaitFor(dependency: database.Parameters.Username ?? throw new InvalidOperationException(message: "Username parameter is not available."))
                .WaitFor(dependency: database.Parameters.Userpass ?? throw new InvalidOperationException(message: "Password parameter is not available."))
                .WithEnvironment(
                    name: "FTP_HOSTNAME",
                    parameter: database.Parameters.Hostname)
                .WithEnvironment(
                    name: "FTP_USERNAME",
                    parameter: database.Parameters.Username)
                .WithEnvironment(
                    name: "FTP_PASSWORD",
                    parameter: database.Parameters.Userpass)
                .WithReference(source: storage.Resource ?? throw new InvalidOperationException(message: "Storage resource is not available."))
                .WithReference(source: database.Connection)
                .PublishAsAzureContainerApp(configure: (infrastructure, app) =>
                {
                    app.Template.Scale.MinReplicas = 1;
                    app.Template.Scale.MaxReplicas = 1;
                });
        
        #endregion
        
        return database;
    }
}
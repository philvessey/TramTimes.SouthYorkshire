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
        
        IResourceBuilder<ParameterResource>? hostname = null;
        
        if (builder.ExecutionContext.IsRunMode)
            hostname = builder
                .AddParameter(
                    name: "transxchange-hostname",
                    secret: false)
                .WithDescription(description: "Hostname for the Traveline FTP server.")
                .WithParentRelationship(parent: database.Resource ?? throw new InvalidOperationException(message: "Database resource is not available."));
        
        if (builder.ExecutionContext.IsPublishMode)
            hostname = builder.AddParameter(
                name: "transxchange-hostname",
                secret: false);
        
        IResourceBuilder<ParameterResource>? username = null;
        
        if (builder.ExecutionContext.IsRunMode)
            username = builder
                .AddParameter(
                    name: "transxchange-username",
                    secret: false)
                .WithDescription(description: "Username for the Traveline FTP server.")
                .WithParentRelationship(parent: database.Resource ?? throw new InvalidOperationException(message: "Database resource is not available."));
        
        if (builder.ExecutionContext.IsPublishMode)
            username = builder.AddParameter(
                name: "transxchange-username",
                secret: false);
        
        IResourceBuilder<ParameterResource>? userpass = null;
        
        if (builder.ExecutionContext.IsRunMode)
            userpass = builder
                .AddParameter(
                    name: "transxchange-userpass",
                    secret: true)
                .WithDescription(description: "Password for the Traveline FTP server.")
                .WithParentRelationship(parent: database.Resource ?? throw new InvalidOperationException(message: "Database resource is not available."));
        
        if (builder.ExecutionContext.IsPublishMode)
            userpass = builder.AddParameter(
                name: "transxchange-userpass",
                secret: true);
        
        #endregion
        
        #region add project
        
        if (builder.ExecutionContext.IsRunMode)
            builder
                .AddProject<Projects.TramTimes_Database_Jobs>(name: "transxchange-builder")
                .WaitFor(dependency: database.Resource ?? throw new InvalidOperationException(message: "Database resource is not available."))
                .WaitFor(dependency: hostname ?? throw new InvalidOperationException(message: "Hostname parameter is not available."))
                .WaitFor(dependency: username ?? throw new InvalidOperationException(message: "Username parameter is not available."))
                .WaitFor(dependency: userpass ?? throw new InvalidOperationException(message: "Password parameter is not available."))
                .WithEnvironment(
                    name: "FTP_HOSTNAME",
                    parameter: hostname)
                .WithEnvironment(
                    name: "FTP_USERNAME",
                    parameter: username)
                .WithEnvironment(
                    name: "FTP_PASSWORD",
                    parameter: userpass)
                .WithParentRelationship(parent: database.Resource)
                .WithReference(source: storage.Resource ?? throw new InvalidOperationException(message: "Storage resource is not available."))
                .WithReference(source: database.Resource);
        
        if (builder.ExecutionContext.IsPublishMode)
            builder
                .AddProject<Projects.TramTimes_Database_Jobs>(name: "transxchange-builder")
                .WaitFor(dependency: database.Connection ?? throw new InvalidOperationException(message: "Database connection is not available."))
                .WaitFor(dependency: hostname ?? throw new InvalidOperationException(message: "Hostname parameter is not available."))
                .WaitFor(dependency: username ?? throw new InvalidOperationException(message: "Username parameter is not available."))
                .WaitFor(dependency: userpass ?? throw new InvalidOperationException(message: "Password parameter is not available."))
                .WithEnvironment(
                    name: "FTP_HOSTNAME",
                    parameter: hostname)
                .WithEnvironment(
                    name: "FTP_USERNAME",
                    parameter: username)
                .WithEnvironment(
                    name: "FTP_PASSWORD",
                    parameter: userpass)
                .WithReference(source: storage.Connection ?? throw new InvalidOperationException(message: "Storage connection is not available."))
                .WithReference(source: database.Connection);
        
        #endregion
        
        return database;
    }
}
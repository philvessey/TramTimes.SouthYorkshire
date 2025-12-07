// ReSharper disable all

using Azure.Provisioning.AppContainers;
using TramTimes.Aspire.Host.Parameters;
using TramTimes.Aspire.Host.Resources;

namespace TramTimes.Aspire.Host.Builders;

public static class DatabaseBuilder
{
    private static readonly string Context = Environment.GetEnvironmentVariable(variable: "ASPIRE_CONTEXT") ?? "Development";
    
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
                .WaitFor(dependency: storage.Blobs ?? throw new InvalidOperationException(message: "Storage blobs are not available."))
                .WaitFor(dependency: storage.Queues ?? throw new InvalidOperationException(message: "Storage queues are not available."))
                .WaitFor(dependency: storage.Tables ?? throw new InvalidOperationException(message: "Storage tables are not available."))
                .WithDataVolume()
                .WithImageTag(tag: "17.6")
                .WithLifetime(lifetime: ContainerLifetime.Persistent)
                .WithUrlForEndpoint(
                    callback: url => url.DisplayLocation = UrlDisplayLocation.DetailsOnly,
                    endpointName: "tcp");
        
        #endregion
        
        #region add database
        
        if (builder.ExecutionContext.IsRunMode)
            database.Resource = database.Service?.AddDatabase(name: "database");
        
        if (builder.ExecutionContext.IsPublishMode)
            database.Connection = builder.AddConnectionString(name: "database");
        
        #endregion
        
        #region add parameters
        
        database.Parameters = new DatabaseParameters();
        
        if (builder.ExecutionContext.IsRunMode)
            database.Parameters.Hostname = builder
                .AddParameter(
                    name: "transxchange-hostname",
                    value: "tnds.basemap.co.uk")
                .WithDescription(description: "Hostname for the Traveline FTP server.")
                .WithParentRelationship(parent: database.Resource ?? throw new InvalidOperationException(message: "Database resource is not available."));
        
        if (builder.ExecutionContext.IsPublishMode)
            database.Parameters.Hostname = builder.AddParameter(
                name: "transxchange-hostname",
                value: "tnds.basemap.co.uk");
        
        if (builder.ExecutionContext.IsRunMode)
            database.Parameters.Username = builder
                .AddParameter(
                    name: "transxchange-username",
                    secret: false)
                .WithDescription(description: "Username for the Traveline FTP server.")
                .WithParentRelationship(parent: database.Resource ?? throw new InvalidOperationException(message: "Database resource is not available."));
        
        if (builder.ExecutionContext.IsPublishMode)
            database.Parameters.Username = builder.AddParameter(
                name: "transxchange-username",
                secret: false);
        
        if (builder.ExecutionContext.IsRunMode)
            database.Parameters.Userpass = builder
                .AddParameter(
                    name: "transxchange-userpass",
                    secret: true)
                .WithDescription(description: "Password for the Traveline FTP server.")
                .WithParentRelationship(parent: database.Resource ?? throw new InvalidOperationException(message: "Database resource is not available."));
        
        if (builder.ExecutionContext.IsPublishMode)
            database.Parameters.Userpass = builder.AddParameter(
                name: "transxchange-userpass",
                secret: true);
        
        #endregion
        
        #region add project
        
        if (builder.ExecutionContext.IsRunMode)
            database.Builder = builder
                .AddProject<Projects.TramTimes_Database_Jobs>(name: "database-builder")
                .WaitFor(dependency: database.Resource ?? throw new InvalidOperationException(message: "Database resource is not available."))
                .WaitFor(dependency: database.Parameters.Hostname ?? throw new InvalidOperationException(message: "Hostname parameter is not available."))
                .WaitFor(dependency: database.Parameters.Username ?? throw new InvalidOperationException(message: "Username parameter is not available."))
                .WaitFor(dependency: database.Parameters.Userpass ?? throw new InvalidOperationException(message: "Password parameter is not available."))
                .WithEnvironment(
                    name: "ASPIRE_CONTEXT",
                    value: Context)
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
                .WithReference(source: storage.Blobs ?? throw new InvalidOperationException(message: "Storage blobs are not available."))
                .WithReference(source: storage.Queues ?? throw new InvalidOperationException(message: "Storage queues are not available."))
                .WithReference(source: storage.Tables ?? throw new InvalidOperationException(message: "Storage tables are not available."))
                .WithReference(source: database.Resource);
        
        if (builder.ExecutionContext.IsPublishMode)
            database.Builder = builder
                .AddProject<Projects.TramTimes_Database_Jobs>(name: "database-builder")
                .WaitFor(dependency: database.Connection ?? throw new InvalidOperationException(message: "Database connection is not available."))
                .WaitFor(dependency: database.Parameters.Hostname ?? throw new InvalidOperationException(message: "Hostname parameter is not available."))
                .WaitFor(dependency: database.Parameters.Username ?? throw new InvalidOperationException(message: "Username parameter is not available."))
                .WaitFor(dependency: database.Parameters.Userpass ?? throw new InvalidOperationException(message: "Password parameter is not available."))
                .WithEnvironment(
                    name: "ASPIRE_CONTEXT",
                    value: "Production")
                .WithEnvironment(
                    name: "FTP_HOSTNAME",
                    parameter: database.Parameters.Hostname)
                .WithEnvironment(
                    name: "FTP_USERNAME",
                    parameter: database.Parameters.Username)
                .WithEnvironment(
                    name: "FTP_PASSWORD",
                    parameter: database.Parameters.Userpass)
                .WithReference(source: database.Connection)
                .PublishAsAzureContainerAppJob(configure: (infrastructure, job) =>
                {
                    var container = job.Template.Containers.Single().Value;
                    
                    if (container is not null)
                    {
                        container.Resources.Cpu = 1.0;
                        container.Resources.Memory = "2.0Gi";
                    }
                    
                    job.Configuration.TriggerType = ContainerAppJobTriggerType.Schedule;
                    job.Configuration.ScheduleTriggerConfig.CronExpression = "0 3 * * *";
                });
        
        #endregion
        
        #region check context
        
        if (Context is "Testing")
            return database;
        
        #endregion
        
        #region add tools
        
        if (builder.ExecutionContext.IsRunMode)
            database.Service?
                .WithPgAdmin(
                    containerName: "server-admin",
                    configureContainer: resource =>
                    {
                        resource.WaitFor(dependency: database.Service);
                        resource.WithLifetime(lifetime: ContainerLifetime.Session);
                        resource.WithParentRelationship(parent: database.Service);
                        resource.WithUrlForEndpoint(
                            callback: url => url.DisplayText = "Administration",
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
                            callback: url => url.DisplayText = "Administration",
                            endpointName: "http");
                    });
        
        #endregion
        
        return database;
    }
}
using Aspire.Hosting.Azure;
using Azure.Provisioning;
using Azure.Provisioning.Storage;
using Microsoft.Extensions.Hosting;

var builder = DistributedApplication.CreateBuilder(args: args);
var storage = builder.AddAzureStorage(name: "storage");

if (!builder.Environment.IsDevelopment())
{
    var principalType = new ProvisioningParameter(
        bicepIdentifier: AzureBicepResource.KnownParameters.PrincipalType,
        type: typeof(string));
    
    var principalId = new ProvisioningParameter(
        bicepIdentifier: AzureBicepResource.KnownParameters.PrincipalId,
        type: typeof(string));
    
    storage.ConfigureInfrastructure(configure: infrastructure =>
    {
        var storageAccount = infrastructure.GetProvisionableResources()
            .OfType<StorageAccount>()
            .FirstOrDefault(predicate: account => account.BicepIdentifier == "storage") ?? new StorageAccount(bicepIdentifier: "storage");
        
        infrastructure.Add(resource: storageAccount.CreateRoleAssignment(
            role: StorageBuiltInRole.StorageAccountContributor,
            principalType: principalType,
            principalId: principalId));
        
        infrastructure.Add(resource: storageAccount.CreateRoleAssignment(
            role: StorageBuiltInRole.StorageBlobDataOwner,
            principalType: principalType,
            principalId: principalId));
    });
}
else
{
    storage.RunAsEmulator(configureContainer: container =>
    {
        container.WithDataVolume();
        container.WithLifetime(lifetime: ContainerLifetime.Persistent);
    });
}

var blobs = storage.AddBlobs(name: "storage-blobs")
    .WithParentRelationship(parent: storage);

var postgres = builder.AddPostgres(name: "postgres")
    .WithEnvironment(
        name: "POSTGRES_DB",
        value: "database")
    .WithBindMount(
        source: "postgres",
        target: "/docker-entrypoint-initdb.d")
    .WithDataVolume()
    .WithLifetime(lifetime: ContainerLifetime.Persistent)
    .WaitFor(dependency: storage);

postgres.WithPgWeb(configureContainer: container =>
{
    container.WithLifetime(lifetime: ContainerLifetime.Persistent);
    container.WithParentRelationship(parent: postgres);
});

var database = postgres.AddDatabase(name: "database")
    .WithParentRelationship(parent: postgres);

builder.AddProject<Projects.TramTimes_Database_Jobs>(name: "database-jobs")
    .WithEnvironment(
        name: "CRON_SCHEDULE",
        parameter: builder.AddParameter(
            name: "database-schedule",
            secret: false))
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
    .WithEnvironment(
        name: "LOCALITIES_URL",
        parameter: builder.AddParameter(
            name: "localities-url",
            secret: false))
    .WithEnvironment(
        name: "STOPS_URL",
        parameter: builder.AddParameter(
            name: "stops-url",
            secret: false))
    .WithParentRelationship(parent: postgres)
    .WithReference(source: blobs)
    .WithReference(source: database)
    .WaitFor(dependency: database);

var app = builder.Build();
app.Run();
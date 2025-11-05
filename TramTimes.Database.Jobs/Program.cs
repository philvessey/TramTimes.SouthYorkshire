using Azure.Data.Tables;
using Azure.Storage.Blobs;
using Azure.Storage.Queues;
using TramTimes.Database.Jobs.Services;

var builder = Host.CreateApplicationBuilder(args: args);

#region get context

var context = Environment.GetEnvironmentVariable(variable: "ASPIRE_CONTEXT") ?? "Development";

#endregion

#region inject defaults

builder
    .AddScheduleDefaults()
    .AddServiceDefaults();

#endregion

#region inject services

builder.AddNpgsqlDataSource(connectionName: "database");

if (context is not "Production")
    builder.AddAzureBlobServiceClient(connectionName: "storage-blobs");

if (context is not "Production")
    builder.AddAzureQueueServiceClient(connectionName: "storage-queues");

if (context is not "Production")
    builder.AddAzureTableServiceClient(connectionName: "storage-tables");

#endregion

#region configure services

builder.Services.AddHostedService<DatabaseService>();

if (context is not "Production")
    builder.Services.AddSingleton(implementationFactory: provider => provider
        .GetRequiredService<BlobServiceClient>()
        .GetBlobContainerClient(blobContainerName: "uploads"));

if (context is not "Production")
    builder.Services.AddSingleton(implementationFactory: provider => provider
        .GetRequiredService<QueueServiceClient>()
        .GetQueueClient(queueName: "jobs"));

if (context is not "Production")
    builder.Services.AddSingleton(implementationFactory: provider => provider
        .GetRequiredService<TableServiceClient>()
        .GetTableClient(tableName: "Logs"));

if (context is not "Production")
    builder.Services.AddHostedService<BlobService>();

if (context is not "Production")
    builder.Services.AddHostedService<QueueService>();

if (context is not "Production")
    builder.Services.AddHostedService<TableService>();

#endregion

#region build application

var application = builder.Build();

#endregion

application.Run();
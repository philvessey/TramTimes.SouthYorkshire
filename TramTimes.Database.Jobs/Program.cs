using Azure.Data.Tables;
using Azure.Storage.Blobs;
using Azure.Storage.Queues;
using TramTimes.Database.Jobs.Services;

var builder = Host.CreateApplicationBuilder(args: args);

#region inject defaults

builder
    .AddScheduleDefaults()
    .AddServiceDefaults();

#endregion

#region inject services

builder.AddNpgsqlDataSource(connectionName: "database");

if (builder.Environment.IsDevelopment())
    builder.AddAzureBlobServiceClient(connectionName: "storage-blobs");

if (builder.Environment.IsDevelopment())
    builder.AddAzureQueueServiceClient(connectionName: "storage-queues");

if (builder.Environment.IsDevelopment())
    builder.AddAzureTableServiceClient(connectionName: "storage-tables");

#endregion

#region configure services

builder.Services.AddHostedService<DatabaseService>();

if (builder.Environment.IsDevelopment())
    builder.Services.AddSingleton(implementationFactory: provider => provider
        .GetRequiredService<BlobServiceClient>()
        .GetBlobContainerClient(blobContainerName: "uploads"));

if (builder.Environment.IsDevelopment())
    builder.Services.AddSingleton(implementationFactory: provider => provider
        .GetRequiredService<QueueServiceClient>()
        .GetQueueClient(queueName: "jobs"));

if (builder.Environment.IsDevelopment())
    builder.Services.AddSingleton(implementationFactory: provider => provider
        .GetRequiredService<TableServiceClient>()
        .GetTableClient(tableName: "Logs"));

if (builder.Environment.IsDevelopment())
    builder.Services.AddHostedService<BlobService>();

if (builder.Environment.IsDevelopment())
    builder.Services.AddHostedService<QueueService>();

if (builder.Environment.IsDevelopment())
    builder.Services.AddHostedService<TableService>();

#endregion

#region build application

var application = builder.Build();

#endregion

application.Run();
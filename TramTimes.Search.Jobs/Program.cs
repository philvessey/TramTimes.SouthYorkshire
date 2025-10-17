using Azure.Data.Tables;
using Azure.Storage.Blobs;
using Azure.Storage.Queues;
using TramTimes.Search.Jobs.Services;

var builder = Host.CreateApplicationBuilder(args: args);

#region inject defaults

builder
    .AddMapperDefaults()
    .AddScheduleDefaults()
    .AddServiceDefaults();

#endregion

#region inject services

builder.AddAzureBlobServiceClient(connectionName: "storage-blobs");
builder.AddAzureQueueServiceClient(connectionName: "storage-queues");
builder.AddAzureTableServiceClient(connectionName: "storage-tables");
builder.AddNpgsqlDataSource(connectionName: "database");
builder.AddElasticsearchClient(connectionName: "search");

#endregion

#region configure services

builder.Services.AddSingleton(implementationFactory: provider => provider
    .GetRequiredService<BlobServiceClient>()
    .GetBlobContainerClient(blobContainerName: "southyorkshire"));

builder.Services.AddSingleton(implementationFactory: provider => provider
    .GetRequiredService<QueueServiceClient>()
    .GetQueueClient(queueName: "southyorkshire"));

builder.Services.AddSingleton(implementationFactory: provider => provider
    .GetRequiredService<TableServiceClient>()
    .GetTableClient(tableName: "southyorkshire"));

builder.Services.AddHostedService<SearchService>();

#endregion

#region build application

var application = builder.Build();

#endregion

application.Run();
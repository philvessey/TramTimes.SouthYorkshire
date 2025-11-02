using Azure.Data.Tables;
using Azure.Storage.Blobs;
using Azure.Storage.Queues;
using TramTimes.Web.Api.Checks;
using TramTimes.Web.Api.Endpoints;
using TramTimes.Web.Api.Services;

var builder = WebApplication.CreateBuilder(args: args);

#region inject defaults

builder
    .AddMapperDefaults()
    .AddServiceDefaults();

#endregion

#region inject services

builder.AddNpgsqlDataSource(connectionName: "database");
builder.AddRedisClient(connectionName: "cache");
builder.AddElasticsearchClient(connectionName: "search");

if (builder.Environment.IsDevelopment())
    builder.AddAzureBlobServiceClient(connectionName: "storage-blobs");

if (builder.Environment.IsDevelopment())
    builder.AddAzureQueueServiceClient(connectionName: "storage-queues");

if (builder.Environment.IsDevelopment())
    builder.AddAzureTableServiceClient(connectionName: "storage-tables");

#endregion

#region configure services

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

#endregion

#region add checks

builder.Services
    .AddHealthChecks()
    .AddCheck<DatabaseCheck>(name: "database-check");

#endregion

#region add extensions

builder.Services
    .AddOpenApi()
    .AddProblemDetails()
    .AddSwaggerGen();

#endregion

#region build application

var application = builder.Build();
application.UseExceptionHandler();

if (application.Environment.IsDevelopment())
    application
        .UseSwagger()
        .UseSwaggerUI();

application.MapHealthChecks(pattern: "/healthz");

if (application.Environment.IsDevelopment())
    application.MapOpenApi();

application.MapDefaultEndpoints();
application.MapDatabaseEndpoints();
application.MapCacheEndpoints();
application.MapSearchEndpoints();

if (application.Environment.IsDevelopment())
    application.MapWebEndpoints();

#endregion

application.Run();
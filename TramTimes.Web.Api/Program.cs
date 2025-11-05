using Azure.Data.Tables;
using Azure.Storage.Blobs;
using Azure.Storage.Queues;
using TramTimes.Web.Api.Checks;
using TramTimes.Web.Api.Endpoints;
using TramTimes.Web.Api.Services;

var builder = WebApplication.CreateBuilder(args: args);

#region get context

var context = Environment.GetEnvironmentVariable(variable: "ASPIRE_CONTEXT") ?? "Development";

#endregion

#region inject defaults

builder
    .AddMapperDefaults()
    .AddServiceDefaults();

#endregion

#region inject services

builder.AddNpgsqlDataSource(connectionName: "database");
builder.AddRedisClient(connectionName: "cache");
builder.AddElasticsearchClient(connectionName: "search");

if (context is not "Production")
    builder.AddAzureBlobServiceClient(connectionName: "storage-blobs");

if (context is not "Production")
    builder.AddAzureQueueServiceClient(connectionName: "storage-queues");

if (context is not "Production")
    builder.AddAzureTableServiceClient(connectionName: "storage-tables");

#endregion

#region configure services

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

if (context is not "Production")
    application
        .UseSwagger()
        .UseSwaggerUI();

application.MapHealthChecks(pattern: "/healthz");

if (context is not "Production")
    application.MapOpenApi();

application.MapDefaultEndpoints();
application.MapDatabaseEndpoints();
application.MapCacheEndpoints();
application.MapSearchEndpoints();

if (context is not "Production")
    application.MapWebEndpoints();

#endregion

application.Run();
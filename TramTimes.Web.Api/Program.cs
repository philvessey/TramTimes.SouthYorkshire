using Azure.Storage.Blobs;
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

builder.AddAzureBlobServiceClient(connectionName: "storage-blobs");
builder.AddNpgsqlDataSource(connectionName: "southyorkshire");
builder.AddRedisClient(connectionName: "cache");
builder.AddElasticsearchClient(connectionName: "search");

#endregion

#region configure services

builder.Services.AddSingleton(implementationFactory: provider => provider
    .GetRequiredService<BlobServiceClient>()
    .GetBlobContainerClient(blobContainerName: "southyorkshire"));

builder.Services.AddHostedService<StorageService>();
builder.Services.AddHostedService<DatabaseService>();

#endregion

#region add checks

builder.Services
    .AddHealthChecks()
    .AddCheck<DatabaseCheck>(name: "database-check")
    .AddCheck<CacheCheck>(name: "cache-check")
    .AddCheck<SearchCheck>(name: "search-check");

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
{
    application.UseSwagger();
    application.UseSwaggerUI();
}

application.MapHealthChecks(pattern: "/healthz");

if (application.Environment.IsDevelopment())
{
    application.MapOpenApi();
}

application.MapDefaultEndpoints();
application.MapDatabaseEndpoints();
application.MapCacheEndpoints();
application.MapSearchEndpoints();

if (application.Environment.IsDevelopment())
{
    application.MapWebEndpoints();
}

#endregion

application.Run();
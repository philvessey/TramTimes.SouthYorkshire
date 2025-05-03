using Azure.Storage.Blobs;
using StackExchange.Redis;
using TramTimes.Cache.Jobs.Services;

var builder = Host.CreateApplicationBuilder(args: args);
builder.AddMapperDefaults();
builder.AddScheduleDefaults();
builder.AddServiceDefaults();

builder.AddAzureBlobClient(connectionName: "blobs");
builder.AddNpgsqlDataSource(connectionName: "database");
builder.AddRedisClient(connectionName: "cache");

var application = builder.Build();
var scope = application.Services.CreateScope();

var blobService = scope.ServiceProvider.GetRequiredService<BlobServiceClient>();
var cacheService = scope.ServiceProvider.GetRequiredService<IConnectionMultiplexer>();

await blobService.GetBlobContainerClient(blobContainerName: "cache")
    .CreateIfNotExistsAsync();

await cacheService.GetDatabase()
    .ExecuteAsync(command: "FLUSHDB");

application.Run();
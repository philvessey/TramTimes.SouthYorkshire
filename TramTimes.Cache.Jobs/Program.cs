using Azure.Storage.Blobs;
using TramTimes.Cache.Jobs.Services;

var builder = Host.CreateApplicationBuilder(args: args);
builder.AddScheduleDefaults();
builder.AddServiceDefaults();

builder.AddAzureBlobClient(connectionName: "blobs");
builder.AddNpgsqlDataSource(connectionName: "database");
builder.AddRedisClient(connectionName: "cache");

var application = builder.Build();
var scope = application.Services.CreateScope();

await scope.ServiceProvider.GetRequiredService<BlobServiceClient>()
    .GetBlobContainerClient(blobContainerName: "cache")
    .CreateIfNotExistsAsync();

application.Run();
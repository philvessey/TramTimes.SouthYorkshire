using Azure.Storage.Blobs;
using TramTimes.Database.Jobs.Services;

var builder = Host.CreateApplicationBuilder(args: args);
builder.AddMapperDefaults();
builder.AddScheduleDefaults();
builder.AddServiceDefaults();

builder.AddAzureBlobClient(connectionName: "blobs");
builder.AddNpgsqlDataSource(connectionName: "database");

var application = builder.Build();
var scope = application.Services.CreateScope();

await scope.ServiceProvider.GetRequiredService<BlobServiceClient>()
    .GetBlobContainerClient(blobContainerName: "database")
    .CreateIfNotExistsAsync();

application.Run();
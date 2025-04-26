using Azure.Storage.Blobs;
using Elastic.Clients.Elasticsearch;
using TramTimes.Search.Jobs.Services;

var builder = Host.CreateApplicationBuilder(args: args);
builder.AddMapperDefaults();
builder.AddScheduleDefaults();
builder.AddServiceDefaults();

builder.AddAzureBlobClient(connectionName: "blobs");
builder.AddNpgsqlDataSource(connectionName: "database");
builder.AddElasticsearchClient(connectionName: "search");

var application = builder.Build();
var scope = application.Services.CreateScope();

var blobService = scope.ServiceProvider.GetRequiredService<BlobServiceClient>();
var searchService = scope.ServiceProvider.GetRequiredService<ElasticsearchClient>();

await blobService
    .GetBlobContainerClient(blobContainerName: "search")
    .CreateIfNotExistsAsync();

await searchService.Indices
    .CreateAsync(index: "search");

application.Run();
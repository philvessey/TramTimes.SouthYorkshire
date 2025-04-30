using Azure.Storage.Blobs;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Mapping;
using TramTimes.Search.Jobs.Models;
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

var response = await searchService.Indices
    .ExistsAsync(indices:"search");

if (response.Exists)
    await searchService.Indices
        .DeleteAsync(indices: "search");

await searchService.Indices
    .CreateAsync<SearchStop>(
        index: "search",
        configureRequest: request => request
            .Mappings(configure: map => map
                .Properties(properties: new Properties<SearchStop>
                {
                    { "code", new TextProperty() },
                    { "id", new KeywordProperty() },
                    { "latitude", new DoubleNumberProperty() },
                    { "location", new GeoPointProperty() },
                    { "longitude", new DoubleNumberProperty() },
                    { "name", new TextProperty() },
                    { "points", new ObjectProperty() }
                })));

application.Run();
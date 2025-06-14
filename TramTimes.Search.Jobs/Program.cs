using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Mapping;
using TramTimes.Search.Jobs.Models;
using TramTimes.Search.Jobs.Services;

var builder = Host.CreateApplicationBuilder(args: args);

#region inject defaults

builder
    .AddMapperDefaults()
    .AddScheduleDefaults()
    .AddServiceDefaults();

#endregion

#region inject services

builder.AddAzureBlobContainerClient(connectionName: "search-storage");
builder.AddNpgsqlDataSource(connectionName: "database");
builder.AddElasticsearchClient(connectionName: "search");

#endregion

#region create scope

var application = builder.Build();
var scope = application.Services.CreateScope();

#endregion

#region create index

await scope.ServiceProvider
    .GetRequiredService<ElasticsearchClient>().Indices
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
                    { "platform", new TextProperty() },
                    { "points", new ObjectProperty() }
                })));

#endregion

application.Run();
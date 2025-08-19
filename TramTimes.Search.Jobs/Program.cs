using Azure.Storage.Blobs;
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
builder.AddNpgsqlDataSource(connectionName: "southyorkshire");

if (builder.Environment.IsDevelopment())
    builder.AddElasticsearchClient(connectionName: "search");

if (builder.Environment.IsProduction())
    builder.AddElasticsearchClient(
        connectionName: "search",
        configureSettings: settings =>
        {
            settings.ApiKey = Environment.GetEnvironmentVariable(variable: "ELASTIC_KEY") ?? string.Empty;
            settings.Endpoint = new Uri(uriString: Environment.GetEnvironmentVariable(variable: "ELASTIC_ENDPOINT") ?? string.Empty);
        });

#endregion

#region configure services

builder.Services.AddSingleton(implementationFactory: provider => provider
    .GetRequiredService<BlobServiceClient>()
    .GetBlobContainerClient(blobContainerName: "southyorkshire"));

builder.Services.AddHostedService<StorageService>();
builder.Services.AddHostedService<DatabaseService>();
builder.Services.AddHostedService<SearchService>();

#endregion

#region build application

var application = builder.Build();

#endregion

application.Run();
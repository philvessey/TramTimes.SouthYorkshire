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
builder.Services.AddHostedService<CacheService>();
builder.Services.AddHostedService<SearchService>();

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
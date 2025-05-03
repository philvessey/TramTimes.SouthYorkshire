using TramTimes.Web.Api.Services;

var builder = WebApplication.CreateBuilder(args: args);
builder.AddMapperDefaults();
builder.AddServiceDefaults();

builder.AddNpgsqlDataSource(connectionName: "database");
builder.AddRedisClient(connectionName: "cache");
builder.AddElasticsearchClient(connectionName: "search");

builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();

var application = builder.Build();
application.UseExceptionHandler();

if (application.Environment.IsDevelopment())
{
    application.MapOpenApi();
}

application.MapDefaultEndpoints();
application.MapDatabaseEndpoints();
application.MapCacheEndpoints();
application.MapSearchEndpoints();
application.Run();
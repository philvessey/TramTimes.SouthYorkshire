using TramTimes.Web.Api.Services;

var builder = WebApplication.CreateBuilder(args: args);

#region inject defaults

builder
    .AddMapperDefaults()
    .AddServiceDefaults();

#endregion

#region inject services

builder.AddNpgsqlDataSource(connectionName: "database");
builder.AddRedisClient(connectionName: "cache");
builder.AddElasticsearchClient(connectionName: "search");

#endregion

#region add extensions

builder.Services
    .AddOpenApi()
    .AddProblemDetails();

#endregion

#region map endpoints

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

#endregion

application.Run();
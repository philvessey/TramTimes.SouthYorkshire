using TramTimes.Search.Jobs.Extensions;
using TramTimes.Search.Jobs.Services;

var builder = Host.CreateApplicationBuilder(args: args);

#region get context

var context = Environment.GetEnvironmentVariable(variable: "ASPIRE_CONTEXT") ?? "Development";

#endregion

#region inject defaults

builder
    .AddScheduleDefaults()
    .AddServiceDefaults();

#endregion

#region inject services

builder.AddNpgsqlDataSource(connectionName: "database");
builder.AddElasticsearchClient(connectionName: "search");

#endregion

#region configure services

var licenseKey = Environment.GetEnvironmentVariable(variable: "AUTOMAPPER_LICENSE") ?? string.Empty;

builder.Services.AddAutoMapper(configAction: expression =>
{
    expression.LicenseKey = licenseKey;
    expression.AddProfile<MapperService>();
});

if (context is "Production")
    builder.Services.AddHostedJobs();

#endregion

#region build application

var application = builder.Build();

#endregion

application.Run();
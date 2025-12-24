using TramTimes.Cache.Jobs.Extensions;
using TramTimes.Cache.Jobs.Services;

var builder = Host.CreateApplicationBuilder(args: args);

#region get context

var context = Environment.GetEnvironmentVariable(variable: "ASPIRE_CONTEXT") ?? "Development";

#endregion

#region inject defaults

builder
    .AddMapperDefaults()
    .AddScheduleDefaults()
    .AddServiceDefaults();

#endregion

#region inject services

builder.AddNpgsqlDataSource(connectionName: "database");
builder.AddRedisClient(connectionName: "cache");

#endregion

#region configure services

if (context is "Production")
    builder.Services.AddHostedJobs();

#endregion

#region build application

var application = builder.Build();

#endregion

application.Run();
using TramTimes.Cache.Jobs.Services;

var builder = Host.CreateApplicationBuilder(args: args);

#region inject defaults

builder
    .AddMapperDefaults()
    .AddScheduleDefaults()
    .AddServiceDefaults();

#endregion

#region inject services

builder.AddAzureBlobContainerClient(connectionName: "southyorkshire");
builder.AddNpgsqlDataSource(connectionName: "database");
builder.AddRedisClient(connectionName: "cache");

#endregion

#region configure services

builder.Services.AddHostedService<StartupService>();

#endregion

#region build application

var application = builder.Build();

#endregion

application.Run();
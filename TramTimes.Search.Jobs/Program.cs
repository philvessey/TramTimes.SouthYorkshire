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

#region configure services

builder.Services.AddHostedService<StartupService>();

#endregion

#region build application

var application = builder.Build();

#endregion

application.Run();
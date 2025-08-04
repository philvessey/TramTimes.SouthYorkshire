using TramTimes.Search.Jobs.Services;

var builder = Host.CreateApplicationBuilder(args: args);

#region inject defaults

builder
    .AddMapperDefaults()
    .AddScheduleDefaults()
    .AddServiceDefaults();

#endregion

#region inject services

builder.AddAzureBlobContainerClient(connectionName: "storage-blobs-southyorkshire");
builder.AddNpgsqlDataSource(connectionName: "southyorkshire");
builder.AddElasticsearchClient(connectionName: "search");

#endregion

#region configure services

builder.Services.AddHostedService<StartupService>();

#endregion

#region build application

var application = builder.Build();

#endregion

application.Run();
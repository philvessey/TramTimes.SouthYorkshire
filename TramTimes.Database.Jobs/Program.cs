using TramTimes.Database.Jobs.Services;

var builder = Host.CreateApplicationBuilder(args: args);

#region inject defaults

builder
    .AddMapperDefaults()
    .AddScheduleDefaults()
    .AddServiceDefaults();

#endregion

#region inject services

builder.AddAzureBlobContainerClient(connectionName: "database-storage");
builder.AddNpgsqlDataSource(connectionName: "database");

#endregion

#region configure services

builder.Services.AddHostedService<StartupService>();

#endregion

#region build application

var application = builder.Build();

#endregion

application.Run();
using TramTimes.Web.Jobs.Services;

var builder = Host.CreateApplicationBuilder(args: args);

#region inject defaults

builder
    .AddScheduleDefaults()
    .AddServiceDefaults();

#endregion

#region inject services

builder.AddAzureBlobContainerClient(connectionName: "storage-blobs-southyorkshire");

#endregion

#region configure services

builder.Services.AddHostedService<StartupService>();

#endregion

#region build application

var application = builder.Build();

#endregion

application.Run();
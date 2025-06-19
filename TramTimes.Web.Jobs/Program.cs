using TramTimes.Web.Jobs.Services;

var builder = Host.CreateApplicationBuilder(args: args);

#region inject defaults

builder
    .AddScheduleDefaults()
    .AddServiceDefaults();

#endregion

#region inject services

builder.AddAzureBlobContainerClient(connectionName: "web-storage");

#endregion

#region create scope

var application = builder.Build();

#endregion

application.Run();
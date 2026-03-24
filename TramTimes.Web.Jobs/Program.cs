using TramTimes.Web.Jobs.Services;

var builder = Host.CreateApplicationBuilder(args: args);

#region add playwright

builder.Services.AddHostedService<PlaywrightService>();

#endregion

#region build application

var application = builder.Build();

#endregion

application.Run();
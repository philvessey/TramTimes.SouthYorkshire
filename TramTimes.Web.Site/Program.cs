using Blazored.LocalStorage;
using Microsoft.AspNetCore.SignalR;
using TramTimes.Web.Site.Components;
using TramTimes.Web.Site.Services;

var builder = WebApplication.CreateBuilder(args: args);

#region inject defaults

builder
    .AddMapperDefaults()
    .AddServiceDefaults();

#endregion

#region inject services

builder.Services
    .AddBlazoredLocalStorage()
    .AddTelerikBlazor();

#endregion

#region configure services

builder.Services.Configure<HubOptions>(configureOptions: options =>
{
    options.MaximumReceiveMessageSize = 1024 * 1024;
});

#endregion

#region add components

builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents();

#endregion

#region map components

var application = builder.Build();
application.UseAntiforgery();
application.UseHttpsRedirection();
application.UseStaticFiles();

if (!application.Environment.IsDevelopment())
{
    application.UseHsts();
}

application.MapStaticAssets();

var endpoint = application.MapRazorComponents<Site>();
endpoint.AddInteractiveServerRenderMode();

#endregion

application.Run();
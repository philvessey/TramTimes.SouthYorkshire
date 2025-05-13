using Blazored.LocalStorage;
using Microsoft.AspNetCore.SignalR;
using TramTimes.Web.Site.Components;
using TramTimes.Web.Site.Services;

var builder = WebApplication.CreateBuilder(args: args);
builder.AddMapperDefaults();
builder.AddServiceDefaults();

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddTelerikBlazor();

var components = builder.Services.AddRazorComponents();
components.AddInteractiveServerComponents();

builder.Services.Configure<HubOptions>(configureOptions: options =>
{
    options.MaximumReceiveMessageSize = 1024 * 1024;
});

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

application.Run();
using Blazored.LocalStorage;
using TramTimes.Web.Site.Components;

var builder = WebApplication.CreateBuilder(args: args);
builder.AddServiceDefaults();

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddTelerikBlazor();
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var application = builder.Build();
application.UseAntiforgery();
application.UseHttpsRedirection();
application.UseStaticFiles();

if (!application.Environment.IsDevelopment())
{
    application.UseHsts();
}

application.MapStaticAssets();
application.MapRazorComponents<Site>()
    .AddInteractiveServerRenderMode();

application.Run();
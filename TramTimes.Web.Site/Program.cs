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

builder.Services.Configure<CookiePolicyOptions>(configureOptions: options =>
{
    options.CheckConsentNeeded = _ => true;
    options.ConsentCookieValue = "true";
    options.MinimumSameSitePolicy = SameSiteMode.Strict;
    options.Secure = CookieSecurePolicy.SameAsRequest;
});

builder.Services.Configure<HubOptions>(configureOptions: options =>
{
    options.MaximumReceiveMessageSize = 1024 * 1024;
});

#endregion

#region add components

builder.Services
    .AddHttpContextAccessor()
    .AddRazorComponents()
    .AddInteractiveServerComponents();

#endregion

#region build application

var application = builder.Build();
application.UseAntiforgery();
application.UseCookiePolicy();
application.UseHttpsRedirection();
application.UseStaticFiles();

if (!application.Environment.IsDevelopment())
    application.UseHsts();

application.MapStaticAssets();

var endpoint = application.MapRazorComponents<Site>();
endpoint.AddInteractiveServerRenderMode();

#endregion

application.Run();
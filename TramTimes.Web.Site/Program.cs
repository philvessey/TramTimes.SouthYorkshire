using System.Net;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.SignalR;
using Polly;
using Polly.Timeout;
using TramTimes.Web.Site.Components;
using TramTimes.Web.Site.Services;

var builder = WebApplication.CreateBuilder(args: args);

#region get context

var context = Environment.GetEnvironmentVariable(variable: "ASPIRE_CONTEXT") ?? "Development";

#endregion

#region inject defaults

builder
    .AddMapperDefaults()
    .AddServiceDefaults();

#endregion

#region configure defaults

var retry = Policy
    .Handle<HttpRequestException>()
    .OrResult<HttpResponseMessage>(response => response.StatusCode is
        HttpStatusCode.InternalServerError or
        HttpStatusCode.NotImplemented or
        HttpStatusCode.BadGateway or
        HttpStatusCode.ServiceUnavailable or
        HttpStatusCode.GatewayTimeout)
    .RetryAsync(retryCount: 10);

var timeout = Policy.TimeoutAsync<HttpResponseMessage>(
    timeout: TimeSpan.FromSeconds(seconds: 10),
    timeoutStrategy: TimeoutStrategy.Pessimistic);

#endregion

#region inject services

builder.Services
    .AddHttpClient(name: "api")
    .AddPolicyHandler(policy: Policy.WrapAsync(policies: [retry, timeout]));

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

#region add checks

builder.Services.AddHealthChecks();

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
application.UseStaticFiles();

if (context is "Production")
    application
        .UseHttpsRedirection()
        .UseHsts();

application.MapDefaultEndpoints();
application.MapHealthChecks(pattern: "/healthz");
application.MapStaticAssets();

var endpoint = application.MapRazorComponents<Site>();
endpoint.AddInteractiveServerRenderMode();

#endregion

application.Run();
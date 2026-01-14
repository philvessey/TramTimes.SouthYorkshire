using System.Net;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Http.Resilience;
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

#region inject services

builder.Services
    .AddHttpClient(name: "api")
    .AddStandardResilienceHandler();

builder.Services.AddTelerikBlazor();

#endregion

#region configure services

builder.Services.Configure<CookiePolicyOptions>(configureOptions: options =>
{
    options.CheckConsentNeeded = _ => true;
    options.ConsentCookieValue = "true";
    options.MinimumSameSitePolicy = SameSiteMode.Strict;
    options.Secure = CookieSecurePolicy.SameAsRequest;
});

builder.Services.Configure<HttpStandardResilienceOptions>(name: "api", configureOptions: options =>
{
    options.AttemptTimeout.Timeout = TimeSpan.FromSeconds(seconds: 10);
    options.TotalRequestTimeout.Timeout = TimeSpan.FromMinutes(minutes: 5);
    options.Retry.BackoffType = DelayBackoffType.Exponential;
    options.Retry.MaxRetryAttempts = 9;
    options.Retry.UseJitter = true;
    options.Retry.ShouldHandle = arguments => ValueTask.FromResult(result:
        arguments.Outcome.Exception is TimeoutException ||
        arguments.Outcome.Exception is TimeoutRejectedException ||
        arguments.Outcome.Result?.StatusCode is
            HttpStatusCode.RequestTimeout or
            HttpStatusCode.TooManyRequests or
            HttpStatusCode.InternalServerError or
            HttpStatusCode.BadGateway or
            HttpStatusCode.ServiceUnavailable or
            HttpStatusCode.GatewayTimeout);
});

builder.Services.Configure<HubOptions>(configureOptions: options =>
{
    options.MaximumReceiveMessageSize = 1024 * 1024;
});

builder.Services.AddHostedService<ResilienceService>();

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
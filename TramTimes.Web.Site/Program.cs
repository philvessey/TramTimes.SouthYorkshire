using System.Net;
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

#region inject services

builder.Services
    .AddHttpClient(name: "cache")
    .AddStandardResilienceHandler(configure: options =>
    {
        options.AttemptTimeout.Timeout = TimeSpan.FromSeconds(seconds: 2);
        options.TotalRequestTimeout.Timeout = TimeSpan.FromSeconds(seconds: 5);
        options.CircuitBreaker.BreakDuration = TimeSpan.FromSeconds(seconds: 30);
        options.CircuitBreaker.FailureRatio = 0.50;
        options.CircuitBreaker.MinimumThroughput = 10;
        options.CircuitBreaker.SamplingDuration = TimeSpan.FromSeconds(seconds: 30);
        options.Retry.BackoffType = DelayBackoffType.Constant;
        options.Retry.Delay = TimeSpan.FromMilliseconds(milliseconds: 50);
        options.Retry.MaxRetryAttempts = 1;
        options.Retry.UseJitter = false;
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

builder.Services
    .AddHttpClient(name: "database")
    .AddStandardResilienceHandler(configure: options =>
    {
        options.AttemptTimeout.Timeout = TimeSpan.FromSeconds(seconds: 10);
        options.TotalRequestTimeout.Timeout = TimeSpan.FromSeconds(seconds: 115);
        options.CircuitBreaker.BreakDuration = TimeSpan.FromSeconds(seconds: 60);
        options.CircuitBreaker.FailureRatio = 0.75;
        options.CircuitBreaker.MinimumThroughput = 5;
        options.CircuitBreaker.SamplingDuration = TimeSpan.FromSeconds(seconds: 60);
        options.Retry.BackoffType = DelayBackoffType.Exponential;
        options.Retry.Delay = TimeSpan.FromMilliseconds(milliseconds: 1000);
        options.Retry.MaxRetryAttempts = 4;
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

builder.Services
    .AddHttpClient(name: "search")
    .AddStandardResilienceHandler(configure: options =>
    {
        options.AttemptTimeout.Timeout = TimeSpan.FromSeconds(seconds: 2);
        options.TotalRequestTimeout.Timeout = TimeSpan.FromSeconds(seconds: 5);
        options.CircuitBreaker.BreakDuration = TimeSpan.FromSeconds(seconds: 30);
        options.CircuitBreaker.FailureRatio = 0.50;
        options.CircuitBreaker.MinimumThroughput = 10;
        options.CircuitBreaker.SamplingDuration = TimeSpan.FromSeconds(seconds: 30);
        options.Retry.BackoffType = DelayBackoffType.Constant;
        options.Retry.Delay = TimeSpan.FromMilliseconds(milliseconds: 50);
        options.Retry.MaxRetryAttempts = 1;
        options.Retry.UseJitter = false;
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
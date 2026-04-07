using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.SignalR;
using StackExchange.Redis;
using TramTimes.Web.Site.Components;
using TramTimes.Web.Site.Services;

var builder = WebApplication.CreateBuilder(args: args);

#region get context

var context = Environment.GetEnvironmentVariable(variable: "ASPIRE_CONTEXT") ?? "Development";

#endregion

#region inject defaults

builder.AddServiceDefaults();

#endregion

#region inject services

builder.AddNpgsqlDataSource(connectionName: "database");
builder.AddRedisClient(connectionName: "cache");
builder.AddElasticsearchClient(connectionName: "search");

var connectionString = builder.Configuration.GetConnectionString(name: "cache") ?? string.Empty;

if (!string.IsNullOrEmpty(value: connectionString))
    builder.Services
        .AddDataProtection()
        .PersistKeysToStackExchangeRedis(
            connectionMultiplexer: ConnectionMultiplexer.Connect(configuration: connectionString),
            key: "browser:southyorkshire")
        .SetApplicationName(applicationName: "web-site");

if (string.IsNullOrEmpty(value: connectionString))
    builder.Services
        .AddDataProtection()
        .SetApplicationName(applicationName: "web-site");

builder.Services.AddTelerikBlazor();

#endregion

#region configure services

var licenseKey = Environment.GetEnvironmentVariable(variable: "AUTOMAPPER_LICENSE") ?? string.Empty;

builder.Services.AddAutoMapper(configAction: expression =>
{
    expression.LicenseKey = licenseKey;
    expression.AddProfile<MapperService>();
});

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

#region add scoped

builder.Services.AddScoped<CacheService>();
builder.Services.AddScoped<ConsentService>();
builder.Services.AddScoped<DatabaseService>();
builder.Services.AddScoped<LayoutService>();
builder.Services.AddScoped<SchemeService>();
builder.Services.AddScoped<SearchService>();
builder.Services.AddScoped<StorageService>();

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
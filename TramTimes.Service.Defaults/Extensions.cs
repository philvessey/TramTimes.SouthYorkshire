// ReSharper disable all

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ServiceDiscovery;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

namespace Microsoft.Extensions.Hosting;

public static class Extensions
{
    private const string HealthEndpointPath = "/health";
    private const string AlivenessEndpointPath = "/alive";
    
    public static TBuilder AddServiceDefaults<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        builder.ConfigureOpenTelemetry();
        builder.AddDefaultHealthChecks();
        
        builder.Services.AddServiceDiscovery();
        builder.Services.ConfigureHttpClientDefaults(configure: http =>
        {
            http.AddStandardResilienceHandler();
            http.AddServiceDiscovery();
        });
        
        return builder;
    }
    
    public static TBuilder ConfigureOpenTelemetry<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        builder.Logging.AddOpenTelemetry(configure: logging =>
        {
            logging.IncludeFormattedMessage = true;
            logging.IncludeScopes = true;
        });
        
        builder.Services.AddOpenTelemetry()
            .WithMetrics(configure: metrics =>
            {
                metrics.AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddRuntimeInstrumentation();
            })
            .WithTracing(configure: tracing =>
            {
                tracing.AddSource(builder.Environment.ApplicationName)
                    .AddAspNetCoreInstrumentation(options =>
                        options.Filter = context =>
                            !context.Request.Path.StartsWithSegments(other: HealthEndpointPath) &&
                            !context.Request.Path.StartsWithSegments(other: AlivenessEndpointPath))
                    .AddHttpClientInstrumentation();
            });
        
        builder.AddOpenTelemetryExporters();
        
        return builder;
    }
    
    private static TBuilder AddOpenTelemetryExporters<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        var useOtlpExporter = !string.IsNullOrWhiteSpace(value: builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"]);
        
        if (useOtlpExporter)
        {
            builder.Services.AddOpenTelemetry()
                .UseOtlpExporter();
        }
        
        return builder;
    }
    
    public static TBuilder AddDefaultHealthChecks<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        builder.Services.AddHealthChecks()
            .AddCheck(
                name: "self",
                tags: ["live"],
                check: () => HealthCheckResult.Healthy());
        
        return builder;
    }
    
    public static WebApplication MapDefaultEndpoints(this WebApplication application)
    {
        if (application.Environment.IsDevelopment())
        {
            application.MapHealthChecks(pattern: HealthEndpointPath);
            application.MapHealthChecks(
                pattern: AlivenessEndpointPath,
                options: new HealthCheckOptions
                {
                    Predicate = registration => registration.Tags.Contains(item: "live")
                });
        }
        
        return application;
    }
}
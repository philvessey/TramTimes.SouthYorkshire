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
        #region inject defaults
        
        builder
            .ConfigureOpenTelemetry()
            .AddDefaultHealthChecks();
        
        #endregion
        
        #region inject services
        
        builder.Services
            .AddServiceDiscovery()
            .ConfigureHttpClientDefaults(configure: http =>
            {
                http.AddStandardResilienceHandler();
                http.AddServiceDiscovery();
            });
        
        #endregion
        
        return builder;
    }
    
    public static TBuilder ConfigureOpenTelemetry<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        #region inject defaults
        
        builder.AddOpenTelemetryExporters();
        
        #endregion
        
        #region inject logging
        
        builder.Logging.AddOpenTelemetry(configure: logging =>
        {
            logging.IncludeFormattedMessage = true;
            logging.IncludeScopes = true;
        });
        
        #endregion
        
        #region inject services
        
        builder.Services
            .AddOpenTelemetry()
            .WithMetrics(configure: metrics =>
            {
                metrics.AddAspNetCoreInstrumentation();
                metrics.AddHttpClientInstrumentation();
                metrics.AddRuntimeInstrumentation();
            })
            .WithTracing(configure: tracing =>
            {
                tracing.AddSource(names: builder.Environment.ApplicationName);
                tracing.AddAspNetCoreInstrumentation(configureAspNetCoreTraceInstrumentationOptions: options =>
                    options.Filter = context =>
                        !context.Request.Path.StartsWithSegments(other: HealthEndpointPath) &&
                        !context.Request.Path.StartsWithSegments(other: AlivenessEndpointPath));
                tracing.AddHttpClientInstrumentation();
            });
        
        #endregion
        
        return builder;
    }
    
    private static TBuilder AddOpenTelemetryExporters<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        #region inject services
        
        if (!string.IsNullOrWhiteSpace(value: builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"]))
            builder.Services
                .AddOpenTelemetry()
                .UseOtlpExporter();
        
        #endregion
        
        return builder;
    }
    
    public static TBuilder AddDefaultHealthChecks<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        #region inject services
        
        builder.Services
            .AddHealthChecks()
            .AddCheck(
                name: "self",
                tags: ["live"],
                check: () => HealthCheckResult.Healthy());
        
        #endregion
        
        return builder;
    }
    
    public static WebApplication MapDefaultEndpoints(this WebApplication application)
    {
        #region map endpoints
        
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
        
        #endregion
        
        return application;
    }
}
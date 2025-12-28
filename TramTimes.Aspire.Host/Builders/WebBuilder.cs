// ReSharper disable all

using Azure.Provisioning;
using Azure.Provisioning.AppContainers;
using TramTimes.Aspire.Host.Parameters;
using TramTimes.Aspire.Host.Resources;

namespace TramTimes.Aspire.Host.Builders;

public static class WebBuilder
{
    private static readonly string _context = Environment.GetEnvironmentVariable(variable: "ASPIRE_CONTEXT") ?? "Development";

    public static void BuildWeb(
        this IDistributedApplicationBuilder builder,
        StorageResources storage,
        DatabaseResources database,
        CacheResources cache,
        SearchResources search) {

        #region build resources

        var web = new WebResources();

        #endregion

        #region add parameters

        web.Parameters = new WebParameters();

        if (builder.ExecutionContext.IsPublishMode)
            web.Parameters.Certificate = builder.AddParameter(
                name: "frontend-certificate",
                secret: false);

        if (builder.ExecutionContext.IsPublishMode)
            web.Parameters.Domain = builder.AddParameter(
                name: "frontend-domain",
                value: "southyorkshire.tramtimes.net");

        #endregion

        #region add api

        if (builder.ExecutionContext.IsRunMode)
            web.Backend = builder
                .AddProject<Projects.TramTimes_Web_Api>(name: "web-api")
                .WaitFor(dependency: cache.Builder ?? throw new InvalidOperationException(message: "Cache builder is not available."))
                .WaitFor(dependency: search.Builder ?? throw new InvalidOperationException(message: "Search builder is not available."))
                .WithEnvironment(
                    name: "ASPIRE_CONTEXT",
                    value: _context)
                .WithExternalHttpEndpoints()
                .WithHttpCommand(
                    displayName: "Build cache",
                    path: "/web/cache/build",
                    commandOptions: new HttpCommandOptions { IconName = "Settings" })
                .WithHttpCommand(
                    displayName: "Delete cache",
                    path: "/web/cache/delete",
                    commandOptions: new HttpCommandOptions { IconName = "Delete" })
                .WithHttpCommand(
                    displayName: "Build index",
                    path: "/web/index/build",
                    commandOptions: new HttpCommandOptions { IconName = "Settings" })
                .WithHttpCommand(
                    displayName: "Delete index",
                    path: "/web/index/delete",
                    commandOptions: new HttpCommandOptions { IconName = "Delete" })
                .WithHttpHealthCheck(path: "/healthz")
                .WithReference(source: storage.Blobs ?? throw new InvalidOperationException(message: "Storage blobs are not available."))
                .WithReference(source: storage.Queues ?? throw new InvalidOperationException(message: "Storage queues are not available."))
                .WithReference(source: storage.Tables ?? throw new InvalidOperationException(message: "Storage tables are not available."))
                .WithReference(source: database.Resource ?? throw new InvalidOperationException(message: "Database resource is not available."))
                .WithReference(source: cache.Service ?? throw new InvalidOperationException(message: "Cache service is not available."))
                .WithReference(source: search.Service ?? throw new InvalidOperationException(message: "Search service is not available."))
                .WithUrlForEndpoint(
                    callback: (ResourceUrlAnnotation url) => url.DisplayLocation = UrlDisplayLocation.DetailsOnly,
                    endpointName: "https")
                .WithUrlForEndpoint(
                    callback: (ResourceUrlAnnotation url) => url.DisplayLocation = UrlDisplayLocation.DetailsOnly,
                    endpointName: "http")
                .WithUrls(callback: callback =>
                {
                    callback.Urls.Add(item: new ResourceUrlAnnotation
                    {
                        DisplayText = "Primary",
                        DisplayLocation = UrlDisplayLocation.SummaryAndDetails,
                        Endpoint = callback.GetEndpoint(name: "https"),
                        Url = "/swagger"
                    });

                    callback.Urls.Add(item: new ResourceUrlAnnotation
                    {
                        DisplayText = "Secondary",
                        DisplayLocation = UrlDisplayLocation.SummaryAndDetails,
                        Endpoint = callback.GetEndpoint(name: "http"),
                        Url = "/swagger"
                    });
                });

        if (builder.ExecutionContext.IsPublishMode)
            web.Backend = builder
                .AddProject<Projects.TramTimes_Web_Api>(name: "web-api")
                .WaitFor(dependency: cache.Builder ?? throw new InvalidOperationException(message: "Cache builder is not available."))
                .WaitFor(dependency: search.Builder ?? throw new InvalidOperationException(message: "Search builder is not available."))
                .WithEnvironment(
                    name: "ASPIRE_CONTEXT",
                    value: "Production")
                .WithExternalHttpEndpoints()
                .WithHttpHealthCheck(path: "/healthz")
                .WithReference(source: database.Connection ?? throw new InvalidOperationException(message: "Database connection is not available."))
                .WithReference(source: cache.Connection ?? throw new InvalidOperationException(message: "Cache connection is not available."))
                .WithReference(source: search.Connection ?? throw new InvalidOperationException(message: "Search connection is not available."))
                .PublishAsAzureContainerApp(configure: (infrastructure, app) =>
                {
                    var container = app.Template.Containers.Single().Value;

                    if (container is not null)
                    {
                        container.Resources.Cpu = 0.5;
                        container.Resources.Memory = "1.0Gi";
                    }

                    app.Template.Scale.MinReplicas = 0;
                    app.Template.Scale.MaxReplicas = 5;
                    app.Template.Scale.CooldownPeriod = 900;

                    app.Template.Scale.Rules.Add(item: new BicepValue<ContainerAppScaleRule>(
                        literal: new ContainerAppScaleRule
                        {
                            Name = "http-requests",
                            Custom = new ContainerAppCustomScaleRule
                            {
                                CustomScaleRuleType = "http",
                                Metadata = new BicepDictionary<string> { { "concurrentRequests", "100" } }
                            }
                        }));

                    app.Template.Scale.Rules.Add(item: new BicepValue<ContainerAppScaleRule>(
                        literal: new ContainerAppScaleRule
                        {
                            Name = "cpu-threshold",
                            Custom = new ContainerAppCustomScaleRule
                            {
                                CustomScaleRuleType = "cpu",
                                Metadata = new BicepDictionary<string>
                                {
                                    { "type", "Utilization" },
                                    { "value", "75" }
                                }
                            }
                        }));

                    app.Template.Scale.Rules.Add(item: new BicepValue<ContainerAppScaleRule>(
                        literal: new ContainerAppScaleRule
                        {
                            Name = "memory-threshold",
                            Custom = new ContainerAppCustomScaleRule
                            {
                                CustomScaleRuleType = "memory",
                                Metadata = new BicepDictionary<string>
                                {
                                    { "type", "Utilization" },
                                    { "value", "75" }
                                }
                            }
                        }));

                    app.Template.Scale.Rules.Add(item: new BicepValue<ContainerAppScaleRule>(
                        literal: new ContainerAppScaleRule
                        {
                            Name = "prewarm-am",
                            Custom = new ContainerAppCustomScaleRule
                            {
                                CustomScaleRuleType = "cron",
                                Metadata = new BicepDictionary<string>
                                {
                                    { "timezone", "UTC" },
                                    { "start", "45 5 * * 1,2,3,4,5" },
                                    { "end", "15 10 * * 1,2,3,4,5" },
                                    { "desiredReplicas", "1" }
                                }
                            }
                        }));

                    app.Template.Scale.Rules.Add(item: new BicepValue<ContainerAppScaleRule>(
                        literal: new ContainerAppScaleRule
                        {
                            Name = "prewarm-pm",
                            Custom = new ContainerAppCustomScaleRule
                            {
                                CustomScaleRuleType = "cron",
                                Metadata = new BicepDictionary<string>
                                {
                                    { "timezone", "UTC" },
                                    { "start", "45 15 * * 1,2,3,4,5" },
                                    { "end", "15 20 * * 1,2,3,4,5" },
                                    { "desiredReplicas", "1" }
                                }
                            }
                        }));
                });

        #endregion

        #region add site

        if (builder.ExecutionContext.IsRunMode)
            web.Frontend = builder
                .AddProject<Projects.TramTimes_Web_Site>(name: "web-site")
                .WaitFor(dependency: web.Backend ?? throw new InvalidOperationException(message: "Web backend is not available."))
                .WithEnvironment(
                    name: "API_ENDPOINT",
                    endpointReference: web.Backend.GetEndpoint(name: "https").Exists
                        ? web.Backend.GetEndpoint(name: "https")
                        : web.Backend.GetEndpoint(name: "http"))
                .WithEnvironment(
                    name: "ASPIRE_CONTEXT",
                    value: _context)
                .WithExternalHttpEndpoints()
                .WithHttpHealthCheck(path: "/healthz")
                .WithUrlForEndpoint(
                    callback: (ResourceUrlAnnotation url) => url.DisplayText = "Primary",
                    endpointName: "https")
                .WithUrlForEndpoint(
                    callback: (ResourceUrlAnnotation url) => url.DisplayText = "Secondary",
                    endpointName: "http");

        if (builder.ExecutionContext.IsPublishMode)
            web.Frontend = builder
                .AddProject<Projects.TramTimes_Web_Site>(name: "web-site")
                .WaitFor(dependency: web.Backend ?? throw new InvalidOperationException(message: "Web backend is not available."))
                .WithEnvironment(
                    name: "API_ENDPOINT",
                    endpointReference: web.Backend.GetEndpoint(name: "https").Exists
                        ? web.Backend.GetEndpoint(name: "https")
                        : web.Backend.GetEndpoint(name: "http"))
                .WithEnvironment(
                    name: "ASPIRE_CONTEXT",
                    value: "Production")
                .WithExternalHttpEndpoints()
                .WithHttpHealthCheck(path: "/healthz")
                .PublishAsAzureContainerApp(configure: (infrastructure, app) =>
                {
                    var container = app.Template.Containers.Single().Value;

                    if (container is not null)
                    {
                        container.Resources.Cpu = 1.0;
                        container.Resources.Memory = "2.0Gi";
                    }

                    app.Template.Scale.MinReplicas = 0;
                    app.Template.Scale.MaxReplicas = 5;
                    app.Template.Scale.CooldownPeriod = 900;

                    app.Template.Scale.Rules.Add(item: new BicepValue<ContainerAppScaleRule>(
                        literal: new ContainerAppScaleRule
                        {
                            Name = "http-requests",
                            Custom = new ContainerAppCustomScaleRule
                            {
                                CustomScaleRuleType = "http",
                                Metadata = new BicepDictionary<string> { { "concurrentRequests", "100" } }
                            }
                        }));

                    app.Template.Scale.Rules.Add(item: new BicepValue<ContainerAppScaleRule>(
                        literal: new ContainerAppScaleRule
                        {
                            Name = "cpu-threshold",
                            Custom = new ContainerAppCustomScaleRule
                            {
                                CustomScaleRuleType = "cpu",
                                Metadata = new BicepDictionary<string>
                                {
                                    { "type", "Utilization" },
                                    { "value", "75" }
                                }
                            }
                        }));

                    app.Template.Scale.Rules.Add(item: new BicepValue<ContainerAppScaleRule>(
                        literal: new ContainerAppScaleRule
                        {
                            Name = "memory-threshold",
                            Custom = new ContainerAppCustomScaleRule
                            {
                                CustomScaleRuleType = "memory",
                                Metadata = new BicepDictionary<string>
                                {
                                    { "type", "Utilization" },
                                    { "value", "75" }
                                }
                            }
                        }));

                    app.ConfigureCustomDomain(
                        certificateName: web.Parameters.Certificate ?? throw new InvalidOperationException(message: "Certificate parameter is not available."),
                        customDomain: web.Parameters.Domain ?? throw new InvalidOperationException(message: "Domain parameter is not available."));
                });

        #endregion
    }
}
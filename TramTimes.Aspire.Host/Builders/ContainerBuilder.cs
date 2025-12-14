// ReSharper disable all

using TramTimes.Aspire.Host.Resources;

namespace TramTimes.Aspire.Host.Builders;

public static class ContainerBuilder
{
    private static readonly string _context = Environment.GetEnvironmentVariable(variable: "ASPIRE_CONTEXT") ?? "Development";

    public static ContainerResources BuildContainer(this IDistributedApplicationBuilder builder)
    {
        #region build resources

        var container = new ContainerResources();

        #endregion

        #region add registry

        if (builder.ExecutionContext.IsPublishMode)
            container.Service = builder.AddAzureContainerRegistry(name: "container-registry");

        #endregion

        #region add environment

        if (builder.ExecutionContext.IsPublishMode)
            container.Resource = builder
                .AddAzureContainerAppEnvironment(name: "application-environment")
                .WithAzureContainerRegistry(registryBuilder: container.Service ?? throw new InvalidOperationException(message: "Container service is not available."));

        #endregion

        return container;
    }
}
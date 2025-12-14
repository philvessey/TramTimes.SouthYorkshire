using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;

namespace TramTimes.Aspire.Host.Extensions;

public static class SearchExtensions
{
    public static void WithKibana<T>(
        this IResourceBuilder<T> builder,
        Action<IResourceBuilder<ContainerResource>>? configureContainer = null,
        string? containerName = null) where T : ElasticsearchResource {

        ArgumentNullException.ThrowIfNull(argument: builder);

        #region build endpoint

        var hostExpression = builder.Resource.PrimaryEndpoint.Property(property: EndpointProperty.Host);
        var portExpression = builder.Resource.PrimaryEndpoint.Property(property: EndpointProperty.Port);

        #endregion

        #region add container

        var containerResource = builder.ApplicationBuilder
            .AddContainer(
                name: containerName ?? "kibana",
                image: "docker.io/library/kibana")
            .WithEnvironment(
                name: "ELASTICSEARCH_HOSTS",
                value: ReferenceExpression.Create(handler: $"http://{hostExpression}:{portExpression}"))
            .WithHttpEndpoint(
                name: "http",
                targetPort: 5601);

        #endregion

        configureContainer?.Invoke(obj: containerResource);
    }

    public static void WithBuildCommand(this IResourceBuilder<ContainerResource> builder)
    {
        builder.WithCommand(
            name: "build-view",
            displayName: "Build View",
            commandOptions: new CommandOptions { IconName = "FormNew" },
            executeCommand: async context =>
            {
                #region build service

                var service = context.ServiceProvider.GetRequiredService<IInteractionService>();

                #endregion

                #region build client

                var httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(uriString: builder.Resource.GetEndpoint(endpointName: "http").Url);

                #endregion

                #region build interaction

                var interaction = await service.PromptInputsAsync(
                    title: "Build data view",
                    message: "Build a data view for Kibana analytics. A data view allows you to explore and visualize your data within Kibana.",
                    inputs:
                    [
                        new InteractionInput
                        {
                            Name = "name",
                            Description = "Name for the Kibana data view.",
                            Placeholder = "southyorkshire",
                            InputType = InputType.Text,
                            Required = true
                        },
                        new InteractionInput
                        {
                            Name = "pattern",
                            Description = "Index pattern for the Kibana data view.",
                            Placeholder = "southyorkshire",
                            InputType = InputType.Text,
                            Required = true
                        }
                    ],
                    cancellationToken: context.CancellationToken);

                if (interaction.Canceled)
                    return CommandResults.Canceled();

                #endregion

                #region build request

                var request = new HttpRequestMessage(
                    method: HttpMethod.Post,
                    requestUri: "/api/data_views/data_view");

                request.Headers.TryAddWithoutValidation(
                    name: "kbn-xsrf",
                    value: "reporting");

                request.Content = JsonContent.Create(inputValue: new
                {
                    data_view = new
                    {
                        name = interaction.Data.ElementAt(index: 0).Value,
                        title = interaction.Data.ElementAt(index: 1).Value
                    }
                });

                #endregion

                #region build response

                var response = await httpClient.SendAsync(request: request);
                response.EnsureSuccessStatusCode();

                #endregion

                return CommandResults.Success();
            });
    }

    public static void WithDeleteCommand(this IResourceBuilder<ContainerResource> builder)
    {
        builder.WithCommand(
            name: "delete-view",
            displayName: "Delete View",
            commandOptions: new CommandOptions { IconName = "FormNew" },
            executeCommand: async context =>
            {
                #region build service

                var service = context.ServiceProvider.GetRequiredService<IInteractionService>();

                #endregion

                #region build client

                var httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(uriString: builder.Resource.GetEndpoint(endpointName: "http").Url);

                #endregion

                #region build request

                var request = new HttpRequestMessage(
                    method: HttpMethod.Get,
                    requestUri: "/api/data_views");

                request.Headers.TryAddWithoutValidation(
                    name: "kbn-xsrf",
                    value: "reporting");

                #endregion

                #region build response

                var response = await httpClient.SendAsync(request: request);
                response.EnsureSuccessStatusCode();

                #endregion

                #region build results

                var results = await response.Content.ReadFromJsonAsync<JsonElement>();

                #endregion

                #region build options

                var options = new List<KeyValuePair<string, string>> { new(string.Empty, "select data view to delete") };

                options.AddRange(collection: results
                    .GetProperty(propertyName: "data_view")
                    .EnumerateArray()
                    .Select(selector: view => new KeyValuePair<string, string>(
                        view.GetProperty(propertyName: "id").GetString() ?? "unknown",
                        view.GetProperty(propertyName: "name").GetString() ?? "unknown"))
                    .OrderBy(keySelector: view => view.Value)
                    .ToList());

                #endregion

                #region build interaction

                var interaction = await service.PromptInputAsync(
                    title: "Delete data view",
                    message: "Delete a data view for Kibana analytics. A data view allows you to explore and visualize your data within Kibana.",
                    input: new InteractionInput
                    {
                        Name = "data view",
                        InputType = InputType.Choice,
                        Options = options
                    },
                    cancellationToken: context.CancellationToken);

                if (interaction.Canceled || interaction.Data.Value == string.Empty)
                    return CommandResults.Canceled();

                #endregion

                #region build request

                request = new HttpRequestMessage(
                    method: HttpMethod.Delete,
                    requestUri: $"/api/data_views/data_view/{interaction.Data.Value}");

                request.Headers.TryAddWithoutValidation(
                    name: "kbn-xsrf",
                    value: "reporting");

                #endregion

                #region build response

                response = await httpClient.SendAsync(request: request);
                response.EnsureSuccessStatusCode();

                #endregion

                return CommandResults.Success();
            });
    }
}
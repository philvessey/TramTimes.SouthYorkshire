using System.Net.Http.Json;
using Aspire.Hosting.Testing;
using Microsoft.Playwright;
using TramTimes.Web.Tests.Managers;
using TramTimes.Web.Utilities.Models;
using Xunit;

namespace TramTimes.Web.Tests;

public class BaseTest(AspireManager aspireManager) : IClassFixture<AspireManager>, IAsyncDisposable
{
    private AspireManager AspireManager { get; } = aspireManager ?? throw new ArgumentNullException(paramName: nameof(aspireManager));
    private PlaywrightManager PlaywrightManager => AspireManager.PlaywrightManager;

    protected async Task ConfigureTestAsync<TEntryPoint>() where TEntryPoint : class
    {
        #region configure aspire

        await AspireManager.ConfigureAsync<TEntryPoint>();

        #endregion
    }

    private async Task HealthTestAsync(
        string resource,
        int timeout = 15) {

        #region check health

        if (AspireManager.Application is null)
            return;

        var tokenSource = new CancellationTokenSource(delay: TimeSpan.FromSeconds(seconds: timeout));

        await AspireManager.Application.ResourceNotifications
            .WaitForResourceHealthyAsync(
                resourceName: resource,
                cancellationToken: tokenSource.Token)
            .WaitAsync(
                timeout: TimeSpan.FromSeconds(seconds: timeout),
                cancellationToken: tokenSource.Token);

        #endregion
    }

    protected async Task<List<WebStopPoint>> QueryTestAsync(
        string id,
        string type = "stop") {

        #region check health

        if (AspireManager.Application is null)
            return [];

        await HealthTestAsync(resource: "web-api");

        #endregion

        #region get endpoint

        var endpoint = AspireManager.Application
            .GetEndpoint(
                resourceName: "web-api",
                endpointName: "https")
            .ToString();

        if (string.IsNullOrEmpty(value: endpoint))
            endpoint = AspireManager.Application
                .GetEndpoint(
                    resourceName: "web-api",
                    endpointName: "http")
                .ToString();

        endpoint = endpoint[..^1];

        #endregion

        #region build service

        var service = new HttpClient();

        #endregion

        #region get response

        var response = await service.GetAsync(requestUri: $"{endpoint}/database/services/{type}/{id}");

        #endregion

        #region check response

        if (!response.IsSuccessStatusCode)
            return [];

        #endregion

        #region get results

        var results = await response.Content.ReadFromJsonAsync<List<WebStopPoint>>() ?? [];

        #endregion

        #region return results

        return results;

        #endregion
    }

    protected async Task RunTestAsync(
        Cookie cookie,
        ColorScheme scheme,
        Func<IPage, Task> test) {

        #region check health

        if (AspireManager.Application is null)
            return;

        await HealthTestAsync(resource: "web-site");

        #endregion

        #region get endpoint

        var endpoint = AspireManager.Application
            .GetEndpoint(
                resourceName: "web-site",
                endpointName: "https")
            .ToString();

        if (string.IsNullOrEmpty(value: endpoint))
            endpoint = AspireManager.Application
                .GetEndpoint(
                    resourceName: "web-site",
                    endpointName: "http")
                .ToString();

        endpoint = endpoint[..^1];

        #endregion

        #region build context

        if (PlaywrightManager.Browser is null)
            return;

        var context = await PlaywrightManager.Browser.NewContextAsync(options: new BrowserNewContextOptions
        {
            BaseURL = endpoint,
            ColorScheme = scheme,
            IgnoreHTTPSErrors = true
        });

        #endregion

        #region add cookies

        if (!string.IsNullOrEmpty(value: cookie.Value))
            await context.AddCookiesAsync(cookies: [cookie]);

        #endregion

        #region build page

        var page = await context.NewPageAsync();

        #endregion

        #region run test

        try
        {
            await test(page);
        }
        finally
        {
            await page.CloseAsync();
        }

        await context.CloseAsync();
        await context.DisposeAsync();

        #endregion
    }

    protected static async Task CompleteTestAsync(string? error)
    {
        #region check pass

        if (string.IsNullOrEmpty(value: error))
            Assert.True(condition: true);

        #endregion

        #region check fail

        if (!string.IsNullOrEmpty(value: error))
            Assert.Fail(message: error);

        #endregion

        #region return result

        await Task.FromResult(result: true);

        #endregion
    }

    protected async Task UploadTestAsync()
    {
        #region check health

        if (AspireManager.Application is null)
            return;

        #endregion

        #region upload files

        using var httpClient = new HttpClient();

        foreach (var item in AspireManager.Storage!.GetFiles())
        {
            await using var fileStream = item.OpenRead();
            var content = new StreamContent(content: fileStream);

            content.Headers.Add(
                name: "Content-Type",
                value: "image/png");

            content.Headers.Add(
                name: "Custom-Name",
                value: item.Name.Replace(
                    oldValue: "|",
                    newValue: "/"));

            var response = await httpClient.PostAsync(
                requestUri: new Uri(
                    baseUri: AspireManager.Application.GetEndpoint(
                        resourceName: "web-api",
                        endpointName: "http"),
                    relativeUri: "web/screenshot/file"),
                content: content);

            if (response.IsSuccessStatusCode)
                item.Delete();
        }

        #endregion
    }

    public async ValueTask DisposeAsync()
    {
        #region suppress finalizer

        GC.SuppressFinalize(obj: this);

        #endregion

        #region dispose managers

        await AspireManager.DisposeAsync();
        await PlaywrightManager.DisposeAsync();

        #endregion
    }
}
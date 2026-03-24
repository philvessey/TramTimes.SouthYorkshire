using Microsoft.Playwright;
using Polly;
using Polly.Retry;

namespace TramTimes.Web.Jobs.Services;

public class PlaywrightService : IHostedService
{
    private readonly ILogger<PlaywrightService> _logger;
    private readonly IHostApplicationLifetime _host;
    private readonly AsyncRetryPolicy _result;

    public PlaywrightService(
        ILogger<PlaywrightService> logger,
        IHostApplicationLifetime host) {

        #region inject servics

        _logger = logger;
        _host = host;

        #endregion

        #region build result

        _result = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(
                sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(value: Math.Pow(
                    x: retryAttempt,
                    y: 2)),
                retryCount: 4);

        #endregion
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        #region build task

        await _result.ExecuteAsync(action: async () =>
        {
            var playwright = await Playwright.CreateAsync();

            var dimensions = new[]
            {
                (Width: 1440, Height: 300),
                (Width: 1440, Height: 600),
                (Width: 320, Height: 640),
                (Width: 480, Height: 640),
                (Width: 728, Height: 640),
            };

            var browser = await playwright.Chromium.LaunchAsync(options: new BrowserTypeLaunchOptions
            {
                Headless = false,
                Args =
                [
                    "--disable-blink-features=AutomationControlled",
                    "--disable-dev-shm-usage",
                    "--disable-extensions",
                    "--disable-gpu",
                    "--disable-plugins",
                    "--disable-software-rasterizer",
                    "--no-sandbox"
                ]
            });

            try
            {
                var context = await browser.NewContextAsync(options: new BrowserNewContextOptions
                {
                    BaseURL = "https://southyorkshire.tramtimes.net",
                    ColorScheme = ColorScheme.Light,
                    UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36",
                    ExtraHTTPHeaders = new Dictionary<string, string>
                    {
                        { "Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8" },
                        { "Accept-Language", "en-US,en;q=0.9" }
                    }
                });

                await context.AddInitScriptAsync(script:
                    """
                        Object.defineProperty(navigator, 'webdriver', {
                            get: () => false,
                        });
                    """);

                foreach (var item in dimensions)
                {
                    var page = await context.NewPageAsync();

                    page.Request += (_, request) =>
                    {
                        if (_logger.IsEnabled(logLevel: LogLevel.Debug))
                            _logger.LogDebug(
                                message: "Request: {method} {url}",
                                args:
                                [
                                    request.Method,
                                    request.Url
                                ]);
                    };

                    page.Response += (_, response) =>
                    {
                        if (_logger.IsEnabled(logLevel: LogLevel.Debug))
                            _logger.LogDebug(
                                message: "Response: {status} {url}",
                                args:
                                [
                                    response.Status,
                                    response.Url
                                ]);
                    };

                    await page.SetViewportSizeAsync(
                        width: item.Width,
                        height: item.Height);

                    await page.GotoAsync(url: "/-1.468535662/53.382525584", options: new PageGotoOptions
                    {
                        WaitUntil = WaitUntilState.Load
                    });

                    await page
                        .GetByTestId(testId: "telerik-map")
                        .GetByTestId(testId: "marker").First
                        .WaitForAsync();

                    await page
                        .GetByTestId(testId: "telerik-list-view")
                        .GetByTestId(testId: "result").First
                        .WaitForAsync();

                    var list = await context.CookiesAsync();

                    if (list.All(predicate: result => result.Name != ".AspNet.Consent"))
                    {
                        var parent = page.GetByTestId(testId: "local-storage-consent__outline");
                        await parent.IsVisibleAsync();

                        var child = parent.GetByTestId(testId: "accept");
                        await child.ClickAsync();
                    }

                    await page.WaitForTimeoutAsync(timeout: 5000);

                    if (_logger.IsEnabled(logLevel: LogLevel.Information))
                        _logger.LogInformation(
                            message: "Playwright service page viewport: {width}x{height}",
                            args:
                            [
                                item.Width,
                                item.Height
                            ]);

                    await page.CloseAsync();
                }

                await context.CloseAsync();
            }
            finally
            {
                await browser.CloseAsync();
            }

            playwright.Dispose();

            if (_logger.IsEnabled(logLevel: LogLevel.Information))
                _logger.LogInformation(
                    message: "Playwright service health status: {status}",
                    args: "Green");

            _host.StopApplication();
        });

        #endregion
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        #region build task

        return Task.CompletedTask;

        #endregion
    }
}
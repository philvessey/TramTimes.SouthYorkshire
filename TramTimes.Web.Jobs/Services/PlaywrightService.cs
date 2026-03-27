using Microsoft.Playwright;
using Polly;
using Polly.Retry;
using TramTimes.Web.Jobs.Builders;

namespace TramTimes.Web.Jobs.Services;

public class PlaywrightService : IHostedService
{
    private readonly ILogger<PlaywrightService> _logger;
    private readonly IHostApplicationLifetime _host;
    private readonly AsyncRetryPolicy _result;

    private readonly string _hostname;
    private readonly string _hostport;
    private readonly string _username;
    private readonly string _password;

    public PlaywrightService(
        ILogger<PlaywrightService> logger,
        IHostApplicationLifetime host) {

        #region inject services

        _logger = logger;
        _host = host;

        #endregion

        #region build proxy

        var random = new Random();

        _hostname = Environment.GetEnvironmentVariable(variable: "PROXY_HOSTNAME") ?? string.Empty;
        _hostport = Environment.GetEnvironmentVariable(variable: "PROXY_HOSTPORT") ?? string.Empty;
        _username = Environment.GetEnvironmentVariable(variable: "PROXY_USERNAME") ?? string.Empty;

        const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        _password = RegexBuilder.GetSession().Replace(
            input: Environment.GetEnvironmentVariable(variable: "PROXY_PASSWORD") ?? string.Empty,
            evaluator: match =>
            {
                var fullMatch = match.Groups[0].Value;
                var oldSession = match.Groups[1].Value;

                var newSession = new string(value: Enumerable.Range(start: 0, count: 8)
                    .Select(selector: _ => characters[random.Next(maxValue: characters.Length)])
                    .ToArray());

                return fullMatch.Replace(
                    oldValue: oldSession,
                    newValue: newSession);
            });

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
                (Width: 468, Height: 640),
                (Width: 728, Height: 640)
            };

            var browser = await playwright.Chromium.LaunchAsync(options: new BrowserTypeLaunchOptions
            {
                Headless = false,
                Proxy = new Proxy
                {
                    Server = $"{_hostname}:{_hostport}",
                    Username = _username,
                    Password = _password
                },
                Args =
                [
                    "--disable-blink-features=AutomationControlled",
                    "--disable-dev-shm-usage",
                    "--no-sandbox"
                ]
            });

            try
            {
                var context = await browser.NewContextAsync(options: new BrowserNewContextOptions
                {
                    BypassCSP = true,
                    ColorScheme = ColorScheme.Light,
                    Permissions = ["storage-access"]
                });

                context.SetDefaultTimeout(timeout: 60000);
                context.SetDefaultNavigationTimeout(timeout: 60000);

                await context.AddInitScriptAsync(
                    script: await File.ReadAllTextAsync(
                        path: Path.Combine(
                            path1: "Scripts",
                            path2: "chromium.js"),
                    cancellationToken: cancellationToken));

                foreach (var item in dimensions)
                {
                    var page = await context.NewPageAsync();

                    await page.SetViewportSizeAsync(
                        width: item.Width,
                        height: item.Height);

                    if (_logger.IsEnabled(logLevel: LogLevel.Information))
                        _logger.LogInformation(
                            message: "Playwright service page viewport: {width}x{height}",
                            args:
                            [
                                item.Width,
                                item.Height
                            ]);

                    await page.GotoAsync(url: "https://ipv4.icanhazip.com", options: new PageGotoOptions
                    {
                        WaitUntil = WaitUntilState.Load
                    });

                    var text = await page.InnerTextAsync(selector: "body");

                    if (_logger.IsEnabled(logLevel: LogLevel.Information))
                        _logger.LogInformation(
                            message: "Playwright service ip address: {ip}",
                            args: text.Trim());

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

                    await page.GotoAsync(url: "https://southyorkshire.tramtimes.net/-1.468535662/53.382525584", options: new PageGotoOptions
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

                    await page.WaitForTimeoutAsync(timeout: 30000);
                    await page.WaitForLoadStateAsync(state: LoadState.Load);

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
using System.Text.RegularExpressions;
using Microsoft.Playwright;
using TramTimes.Web.Tests.Cookies;
using TramTimes.Web.Tests.Managers;
using Xunit;

namespace TramTimes.Web.Tests.Pages.Stop.Light.TelerikMap;

public class MarkerClick(AspireManager aspireManager) : BaseTest(aspireManager: aspireManager)
{
    private AspireManager AspireManager { get; } = aspireManager ?? throw new ArgumentNullException(paramName: nameof(aspireManager));
    private byte[]? Screenshot { get; set; }
    private string? Error { get; set; }

    [Theory]
    [InlineData("9400ZZSYHFW1", 53.328532846077614, -1.3443136700078966, 1)]
    [InlineData("9400ZZSYMAL1", 53.40064593919049, -1.5082120329876791, 2)]
    [InlineData("9400ZZSYMID1", 53.41586234037237, -1.510067739914952, 3)]
    public async Task Desktop(
        string id,
        double lat,
        double lon,
        int run) {

        await ConfigureTestAsync<Projects.TramTimes_Aspire_Host>();

        await RunTestAsync(cookie: ConsentCookies.True, scheme: ColorScheme.Light, test: async page =>
        {
            #region configure page

            await page.SetViewportSizeAsync(
                width: 1440,
                height: 900);

            #endregion

            #region load page

            await page.GotoAsync(url: $"/stop/{id}/{lon}/{lat}");

            #endregion

            #region wait page

            await page.WaitForResponseAsync(urlOrPredicate: response =>
                response.Url.Contains(value: "pin.png") &&
                response.Status is 200 or 304);

            #endregion

            #region test page

            Error = string.Empty;

            try
            {
                var parent = page.GetByTestId(testId: "telerik-map");

                await Assertions
                    .Expect(locator: parent)
                    .ToBeInViewportAsync();

                var child = parent.GetByTestId(testId: "marker").First;

                await Assertions
                    .Expect(locator: child)
                    .ToBeInViewportAsync();

                await child.ClickAsync();

                await page.WaitForConsoleMessageAsync(options: new PageWaitForConsoleMessageOptions
                {
                    Predicate = message => message.Text.Contains(value: "stop: consent") ||
                                           message.Text.Contains(value: "stop: list") ||
                                           message.Text.Contains(value: "stop: map") ||
                                           message.Text.Contains(value: "stop: screen") ||
                                           message.Text.Contains(value: "stop: search")
                });

                await Assertions
                    .Expect(page: page)
                    .ToHaveURLAsync(urlOrRegExp: new Regex(pattern: $"/{id}"));

                parent = page.GetByTestId(testId: "telerik-map");

                await Assertions
                    .Expect(locator: parent)
                    .ToBeInViewportAsync();

                child = parent.GetByTestId(testId: "marker").First;

                await Assertions
                    .Expect(locator: child)
                    .ToBeInViewportAsync();

                parent = page.GetByTestId(testId: "telerik-list-view");

                await Assertions
                    .Expect(locator: parent)
                    .ToBeInViewportAsync();

                child = parent.GetByTestId(testId: "result").First;

                await Assertions
                    .Expect(locator: child)
                    .ToBeInViewportAsync();

                parent = page.GetByLabel(text: "Options list");

                await Assertions
                    .Expect(locator: parent).Not
                    .ToBeInViewportAsync();

                await page.Mouse.MoveAsync(
                    x: 0,
                    y: 0);
            }
            catch (Exception e)
            {
                Error = e.Message;
            }
            finally
            {
                Screenshot = await page.ScreenshotAsync();
            }

            #endregion

            #region save page

            await File.WriteAllBytesAsync(
                path: Path.Combine(
                    path1: AspireManager.Storage!.FullName,
                    path2: $"stop|light|telerik-map|marker-click|run{run}|desktop.png"),
                bytes: Screenshot ?? []);

            await UploadTestAsync();

            #endregion
        });

        await CompleteTestAsync(error: Error);
    }

    [Theory]
    [InlineData("9400ZZSYHFW1", 53.328532846077614, -1.3443136700078966, 1)]
    [InlineData("9400ZZSYMAL1", 53.40064593919049, -1.5082120329876791, 2)]
    [InlineData("9400ZZSYMID1", 53.41586234037237, -1.510067739914952, 3)]
    public async Task Mobile(
        string id,
        double lat,
        double lon,
        int run) {

        await ConfigureTestAsync<Projects.TramTimes_Aspire_Host>();

        await RunTestAsync(cookie: ConsentCookies.False, scheme: ColorScheme.Light, test: async page =>
        {
            #region configure page

            await page.SetViewportSizeAsync(
                width: 360,
                height: 640);

            #endregion

            #region load page

            await page.GotoAsync(url: $"/stop/{id}/{lon}/{lat}");

            #endregion

            #region wait page

            await page.WaitForResponseAsync(urlOrPredicate: response =>
                response.Url.Contains(value: "pin.png") &&
                response.Status is 200 or 304);

            #endregion

            #region test page

            Error = string.Empty;

            try
            {
                var parent = page.GetByTestId(testId: "telerik-map");

                await Assertions
                    .Expect(locator: parent)
                    .ToBeInViewportAsync();

                var child = parent.GetByTestId(testId: "marker").First;

                await Assertions
                    .Expect(locator: child)
                    .ToBeInViewportAsync();

                await child.ClickAsync();

                await page.WaitForConsoleMessageAsync(options: new PageWaitForConsoleMessageOptions
                {
                    Predicate = message => message.Text.Contains(value: "stop: consent") ||
                                           message.Text.Contains(value: "stop: list") ||
                                           message.Text.Contains(value: "stop: map") ||
                                           message.Text.Contains(value: "stop: screen") ||
                                           message.Text.Contains(value: "stop: search")
                });

                await Assertions
                    .Expect(page: page)
                    .ToHaveURLAsync(urlOrRegExp: new Regex(pattern: $"/{id}"));

                parent = page.GetByTestId(testId: "telerik-map");

                await Assertions
                    .Expect(locator: parent)
                    .ToBeInViewportAsync();

                child = parent.GetByTestId(testId: "marker").First;

                await Assertions
                    .Expect(locator: child)
                    .ToBeInViewportAsync();

                parent = page.GetByTestId(testId: "telerik-list-view");

                await Assertions
                    .Expect(locator: parent)
                    .ToBeInViewportAsync();

                child = parent.GetByTestId(testId: "result").First;

                await Assertions
                    .Expect(locator: child)
                    .ToBeInViewportAsync();

                parent = page.GetByLabel(text: "Options list");

                await Assertions
                    .Expect(locator: parent).Not
                    .ToBeInViewportAsync();

                await page.Mouse.MoveAsync(
                    x: 0,
                    y: 0);
            }
            catch (Exception e)
            {
                Error = e.Message;
            }
            finally
            {
                Screenshot = await page.ScreenshotAsync();
            }

            #endregion

            #region save page

            await File.WriteAllBytesAsync(
                path: Path.Combine(
                    path1: AspireManager.Storage!.FullName,
                    path2: $"stop|light|telerik-map|marker-click|run{run}|mobile.png"),
                bytes: Screenshot ?? []);

            await UploadTestAsync();

            #endregion
        });

        await CompleteTestAsync(error: Error);
    }
}
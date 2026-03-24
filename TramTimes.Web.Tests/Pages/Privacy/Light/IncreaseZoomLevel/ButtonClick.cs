using Microsoft.Playwright;
using TramTimes.Web.Tests.Cookies;
using TramTimes.Web.Tests.Managers;
using TramTimes.Web.Utilities.Builders;
using Xunit;

namespace TramTimes.Web.Tests.Pages.Privacy.Light.IncreaseZoomLevel;

[Collection(name: "AspireCollection")]
public class ButtonClick(AspireManager aspireManager) : BaseTest(aspireManager: aspireManager)
{
    private byte[]? Screenshot { get; set; }
    private string? Error { get; set; }

    [Theory]
    [InlineData(53.32853446547, -1.344313639, 1)]
    [InlineData(53.40064755511, -1.50821200817, 2)]
    [InlineData(53.41586395553, -1.51006771516, 3)]
    public async Task Desktop(
        double lat,
        double lon,
        int run) {

        await ConfigureTestAsync<Projects.TramTimes_Aspire_Host>();

        await RunTestAsync(cookies: ConsentCookies.Accepted, scheme: ColorScheme.Light, test: async page =>
        {
            #region configure page

            await page.SetViewportSizeAsync(
                width: 1440,
                height: 900);

            #endregion

            #region load page

            await page.GotoAsync(url: $"/privacy/{lon}/{lat}", options: new PageGotoOptions
            {
                WaitUntil = WaitUntilState.Load
            });

            #endregion

            #region wait page

            await page
                .GetByTestId(testId: "telerik-map")
                .GetByTestId(testId: "marker").First
                .WaitForAsync();

            await page
                .GetByTestId(testId: "telerik-list-view")
                .GetByTestId(testId: "result").First
                .WaitForAsync();

            #endregion

            #region test page

            Error = string.Empty;

            try
            {
                var parent = page.GetByTestId(testId: "increase-zoom-level");

                await Assertions
                    .Expect(locator: parent)
                    .ToBeInViewportAsync();

                await parent.ClickAsync();

                await page.WaitForTimeoutAsync(timeout: 5000);
                await page.WaitForLoadStateAsync(state: LoadState.Load);

                await Assertions
                    .Expect(page: page)
                    .ToHaveURLAsync(urlOrRegExp: RegexBuilder.GetUrl());

                await page
                    .GetByTestId(testId: "telerik-map")
                    .GetByTestId(testId: "marker").First
                    .WaitForAsync();

                await page
                    .GetByTestId(testId: "telerik-list-view")
                    .GetByTestId(testId: "result").First
                    .WaitForAsync();
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
                    path2: $"privacy|light|increase-zoom-level|button-click|run{run}|desktop.png"),
                bytes: Screenshot ?? []);

            await UploadTestAsync();

            #endregion
        });

        await CompleteTestAsync(error: Error);
    }

    [Theory]
    [InlineData(53.32853446547, -1.344313639, 1)]
    [InlineData(53.40064755511, -1.50821200817, 2)]
    [InlineData(53.41586395553, -1.51006771516, 3)]
    public async Task Mobile(
        double lat,
        double lon,
        int run) {

        await ConfigureTestAsync<Projects.TramTimes_Aspire_Host>();

        await RunTestAsync(cookies: ConsentCookies.Accepted, scheme: ColorScheme.Light, test: async page =>
        {
            #region configure page

            await page.SetViewportSizeAsync(
                width: 360,
                height: 640);

            #endregion

            #region load page

            await page.GotoAsync(url: $"/privacy/{lon}/{lat}", options: new PageGotoOptions
            {
                WaitUntil = WaitUntilState.Load
            });

            #endregion

            #region wait page

            await page
                .GetByTestId(testId: "telerik-map")
                .GetByTestId(testId: "marker").First
                .WaitForAsync();

            await page
                .GetByTestId(testId: "telerik-list-view")
                .GetByTestId(testId: "result").First
                .WaitForAsync();

            #endregion

            #region test page

            Error = string.Empty;

            try
            {
                var parent = page.GetByTestId(testId: "increase-zoom-level");

                await Assertions
                    .Expect(locator: parent)
                    .ToBeInViewportAsync();

                await parent.ClickAsync();

                await page.WaitForTimeoutAsync(timeout: 5000);
                await page.WaitForLoadStateAsync(state: LoadState.Load);

                await Assertions
                    .Expect(page: page)
                    .ToHaveURLAsync(urlOrRegExp: RegexBuilder.GetUrl());

                await page
                    .GetByTestId(testId: "telerik-map")
                    .GetByTestId(testId: "marker").First
                    .WaitForAsync();

                await page
                    .GetByTestId(testId: "telerik-list-view")
                    .GetByTestId(testId: "result").First
                    .WaitForAsync();
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
                    path2: $"privacy|light|increase-zoom-level|button-click|run{run}|mobile.png"),
                bytes: Screenshot ?? []);

            await UploadTestAsync();

            #endregion
        });

        await CompleteTestAsync(error: Error);
    }
}
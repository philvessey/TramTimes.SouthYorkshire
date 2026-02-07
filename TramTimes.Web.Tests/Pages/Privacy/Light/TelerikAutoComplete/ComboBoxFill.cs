using Microsoft.Playwright;
using TramTimes.Web.Tests.Cookies;
using TramTimes.Web.Tests.Managers;
using Xunit;

namespace TramTimes.Web.Tests.Pages.Privacy.Light.TelerikAutoComplete;

public class ComboBoxFill(AspireManager aspireManager) : BaseTest(aspireManager: aspireManager)
{
    private AspireManager AspireManager { get; } = aspireManager ?? throw new ArgumentNullException(paramName: nameof(aspireManager));
    private byte[]? Screenshot { get; set; }
    private string? Error { get; set; }

    [Theory]
    [InlineData("Halfway", 53.328532846077614, -1.3443136700078966, "halfw", 1)]
    [InlineData("Malin Bridge", 53.40064593919049, -1.5082120329876791, "malin", 2)]
    [InlineData("Middlewood", 53.41586234037237, -1.510067739914952, "middl", 3)]
    public async Task Desktop(
        string name,
        double lat,
        double lon,
        string query,
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

            await page.GotoAsync(url: $"/privacy/{lon}/{lat}", options: new PageGotoOptions
            {
                WaitUntil = WaitUntilState.NetworkIdle
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
                var parent = page.GetByTestId(testId: "telerik-auto-complete");

                await Assertions
                    .Expect(locator: parent)
                    .ToBeInViewportAsync();

                var child = parent.GetByRole(role: AriaRole.Combobox);

                await Assertions
                    .Expect(locator: child)
                    .ToBeInViewportAsync();

                await child.FillAsync(value: query);

                await page.WaitForTimeoutAsync(timeout: 5000);
                await page.WaitForLoadStateAsync(state: LoadState.NetworkIdle);

                parent = page.GetByLabel(text: "Options list");

                await Assertions
                    .Expect(locator: parent)
                    .ToBeInViewportAsync();

                child = parent.GetByTestId(testId: "result").First;

                await Assertions
                    .Expect(locator: child)
                    .ToBeInViewportAsync();

                var item = child.GetByTestId(testId: "name");

                await Assertions
                    .Expect(locator: item)
                    .ToContainTextAsync(expected: name);
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
                    path2: $"privacy|light|telerik-auto-complete|combo-box-fill|run{run}|desktop.png"),
                bytes: Screenshot ?? []);

            await UploadTestAsync();

            #endregion
        });

        await CompleteTestAsync(error: Error);
    }

    [Theory]
    [InlineData("Halfway", 53.328532846077614, -1.3443136700078966, "halfw", 1)]
    [InlineData("Malin Bridge", 53.40064593919049, -1.5082120329876791, "malin", 2)]
    [InlineData("Middlewood", 53.41586234037237, -1.510067739914952, "middl", 3)]
    public async Task Mobile(
        string name,
        double lat,
        double lon,
        string query,
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

            await page.GotoAsync(url: $"/privacy/{lon}/{lat}", options: new PageGotoOptions
            {
                WaitUntil = WaitUntilState.NetworkIdle
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
                var parent = page.GetByTestId(testId: "telerik-auto-complete");

                await Assertions
                    .Expect(locator: parent)
                    .ToBeInViewportAsync();

                var child = parent.GetByRole(role: AriaRole.Combobox);

                await Assertions
                    .Expect(locator: child)
                    .ToBeInViewportAsync();

                await child.FillAsync(value: query);

                await page.WaitForTimeoutAsync(timeout: 5000);
                await page.WaitForLoadStateAsync(state: LoadState.NetworkIdle);

                parent = page.GetByLabel(text: "Options list");

                await Assertions
                    .Expect(locator: parent)
                    .ToBeInViewportAsync();

                child = parent.GetByTestId(testId: "result").First;

                await Assertions
                    .Expect(locator: child)
                    .ToBeInViewportAsync();

                var item = child.GetByTestId(testId: "name");

                await Assertions
                    .Expect(locator: item)
                    .ToContainTextAsync(expected: name);
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
                    path2: $"privacy|light|telerik-auto-complete|combo-box-fill|run{run}|mobile.png"),
                bytes: Screenshot ?? []);

            await UploadTestAsync();

            #endregion
        });

        await CompleteTestAsync(error: Error);
    }
}
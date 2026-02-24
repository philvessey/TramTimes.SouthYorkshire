using Microsoft.Playwright;
using TramTimes.Web.Tests.Cookies;
using TramTimes.Web.Tests.Managers;
using TramTimes.Web.Tests.Models;
using TramTimes.Web.Tests.Services;
using TramTimes.Web.Utilities.Extensions;
using Xunit;
using Xunit.Sdk;

namespace TramTimes.Web.Tests.Pages.Trip.Dark.TelerikMap;

public class MarkerHover(AspireManager aspireManager) : BaseTest(aspireManager: aspireManager)
{
    private AspireManager AspireManager { get; } = aspireManager ?? throw new ArgumentNullException(paramName: nameof(aspireManager));
    private byte[]? Screenshot { get; set; }
    private string? Error { get; set; }

    [Theory]
    [InlineData("9400ZZSYHFW1", "Halfway", 53.328532846077614, -1.3443136700078966, 1)]
    [InlineData("9400ZZSYMAL1", "Malin Bridge", 53.40064593919049, -1.5082120329876791, 2)]
    [InlineData("9400ZZSYMID1", "Middlewood", 53.41586234037237, -1.510067739914952, 3)]
    public async Task Desktop(
        string id,
        string name,
        double lat,
        double lon,
        int run) {

        await ConfigureTestAsync<Projects.TramTimes_Aspire_Host>();

        #region map test

        var mapper = MapperService.CreateMapper();

        #endregion

        #region check test

        if (DateTime.Now.Second > 30)
            await Task.Delay(delay: TimeSpan.FromSeconds(value: 60 - DateTime.Now.Second + 1));

        #endregion

        #region query test

        var results = await QueryTestAsync(id: id);

        if (results.IsNullOrEmpty())
            throw new XunitException(userMessage: "Invalid data from api query.");

        #endregion

        #region build test

        var points = mapper.Map<List<TelerikStopPoint>>(source: results);

        if (points.IsNullOrEmpty())
            throw new XunitException(userMessage: "Invalid data from api query.");

        var tripId = points
            .Where(predicate: point => point.DepartureDateTime >= new DateTime(
                year: DateTime.Now.Year,
                month: DateTime.Now.Month,
                day: DateTime.Now.Day,
                hour: DateTime.Now.Hour,
                minute: DateTime.Now.Minute,
                second: 0))
            .ElementAtOrDefault(index: 0)?.TripId ?? string.Empty;

        if (string.IsNullOrEmpty(value: tripId))
            throw new XunitException(userMessage: "Invalid data from api query.");

        #endregion

        await RunTestAsync(cookies: ConsentCookies.Accepted, scheme: ColorScheme.Dark, test: async page =>
        {
            #region configure page

            await page.SetViewportSizeAsync(
                width: 1440,
                height: 900);

            #endregion

            #region load page

            await page.GotoAsync(url: $"/trip/{tripId}/{id}/{lon}/{lat}", options: new PageGotoOptions
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
                var parent = page.GetByTestId(testId: "telerik-map");

                await Assertions
                    .Expect(locator: parent)
                    .ToBeInViewportAsync();

                var child = parent.GetByTestId(testId: "marker").First;

                await Assertions
                    .Expect(locator: child)
                    .ToBeInViewportAsync();

                await child.HoverAsync();

                await page.WaitForTimeoutAsync(timeout: 5000);
                await page.WaitForLoadStateAsync(state: LoadState.NetworkIdle);

                parent = page.GetByTestId(testId: "tooltip");

                await Assertions
                    .Expect(locator: parent)
                    .ToBeInViewportAsync();

                child = parent.GetByTestId(testId: "name");

                await Assertions
                    .Expect(locator: child)
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
                    path2: $"trip|dark|telerik-map|marker-hover|run{run}|desktop.png"),
                bytes: Screenshot ?? []);

            await UploadTestAsync();

            #endregion
        });

        await CompleteTestAsync(error: Error);
    }

    [Theory]
    [InlineData("9400ZZSYHFW1", "Halfway", 53.328532846077614, -1.3443136700078966, 1)]
    [InlineData("9400ZZSYMAL1", "Malin Bridge", 53.40064593919049, -1.5082120329876791, 2)]
    [InlineData("9400ZZSYMID1", "Middlewood", 53.41586234037237, -1.510067739914952, 3)]
    public async Task Mobile(
        string id,
        string name,
        double lat,
        double lon,
        int run) {

        await ConfigureTestAsync<Projects.TramTimes_Aspire_Host>();

        #region map test

        var mapper = MapperService.CreateMapper();

        #endregion

        #region check test

        if (DateTime.Now.Second > 30)
            await Task.Delay(delay: TimeSpan.FromSeconds(value: 60 - DateTime.Now.Second + 1));

        #endregion

        #region query test

        var results = await QueryTestAsync(id: id);

        if (results.IsNullOrEmpty())
            throw new XunitException(userMessage: "Invalid data from api query.");

        #endregion

        #region build test

        var points = mapper.Map<List<TelerikStopPoint>>(source: results);

        if (points.IsNullOrEmpty())
            throw new XunitException(userMessage: "Invalid data from api query.");

        var tripId = points
            .Where(predicate: point => point.DepartureDateTime >= new DateTime(
                year: DateTime.Now.Year,
                month: DateTime.Now.Month,
                day: DateTime.Now.Day,
                hour: DateTime.Now.Hour,
                minute: DateTime.Now.Minute,
                second: 0))
            .ElementAtOrDefault(index: 0)?.TripId ?? string.Empty;

        if (string.IsNullOrEmpty(value: tripId))
            throw new XunitException(userMessage: "Invalid data from api query.");

        #endregion

        await RunTestAsync(cookies: ConsentCookies.Rejected, scheme: ColorScheme.Dark, test: async page =>
        {
            #region configure page

            await page.SetViewportSizeAsync(
                width: 360,
                height: 640);

            #endregion

            #region load page

            await page.GotoAsync(url: $"/trip/{tripId}/{id}/{lon}/{lat}", options: new PageGotoOptions
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
                var parent = page.GetByTestId(testId: "telerik-map");

                await Assertions
                    .Expect(locator: parent)
                    .ToBeInViewportAsync();

                var child = parent.GetByTestId(testId: "marker").First;

                await Assertions
                    .Expect(locator: child)
                    .ToBeInViewportAsync();

                await child.HoverAsync();

                await page.WaitForTimeoutAsync(timeout: 5000);
                await page.WaitForLoadStateAsync(state: LoadState.NetworkIdle);

                parent = page.GetByTestId(testId: "tooltip");

                await Assertions
                    .Expect(locator: parent)
                    .ToBeInViewportAsync();

                child = parent.GetByTestId(testId: "name");

                await Assertions
                    .Expect(locator: child)
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
                    path2: $"trip|dark|telerik-map|marker-hover|run{run}|mobile.png"),
                bytes: Screenshot ?? []);

            await UploadTestAsync();

            #endregion
        });

        await CompleteTestAsync(error: Error);
    }
}
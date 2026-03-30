using Microsoft.Playwright;
using TramTimes.Web.Tests.Cookies;
using TramTimes.Web.Tests.Managers;
using TramTimes.Web.Tests.Models;
using TramTimes.Web.Tests.Services;
using TramTimes.Web.Utilities.Extensions;
using Xunit;
using Xunit.Sdk;

namespace TramTimes.Web.Tests.Pages.Trip.Dark.BannerVertical;

[Collection(name: "AspireCollection")]
public class PlaceholderVisible(AspireManager aspireManager) : BaseTest(aspireManager: aspireManager)
{
    private byte[]? Screenshot { get; set; }
    private string? Error { get; set; }

    [Theory]
    [InlineData("9400ZZSYHFW1", 53.32853446547, -1.344313639, 1)]
    [InlineData("9400ZZSYMAL1", 53.40064755511, -1.50821200817, 2)]
    [InlineData("9400ZZSYMID1", 53.41586395553, -1.51006771516, 3)]
    public async Task Desktop(
        string id,
        double lat,
        double lon,
        int run) {

        await ConfigureTestAsync<Projects.TramTimes_Aspire_Host>();

        #region map test

        var mapper = MapperService.CreateMapper();

        #endregion

        #region check test

        if (DateTime.UtcNow.Second > 30)
            await Task.Delay(
                delay: TimeSpan.FromSeconds(value: 60 - DateTime.UtcNow.Second + 1),
                cancellationToken: TestContext.Current.CancellationToken);

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
                year: DateTime.UtcNow.Year,
                month: DateTime.UtcNow.Month,
                day: DateTime.UtcNow.Day,
                hour: DateTime.UtcNow.Hour,
                minute: DateTime.UtcNow.Minute,
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
                var parent = page.GetByTestId(testId: "banner-vertical");

                await Assertions
                    .Expect(locator: parent)
                    .ToBeInViewportAsync();

                var child = parent.GetByTestId(testId: "placeholder");

                await Assertions
                    .Expect(locator: child)
                    .ToBeInViewportAsync();
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
                    path2: $"trip|dark|banner-vertical|placeholder-visible|run{run}|desktop.png"),
                bytes: Screenshot ?? []);

            await UploadTestAsync();

            #endregion
        });

        await CompleteTestAsync(error: Error);
    }

    [Theory]
    [InlineData("9400ZZSYHFW1", 53.32853446547, -1.344313639, 1)]
    [InlineData("9400ZZSYMAL1", 53.40064755511, -1.50821200817, 2)]
    [InlineData("9400ZZSYMID1", 53.41586395553, -1.51006771516, 3)]
    public async Task Mobile(
        string id,
        double lat,
        double lon,
        int run) {

        await ConfigureTestAsync<Projects.TramTimes_Aspire_Host>();

        #region map test

        var mapper = MapperService.CreateMapper();

        #endregion

        #region check test

        if (DateTime.UtcNow.Second > 30)
            await Task.Delay(
                delay: TimeSpan.FromSeconds(value: 60 - DateTime.UtcNow.Second + 1),
                cancellationToken: TestContext.Current.CancellationToken);

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
                year: DateTime.UtcNow.Year,
                month: DateTime.UtcNow.Month,
                day: DateTime.UtcNow.Day,
                hour: DateTime.UtcNow.Hour,
                minute: DateTime.UtcNow.Minute,
                second: 0))
            .ElementAtOrDefault(index: 0)?.TripId ?? string.Empty;

        if (string.IsNullOrEmpty(value: tripId))
            throw new XunitException(userMessage: "Invalid data from api query.");

        #endregion

        await RunTestAsync(cookies: ConsentCookies.Accepted, scheme: ColorScheme.Dark, test: async page =>
        {
            #region configure page

            await page.SetViewportSizeAsync(
                width: 360,
                height: 640);

            #endregion

            #region load page

            await page.GotoAsync(url: $"/trip/{tripId}/{id}/{lon}/{lat}", options: new PageGotoOptions
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
                var parent = page.GetByTestId(testId: "banner-vertical");

                await Assertions
                    .Expect(locator: parent).Not
                    .ToBeInViewportAsync();
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
                    path2: $"trip|dark|banner-vertical|placeholder-visible|run{run}|mobile.png"),
                bytes: Screenshot ?? []);

            await UploadTestAsync();

            #endregion
        });

        await CompleteTestAsync(error: Error);
    }
}
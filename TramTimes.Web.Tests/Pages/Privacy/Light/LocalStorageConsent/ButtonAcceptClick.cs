using System.Text.RegularExpressions;
using Microsoft.Playwright;
using TramTimes.Web.Tests.Cookies;
using TramTimes.Web.Tests.Managers;
using Xunit;

namespace TramTimes.Web.Tests.Pages.Privacy.Light.LocalStorageConsent;

public class ButtonAcceptClick(AspireManager aspireManager) : BaseTest(aspireManager: aspireManager)
{
    private AspireManager AspireManager { get; } = aspireManager ?? throw new ArgumentNullException(paramName: nameof(aspireManager));
    private byte[]? Screenshot { get; set; }
    private string? Error { get; set; }
    
    [Theory]
    [InlineData(53.328532846077614, -1.3443136700078966, 1)]
    [InlineData(53.40064593919049, -1.5082120329876791, 2)]
    [InlineData(53.41586234037237, -1.510067739914952, 3)]
    public async Task Desktop(
        double lat,
        double lon,
        int run) {
        
        await ConfigureTestAsync<Projects.TramTimes_Aspire_Host>();
        
        await RunTestAsync(cookie: ConsentCookies.Unknown, scheme: ColorScheme.Light, test: async page =>
        {
            #region configure page
            
            await page.SetViewportSizeAsync(
                width: 1440,
                height: 900);
            
            #endregion
            
            #region load page
            
            await page.GotoAsync(url: $"/privacy/{lon}/{lat}");
            
            #endregion
            
            #region wait page
            
            await page.WaitForResponseAsync(urlOrPredicate: response =>
                response.Url.Contains(value: "https://cdn.mapmarker.io/api/") &&
                response.Status is 200);
            
            #endregion
            
            #region test page
            
            Error = string.Empty;
            
            try
            {
                var parent = page.GetByTestId(testId: "local-storage-consent__outline");
                
                await Assertions
                    .Expect(locator: parent)
                    .ToBeInViewportAsync();
                
                var child = parent.GetByTestId(testId: "accept");
                
                await Assertions
                    .Expect(locator: child)
                    .ToBeInViewportAsync();
                
                await child.ClickAsync();
                
                await page.WaitForConsoleMessageAsync(options: new PageWaitForConsoleMessageOptions
                {
                    Predicate = message => message.Text.Contains(value: "privacy: consent") ||
                                           message.Text.Contains(value: "privacy: list") ||
                                           message.Text.Contains(value: "privacy: map") ||
                                           message.Text.Contains(value: "privacy: screen") ||
                                           message.Text.Contains(value: "privacy: search")
                });
                
                await Assertions
                    .Expect(page: page)
                    .ToHaveURLAsync(urlOrRegExp: new Regex(pattern: $"/{lon}/{lat}"));
                
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
                    path2: $"privacy|light|local-storage-consent|button-accept-click|run{run}|desktop.png"),
                bytes: Screenshot ?? []);
            
            await UploadTestAsync();
            
            #endregion
        });
        
        await CompleteTestAsync(error: Error);
    }
    
    [Theory]
    [InlineData(53.328532846077614, -1.3443136700078966, 1)]
    [InlineData(53.40064593919049, -1.5082120329876791, 2)]
    [InlineData(53.41586234037237, -1.510067739914952, 3)]
    public async Task Mobile(
        double lat,
        double lon,
        int run) {
        
        await ConfigureTestAsync<Projects.TramTimes_Aspire_Host>();
        
        await RunTestAsync(cookie: ConsentCookies.Unknown, scheme: ColorScheme.Light, test: async page =>
        {
            #region configure page
            
            await page.SetViewportSizeAsync(
                width: 360,
                height: 640);
            
            #endregion
            
            #region load page
            
            await page.GotoAsync(url: $"/privacy/{lon}/{lat}");
            
            #endregion
            
            #region wait page
            
            await page.WaitForResponseAsync(urlOrPredicate: response =>
                response.Url.Contains(value: "https://cdn.mapmarker.io/api/") &&
                response.Status is 200);
            
            #endregion
            
            #region test page
            
            Error = string.Empty;
            
            try
            {
                var parent = page.GetByTestId(testId: "local-storage-consent__outline");
                
                await Assertions
                    .Expect(locator: parent)
                    .ToBeInViewportAsync();
                
                var child = parent.GetByTestId(testId: "accept");
                
                await Assertions
                    .Expect(locator: child)
                    .ToBeInViewportAsync();
                
                await child.ClickAsync();
                
                await page.WaitForConsoleMessageAsync(options: new PageWaitForConsoleMessageOptions
                {
                    Predicate = message => message.Text.Contains(value: "privacy: consent") ||
                                           message.Text.Contains(value: "privacy: list") ||
                                           message.Text.Contains(value: "privacy: map") ||
                                           message.Text.Contains(value: "privacy: screen") ||
                                           message.Text.Contains(value: "privacy: search")
                });
                
                await Assertions
                    .Expect(page: page)
                    .ToHaveURLAsync(urlOrRegExp: new Regex(pattern: $"/{lon}/{lat}"));
                
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
                    path2: $"privacy|light|local-storage-consent|button-accept-click|run{run}|mobile.png"),
                bytes: Screenshot ?? []);
            
            await UploadTestAsync();
            
            #endregion
        });
        
        await CompleteTestAsync(error: Error);
    }
}
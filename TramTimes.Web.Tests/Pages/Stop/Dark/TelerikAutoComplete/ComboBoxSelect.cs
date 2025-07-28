using System.Text.RegularExpressions;
using Microsoft.Playwright;
using TramTimes.Web.Tests.Cookies;
using TramTimes.Web.Tests.Managers;
using Xunit;

namespace TramTimes.Web.Tests.Pages.Stop.Dark.TelerikAutoComplete;

public class ComboBoxSelect(AspireManager aspireManager) : BaseTest(aspireManager: aspireManager)
{
    private AspireManager AspireManager { get; } = aspireManager ?? throw new ArgumentNullException(paramName: nameof(aspireManager));
    private byte[]? Screenshot { get; set; }
    private string? Error { get; set; }
    
    [Theory]
    [InlineData("9400ZZSYHFW1", "Halfway",53.328532846077614, -1.3443136700078966, "halfw", 1)]
    [InlineData("9400ZZSYMAL1", "Malin Bridge", 53.40064593919049, -1.5082120329876791, "malin", 2)]
    [InlineData("9400ZZSYMID1", "Middlewood", 53.41586234037237, -1.510067739914952, "middl", 3)]
    public async Task Desktop(
        string id,
        string name,
        double lat,
        double lon,
        string query,
        int run) {
        
        await ConfigureTestAsync<Projects.TramTimes_Aspire_Host>();
        
        await RunTestAsync(cookie: ConsentCookies.True, scheme: ColorScheme.Dark, test: async page =>
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
                response.Url.Contains(value: "https://cdn.mapmarker.io/api/") &&
                response.Status == 200);
            
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
                
                await page.WaitForConsoleMessageAsync(options: new PageWaitForConsoleMessageOptions
                {
                    Predicate = message => message.Text.Equals(value: $"stop: search read {query}")
                });
                
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
                
                await item.ClickAsync();
                
                await Assertions
                    .Expect(page: page)
                    .ToHaveURLAsync(urlOrRegExp: new Regex(pattern: $"/stop/{id}"));
                
                parent = page.GetByLabel(text: "Options list");
                
                await Assertions
                    .Expect(locator: parent).Not
                    .ToBeInViewportAsync();
                
                parent = page.GetByTestId(testId: "telerik-map");
                
                await Assertions
                    .Expect(locator: parent)
                    .ToBeInViewportAsync();
                
                child = parent.GetByTestId(testId: "marker").First;
                
                await Assertions
                    .Expect(locator: child)
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
                    path2: $"stop|dark|telerik-auto-complete|combo-box-select|run{run}|desktop.png"),
                bytes: Screenshot ?? []);
            
            await UploadTestAsync();
            
            #endregion
        });
        
        await CompleteTestAsync(error: Error);
    }
    
    [Theory]
    [InlineData("9400ZZSYHFW1", "Halfway",53.328532846077614, -1.3443136700078966, "halfw", 1)]
    [InlineData("9400ZZSYMAL1", "Malin Bridge", 53.40064593919049, -1.5082120329876791, "malin", 2)]
    [InlineData("9400ZZSYMID1", "Middlewood", 53.41586234037237, -1.510067739914952, "middl", 3)]
    public async Task Mobile(
        string id,
        string name,
        double lat,
        double lon,
        string query,
        int run) {
        
        await ConfigureTestAsync<Projects.TramTimes_Aspire_Host>();
        
        await RunTestAsync(cookie: ConsentCookies.False, scheme: ColorScheme.Dark, test: async page =>
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
                response.Url.Contains(value: "https://cdn.mapmarker.io/api/") &&
                response.Status == 200);
            
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
                
                await page.WaitForConsoleMessageAsync(options: new PageWaitForConsoleMessageOptions
                {
                    Predicate = message => message.Text.Equals(value: $"stop: search read {query}")
                });
                
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
                
                await item.ClickAsync();
                
                await Assertions
                    .Expect(page: page)
                    .ToHaveURLAsync(urlOrRegExp: new Regex(pattern: $"/stop/{id}"));
                
                parent = page.GetByLabel(text: "Options list");
                
                await Assertions
                    .Expect(locator: parent).Not
                    .ToBeInViewportAsync();
                
                parent = page.GetByTestId(testId: "telerik-map");
                
                await Assertions
                    .Expect(locator: parent)
                    .ToBeInViewportAsync();
                
                child = parent.GetByTestId(testId: "marker").First;
                
                await Assertions
                    .Expect(locator: child)
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
                    path2: $"stop|dark|telerik-auto-complete|combo-box-select|run{run}|mobile.png"),
                bytes: Screenshot ?? []);
            
            await UploadTestAsync();
            
            #endregion
        });
        
        await CompleteTestAsync(error: Error);
    }
}
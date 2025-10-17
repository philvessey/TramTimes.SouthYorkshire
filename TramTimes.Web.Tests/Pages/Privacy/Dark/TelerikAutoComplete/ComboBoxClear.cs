using Microsoft.Playwright;
using TramTimes.Web.Tests.Cookies;
using TramTimes.Web.Tests.Managers;
using Xunit;

namespace TramTimes.Web.Tests.Pages.Privacy.Dark.TelerikAutoComplete;

public class ComboBoxClear(AspireManager aspireManager) : BaseTest(aspireManager: aspireManager)
{
    private AspireManager AspireManager { get; } = aspireManager ?? throw new ArgumentNullException(paramName: nameof(aspireManager));
    private byte[]? Screenshot { get; set; }
    private string? Error { get; set; }
    
    [Theory]
    [InlineData("Halfway",53.328532846077614, -1.3443136700078966, "halfw", 1)]
    [InlineData("Malin Bridge", 53.40064593919049, -1.5082120329876791, "malin", 2)]
    [InlineData("Middlewood", 53.41586234037237, -1.510067739914952, "middl", 3)]
    public async Task Desktop(
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
            
            await page.GotoAsync(url: $"/privacy/{lon}/{lat}");
            
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
                    Predicate = message => message.Text.Contains(value: "privacy: consent") ||
                                           message.Text.Contains(value: "privacy: list") ||
                                           message.Text.Contains(value: "privacy: map") ||
                                           message.Text.Contains(value: "privacy: screen") ||
                                           message.Text.Contains(value: "privacy: search")
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
                
                parent = page.GetByTestId(testId: "telerik-auto-complete");
                
                await Assertions
                    .Expect(locator: parent)
                    .ToBeInViewportAsync();
                
                child = parent.GetByRole(role: AriaRole.Button);
                
                await Assertions
                    .Expect(locator: child)
                    .ToBeInViewportAsync();
                
                await child.ClickAsync();
                
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
                    path2: $"privacy|dark|telerik-auto-complete|combo-box-clear|run{run}|desktop.png"),
                bytes: Screenshot ?? []);
            
            await UploadTestAsync();
            
            #endregion
        });
        
        await CompleteTestAsync(error: Error);
    }
    
    [Theory]
    [InlineData("Halfway",53.328532846077614, -1.3443136700078966, "halfw", 1)]
    [InlineData("Malin Bridge", 53.40064593919049, -1.5082120329876791, "malin", 2)]
    [InlineData("Middlewood", 53.41586234037237, -1.510067739914952, "middl", 3)]
    public async Task Mobile(
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
            
            await page.GotoAsync(url: $"/privacy/{lon}/{lat}");
            
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
                    Predicate = message => message.Text.Contains(value: "privacy: consent") ||
                                           message.Text.Contains(value: "privacy: list") ||
                                           message.Text.Contains(value: "privacy: map") ||
                                           message.Text.Contains(value: "privacy: screen") ||
                                           message.Text.Contains(value: "privacy: search")
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
                
                parent = page.GetByTestId(testId: "telerik-auto-complete");
                
                await Assertions
                    .Expect(locator: parent)
                    .ToBeInViewportAsync();
                
                child = parent.GetByRole(role: AriaRole.Button);
                
                await Assertions
                    .Expect(locator: child)
                    .ToBeInViewportAsync();
                
                await child.ClickAsync();
                
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
                    path2: $"privacy|dark|telerik-auto-complete|combo-box-clear|run{run}|mobile.png"),
                bytes: Screenshot ?? []);
            
            await UploadTestAsync();
            
            #endregion
        });
        
        await CompleteTestAsync(error: Error);
    }
}
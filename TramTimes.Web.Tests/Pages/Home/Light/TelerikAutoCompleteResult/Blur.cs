using Microsoft.Playwright;
using TramTimes.Web.Tests.Cookies;
using TramTimes.Web.Tests.Managers;
using Xunit;

namespace TramTimes.Web.Tests.Pages.Home.Light.TelerikAutoCompleteResult;

public class Blur(AspireManager aspireManager) : BaseTest(aspireManager: aspireManager)
{
    private AspireManager AspireManager { get; } = aspireManager ?? throw new ArgumentNullException(paramName: nameof(aspireManager));
    private byte[]? Screenshot { get; set; }
    private string? Error { get; set; }
    
    [Theory]
    [InlineData("Halfway", 53.328532846077614, -1.3443136700078966, "To City", "halfw", 1)]
    [InlineData("Malin Bridge", 53.40064593919049, -1.5082120329876791, "To City", "malin", 2)]
    [InlineData("Middlewood", 53.41586234037237, -1.510067739914952, "To City", "middl", 3)]
    public async Task ExtraSmall(
        string name,
        double lat,
        double lon,
        string direction,
        string query,
        int run) {
        
        await ConfigureTestAsync<Projects.TramTimes_Aspire_Host>();
        
        await RunTestAsync(cookie: ConsentCookies.False, scheme: ColorScheme.Light, test: async page =>
        {
            #region configure page
            
            await page.SetViewportSizeAsync(
                width: 320,
                height: 568);
            
            #endregion
            
            #region load page
            
            await page.GotoAsync(url: $"/{lon}/{lat}");
            
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
                var locator = page.GetByTestId(testId: "telerik-auto-complete");
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
                
                locator = locator.GetByRole(role: AriaRole.Combobox);
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
                
                await locator.FillAsync(value: query);
                
                await page.WaitForConsoleMessageAsync(options: new PageWaitForConsoleMessageOptions
                {
                    Predicate = message => message.Text.Equals(value: $"home: search filter {query}")
                });
                
                locator = page.GetByTestId(testId: "telerik-auto-complete__result").First;
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
                
                locator = locator.GetByTestId(testId: "name");
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
                
                locator = locator.GetByText(text: name);
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
                
                locator = page.GetByTestId(testId: "telerik-auto-complete__result").First;
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
                
                locator = locator.GetByTestId(testId: "direction");
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
                
                locator = locator.GetByText(text: direction);
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
                
                locator = page.GetByTestId(testId: "telerik-auto-complete__result").First;
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
                
                locator = locator.GetByTestId(testId: "services");
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
                
                locator = page.GetByTestId(testId: "telerik-auto-complete");
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
                
                locator = locator.GetByRole(role: AriaRole.Combobox);
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
                
                await locator.BlurAsync();
                
                locator = page.GetByTestId(testId: "telerik-auto-complete__result").First;
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeHiddenAsync();
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
                    path2: $"home|light|telerik-auto-complete-result|blur|run{run}|01.png"),
                bytes: Screenshot ?? []);
            
            await UploadTestAsync();
            
            #endregion
        });
        
        await CompleteTestAsync(error: Error);
    }
    
    [Theory]
    [InlineData("Halfway", 53.328532846077614, -1.3443136700078966, "To City", "halfw", 1)]
    [InlineData("Malin Bridge", 53.40064593919049, -1.5082120329876791, "To City", "malin", 2)]
    [InlineData("Middlewood", 53.41586234037237, -1.510067739914952, "To City", "middl", 3)]
    public async Task Small(
        string name,
        double lat,
        double lon,
        string direction,
        string query,
        int run) {
        
        await ConfigureTestAsync<Projects.TramTimes_Aspire_Host>();
        
        await RunTestAsync(cookie: ConsentCookies.False, scheme: ColorScheme.Light, test: async page =>
        {
            #region configure page
            
            await page.SetViewportSizeAsync(
                width: 640,
                height: 480);
            
            #endregion
            
            #region load page
            
            await page.GotoAsync(url: $"/{lon}/{lat}");
            
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
                var locator = page.GetByTestId(testId: "telerik-auto-complete");
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
                
                locator = locator.GetByRole(role: AriaRole.Combobox);
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
                
                await locator.FillAsync(value: query);
                
                await page.WaitForConsoleMessageAsync(options: new PageWaitForConsoleMessageOptions
                {
                    Predicate = message => message.Text.Equals(value: $"home: search filter {query}")
                });
                
                locator = page.GetByTestId(testId: "telerik-auto-complete__result").First;
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
                
                locator = locator.GetByTestId(testId: "name");
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
                
                locator = locator.GetByText(text: name);
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
                
                locator = page.GetByTestId(testId: "telerik-auto-complete__result").First;
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
                
                locator = locator.GetByTestId(testId: "direction");
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
                
                locator = locator.GetByText(text: direction);
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
                
                locator = page.GetByTestId(testId: "telerik-auto-complete__result").First;
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
                
                locator = locator.GetByTestId(testId: "services");
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
                
                locator = page.GetByTestId(testId: "telerik-auto-complete");
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
                
                locator = locator.GetByRole(role: AriaRole.Combobox);
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
                
                await locator.BlurAsync();
                
                locator = page.GetByTestId(testId: "telerik-auto-complete__result").First;
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeHiddenAsync();
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
                    path2: $"home|light|telerik-auto-complete-result|blur|run{run}|02.png"),
                bytes: Screenshot ?? []);
            
            await UploadTestAsync();
            
            #endregion
        });
        
        await CompleteTestAsync(error: Error);
    }
    
    [Theory]
    [InlineData("Halfway", 53.328532846077614, -1.3443136700078966, "To City", "halfw", 1)]
    [InlineData("Malin Bridge", 53.40064593919049, -1.5082120329876791, "To City", "malin", 2)]
    [InlineData("Middlewood", 53.41586234037237, -1.510067739914952, "To City", "middl", 3)]
    public async Task Medium(
        string name,
        double lat,
        double lon,
        string direction,
        string query,
        int run) {
        
        await ConfigureTestAsync<Projects.TramTimes_Aspire_Host>();
        
        await RunTestAsync(cookie: ConsentCookies.False, scheme: ColorScheme.Light, test: async page =>
        {
            #region configure page
            
            await page.SetViewportSizeAsync(
                width: 800,
                height: 600);
            
            #endregion
            
            #region load page
            
            await page.GotoAsync(url: $"/{lon}/{lat}");
            
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
                var locator = page.GetByTestId(testId: "telerik-auto-complete");
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
                
                locator = locator.GetByRole(role: AriaRole.Combobox);
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
                
                await locator.FillAsync(value: query);
                
                await page.WaitForConsoleMessageAsync(options: new PageWaitForConsoleMessageOptions
                {
                    Predicate = message => message.Text.Equals(value: $"home: search filter {query}")
                });
                
                locator = page.GetByTestId(testId: "telerik-auto-complete__result").First;
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
                
                locator = locator.GetByTestId(testId: "name");
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
                
                locator = locator.GetByText(text: name);
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
                
                locator = page.GetByTestId(testId: "telerik-auto-complete__result").First;
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
                
                locator = locator.GetByTestId(testId: "direction");
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
                
                locator = locator.GetByText(text: direction);
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
                
                locator = page.GetByTestId(testId: "telerik-auto-complete__result").First;
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
                
                locator = locator.GetByTestId(testId: "services");
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
                
                locator = page.GetByTestId(testId: "telerik-auto-complete");
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
                
                locator = locator.GetByRole(role: AriaRole.Combobox);
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
                
                await locator.BlurAsync();
                
                locator = page.GetByTestId(testId: "telerik-auto-complete__result").First;
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeHiddenAsync();
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
                    path2: $"home|light|telerik-auto-complete-result|blur|run{run}|03.png"),
                bytes: Screenshot ?? []);
            
            await UploadTestAsync();
            
            #endregion
        });
        
        await CompleteTestAsync(error: Error);
    }
    
    [Theory]
    [InlineData("Halfway", 53.328532846077614, -1.3443136700078966, "To City", "halfw", 1)]
    [InlineData("Malin Bridge", 53.40064593919049, -1.5082120329876791, "To City", "malin", 2)]
    [InlineData("Middlewood", 53.41586234037237, -1.510067739914952, "To City", "middl", 3)]
    public async Task Large(
        string name,
        double lat,
        double lon,
        string direction,
        string query,
        int run) {
        
        await ConfigureTestAsync<Projects.TramTimes_Aspire_Host>();
        
        await RunTestAsync(cookie: ConsentCookies.False, scheme: ColorScheme.Light, test: async page =>
        {
            #region configure page
            
            await page.SetViewportSizeAsync(
                width: 1024,
                height: 768);
            
            #endregion
            
            #region load page
            
            await page.GotoAsync(url: $"/{lon}/{lat}");
            
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
                var locator = page.GetByTestId(testId: "telerik-auto-complete");
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
                
                locator = locator.GetByRole(role: AriaRole.Combobox);
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
                
                await locator.FillAsync(value: query);
                
                await page.WaitForConsoleMessageAsync(options: new PageWaitForConsoleMessageOptions
                {
                    Predicate = message => message.Text.Equals(value: $"home: search filter {query}")
                });
                
                locator = page.GetByTestId(testId: "telerik-auto-complete__result").First;
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
                
                locator = locator.GetByTestId(testId: "name");
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
                
                locator = locator.GetByText(text: name);
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
                
                locator = page.GetByTestId(testId: "telerik-auto-complete__result").First;
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
                
                locator = locator.GetByTestId(testId: "direction");
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
                
                locator = locator.GetByText(text: direction);
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
                
                locator = page.GetByTestId(testId: "telerik-auto-complete__result").First;
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
                
                locator = locator.GetByTestId(testId: "services");
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
                
                locator = page.GetByTestId(testId: "telerik-auto-complete");
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
                
                locator = locator.GetByRole(role: AriaRole.Combobox);
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
                
                await locator.BlurAsync();
                
                locator = page.GetByTestId(testId: "telerik-auto-complete__result").First;
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeHiddenAsync();
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
                    path2: $"home|light|telerik-auto-complete-result|blur|run{run}|04.png"),
                bytes: Screenshot ?? []);
            
            await UploadTestAsync();
            
            #endregion
        });
        
        await CompleteTestAsync(error: Error);
    }
    
    [Theory]
    [InlineData("Halfway", 53.328532846077614, -1.3443136700078966, "To City", "halfw", 1)]
    [InlineData("Malin Bridge", 53.40064593919049, -1.5082120329876791, "To City", "malin", 2)]
    [InlineData("Middlewood", 53.41586234037237, -1.510067739914952, "To City", "middl", 3)]
    public async Task ExtraLarge(
        string name,
        double lat,
        double lon,
        string direction,
        string query,
        int run) {
        
        await ConfigureTestAsync<Projects.TramTimes_Aspire_Host>();
        
        await RunTestAsync(cookie: ConsentCookies.False, scheme: ColorScheme.Light, test: async page =>
        {
            #region configure page
            
            await page.SetViewportSizeAsync(
                width: 1280,
                height: 800);
            
            #endregion
            
            #region load page
            
            await page.GotoAsync(url: $"/{lon}/{lat}");
            
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
                var locator = page.GetByTestId(testId: "telerik-auto-complete");
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
                
                locator = locator.GetByRole(role: AriaRole.Combobox);
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
                
                await locator.FillAsync(value: query);
                
                await page.WaitForConsoleMessageAsync(options: new PageWaitForConsoleMessageOptions
                {
                    Predicate = message => message.Text.Equals(value: $"home: search filter {query}")
                });
                
                locator = page.GetByTestId(testId: "telerik-auto-complete__result").First;
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
                
                locator = locator.GetByTestId(testId: "name");
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
                
                locator = locator.GetByText(text: name);
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
                
                locator = page.GetByTestId(testId: "telerik-auto-complete__result").First;
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
                
                locator = locator.GetByTestId(testId: "direction");
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
                
                locator = locator.GetByText(text: direction);
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
                
                locator = page.GetByTestId(testId: "telerik-auto-complete__result").First;
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
                
                locator = locator.GetByTestId(testId: "services");
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
                
                locator = page.GetByTestId(testId: "telerik-auto-complete");
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
                
                locator = locator.GetByRole(role: AriaRole.Combobox);
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
                
                await locator.BlurAsync();
                
                locator = page.GetByTestId(testId: "telerik-auto-complete__result").First;
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeHiddenAsync();
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
                    path2: $"home|light|telerik-auto-complete-result|blur|run{run}|05.png"),
                bytes: Screenshot ?? []);
            
            await UploadTestAsync();
            
            #endregion
        });
        
        await CompleteTestAsync(error: Error);
    }
    
    [Theory]
    [InlineData("Halfway", 53.328532846077614, -1.3443136700078966, "To City", "halfw", 1)]
    [InlineData("Malin Bridge", 53.40064593919049, -1.5082120329876791, "To City", "malin", 2)]
    [InlineData("Middlewood", 53.41586234037237, -1.510067739914952, "To City", "middl", 3)]
    public async Task ExtraExtraLarge(
        string name,
        double lat,
        double lon,
        string direction,
        string query,
        int run) {
        
        await ConfigureTestAsync<Projects.TramTimes_Aspire_Host>();
        
        await RunTestAsync(cookie: ConsentCookies.False, scheme: ColorScheme.Light, test: async page =>
        {
            #region configure page
            
            await page.SetViewportSizeAsync(
                width: 1440,
                height: 900);
            
            #endregion
            
            #region load page
            
            await page.GotoAsync(url: $"/{lon}/{lat}");
            
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
                var locator = page.GetByTestId(testId: "telerik-auto-complete");
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
                
                locator = locator.GetByRole(role: AriaRole.Combobox);
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
                
                await locator.FillAsync(value: query);
                
                await page.WaitForConsoleMessageAsync(options: new PageWaitForConsoleMessageOptions
                {
                    Predicate = message => message.Text.Equals(value: $"home: search filter {query}")
                });
                
                locator = page.GetByTestId(testId: "telerik-auto-complete__result").First;
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
                
                locator = locator.GetByTestId(testId: "name");
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
                
                locator = locator.GetByText(text: name);
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
                
                locator = page.GetByTestId(testId: "telerik-auto-complete__result").First;
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
                
                locator = locator.GetByTestId(testId: "direction");
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
                
                locator = locator.GetByText(text: direction);
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
                
                locator = page.GetByTestId(testId: "telerik-auto-complete__result").First;
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
                
                locator = locator.GetByTestId(testId: "services");
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
                
                locator = page.GetByTestId(testId: "telerik-auto-complete");
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
                
                locator = locator.GetByRole(role: AriaRole.Combobox);
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
                
                await locator.BlurAsync();
                
                locator = page.GetByTestId(testId: "telerik-auto-complete__result").First;
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeHiddenAsync();
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
                    path2: $"home|light|telerik-auto-complete-result|blur|run{run}|06.png"),
                bytes: Screenshot ?? []);
            
            await UploadTestAsync();
            
            #endregion
        });
        
        await CompleteTestAsync(error: Error);
    }
}
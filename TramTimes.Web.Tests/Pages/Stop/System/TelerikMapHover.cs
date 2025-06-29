using Microsoft.Playwright;
using TramTimes.Web.Tests.Managers;
using Xunit;

namespace TramTimes.Web.Tests.Pages.Stop.System;

public class TelerikMapHover(AspireManager aspireManager) : BaseTest(aspireManager: aspireManager)
{
    private AspireManager AspireManager { get; } = aspireManager ?? throw new ArgumentNullException(paramName: nameof(aspireManager));
    private byte[]? Screenshot { get; set; }
    private string? Error { get; set; }
    
    [Theory]
    [InlineData("9400ZZSYHFW1", "Halfway", 53.328532846077614, -1.3443136700078966, 1)]
    [InlineData("9400ZZSYMAL1", "Malin Bridge", 53.40064593919049, -1.5082120329876791, 2)]
    [InlineData("9400ZZSYMID1", "Middlewood", 53.41586234037237, -1.510067739914952, 3)]
    public async Task ExtraSmall(
        string id,
        string name,
        double lat,
        double lon,
        int run) {
        
        await ConfigureTestAsync<Projects.TramTimes_Aspire_Host>();
        
        await RunTestSystemModeAsync(test: async page =>
        {
            #region configure page
            
            await page.SetViewportSizeAsync(
                width: 320,
                height: 568);
            
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
                var locator = page.Locator(selector: "telerik-map");
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
                
                locator = page.Locator(selector: ".k-map-marker").First;
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
                
                await locator.HoverAsync();
                
                locator = page.GetByText(text: name);
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
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
                    path2: $"stop|system|telerik-map-hover|run{run}|01.png"),
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
    public async Task Small(
        string id,
        string name,
        double lat,
        double lon,
        int run) {
        
        await ConfigureTestAsync<Projects.TramTimes_Aspire_Host>();
        
        await RunTestSystemModeAsync(test: async page =>
        {
            #region configure page
            
            await page.SetViewportSizeAsync(
                width: 640,
                height: 480);
            
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
                var locator = page.Locator(selector: "telerik-map");
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
                
                locator = page.Locator(selector: ".k-map-marker").First;
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
                
                await locator.HoverAsync();
                
                locator = page.GetByText(text: name);
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
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
                    path2: $"stop|system|telerik-map-hover|run{run}|02.png"),
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
    public async Task Medium(
        string id,
        string name,
        double lat,
        double lon,
        int run) {
        
        await ConfigureTestAsync<Projects.TramTimes_Aspire_Host>();
        
        await RunTestSystemModeAsync(test: async page =>
        {
            #region configure page
            
            await page.SetViewportSizeAsync(
                width: 800,
                height: 600);
            
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
                var locator = page.Locator(selector: "telerik-map");
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
                
                locator = page.Locator(selector: ".k-map-marker").First;
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
                
                await locator.HoverAsync();
                
                locator = page.GetByText(text: name);
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
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
                    path2: $"stop|system|telerik-map-hover|run{run}|03.png"),
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
    public async Task Large(
        string id,
        string name,
        double lat,
        double lon,
        int run) {
        
        await ConfigureTestAsync<Projects.TramTimes_Aspire_Host>();
        
        await RunTestSystemModeAsync(test: async page =>
        {
            #region configure page
            
            await page.SetViewportSizeAsync(
                width: 1024,
                height: 768);
            
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
                var locator = page.Locator(selector: "telerik-map");
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
                
                locator = page.Locator(selector: ".k-map-marker").First;
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
                
                await locator.HoverAsync();
                
                locator = page.GetByText(text: name);
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
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
                    path2: $"stop|system|telerik-map-hover|run{run}|04.png"),
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
    public async Task ExtraLarge(
        string id,
        string name,
        double lat,
        double lon,
        int run) {
        
        await ConfigureTestAsync<Projects.TramTimes_Aspire_Host>();
        
        await RunTestSystemModeAsync(test: async page =>
        {
            #region configure page
            
            await page.SetViewportSizeAsync(
                width: 1280,
                height: 800);
            
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
                var locator = page.Locator(selector: "telerik-map");
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
                
                locator = page.Locator(selector: ".k-map-marker").First;
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
                
                await locator.HoverAsync();
                
                locator = page.GetByText(text: name);
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
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
                    path2: $"stop|system|telerik-map-hover|run{run}|05.png"),
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
    public async Task ExtraExtraLarge(
        string id,
        string name,
        double lat,
        double lon,
        int run) {
        
        await ConfigureTestAsync<Projects.TramTimes_Aspire_Host>();
        
        await RunTestSystemModeAsync(test: async page =>
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
                var locator = page.Locator(selector: "telerik-map");
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
                
                locator = page.Locator(selector: ".k-map-marker").First;
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
                
                await locator.HoverAsync();
                
                locator = page.GetByText(text: name);
                
                await Assertions
                    .Expect(locator: locator)
                    .ToBeVisibleAsync();
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
                    path2: $"stop|system|telerik-map-hover|run{run}|06.png"),
                bytes: Screenshot ?? []);
            
            await UploadTestAsync();
            
            #endregion
        });
        
        await CompleteTestAsync(error: Error);
    }
}
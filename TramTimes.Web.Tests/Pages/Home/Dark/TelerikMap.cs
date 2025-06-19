using Microsoft.Playwright;
using TramTimes.Web.Tests.Managers;
using Xunit;

namespace TramTimes.Web.Tests.Pages.Home.Dark;

public class TelerikMap(AspireManager aspireManager) : BaseTest(aspireManager: aspireManager)
{
    private AspireManager AspireManager { get; } = aspireManager ?? throw new ArgumentNullException(paramName: nameof(aspireManager));
    private byte[]? Screenshot { get; set; }
    private string? Error { get; set; }
    
    [Fact]
    public async Task ExtraSmall()
    {
        await ConfigureTestAsync<Projects.TramTimes_Aspire_Host>();
        
        await RunTestDarkModeAsync(test: async page =>
        {
            #region configure page
            
            await page.SetViewportSizeAsync(
                width: 320,
                height: 568);
            
            #endregion
            
            #region load page
            
            await page.GotoAsync(url: "/");
            await page.WaitForLoadStateAsync(state: LoadState.NetworkIdle);
            
            #endregion
            
            #region test page
            
            Error = string.Empty;
            
            try
            {
                await Assertions.Expect(locator: page.Locator(selector: "#home-telerik-map")).ToBeVisibleAsync();
                await Assertions.Expect(locator: page.Locator(selector: "span:nth-child(3) > .k-map-marker")).ToBeVisibleAsync();
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
                    path2: "home-dark-telerik-map-extra-small.png"),
                bytes: Screenshot);
            
            await UploadTestAsync();
            
            #endregion
        });
        
        await CompleteTestAsync(error: Error);
    }
    
    [Fact]
    public async Task Small()
    {
        await ConfigureTestAsync<Projects.TramTimes_Aspire_Host>();
        
        await RunTestDarkModeAsync(test: async page =>
        {
            #region configure page
            
            await page.SetViewportSizeAsync(
                width: 640,
                height: 480);
            
            #endregion
            
            #region load page
            
            await page.GotoAsync(url: "/");
            await page.WaitForLoadStateAsync(state: LoadState.NetworkIdle);
            
            #endregion
            
            #region test page
            
            Error = string.Empty;
            
            try
            {
                await Assertions.Expect(locator: page.Locator(selector: "#home-telerik-map")).ToBeVisibleAsync();
                await Assertions.Expect(locator: page.Locator(selector: "span:nth-child(3) > .k-map-marker")).ToBeVisibleAsync();
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
                    path2: "home-dark-telerik-map-small.png"),
                bytes: Screenshot);
            
            await UploadTestAsync();
            
            #endregion
        });
        
        await CompleteTestAsync(error: Error);
    }
    
    [Fact]
    public async Task Medium()
    {
        await ConfigureTestAsync<Projects.TramTimes_Aspire_Host>();
        
        await RunTestDarkModeAsync(test: async page =>
        {
            #region configure page
            
            await page.SetViewportSizeAsync(
                width: 800,
                height: 600);
            
            #endregion
            
            #region load page
            
            await page.GotoAsync(url: "/");
            await page.WaitForLoadStateAsync(state: LoadState.NetworkIdle);
            
            #endregion
            
            #region test page
            
            Error = string.Empty;
            
            try
            {
                await Assertions.Expect(locator: page.Locator(selector: "#home-telerik-map")).ToBeVisibleAsync();
                await Assertions.Expect(locator: page.Locator(selector: "span:nth-child(3) > .k-map-marker")).ToBeVisibleAsync();
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
                    path2: "home-dark-telerik-map-medium.png"),
                bytes: Screenshot);
            
            await UploadTestAsync();
            
            #endregion
        });
        
        await CompleteTestAsync(error: Error);
    }
    
    [Fact]
    public async Task Large()
    {
        await ConfigureTestAsync<Projects.TramTimes_Aspire_Host>();
        
        await RunTestDarkModeAsync(test: async page =>
        {
            #region configure page
            
            await page.SetViewportSizeAsync(
                width: 1024,
                height: 768);
            
            #endregion
            
            #region load page
            
            await page.GotoAsync(url: "/");
            await page.WaitForLoadStateAsync(state: LoadState.NetworkIdle);
            
            #endregion
            
            #region test page
            
            Error = string.Empty;
            
            try
            {
                await Assertions.Expect(locator: page.Locator(selector: "#home-telerik-map")).ToBeVisibleAsync();
                await Assertions.Expect(locator: page.Locator(selector: "span:nth-child(3) > .k-map-marker")).ToBeVisibleAsync();
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
                    path2: "home-dark-telerik-map-large.png"),
                bytes: Screenshot);
            
            await UploadTestAsync();
            
            #endregion
        });
        
        await CompleteTestAsync(error: Error);
    }
    
    [Fact]
    public async Task ExtraLarge()
    {
        await ConfigureTestAsync<Projects.TramTimes_Aspire_Host>();
        
        await RunTestDarkModeAsync(test: async page =>
        {
            #region configure page
            
            await page.SetViewportSizeAsync(
                width: 1280,
                height: 800);
            
            #endregion
            
            #region load page
            
            await page.GotoAsync(url: "/");
            await page.WaitForLoadStateAsync(state: LoadState.NetworkIdle);
            
            #endregion
            
            #region test page
            
            Error = string.Empty;
            
            try
            {
                await Assertions.Expect(locator: page.Locator(selector: "#home-telerik-map")).ToBeVisibleAsync();
                await Assertions.Expect(locator: page.Locator(selector: "span:nth-child(3) > .k-map-marker")).ToBeVisibleAsync();
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
                    path2: "home-dark-telerik-map-extra-large.png"),
                bytes: Screenshot);
            
            await UploadTestAsync();
            
            #endregion
        });
        
        await CompleteTestAsync(error: Error);
    }
    
    [Fact]
    public async Task ExtraExtraLarge()
    {
        await ConfigureTestAsync<Projects.TramTimes_Aspire_Host>();
        
        await RunTestDarkModeAsync(test: async page =>
        {
            #region configure page
            
            await page.SetViewportSizeAsync(
                width: 1440,
                height: 900);
            
            #endregion
            
            #region load page
            
            await page.GotoAsync(url: "/");
            await page.WaitForLoadStateAsync(state: LoadState.NetworkIdle);
            
            #endregion
            
            #region test page
            
            Error = string.Empty;
            
            try
            {
                await Assertions.Expect(locator: page.Locator(selector: "#home-telerik-map")).ToBeVisibleAsync();
                await Assertions.Expect(locator: page.Locator(selector: "span:nth-child(3) > .k-map-marker")).ToBeVisibleAsync();
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
                    path2: "home-dark-telerik-map-extra-extra-large.png"),
                bytes: Screenshot);
            
            await UploadTestAsync();
            
            #endregion
        });
        
        await CompleteTestAsync(error: Error);
    }
}
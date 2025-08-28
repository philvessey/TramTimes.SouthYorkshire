using Microsoft.Playwright;
using TramTimes.Web.Tests.Cookies;
using TramTimes.Web.Tests.Managers;
using Xunit;

namespace TramTimes.Web.Tests.Pages.Privacy.Light.SourceCodeRepository;

public class ButtonClick(AspireManager aspireManager) : BaseTest(aspireManager: aspireManager)
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
        
        await RunTestAsync(cookie: ConsentCookies.True, scheme: ColorScheme.Light, test: async page =>
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
                response.Status == 200);
            
            #endregion
            
            #region test page
            
            Error = string.Empty;
            
            try
            {
                var parent = page.GetByTestId(testId: "source-code-repository");
                
                await Assertions
                    .Expect(locator: parent)
                    .ToBeInViewportAsync();
                
                var child = parent.GetByTestId(testId: "icon");
                
                await Assertions
                    .Expect(locator: child)
                    .ToBeInViewportAsync();
                
                await child.ClickAsync();
                
                await Assertions
                    .Expect(page: page)
                    .ToHaveURLAsync(urlOrRegExp: "https://github.com/philvessey/TramTimes.SouthYorkshire");
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
                    path2: $"privacy|light|source-code-repository|button-click|run{run}|desktop.png"),
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
        
        await RunTestAsync(cookie: ConsentCookies.False, scheme: ColorScheme.Light, test: async page =>
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
                response.Status == 200);
            
            #endregion
            
            #region test page
            
            Error = string.Empty;
            
            try
            {
                var parent = page.GetByTestId(testId: "source-code-repository");
                
                await Assertions
                    .Expect(locator: parent)
                    .ToBeInViewportAsync();
                
                var child = parent.GetByTestId(testId: "icon");
                
                await Assertions
                    .Expect(locator: child)
                    .ToBeInViewportAsync();
                
                await child.ClickAsync();
                
                await Assertions
                    .Expect(page: page)
                    .ToHaveURLAsync(urlOrRegExp: "https://github.com/philvessey/TramTimes.SouthYorkshire");
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
                    path2: $"privacy|light|source-code-repository|button-click|run{run}|mobile.png"),
                bytes: Screenshot ?? []);
            
            await UploadTestAsync();
            
            #endregion
        });
        
        await CompleteTestAsync(error: Error);
    }
}
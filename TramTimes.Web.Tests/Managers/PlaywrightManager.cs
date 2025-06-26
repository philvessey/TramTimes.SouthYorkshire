// ReSharper disable all

using System.Diagnostics;
using Microsoft.Playwright;
using Xunit;

namespace TramTimes.Web.Tests.Managers;

public class PlaywrightManager : IAsyncLifetime
{
    private IPlaywright Manager { get; set; } = null!;
    public IBrowser? Browser { get; set; }
    
    public async Task InitializeAsync()
    {
        #region configure manager
        
        Assertions.SetDefaultExpectTimeout(timeout: 15_000);
        
        #endregion
        
        #region create manager
        
        Manager = await Playwright.CreateAsync();
        
        #endregion
        
        #region launch browser
        
        Browser = await Manager.Chromium.LaunchAsync(options: new BrowserTypeLaunchOptions
        {
            Headless = Debugger.IsAttached is false
        });
        
        #endregion
    }
    
    public async Task DisposeAsync()
    {
        #region close browser
        
        await Browser!.CloseAsync();
        
        #endregion
        
        #region dispose manager
        
        Manager.Dispose();
        
        #endregion
    }
}
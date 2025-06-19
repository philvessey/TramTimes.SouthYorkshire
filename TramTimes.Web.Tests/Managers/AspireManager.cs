// ReSharper disable all

using Aspire.Hosting;
using Aspire.Hosting.Testing;
using Xunit;

namespace TramTimes.Web.Tests.Managers;

public class AspireManager : IAsyncLifetime
{
    public PlaywrightManager PlaywrightManager { get; set; } = new();
    public DistributedApplication? Application { get; set; }
    public DirectoryInfo? Storage { get; set; }
    
    public async Task ConfigureAsync<TEntryPoint>() where TEntryPoint : class
    {
        #region check application
        
        if (Application is not null)
            return;
        
        #endregion
        
        #region check storage
        
        if (Storage is not null)
            return;
        
        #endregion
        
        #region create application
        
        var builder = await DistributedApplicationTestingBuilder.CreateAsync<TEntryPoint>();
        builder.Configuration["ASPIRE_ALLOW_UNSECURED_TRANSPORT"] = "true";
        
        Application = await builder.BuildAsync();
        
        #endregion
        
        #region create storage
		
        var guid = Guid.NewGuid();
        
        Storage = Directory.CreateDirectory(path: Path.Combine(
            path1: Path.GetTempPath(),
            path2: guid.ToString()));
		
        #endregion
        
        #region start application
        
        await Application.StartAsync();
        
        #endregion
    }
    
    public async Task InitializeAsync()
    {
        #region initialize manager
        
        await PlaywrightManager.InitializeAsync();
        
        #endregion
    }
    
    public async Task DisposeAsync()
    {
        #region stop application
        
        await Application!.StopAsync();
        await Application!.DisposeAsync();
        
        #endregion
        
        #region stop storage
		
        Storage!.Delete(recursive: true);
		
        #endregion
        
        #region dispose manager
        
        await PlaywrightManager.DisposeAsync();
        
        #endregion
    }
}
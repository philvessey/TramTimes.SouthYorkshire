using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.JSInterop;

namespace TramTimes.Web.Site.Components.Shared;

public partial class LocalStorageConsent : ComponentBase
{
    private IJSObjectReference? Manager { get; set; }
    private string? ConsentCookie { get; set; }
    private bool ShowPolicy { get; set; }
    
    protected override void OnInitialized()
    {
        #region get feature
        
        var feature = AccessorService.HttpContext?.Features.Get<ITrackingConsentFeature>();
        
        #endregion
        
        #region create consent
        
        ConsentCookie = feature?.CreateConsentCookie();
        
        #endregion
        
        #region toggle policy
        
        ShowPolicy = false;
        
        #endregion
    }
    
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        #region create manager
        
        if (firstRender)
            Manager = await JavascriptService.InvokeAsync<IJSObjectReference>(
                identifier: "import",
                args: "./Components/Shared/LocalStorageConsent.razor.js");
        
        #endregion
    }
    
    private void ShowPrivacyPolicy()
    {
        #region toggle policy
        
        ShowPolicy = !ShowPolicy;
        
        #endregion
    }
    
    private async Task AcceptPrivacyPolicy()
    {
        #region set cookie
        
        if (Manager is not null)
            await Manager.InvokeVoidAsync(
                identifier: "setCookie",
                args: [ConsentCookie, DateTime.UtcNow
                    .AddDays(value: 365)
                    .ToString(provider: CultureInfo.InvariantCulture)]);
        
        #endregion
        
        #region navigate page
        
        NavigationService.NavigateTo(
            uri: NavigationService.Uri,
            forceLoad: true);
        
        #endregion
    }
    
    private async Task RejectPrivacyPolicy()
    {
        #region set cookie
        
        if (Manager is not null)
            await Manager.InvokeVoidAsync(
                identifier: "setCookie",
                args: [ConsentCookie, DateTime.UtcNow
                    .AddDays(value: -1)
                    .ToString(provider: CultureInfo.InvariantCulture)]);
        
        #endregion
        
        #region navigate page
        
        NavigationService.NavigateTo(
            uri: NavigationService.Uri,
            forceLoad: true);
        
        #endregion
    }
    
    async ValueTask IAsyncDisposable.DisposeAsync()
    {
        #region suppress finalizer
		
        GC.SuppressFinalize(obj: this);
		
        #endregion
        
        #region dispose manager
        
        try
        {
            if (Manager is not null)
                await Manager.DisposeAsync();
        }
        catch (JSDisconnectedException e)
        {
            LoggerService.LogInformation(
                message: "Exception: {exception}",
                args: e.ToString());
        }
        
        #endregion
    }
}
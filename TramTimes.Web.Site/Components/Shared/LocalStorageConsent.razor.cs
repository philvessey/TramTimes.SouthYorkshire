using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.JSInterop;

namespace TramTimes.Web.Site.Components.Shared;

public partial class LocalStorageConsent : ComponentBase
{
    private IJSObjectReference? Manager { get; set; }
    private string? ConsentCookie { get; set; }
    private bool ShowOutline { get; set; }
    private bool ShowPolicy { get; set; }
    private bool Disposed { get; set; }
    
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        
        #region get feature
        
        var feature = AccessorService.HttpContext?.Features.Get<ITrackingConsentFeature>();
        
        #endregion
        
        #region create consent
        
        ConsentCookie = feature?.CreateConsentCookie();
        
        #endregion
        
        #region set toggles
        
        ShowOutline = false;
        ShowPolicy = false;
        
        #endregion
    }
    
    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();
    }
    
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender: firstRender);
        
        # region check disposed
        
        if (Disposed)
            return;
        
        #endregion
        
        #region create manager
        
        if (firstRender)
        {
            try
            {
                Manager = await JavascriptService.InvokeAsync<IJSObjectReference>(
                    identifier: "import",
                    args: "./Components/Shared/LocalStorageConsent.razor.js");
                
                var consent = await Manager.InvokeAsync<string>(
                    identifier: "getCookie",
                    args: ".AspNet.Consent");
                
                if (string.IsNullOrEmpty(value: consent))
                    ShowPrivacyOutline();
            }
            catch (ObjectDisposedException e)
            {
                LoggerService.LogInformation(
                    message: "Exception: {exception}",
                    args: e.ToString());
            }
        }
        
        #endregion
    }
    
    private void ShowPrivacyOutline()
    {
        #region set toggles
        
        ShowOutline = true;
        ShowPolicy = false;
        
        #endregion
        
        #region change state
        
        StateHasChanged();
        
        #endregion
    }
    
    private void ShowPrivacyPolicy()
    {
        #region set toggles
        
        ShowOutline = false;
        ShowPolicy = true;
        
        #endregion
        
        #region change state
        
        StateHasChanged();
        
        #endregion
    }
    
    private async Task AcceptPrivacyPolicyAsync()
    {
        # region check disposed
        
        if (Disposed)
            return;
        
        #endregion
        
        #region set cookie
        
        ConsentCookie = ConsentCookie?.Replace(
            oldValue: "false",
            newValue: "true");
        
        try
        {
            if (Manager is not null)
                await Manager.InvokeVoidAsync(
                    identifier: "setCookie",
                    args: [ConsentCookie, DateTime.UtcNow
                        .AddDays(value: 365)
                        .ToString(provider: CultureInfo.InvariantCulture)]);
        }
        catch (ObjectDisposedException e)
        {
            LoggerService.LogInformation(
                message: "Exception: {exception}",
                args: e.ToString());
        }
        
        #endregion
        
        #region navigate page
        
        NavigationService.NavigateTo(
            uri: NavigationService.Uri,
            forceLoad: true,
            replace: true);
        
        #endregion
    }
    
    private async Task RejectPrivacyPolicyAsync()
    {
        # region check disposed
        
        if (Disposed)
            return;
        
        #endregion
        
        #region set cookie
        
        ConsentCookie = ConsentCookie?.Replace(
            oldValue: "true",
            newValue: "false");
        
        try
        {
            if (Manager is not null)
                await Manager.InvokeVoidAsync(
                    identifier: "setCookie",
                    args: [ConsentCookie, DateTime.UtcNow
                        .AddDays(value: 365)
                        .ToString(provider: CultureInfo.InvariantCulture)]);
        }
        catch (ObjectDisposedException e)
        {
            LoggerService.LogInformation(
                message: "Exception: {exception}",
                args: e.ToString());
        }
        
        #endregion
        
        #region navigate page
        
        NavigationService.NavigateTo(
            uri: NavigationService.Uri,
            forceLoad: true,
            replace: true);
        
        #endregion
    }
    
    async ValueTask IAsyncDisposable.DisposeAsync()
    {
        # region set disposed
        
        Disposed = true;
        
        #endregion
        
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
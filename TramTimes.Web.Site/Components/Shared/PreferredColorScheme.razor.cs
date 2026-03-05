using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using TramTimes.Web.Site.Types;
using TramTimes.Web.Utilities.Extensions;

namespace TramTimes.Web.Site.Components.Shared;

public partial class PreferredColorScheme : ComponentBase, IAsyncDisposable
{
    private IJSObjectReference? Manager { get; set; }
    private SchemeType? SchemeState { get; set; }
    private bool Disposed { get; set; }

    protected override void OnParametersSet()
    {
        #region get state

        var scheme = SchemeService.Get();

        #endregion

        #region get cookie

        scheme = scheme is "system"
            ? AccessorService.HttpContext?.Request.Cookies[".AspNet.Preference.Scheme"] ?? "system"
            : scheme;

        if (scheme is not "dark" and not "light")
            scheme = "system";

        #endregion

        #region set state

        SchemeState = scheme switch
        {
            "dark" => SchemeType.Dark,
            "light" => SchemeType.Light,
            _ => SchemeType.System
        };

        #endregion
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        #region check disposed

        if (Disposed)
            return;

        #endregion

        #region import javascript

        if (firstRender)
        {
            Manager = await JavascriptService.InvokeAsync<IJSObjectReference>(
                identifier: "import",
                args: "./Components/Shared/PreferredColorScheme.razor.js");
        }

        #endregion
    }

    private async Task ChangeSchemeAsync()
    {
        #region check disposed

        if (Disposed)
            return;

        #endregion

        #region get state

        var scheme = SchemeState switch
        {
            SchemeType.Dark => "light",
            SchemeType.Light => "system",
            _ => "dark"
        };

        #endregion

        #region set cookie

        var cookie = NavigationService.Uri.StartsWithIgnoreCase(value: "https")
            ? $".AspNet.Preference.Scheme={scheme}; SameSite=Strict; Secure"
            : $".AspNet.Preference.Scheme={scheme}; SameSite=Strict";

        if (ConsentService.Consent is true)
            if (Manager is not null)
                await Manager.InvokeVoidAsync(
                    identifier: "setCookie",
                    args: [
                        cookie,
                        DateTime.UtcNow
                            .AddDays(value: 365)
                            .ToString(provider: CultureInfo.InvariantCulture)
                    ]);

        #endregion

        #region set state

        SchemeService.Set(scheme: scheme);

        #endregion
    }

    async ValueTask IAsyncDisposable.DisposeAsync()
    {
        #region set disposed

        Disposed = true;

        #endregion

        #region suppress finalizer

        GC.SuppressFinalize(obj: this);

        #endregion

        #region dispose javascript

        if (Manager is not null)
            await Manager.DisposeAsync();

        #endregion
    }
}
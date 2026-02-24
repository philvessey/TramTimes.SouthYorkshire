using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using TramTimes.Web.Site.Defaults;
using TramTimes.Web.Utilities.Extensions;

namespace TramTimes.Web.Site.Components.Shared;

public partial class NavigateToLocation : ComponentBase, IAsyncDisposable
{
    private IJSObjectReference? Manager { get; set; }
    private bool Disposed { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        #region check disposed

        if (Disposed)
            return;

        #endregion

        #region create manager

        if (firstRender)
        {
            Manager = await JavascriptService.InvokeAsync<IJSObjectReference>(
                identifier: "import",
                args: "./Components/Shared/NavigateToLocation.razor.js");
        }

        #endregion
    }

    private async Task GetPositionAsync()
    {
        #region check disposed

        if (Disposed)
            return;

        #endregion

        #region get position

        if (Manager is not null)
            await Manager.InvokeVoidAsync(
                identifier: "getPosition",
                args: [DotNetObjectReference.Create(value: this), Longitude, Latitude]);

        #endregion
    }

    [JSInvokable]
    public void OnPosition(
        double longitude,
        double latitude) {

        #region check disposed

        if (Disposed)
            return;

        #endregion

        #region get page

        var privacy = NavigationService.Uri.StartsWithIgnoreCase(value: NavigationService.BaseUri + "privacy");
        var stop = NavigationService.Uri.StartsWithIgnoreCase(value: NavigationService.BaseUri + "stop");
        var trip = NavigationService.Uri.StartsWithIgnoreCase(value: NavigationService.BaseUri + "trip");

        #endregion

        #region navigate to

        NavigationService.NavigateTo(
            uri: $"/{longitude}/{latitude}/{TelerikMapDefaults.Zoom}",
            replace: !privacy && !stop && !trip);

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

        #region dispose manager

        if (Manager is not null)
            await Manager.DisposeAsync();

        #endregion
    }
}
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using TramTimes.Web.Site.Defaults;

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

        #region import javascript

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
                args: [
                    DotNetObjectReference.Create(value: this),
                    Longitude,
                    Latitude
                ]);

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

        #region navigate to

        NavigationService.NavigateTo(
            uri: $"/{longitude}/{latitude}/{TelerikMapDefaults.Zoom}",
            replace: Page is "home");

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
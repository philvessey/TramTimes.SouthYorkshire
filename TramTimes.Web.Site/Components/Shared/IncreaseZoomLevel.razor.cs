using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using TramTimes.Web.Site.Defaults;

namespace TramTimes.Web.Site.Components.Shared;

public partial class IncreaseZoomLevel : ComponentBase, IAsyncDisposable
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
                args: "./Components/Shared/IncreaseZoomLevel.razor.js");
        }

        #endregion
    }

    private async Task ChangeZoomAsync()
    {
        #region check disposed

        if (Disposed)
            return;

        #endregion

        #region get zoom

        if (!Zoom.HasValue)
            return;

        var zoom = Zoom.Value % 1 is not 0
            ? Math.Ceiling(a: Zoom.Value)
            : Zoom.Value + 1;

        if (zoom > TelerikMapDefaults.MaxZoom)
            return;

        #endregion

        #region output console

        if (Manager is not null)
            await Manager.InvokeVoidAsync(
                identifier: "writeConsole",
                args: $"{Page}: map zoom {zoom}");

        #endregion

        #region navigate to

        NavigationService.NavigateTo(
            uri: $"/{Longitude}/{Latitude}/{zoom}",
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
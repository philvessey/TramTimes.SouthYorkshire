using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace TramTimes.Web.Site.Components.Shared;

public partial class SourceCodeRepository : ComponentBase, IAsyncDisposable
{
    private IJSObjectReference? Manager { get; set; }
    private bool Disposed { get; set; }
    
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
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
                    args: "./Components/Shared/SourceCodeRepository.razor.js");
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
    
    private async Task NavigateToAsync()
    {
        # region check disposed
        
        if (Disposed)
            return;
        
        #endregion
        
        #region navigate to
        
        try
        {
            if (Manager is not null)
                await Manager.InvokeVoidAsync(
                    identifier: "navigateTo",
                    args: Url);
        }
        catch (ObjectDisposedException e)
        {
            LoggerService.LogInformation(
                message: "Exception: {exception}",
                args: e.ToString());
        }
        
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
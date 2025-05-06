using Telerik.Blazor.Components;
using TramTimes.Web.Site.Defaults;

namespace TramTimes.Web.Site.Components.Pages;

public partial class Home
{
    private double[] Center { get; set; } = TelerikMapDefaults.Center;
    private double[] Extent { get; set; } = TelerikMapDefaults.Extent;
    private double Zoom { get; set; } = TelerikMapDefaults.Zoom;
    
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);
        
        if (firstRender)
        {
            var location = await StorageService.GetItemAsync<double[]>(key: "location");
            
            if (location is not null && !Latitude.HasValue && !Longitude.HasValue)
            {
                Center =
                [
                    (location.ElementAt(index: 0) + location.ElementAt(index: 2)) / 2,
                    (location.ElementAt(index: 1) + location.ElementAt(index: 3)) / 2
                ];
            }
            else if (Latitude.HasValue && Longitude.HasValue)
            {
                Center =
                [
                    Latitude.Value,
                    Longitude.Value
                ];
            }
            
            Extent =
            [
                Center.ElementAt(index: 0) + 0.005,
                Center.ElementAt(index: 1) - 0.005,
                Center.ElementAt(index: 0) - 0.005,
                Center.ElementAt(index: 1) + 0.005
            ];
            
            StateHasChanged();
            
            await StorageService.SetItemAsync(
                key: "location",
                data: Extent);
        }
    }
    
    private async Task OnPanEnd(MapPanEndEventArgs args)
    {
        Center = args.Center;
        Extent = args.Extent;
        
        await StorageService.SetItemAsync(
            key: "location",
            data: Extent);
    }
    
    private async Task OnZoomEnd(MapZoomEndEventArgs args)
    {
        Center = args.Center;
        Extent = args.Extent;
        Zoom = args.Zoom;
        
        await StorageService.SetItemAsync(
            key: "location",
            data: Extent);
    }
}
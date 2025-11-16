using Geolocation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.JSInterop;
using Telerik.Blazor.Components;
using Telerik.DataSource;
using TramTimes.Web.Site.Builders;
using TramTimes.Web.Site.Defaults;
using TramTimes.Web.Site.Models;
using TramTimes.Web.Site.Types;
using TramTimes.Web.Utilities.Extensions;
using TramTimes.Web.Utilities.Models;

namespace TramTimes.Web.Site.Components.Pages;

public partial class Home : ComponentBase, IAsyncDisposable
{
    private List<TelerikStop> ListData { get; set; } = [];
    private List<TelerikStop> MapData { get; set; } = [];
    private List<TelerikStop> SearchData { get; set; } = [];
    private double[] Center { get; set; } = [];
    private IJSObjectReference? JavascriptManager { get; set; }
    private TelerikListView<TelerikStop>? ListManager { get; set; }
    private TelerikMap? MapManager { get; set; }
    private string? Query { get; set; }
    private string? Title { get; set; }
    private bool? Disposed { get; set; }
    
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        
        #region set default center
        
        if (Center.IsNullOrEmpty())
            Center = TelerikMapDefaults.Center;
        
        #endregion
        
        #region set default query
        
        Query ??= string.Empty;
        
        #endregion
        
        #region set default title
        
        Title ??= "TramTimes - South Yorkshire";
        
        #endregion
    }
    
    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();
        
        #region get storage consent
        
        var feature = AccessorService.HttpContext?.Features.Get<ITrackingConsentFeature>();
        var consent = feature?.CanTrack ?? false;
        
        #endregion
        
        #region get map location
        
        if (Latitude.HasValue && Longitude.HasValue)
        {
            Center =
            [
                Latitude.Value,
                Longitude.Value
            ];
        }
        
        if (Zoom.HasValue && Zoom > TelerikMapDefaults.MaxZoom)
        {
            Zoom = TelerikMapDefaults.MaxZoom;
        }
        else if (Zoom.HasValue && Zoom < TelerikMapDefaults.MinZoom)
        {
            Zoom = TelerikMapDefaults.MinZoom;
        }
        else if (!Zoom.HasValue)
        {
            Zoom = TelerikMapDefaults.Zoom;
        }
        
        if (consent)
        {
            var location = await StorageService.GetItemAsync<double[]>(key: "location") ?? [];
            
            if (!location.IsNullOrEmpty() && !Latitude.HasValue && !Longitude.HasValue)
            {
                Center =
                [
                    location.ElementAt(index: 0),
                    location.ElementAt(index: 1)
                ];
            }
        }
        
        #endregion
        
        #region navigate to home
        
        if (NavigationService.Uri.Equals(value: NavigationService.BaseUri))
        {
            NavigationService.NavigateTo(
                uri: $"/{Center.ElementAt(index: 1)}/{Center.ElementAt(index: 0)}/{Zoom}",
                replace: true);
            
            return;
        }
        
        #endregion
        
        #region get local time
        
        var currentDateTime = new DateTime(
            year: DateTime.Now.Year,
            month: DateTime.Now.Month,
            day: DateTime.Now.Day,
            hour: DateTime.Now.Hour,
            minute: DateTime.Now.Minute,
            second: 0);
        
        var offsetDateTime = currentDateTime
            .AddHours(value: 1)
            .AddMinutes(value: 59)
            .AddSeconds(value: 59);
        
        #endregion
        
        #region get local storage
        
        List<TelerikStop> cache = [];
        
        if (consent)
            cache = await StorageService.GetItemAsync<List<TelerikStop>>(key: "cache") ?? [];
        
        MapData = [];
        MapData.AddRange(collection: cache);
        
        foreach (var item in MapData)
        {
            item.Distance = GeoCalculator.GetDistance(
                originLatitude: item.Latitude ?? 0,
                originLongitude: item.Longitude ?? 0,
                destinationLatitude: Center.ElementAt(index: 0),
                destinationLongitude: Center.ElementAt(index: 1),
                distanceUnit: DistanceUnit.Meters);
            
            item.Points = item.Points?
                .Where(predicate: point => point.DepartureDateTime >= currentDateTime)
                .Where(predicate: point => point.DepartureDateTime <= offsetDateTime)
                .ToList();
        }
        
        MapData.RemoveAll(match: stop => stop.Points?.IsNullOrEmpty() == true);
        MapData.RemoveAll(match: stop => stop.Points?.All(predicate: point => point.DepartureDateTime < currentDateTime) == true);
        MapData.RemoveAll(match: stop => stop.Points?.All(predicate: point => point.DepartureDateTime > offsetDateTime) == true);
        
        MapData = MapData
            .OrderBy(keySelector: stop => stop.Distance)
            .ToList();
        
        #endregion
        
        #region build query data
        
        var response = await HttpService.GetAsync(requestUri: QueryBuilder.GetStopsFromSearch(
            type: QueryType.StopPoint,
            value: Center));
        
        if (!response.IsSuccessStatusCode)
            response = await HttpService.GetAsync(requestUri: QueryBuilder.GetStopsFromDatabase(
                type: QueryType.StopPoint,
                value: Center));
        
        #endregion
        
        #region build results data
        
        List<WebStop> data = [];
        
        if (response.IsSuccessStatusCode)
            data = await response.Content.ReadFromJsonAsync<List<WebStop>>() ?? [];
        
        MapData.RemoveAll(match: stop => data.Any(predicate: item => stop.Id == item.Id));
        MapData.AddRange(collection: MapperService.Map<List<TelerikStop>>(source: data));
        
        foreach (var item in MapData)
        {
            item.Distance = GeoCalculator.GetDistance(
                originLatitude: item.Latitude ?? 0,
                originLongitude: item.Longitude ?? 0,
                destinationLatitude: Center.ElementAt(index: 0),
                destinationLongitude: Center.ElementAt(index: 1),
                distanceUnit: DistanceUnit.Meters);
            
            item.Points = item.Points?
                .Where(predicate: point => point.DepartureDateTime >= currentDateTime)
                .Where(predicate: point => point.DepartureDateTime <= offsetDateTime)
                .ToList();
        }
        
        MapData.RemoveAll(match: stop => stop.Points?.IsNullOrEmpty() == true);
        MapData.RemoveAll(match: stop => stop.Points?.All(predicate: point => point.DepartureDateTime < currentDateTime) == true);
        MapData.RemoveAll(match: stop => stop.Points?.All(predicate: point => point.DepartureDateTime > offsetDateTime) == true);
        
        MapData = MapData
            .OrderBy(keySelector: stop => stop.Distance)
            .ToList();
        
        #endregion
        
        #region rebind list view
        
        ListManager?.Rebind();
        
        #endregion
        
        #region clear local storage
        
        await StorageService.ClearAsync();
        
        #endregion
        
        #region save local storage
        
        if (consent)
            await StorageService.SetItemAsync(
                key: "location",
                data: Center);
        
        if (consent)
            await StorageService.SetItemAsync(
                key: "cache",
                data: MapData.OrderBy(keySelector: stop => stop.Id));
        
        #endregion
    }
    
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender: firstRender);
        
        # region check component disposed
        
        if (Disposed.HasValue && Disposed.Value)
            return;
        
        #endregion
        
        #region create javascript manager
        
        if (firstRender)
        {
            JavascriptManager = await JavascriptService.InvokeAsync<IJSObjectReference>(
                identifier: "import",
                args: "./Components/Pages/Home.razor.js");
            
            await JavascriptManager.InvokeVoidAsync(
                identifier: "registerResize",
                args: DotNetObjectReference.Create(value: this));
            
            var feature = AccessorService.HttpContext?.Features.Get<ITrackingConsentFeature>();
            var consent = "unknown";
            
            if (feature is not null)
                consent = feature.CanTrack
                    ? "accept"
                    : "reject";
            
            await JavascriptManager.InvokeVoidAsync(
                identifier: "writeConsole",
                args: $"home: consent {consent}");
        }
        
        #endregion
    }
    
    private async Task OnListReadAsync(ListViewReadEventArgs readEventArgs)
    {
        #region get local storage
        
        ListData = [];
        ListData.AddRange(collection: MapData);
        
        #endregion
        
        #region build results data
        
        readEventArgs.Data = ListData.OrderBy(keySelector: stop => stop.Distance);
        
        #endregion
        
        # region check component disposed
        
        if (Disposed.HasValue && Disposed.Value)
            return;
        
        #endregion
        
        #region output console message
        
        if (JavascriptManager is not null)
            await JavascriptManager.InvokeVoidAsync(
                identifier: "writeConsole",
                args: $"home: list read {Center.ElementAt(index: 1)}/{Center.ElementAt(index: 0)}");
        
        #endregion
    }
    
    private async Task OnListChange(string? stopId)
    {
        #region get stop data
        
        var stop = new TelerikStop();
        
        if (MapData.Any(predicate: item => item.Id!.Equals(value: stopId)))
            stop = MapData.First(predicate: item => item.Id!.Equals(value: stopId));
        
        if (stop.Id is null)
            return;
        
        #endregion
        
        # region check component disposed
        
        if (Disposed.HasValue && Disposed.Value)
            return;
        
        #endregion
        
        #region output console message
        
        if (JavascriptManager is not null)
            await JavascriptManager.InvokeVoidAsync(
                identifier: "writeConsole",
                args: $"home: list change {stop.Id}");
        
        #endregion
        
        #region navigate to stop
        
        if (NavigationService.Uri.Contains(value: $"/stop/{stop.Id}"))
            return;
        
        NavigationService.NavigateTo(uri: stop.Longitude is not null && stop.Latitude is not null
            ? $"/stop/{stop.Id}/{stop.Longitude}/{stop.Latitude}/{TelerikMapDefaults.Zoom}"
            : $"/stop/{stop.Id}/{Center.ElementAt(index: 1)}/{Center.ElementAt(index: 0)}/{TelerikMapDefaults.Zoom}");
        
        #endregion
    }
    
    private async Task OnMapMarkerClick(MapMarkerClickEventArgs args)
    {
        #region get stop data
        
        if (args.DataItem is not TelerikStop stop)
            return;
        
        if (stop.Id is null)
            return;
        
        #endregion
        
        # region check component disposed
        
        if (Disposed.HasValue && Disposed.Value)
            return;
        
        #endregion
        
        #region output console message
        
        if (JavascriptManager is not null)
            await JavascriptManager.InvokeVoidAsync(
                identifier: "writeConsole",
                args: $"home: map click {stop.Id}");
        
        #endregion
        
        #region navigate to stop
        
        if (NavigationService.Uri.Contains(value: $"/stop/{stop.Id}"))
            return;
        
        NavigationService.NavigateTo(uri: stop.Longitude is not null && stop.Latitude is not null
            ? $"/stop/{stop.Id}/{stop.Longitude}/{stop.Latitude}/{TelerikMapDefaults.Zoom}"
            : $"/stop/{stop.Id}/{Center.ElementAt(index: 1)}/{Center.ElementAt(index: 0)}/{TelerikMapDefaults.Zoom}");
        
        #endregion
    }
    
    private async Task OnMapPanEndAsync(MapPanEndEventArgs args)
    {
        #region get map location
        
        Center = args.Center;
        
        #endregion
        
        # region check component disposed
        
        if (Disposed.HasValue && Disposed.Value)
            return;
        
        #endregion
        
        #region output console message
        
        if (JavascriptManager is not null)
            await JavascriptManager.InvokeVoidAsync(
                identifier: "writeConsole",
                args: $"home: map pan {Center.ElementAt(index: 1)}/{Center.ElementAt(index: 0)}");
        
        #endregion
        
        #region navigate to home
        
        NavigationService.NavigateTo(
            uri: $"/{Center.ElementAt(index: 1)}/{Center.ElementAt(index: 0)}/{Zoom}",
            replace: true);
        
        #endregion
    }
    
    private async Task OnMapZoomEndAsync(MapZoomEndEventArgs args)
    {
        #region get map location
        
        Center = args.Center;
        Zoom = args.Zoom;
        
        #endregion
        
        # region check component disposed
        
        if (Disposed.HasValue && Disposed.Value)
            return;
        
        #endregion
        
        #region output console message
        
        if (JavascriptManager is not null)
            await JavascriptManager.InvokeVoidAsync(
                identifier: "writeConsole",
                args: $"home: map zoom {Zoom}");
        
        #endregion
        
        #region navigate to home
        
        NavigationService.NavigateTo(
            uri: $"/{Center.ElementAt(index: 1)}/{Center.ElementAt(index: 0)}/{Zoom}",
            replace: true);
        
        #endregion
    }
    
    [JSInvokable]
    public async Task OnScreenResized()
    {
        #region refresh map view
        
        MapManager?.Refresh();
        
        #endregion
        
        # region check component disposed
        
        if (Disposed.HasValue && Disposed.Value)
            return;
        
        #endregion
        
        #region output console message
        
        if (JavascriptManager is not null)
            await JavascriptManager.InvokeVoidAsync(
                identifier: "writeConsole",
                args: "home: screen resized");
        
        #endregion
    }
    
    private async Task OnSearchChange(object? stopId)
    {
        #region get stop data
        
        var stop = new TelerikStop();
        
        if (SearchData.Any(predicate: item => item.Id!.Equals(value: stopId as string)))
            stop = SearchData.First(predicate: item => item.Id!.Equals(value: stopId as string));
        
        Query = string.Empty;
        
        if (stop.Id is null)
            return;
        
        #endregion
        
        # region check component disposed
        
        if (Disposed.HasValue && Disposed.Value)
            return;
        
        #endregion
        
        #region output console message
        
        if (JavascriptManager is not null)
            await JavascriptManager.InvokeVoidAsync(
                identifier: "writeConsole",
                args: $"home: search change {stop.Id}");
        
        #endregion
        
        #region navigate to stop
        
        if (NavigationService.Uri.Contains(value: $"/stop/{stop.Id}"))
            return;
        
        NavigationService.NavigateTo(uri: stop.Longitude is not null && stop.Latitude is not null
            ? $"/stop/{stop.Id}/{stop.Longitude}/{stop.Latitude}/{TelerikMapDefaults.Zoom}"
            : $"/stop/{stop.Id}/{Center.ElementAt(index: 1)}/{Center.ElementAt(index: 0)}/{TelerikMapDefaults.Zoom}");
        
        #endregion
    }
    
    private async Task OnSearchCloseAsync()
    {
        # region check component disposed
        
        if (Disposed.HasValue && Disposed.Value)
            return;
        
        #endregion
        
        #region output console message
        
        if (JavascriptManager is not null)
            await JavascriptManager.InvokeVoidAsync(
                identifier: "writeConsole",
                args: "home: search close");
        
        #endregion
    }
    
    private async Task OnSearchOpenAsync()
    {
        # region check component disposed
        
        if (Disposed.HasValue && Disposed.Value)
            return;
        
        #endregion
        
        #region output console message
        
        if (JavascriptManager is not null)
            await JavascriptManager.InvokeVoidAsync(
                identifier: "writeConsole",
                args: "home: search open");
        
        #endregion
    }
    
    private async Task OnSearchReadAsync(AutoCompleteReadEventArgs readEventArgs)
    {
        #region get search input
        
        IList<IFilterDescriptor>? filterDescriptors = readEventArgs.Request.Filters;
        
        if (filterDescriptors.FirstOrDefault() is not FilterDescriptor filterDescriptor)
            return;
        
        var name = filterDescriptor.Value.ToString() ?? string.Empty;
        
        #endregion
        
        #region get storage consent
        
        var feature = AccessorService.HttpContext?.Features.Get<ITrackingConsentFeature>();
        var consent = feature?.CanTrack ?? false;
        
        #endregion
        
        #region get local time
        
        var currentDateTime = new DateTime(
            year: DateTime.Now.Year,
            month: DateTime.Now.Month,
            day: DateTime.Now.Day,
            hour: DateTime.Now.Hour,
            minute: DateTime.Now.Minute,
            second: 0);
        
        var offsetDateTime = currentDateTime
            .AddHours(value: 1)
            .AddMinutes(value: 59)
            .AddSeconds(value: 59);
        
        #endregion
        
        #region get local storage
        
        List<TelerikStop> cache = [];
        
        if (consent)
            cache = await StorageService.GetItemAsync<List<TelerikStop>>(key: "cache") ?? [];
        
        SearchData = [];
        SearchData.AddRange(collection: cache);
        
        foreach (var item in SearchData)
        {
            item.Distance = GeoCalculator.GetDistance(
                originLatitude: item.Latitude ?? 0,
                originLongitude: item.Longitude ?? 0,
                destinationLatitude: Center.ElementAt(index: 0),
                destinationLongitude: Center.ElementAt(index: 1),
                distanceUnit: DistanceUnit.Meters);
            
            item.Points = item.Points?
                .Where(predicate: point => point.DepartureDateTime >= currentDateTime)
                .Where(predicate: point => point.DepartureDateTime <= offsetDateTime)
                .ToList();
        }
        
        SearchData.RemoveAll(match: stop => stop.Points?.IsNullOrEmpty() == true);
        SearchData.RemoveAll(match: stop => stop.Points?.All(predicate: point => point.DepartureDateTime < currentDateTime) == true);
        SearchData.RemoveAll(match: stop => stop.Points?.All(predicate: point => point.DepartureDateTime > offsetDateTime) == true);
        
        readEventArgs.Data = SearchData
            .OrderByDescending(keySelector: stop => stop.Name.ContainsIgnoreCase(value: name))
            .ThenByDescending(keySelector: stop => stop.Direction.ContainsIgnoreCase(value: name))
            .ThenBy(keySelector: stop => stop.Name)
            .ThenBy(keySelector: stop => stop.Id);
        
        if (name.Length < TelerikAutoCompleteDefaults.MinQueryLength)
            return;
        
        #endregion
        
        #region build query data
        
        var response = await HttpService.GetAsync(requestUri: QueryBuilder.GetStopsFromSearch(
            type: QueryType.StopName,
            value: name));
        
        if (!response.IsSuccessStatusCode)
            response = await HttpService.GetAsync(requestUri: QueryBuilder.GetStopsFromDatabase(
                type: QueryType.StopName,
                value: name));
        
        #endregion
        
        #region build results data
        
        List<WebStop> data = [];
        
        if (response.IsSuccessStatusCode)
            data = await response.Content.ReadFromJsonAsync<List<WebStop>>() ?? [];
        
        SearchData.RemoveAll(match: stop => data.Any(predicate: item => stop.Id == item.Id));
        SearchData.AddRange(collection: MapperService.Map<List<TelerikStop>>(source: data));
        
        foreach (var item in SearchData)
        {
            item.Distance = GeoCalculator.GetDistance(
                originLatitude: item.Latitude ?? 0,
                originLongitude: item.Longitude ?? 0,
                destinationLatitude: Center.ElementAt(index: 0),
                destinationLongitude: Center.ElementAt(index: 1),
                distanceUnit: DistanceUnit.Meters);
            
            item.Points = item.Points?
                .Where(predicate: point => point.DepartureDateTime >= currentDateTime)
                .Where(predicate: point => point.DepartureDateTime <= offsetDateTime)
                .ToList();
        }
        
        SearchData.RemoveAll(match: stop => stop.Points?.IsNullOrEmpty() == true);
        SearchData.RemoveAll(match: stop => stop.Points?.All(predicate: point => point.DepartureDateTime < currentDateTime) == true);
        SearchData.RemoveAll(match: stop => stop.Points?.All(predicate: point => point.DepartureDateTime > offsetDateTime) == true);
        
        readEventArgs.Data = SearchData
            .OrderByDescending(keySelector: stop => stop.Name.ContainsIgnoreCase(value: name))
            .ThenByDescending(keySelector: stop => stop.Direction.ContainsIgnoreCase(value: name))
            .ThenBy(keySelector: stop => stop.Name)
            .ThenBy(keySelector: stop => stop.Id);
        
        #endregion
        
        #region clear local storage
        
        await StorageService.ClearAsync();
        
        #endregion
        
        #region save local storage
        
        if (consent)
            await StorageService.SetItemAsync(
                key: "location",
                data: Center);
        
        if (consent)
            await StorageService.SetItemAsync(
                key: "cache",
                data: SearchData.OrderBy(keySelector: stop => stop.Id));
        
        #endregion
        
        # region check component disposed
        
        if (Disposed.HasValue && Disposed.Value)
            return;
        
        #endregion
        
        #region output console message
        
        if (JavascriptManager is not null)
            await JavascriptManager.InvokeVoidAsync(
                identifier: "writeConsole",
                args: $"home: search read {name}");
        
        #endregion
    }
    
    async ValueTask IAsyncDisposable.DisposeAsync()
    {
        # region set component disposed
        
        Disposed = true;
        
        #endregion
        
        #region suppress object finalizer
		
        GC.SuppressFinalize(obj: this);
		
        #endregion
        
        #region dispose javascript manager
        
        if (JavascriptManager is not null)
            await JavascriptManager.DisposeAsync();
        
        #endregion
    }
}
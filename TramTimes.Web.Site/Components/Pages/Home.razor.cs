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

public partial class Home : ComponentBase
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
        
        #region rebind list view
        
        ListManager?.Rebind();
        
        #endregion
        
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
        else
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
        
        #region navigate to page
        
        if (NavigationService.Uri.Equals(value: NavigationService.BaseUri))
            NavigationService.NavigateTo(
                uri: $"/{Center.ElementAt(index: 1)}/{Center.ElementAt(index: 0)}/{Zoom}",
                forceLoad: true,
                replace: true);
        
        #endregion
        
        #region get local storage
        
        List<TelerikStop> cache = [];
        
        if (consent)
            cache = await StorageService.GetItemAsync<List<TelerikStop>>(key: "cache") ?? [];
        
        MapData = [];
        
        if (!cache.IsNullOrEmpty())
            MapData.AddRange(collection: cache);
        
        foreach (var item in MapData)
            item.Distance = GeoCalculator.GetDistance(
                originLatitude: item.Latitude ?? 0,
                originLongitude: item.Longitude ?? 0,
                destinationLatitude: Center.ElementAt(index: 0),
                destinationLongitude: Center.ElementAt(index: 1),
                distanceUnit: DistanceUnit.Meters);
        
        foreach (var item in MapData)
            item.Points = item.Points?
                .Where(predicate: point => point.DepartureDateTime > DateTime.Now)
                .ToList();
        
        if (MapData.Any(predicate: stop => stop.Points.IsNullOrEmpty()))
            MapData.RemoveAll(match: stop => stop.Points.IsNullOrEmpty());
        
        MapData = MapData
            .OrderBy(keySelector: stop => stop.Distance)
            .ToList();
        
        #endregion
        
        #region build query data
        
        var query = QueryBuilder.GetStopsFromSearch(
            type: QueryType.StopPoint,
            value: Center);
        
        var response = await HttpService.GetAsync(requestUri: query);
        
        if (!response.IsSuccessStatusCode)
        {
            query = QueryBuilder.GetStopsFromDatabase(
                type: QueryType.StopPoint,
                value: Center);
            
            response = await HttpService.GetAsync(requestUri: query);
        }
        
        #endregion
        
        #region build results data
        
        List<WebStop> data = [];
        
        if (response.IsSuccessStatusCode)
            data = await response.Content.ReadFromJsonAsync<List<WebStop>>() ?? [];
        
        if (MapData.Any(predicate: stop => data.Any(predicate: item => stop.Id == item.Id)))
            MapData.RemoveAll(match: stop => data.Any(predicate: item => stop.Id == item.Id));
        
        if (!data.IsNullOrEmpty())
            MapData.AddRange(collection: MapperService.Map<List<TelerikStop>>(source: data));
        
        foreach (var item in MapData)
            item.Distance = GeoCalculator.GetDistance(
                originLatitude: item.Latitude ?? 0,
                originLongitude: item.Longitude ?? 0,
                destinationLatitude: Center.ElementAt(index: 0),
                destinationLongitude: Center.ElementAt(index: 1),
                distanceUnit: DistanceUnit.Meters);
        
        foreach (var item in MapData)
            item.Points = item.Points?
                .Where(predicate: point => point.DepartureDateTime > DateTime.Now)
                .ToList();
        
        if (MapData.Any(predicate: stop => stop.Points.IsNullOrEmpty()))
            MapData.RemoveAll(match: stop => stop.Points.IsNullOrEmpty());
        
        MapData = MapData
            .OrderBy(keySelector: stop => stop.Distance)
            .ToList();
        
        #endregion
        
        #region clear local storage
        
        await StorageService.ClearAsync();
        
        #endregion
        
        #region save local storage
        
        if (consent)
        {
            await StorageService.SetItemAsync(
                key: "location",
                data: Center);
            
            await StorageService.SetItemAsync(
                key: "cache",
                data: MapData.OrderBy(keySelector: stop => stop.Id));
        }
        
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
            try
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
            catch (ObjectDisposedException e)
            {
                LoggerService.LogInformation(
                    message: "Exception: {exception}",
                    args: e.ToString());
            }
        }
        
        #endregion
    }
    
    private async Task OnListReadAsync(ListViewReadEventArgs readEventArgs)
    {
        #region get storage consent
        
        var feature = AccessorService.HttpContext?.Features.Get<ITrackingConsentFeature>();
        var consent = feature?.CanTrack ?? false;
        
        #endregion
        
        #region get local storage
        
        List<TelerikStop> cache = [];
        
        if (consent)
            cache = await StorageService.GetItemAsync<List<TelerikStop>>(key: "cache") ?? [];
        
        ListData = [];
        
        if (!cache.IsNullOrEmpty())
            ListData.AddRange(collection: cache);
        
        foreach (var item in ListData)
            item.Distance = GeoCalculator.GetDistance(
                originLatitude: item.Latitude ?? 0,
                originLongitude: item.Longitude ?? 0,
                destinationLatitude: Center.ElementAt(index: 0),
                destinationLongitude: Center.ElementAt(index: 1),
                distanceUnit: DistanceUnit.Meters);
        
        foreach (var item in ListData)
            item.Points = item.Points?
                .Where(predicate: point => point.DepartureDateTime > DateTime.Now)
                .ToList();
        
        if (ListData.Any(predicate: stop => stop.Points.IsNullOrEmpty()))
            ListData.RemoveAll(match: stop => stop.Points.IsNullOrEmpty());
        
        readEventArgs.Data = ListData.OrderBy(keySelector: stop => stop.Distance);
        
        #endregion
        
        #region build query data
        
        var query = QueryBuilder.GetStopsFromSearch(
            type: QueryType.StopPoint,
            value: Center);
        
        var response = await HttpService.GetAsync(requestUri: query);
        
        if (!response.IsSuccessStatusCode)
        {
            query = QueryBuilder.GetStopsFromDatabase(
                type: QueryType.StopPoint,
                value: Center);
            
            response = await HttpService.GetAsync(requestUri: query);
        }
        
        #endregion
        
        #region build results data
        
        List<WebStop> data = [];
        
        if (response.IsSuccessStatusCode)
            data = await response.Content.ReadFromJsonAsync<List<WebStop>>() ?? [];
        
        if (ListData.Any(predicate: stop => data.Any(predicate: item => stop.Id == item.Id)))
            ListData.RemoveAll(match: stop => data.Any(predicate: item => stop.Id == item.Id));
        
        if (!data.IsNullOrEmpty())
            ListData.AddRange(collection: MapperService.Map<List<TelerikStop>>(source: data));
        
        foreach (var item in ListData)
            item.Distance = GeoCalculator.GetDistance(
                originLatitude: item.Latitude ?? 0,
                originLongitude: item.Longitude ?? 0,
                destinationLatitude: Center.ElementAt(index: 0),
                destinationLongitude: Center.ElementAt(index: 1),
                distanceUnit: DistanceUnit.Meters);
        
        foreach (var item in ListData)
            item.Points = item.Points?
                .Where(predicate: point => point.DepartureDateTime > DateTime.Now)
                .ToList();
        
        if (ListData.Any(predicate: stop => stop.Points.IsNullOrEmpty()))
            ListData.RemoveAll(match: stop => stop.Points.IsNullOrEmpty());
        
        readEventArgs.Data = ListData.OrderBy(keySelector: stop => stop.Distance);
        
        #endregion
        
        #region clear local storage
        
        await StorageService.ClearAsync();
        
        #endregion
        
        #region save local storage
        
        if (consent)
        {
            await StorageService.SetItemAsync(
                key: "location",
                data: Center);
            
            await StorageService.SetItemAsync(
                key: "cache",
                data: ListData.OrderBy(keySelector: stop => stop.Id));
        }
        
        #endregion
        
        # region check component disposed
        
        if (Disposed.HasValue && Disposed.Value)
            return;
        
        #endregion
        
        #region output console message
        
        try
        {
            if (JavascriptManager is not null)
                await JavascriptManager.InvokeVoidAsync(
                    identifier: "writeConsole",
                    args: $"home: list read {Center.ElementAt(index: 1)}/{Center.ElementAt(index: 0)}");
        }
        catch (ObjectDisposedException e)
        {
            LoggerService.LogInformation(
                message: "Exception: {exception}",
                args: e.ToString());
        }
        
        #endregion
    }
    
    private async Task OnListChange(string? stopId)
    {
        #region get stop data
        
        var stop = new TelerikStop();
        
        if (ListData.Any(predicate: item => item.Id!.Equals(value: stopId)))
            stop = ListData.First(predicate: item => item.Id!.Equals(value: stopId));
        
        if (stop.Id is null)
            return;
        
        #endregion
        
        # region check component disposed
        
        if (Disposed.HasValue && Disposed.Value)
            return;
        
        #endregion
        
        #region output console message
        
        try
        {
            if (JavascriptManager is not null)
                await JavascriptManager.InvokeVoidAsync(
                    identifier: "writeConsole",
                    args: $"home: list change {stop.Id}");
        }
        catch (ObjectDisposedException e)
        {
            LoggerService.LogInformation(
                message: "Exception: {exception}",
                args: e.ToString());
        }
        
        #endregion
        
        #region navigate to stop
        
        if (NavigationService.Uri.Contains(value: $"/stop/{stop.Id}"))
            return;
        
        NavigationService.NavigateTo(uri: stop.Longitude is not null && stop.Latitude is not null
            ? $"/stop/{stop.Id}/{stop.Longitude}/{stop.Latitude}/{TelerikMapDefaults.Zoom}"
            : $"/stop/{stopId}");
        
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
        
        try
        {
            if (JavascriptManager is not null)
                await JavascriptManager.InvokeVoidAsync(
                    identifier: "writeConsole",
                    args: $"home: map click {stop.Id}");
        }
        catch (ObjectDisposedException e)
        {
            LoggerService.LogInformation(
                message: "Exception: {exception}",
                args: e.ToString());
        }
        
        #endregion
        
        #region navigate to stop
        
        if (NavigationService.Uri.Contains(value: $"/stop/{stop.Id}"))
            return;
        
        NavigationService.NavigateTo(uri: stop.Longitude is not null && stop.Latitude is not null
            ? $"/stop/{stop.Id}/{stop.Longitude}/{stop.Latitude}/{TelerikMapDefaults.Zoom}"
            : $"/stop/{stop.Id}");
        
        #endregion
    }
    
    private async Task OnMapPanEndAsync(MapPanEndEventArgs args)
    {
        #region get map location
        
        Center = args.Center;
        
        #endregion
        
        #region rebind list view
        
        ListManager?.Rebind();
        
        #endregion
        
        #region get storage consent
        
        var feature = AccessorService.HttpContext?.Features.Get<ITrackingConsentFeature>();
        var consent = feature?.CanTrack ?? false;
        
        #endregion
        
        #region get local storage
        
        List<TelerikStop> cache = [];
        
        if (consent)
            cache = await StorageService.GetItemAsync<List<TelerikStop>>(key: "cache") ?? [];
        
        MapData = [];
        
        if (!cache.IsNullOrEmpty())
            MapData.AddRange(collection: cache);
        
        foreach (var item in MapData)
            item.Distance = GeoCalculator.GetDistance(
                originLatitude: item.Latitude ?? 0,
                originLongitude: item.Longitude ?? 0,
                destinationLatitude: Center.ElementAt(index: 0),
                destinationLongitude: Center.ElementAt(index: 1),
                distanceUnit: DistanceUnit.Meters);
        
        foreach (var item in MapData)
            item.Points = item.Points?
                .Where(predicate: point => point.DepartureDateTime > DateTime.Now)
                .ToList();
        
        if (MapData.Any(predicate: stop => stop.Points.IsNullOrEmpty()))
            MapData.RemoveAll(match: stop => stop.Points.IsNullOrEmpty());
        
        MapData = MapData
            .OrderBy(keySelector: stop => stop.Distance)
            .ToList();
        
        #endregion
        
        #region build query data
        
        var query = QueryBuilder.GetStopsFromSearch(
            type: QueryType.StopLocation,
            value: args.Extent);
        
        var response = await HttpService.GetAsync(requestUri: query);
        
        if (!response.IsSuccessStatusCode)
        {
            query = QueryBuilder.GetStopsFromDatabase(
                type: QueryType.StopLocation,
                value: args.Extent);
            
            response = await HttpService.GetAsync(requestUri: query);
        }
        
        #endregion
        
        #region build results data
        
        List<WebStop> data = [];
        
        if (response.IsSuccessStatusCode)
            data = await response.Content.ReadFromJsonAsync<List<WebStop>>() ?? [];
        
        if (MapData.Any(predicate: stop => data.Any(predicate: item => stop.Id == item.Id)))
            MapData.RemoveAll(match: stop => data.Any(predicate: item => stop.Id == item.Id));
        
        if (!data.IsNullOrEmpty())
            MapData.AddRange(collection: MapperService.Map<List<TelerikStop>>(source: data));
        
        foreach (var item in MapData)
            item.Distance = GeoCalculator.GetDistance(
                originLatitude: item.Latitude ?? 0,
                originLongitude: item.Longitude ?? 0,
                destinationLatitude: Center.ElementAt(index: 0),
                destinationLongitude: Center.ElementAt(index: 1),
                distanceUnit: DistanceUnit.Meters);
        
        foreach (var item in MapData)
            item.Points = item.Points?
                .Where(predicate: point => point.DepartureDateTime > DateTime.Now)
                .ToList();
        
        if (MapData.Any(predicate: stop => stop.Points.IsNullOrEmpty()))
            MapData.RemoveAll(match: stop => stop.Points.IsNullOrEmpty());
        
        MapData = MapData
            .OrderBy(keySelector: stop => stop.Distance)
            .ToList();
        
        #endregion
        
        #region clear local storage
        
        await StorageService.ClearAsync();
        
        #endregion
        
        #region save local storage
        
        if (consent)
        {
            await StorageService.SetItemAsync(
                key: "location",
                data: Center);
            
            await StorageService.SetItemAsync(
                key: "cache",
                data: MapData.OrderBy(keySelector: stop => stop.Id));
        }
        
        #endregion
        
        # region check component disposed
        
        if (Disposed.HasValue && Disposed.Value)
            return;
        
        #endregion
        
        #region output console message
        
        try
        {
            if (JavascriptManager is not null)
                await JavascriptManager.InvokeVoidAsync(
                    identifier: "writeConsole",
                    args: $"home: map pan {Center.ElementAt(index: 1)}/{Center.ElementAt(index: 0)}");
        }
        catch (ObjectDisposedException e)
        {
            LoggerService.LogInformation(
                message: "Exception: {exception}",
                args: e.ToString());
        }
        
        #endregion
    }
    
    private async Task OnMapZoomEndAsync(MapZoomEndEventArgs args)
    {
        #region get map location
        
        Center = args.Center;
        Zoom = args.Zoom;
        
        #endregion
        
        #region rebind list view
        
        ListManager?.Rebind();
        
        #endregion
        
        #region get storage consent
        
        var feature = AccessorService.HttpContext?.Features.Get<ITrackingConsentFeature>();
        var consent = feature?.CanTrack ?? false;
        
        #endregion
        
        #region get local storage
        
        List<TelerikStop> cache = [];
        
        if (consent)
            cache = await StorageService.GetItemAsync<List<TelerikStop>>(key: "cache") ?? [];
        
        MapData = [];
        
        if (!cache.IsNullOrEmpty())
            MapData.AddRange(collection: cache);
        
        foreach (var item in MapData)
            item.Distance = GeoCalculator.GetDistance(
                originLatitude: item.Latitude ?? 0,
                originLongitude: item.Longitude ?? 0,
                destinationLatitude: Center.ElementAt(index: 0),
                destinationLongitude: Center.ElementAt(index: 1),
                distanceUnit: DistanceUnit.Meters);
        
        foreach (var item in MapData)
            item.Points = item.Points?
                .Where(predicate: point => point.DepartureDateTime > DateTime.Now)
                .ToList();
        
        if (MapData.Any(predicate: stop => stop.Points.IsNullOrEmpty()))
            MapData.RemoveAll(match: stop => stop.Points.IsNullOrEmpty());
        
        MapData = MapData
            .OrderBy(keySelector: stop => stop.Distance)
            .ToList();
        
        #endregion
        
        #region build query data
        
        var query = QueryBuilder.GetStopsFromSearch(
            type: QueryType.StopLocation,
            value: args.Extent);
        
        var response = await HttpService.GetAsync(requestUri: query);
        
        if (!response.IsSuccessStatusCode)
        {
            query = QueryBuilder.GetStopsFromDatabase(
                type: QueryType.StopLocation,
                value: args.Extent);
            
            response = await HttpService.GetAsync(requestUri: query);
        }
        
        #endregion
        
        #region build results data
        
        List<WebStop> data = [];
        
        if (response.IsSuccessStatusCode)
            data = await response.Content.ReadFromJsonAsync<List<WebStop>>() ?? [];
        
        if (MapData.Any(predicate: stop => data.Any(predicate: item => stop.Id == item.Id)))
            MapData.RemoveAll(match: stop => data.Any(predicate: item => stop.Id == item.Id));
        
        if (!data.IsNullOrEmpty())
            MapData.AddRange(collection: MapperService.Map<List<TelerikStop>>(source: data));
        
        foreach (var item in MapData)
            item.Distance = GeoCalculator.GetDistance(
                originLatitude: item.Latitude ?? 0,
                originLongitude: item.Longitude ?? 0,
                destinationLatitude: Center.ElementAt(index: 0),
                destinationLongitude: Center.ElementAt(index: 1),
                distanceUnit: DistanceUnit.Meters);
        
        foreach (var item in MapData)
            item.Points = item.Points?
                .Where(predicate: point => point.DepartureDateTime > DateTime.Now)
                .ToList();
        
        if (MapData.Any(predicate: stop => stop.Points.IsNullOrEmpty()))
            MapData.RemoveAll(match: stop => stop.Points.IsNullOrEmpty());
        
        MapData = MapData
            .OrderBy(keySelector: stop => stop.Distance)
            .ToList();
        
        #endregion
        
        #region clear local storage
        
        await StorageService.ClearAsync();
        
        #endregion
        
        #region save local storage
        
        if (consent)
        {
            await StorageService.SetItemAsync(
                key: "location",
                data: Center);
            
            await StorageService.SetItemAsync(
                key: "cache",
                data: MapData.OrderBy(keySelector: stop => stop.Id));
        }
        
        #endregion
        
        # region check component disposed
        
        if (Disposed.HasValue && Disposed.Value)
            return;
        
        #endregion
        
        #region output console message
        
        try
        {
            if (JavascriptManager is not null)
                await JavascriptManager.InvokeVoidAsync(
                    identifier: "writeConsole",
                    args: $"home: map zoom {Zoom}");
        }
        catch (ObjectDisposedException e)
        {
            LoggerService.LogInformation(
                message: "Exception: {exception}",
                args: e.ToString());
        }
        
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
        
        try
        {
            if (JavascriptManager is not null)
                await JavascriptManager.InvokeVoidAsync(
                    identifier: "writeConsole",
                    args: $"home: screen resized {Center.ElementAt(index: 1)}/{Center.ElementAt(index: 0)}");
        }
        catch (ObjectDisposedException e)
        {
            LoggerService.LogInformation(
                message: "Exception: {exception}",
                args: e.ToString());
        }
        
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
        
        try
        {
            if (JavascriptManager is not null)
                await JavascriptManager.InvokeVoidAsync(
                    identifier: "writeConsole",
                    args: $"home: search change {stop.Id}");
        }
        catch (ObjectDisposedException e)
        {
            LoggerService.LogInformation(
                message: "Exception: {exception}",
                args: e.ToString());
        }
        
        #endregion
        
        #region navigate to stop
        
        if (NavigationService.Uri.Contains(value: $"/stop/{stop.Id}"))
            return;
        
        NavigationService.NavigateTo(uri: stop.Longitude is not null && stop.Latitude is not null
            ? $"/stop/{stop.Id}/{stop.Longitude}/{stop.Latitude}/{TelerikMapDefaults.Zoom}"
            : $"/stop/{stopId}");
        
        #endregion
    }
    
    private async Task OnSearchCloseAsync()
    {
        # region check component disposed
        
        if (Disposed.HasValue && Disposed.Value)
            return;
        
        #endregion
        
        #region output console message
        
        try
        {
            if (JavascriptManager is not null)
                await JavascriptManager.InvokeVoidAsync(
                    identifier: "writeConsole",
                    args: "home: search close");
        }
        catch (ObjectDisposedException e)
        {
            LoggerService.LogInformation(
                message: "Exception: {exception}",
                args: e.ToString());
        }
        
        #endregion
    }
    
    private async Task OnSearchOpenAsync()
    {
        # region check component disposed
        
        if (Disposed.HasValue && Disposed.Value)
            return;
        
        #endregion
        
        #region output console message
        
        try
        {
            if (JavascriptManager is not null)
                await JavascriptManager.InvokeVoidAsync(
                    identifier: "writeConsole",
                    args: "home: search open");
        }
        catch (ObjectDisposedException e)
        {
            LoggerService.LogInformation(
                message: "Exception: {exception}",
                args: e.ToString());
        }
        
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
        
        #region get local storage
        
        List<TelerikStop> cache = [];
        
        if (consent)
            cache = await StorageService.GetItemAsync<List<TelerikStop>>(key: "cache") ?? [];
        
        SearchData = [];
        
        if (!cache.IsNullOrEmpty())
            SearchData.AddRange(collection: cache);
        
        foreach (var item in SearchData)
            item.Points = item.Points?
                .Where(predicate: point => point.DepartureDateTime > DateTime.Now)
                .ToList();
        
        if (SearchData.Any(predicate: stop => stop.Points.IsNullOrEmpty()))
            SearchData.RemoveAll(match: stop => stop.Points.IsNullOrEmpty());
        
        readEventArgs.Data = SearchData
            .OrderByDescending(keySelector: stop => stop.Name.ContainsIgnoreCase(value: name))
            .ThenByDescending(keySelector: stop => stop.Direction.ContainsIgnoreCase(value: name))
            .ThenBy(keySelector: stop => stop.Name)
            .ThenBy(keySelector: stop => stop.Id);
        
        if (name.Length < TelerikAutoCompleteDefaults.MinQueryLength)
            return;
        
        #endregion
        
        #region build query data
        
        var query = QueryBuilder.GetStopsFromSearch(
            type: QueryType.StopName,
            value: name);
        
        var response = await HttpService.GetAsync(requestUri: query);
        
        if (!response.IsSuccessStatusCode)
        {
            query = QueryBuilder.GetStopsFromDatabase(
                type: QueryType.StopName,
                value: name);
            
            response = await HttpService.GetAsync(requestUri: query);
        }
        
        #endregion
        
        #region build results data
        
        List<WebStop> data = [];
        
        if (response.IsSuccessStatusCode)
            data = await response.Content.ReadFromJsonAsync<List<WebStop>>() ?? [];
        
        if (SearchData.Any(predicate: stop => data.Any(predicate: item => stop.Id == item.Id)))
            SearchData.RemoveAll(match: stop => data.Any(predicate: item => stop.Id == item.Id));
        
        if (!data.IsNullOrEmpty())
            SearchData.AddRange(collection: MapperService.Map<List<TelerikStop>>(source: data));
        
        foreach (var item in SearchData)
            item.Points = item.Points?
                .Where(predicate: point => point.DepartureDateTime > DateTime.Now)
                .ToList();
        
        if (SearchData.Any(predicate: stop => stop.Points.IsNullOrEmpty()))
            SearchData.RemoveAll(match: stop => stop.Points.IsNullOrEmpty());
        
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
        {
            await StorageService.SetItemAsync(
                key: "location",
                data: Center);
            
            await StorageService.SetItemAsync(
                key: "cache",
                data: SearchData.OrderBy(keySelector: stop => stop.Id));
        }
        
        #endregion
        
        # region check component disposed
        
        if (Disposed.HasValue && Disposed.Value)
            return;
        
        #endregion
        
        #region output console message
        
        try
        {
            if (JavascriptManager is not null)
                await JavascriptManager.InvokeVoidAsync(
                    identifier: "writeConsole",
                    args: $"home: search read {name}");
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
        # region set component disposed
        
        Disposed = true;
        
        #endregion
        
        #region suppress object finalizer
		
        GC.SuppressFinalize(obj: this);
		
        #endregion
        
        #region dispose javascript manager
        
        try
        {
            if (JavascriptManager is not null)
                await JavascriptManager.DisposeAsync();
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
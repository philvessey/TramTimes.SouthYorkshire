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

public partial class Stop : ComponentBase
{ 
    private List<TelerikStopPoint> ListData { get; set; } = [];
    private List<TelerikStop> MapData { get; set; } = [];
    private List<TelerikStop> SearchData { get; set; } = [];
    private TelerikStop StopData { get; set; } = new();
    private double[] Center { get; set; } = [];
    private IJSObjectReference? JavascriptManager { get; set; }
    private TelerikListView<TelerikStopPoint>? ListManager { get; set; }
    private bool? Disposed { get; set; }
    private string? Query { get; set; }
    private string? Title { get; set; }
    
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
        
        #region clear stop data
        
        StopData = new TelerikStop();
        
        #endregion
        
        #region get page title
        
        Title = $"TramTimes - South Yorkshire - {Id}";
        
        #endregion
        
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
        
        #region build query data
        
        var query = QueryBuilder.GetStopsFromSearch(
            type: QueryType.StopId,
            value: Id);
        
        var response = await HttpService.GetAsync(requestUri: query);
        
        if (!response.IsSuccessStatusCode)
        {
            query = QueryBuilder.GetStopsFromDatabase(
                type: QueryType.StopId,
                value: Id);
            
            response = await HttpService.GetAsync(requestUri: query);
        }
        
        #endregion
        
        #region build results data
        
        List<WebStop> data = [];
        
        if (response.IsSuccessStatusCode)
            data = await response.Content.ReadFromJsonAsync<List<WebStop>>() ?? [];
        
        if (!data.IsNullOrEmpty())
            StopData = MapperService.Map<TelerikStop>(source: data.FirstOrDefault());
        
        #endregion
        
        #region navigate to page
        
        if (StopData is { Latitude: not null, Longitude: not null })
        {
            if (Math.Abs(value: StopData.Latitude.Value - Latitude.GetValueOrDefault()) > 1e-6)
                NavigationService.NavigateTo(
                    uri: $"/stop/{StopData.Id}/{StopData.Longitude}/{StopData.Latitude}/{Zoom}",
                    forceLoad: true,
                    replace: true);
            
            if (Math.Abs(value: StopData.Longitude.Value - Longitude.GetValueOrDefault()) > 1e-6)
                NavigationService.NavigateTo(
                    uri: $"/stop/{StopData.Id}/{StopData.Longitude}/{StopData.Latitude}/{Zoom}",
                    forceLoad: true,
                    replace: true);
        }
        
        #endregion
        
        #region set page title
        
        Title = $"TramTimes - South Yorkshire - {StopData.Name ?? StopData.Id ?? Id}";
        
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
        
        query = QueryBuilder.GetStopsFromSearch(
            type: QueryType.StopPoint,
            value: Center);
        
        response = await HttpService.GetAsync(requestUri: query);
        
        if (!response.IsSuccessStatusCode)
        {
            query = QueryBuilder.GetStopsFromDatabase(
                type: QueryType.StopPoint,
                value: Center);
            
            response = await HttpService.GetAsync(requestUri: query);
        }
        
        #endregion
        
        #region build results data
        
        data = [];
        
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
                    args: "./Components/Pages/Stop.razor.js");
                
                var feature = AccessorService.HttpContext?.Features.Get<ITrackingConsentFeature>();
                var consent = "unknown";
                
                if (feature is not null)
                    consent = feature.CanTrack
                        ? "accept"
                        : "reject";
                
                await JavascriptManager.InvokeVoidAsync(
                    identifier: "writeConsole",
                    args: $"stop: consent {consent}");
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
        #region build query data
        
        var query = QueryBuilder.GetServicesFromCache(
            type: QueryType.StopId,
            value: StopData.Id ?? Id);
        
        var response = await HttpService.GetAsync(requestUri: query);
        
        if (!response.IsSuccessStatusCode)
        {
            query = QueryBuilder.GetServicesFromDatabase(
                type: QueryType.StopId,
                value: StopData.Id ?? Id);
            
            response = await HttpService.GetAsync(requestUri: query);
        }
        
        #endregion
        
        #region build results data
        
        List<WebStopPoint> data = [];
        
        if (response.IsSuccessStatusCode)
            data = await response.Content.ReadFromJsonAsync<List<WebStopPoint>>() ?? [];
        
        ListData = [];
        
        if (!data.IsNullOrEmpty())
            ListData = MapperService.Map<List<TelerikStopPoint>>(source: data);
        
        if (ListData.Any(predicate: point => point.DepartureDateTime < DateTime.Now))
            ListData.RemoveAll(match: point => point.DepartureDateTime < DateTime.Now);
        
        readEventArgs.Data = ListData;
        
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
                    args: $"stop: list read {StopData.Id ?? Id}");
        }
        catch (ObjectDisposedException e)
        {
            LoggerService.LogInformation(
                message: "Exception: {exception}",
                args: e.ToString());
        }
        
        #endregion
    }
    
    private void OnListChange(string id)
    {
        #region navigate to trip
        
        if (NavigationService.Uri.Contains(value: $"/trip/{id}"))
            return;
        
        if (StopData is { Latitude: not null, Longitude: not null })
            NavigationService.NavigateTo(uri: $"/trip/{id}/{StopData.Longitude}/{StopData.Latitude}/{TelerikMapDefaults.Zoom}");
        
        #endregion
    }
    
    private void OnMapMarkerClick(MapMarkerClickEventArgs args)
    {
        #region navigate to stop
        
        if (args.DataItem is not TelerikStop stop)
            return;
        
        if (NavigationService.Uri.Contains(value: $"/stop/{stop.Id}"))
            return;
        
        if (stop.Id is not null && stop.Longitude is not null && stop.Latitude is not null)
            NavigationService.NavigateTo(uri: $"/stop/{stop.Id}/{stop.Longitude}/{stop.Latitude}/{TelerikMapDefaults.Zoom}");
        
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
                    args: $"stop: map pan {Center.ElementAt(index: 1)}/{Center.ElementAt(index: 0)}");
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
                    args: $"stop: map zoom {Zoom}");
        }
        catch (ObjectDisposedException e)
        {
            LoggerService.LogInformation(
                message: "Exception: {exception}",
                args: e.ToString());
        }
        
        #endregion
    }
    
    private void OnSearchChange(object id)
    {
        #region navigate to stop
        
        var stop = new TelerikStop();
        
        if (SearchData.Any(predicate: item => item.Id!.Equals(value: id as string)))
            stop = SearchData.First(predicate: item => item.Id!.Equals(value: id as string));
        
        Query = string.Empty;
        
        if (NavigationService.Uri.Contains(value: $"/stop/{stop.Id}"))
            return;
        
        if (stop.Id is not null && stop.Longitude is not null && stop.Latitude is not null)
            NavigationService.NavigateTo(uri: $"/stop/{stop.Id}/{stop.Longitude}/{stop.Latitude}/{TelerikMapDefaults.Zoom}");
        
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
                    args: "stop: search close");
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
                    args: "stop: search open");
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
                    args: $"stop: search read {name}");
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
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
    private double[] Extent { get; set; } = [];
    private IJSObjectReference? JavascriptManager { get; set; }
    private TelerikListView<TelerikStopPoint>? ListManager { get; set; }
    private string? Query { get; set; }
    private string? Title { get; set; }
    
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        
        #region set default center
        
        if (Center.IsNullOrEmpty())
            Center = TelerikMapDefaults.Center;
        
        #endregion
        
        #region set default extent
        
        if (Extent.IsNullOrEmpty())
            Extent = TelerikMapDefaults.Extent;
        
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
            Extent =
            [
                Latitude.Value + TelerikMapDefaults.ExtentOffset,
                Longitude.Value - TelerikMapDefaults.ExtentOffset,
                Latitude.Value - TelerikMapDefaults.ExtentOffset,
                Longitude.Value + TelerikMapDefaults.ExtentOffset
            ];
            
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
                    (location.ElementAt(index: 0) + location.ElementAt(index: 2)) / 2,
                    (location.ElementAt(index: 1) + location.ElementAt(index: 3)) / 2
                ];
                
                Extent =
                [
                    location.ElementAt(index: 0),
                    location.ElementAt(index: 1),
                    location.ElementAt(index: 2),
                    location.ElementAt(index: 3)
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
        
        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadFromJsonAsync<List<WebStop>>() ?? [];
            StopData = MapperService.Map<TelerikStop>(source: data.FirstOrDefault());
        }
        
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
        {
            cache = await StorageService.GetItemAsync<List<TelerikStop>>(key: "cache") ?? [];
            
            foreach (var item in cache)
                item.Points = item.Points?
                    .Where(predicate: point => point.DepartureDateTime > DateTime.Now)
                    .ToList();
            
            if (cache.Any(predicate: stop => stop.Points.IsNullOrEmpty()))
                cache.RemoveAll(match: stop => stop.Points.IsNullOrEmpty());
        }
        
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
        
        MapData = MapData
            .OrderBy(keySelector: stop => stop.Distance)
            .ThenBy(keySelector: stop => stop.Name)
            .ToList();
        
        #endregion
        
        #region build query data
        
        query = QueryBuilder.GetStopsFromSearch(
            type: QueryType.StopLocation,
            value: Extent);
        
        response = await HttpService.GetAsync(requestUri: query);
        
        if (!response.IsSuccessStatusCode)
        {
            query = QueryBuilder.GetStopsFromDatabase(
                type: QueryType.StopLocation,
                value: Extent);
            
            response = await HttpService.GetAsync(requestUri: query);
        }
        
        #endregion
        
        #region build results data
        
        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadFromJsonAsync<List<WebStop>>() ?? [];
            
            foreach (var item in data)
            {
                if (MapData.FirstOrDefault(predicate: stop => stop.Id == item.Id) is not null)
                    MapData.Remove(item: MapData.First(predicate: stop => stop.Id == item.Id));
                
                if (!item.Points.IsNullOrEmpty())
                    MapData.Add(item: MapperService.Map<TelerikStop>(source: item));
            }
        }
        
        foreach (var item in MapData)
            item.Distance = GeoCalculator.GetDistance(
                originLatitude: item.Latitude ?? 0,
                originLongitude: item.Longitude ?? 0,
                destinationLatitude: Center.ElementAt(index: 0),
                destinationLongitude: Center.ElementAt(index: 1),
                distanceUnit: DistanceUnit.Meters);
        
        MapData = MapData
            .OrderBy(keySelector: stop => stop.Distance)
            .ThenBy(keySelector: stop => stop.Name)
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
                data: Extent);
            
            await StorageService.SetItemAsync(
                key: "cache",
                data: MapData.OrderBy(keySelector: stop => stop.Id));
        }
        
        #endregion
    }
    
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender: firstRender);
        
        #region create javascript manager
        
        if (firstRender)
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
        
        ListData = [];
        
        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadFromJsonAsync<List<WebStopPoint>>() ?? [];
            ListData = MapperService.Map<List<TelerikStopPoint>>(source: data);
        }
        
        readEventArgs.Data = ListData;
        
        #endregion
        
        #region output console message
        
        if (JavascriptManager is not null)
            await JavascriptManager.InvokeVoidAsync(
                identifier: "writeConsole",
                args: $"stop: list read {StopData.Id ?? Id}");
        
        #endregion
    }
    
    private void OnListChange(string tripId)
    {
        #region navigate to trip
        
        if (StopData is { Latitude: not null, Longitude: not null })
            NavigationService.NavigateTo(uri: $"/trip/{tripId}/{StopData.Longitude}/{StopData.Latitude}/{TelerikMapDefaults.Zoom}");
        
        #endregion
    }
    
    private void OnMapMarkerClick(MapMarkerClickEventArgs args)
    {
        #region navigate to stop
        
        if (args.DataItem is TelerikStop stop)
            NavigationService.NavigateTo(uri: $"/stop/{stop.Id}/{stop.Longitude}/{stop.Latitude}/{TelerikMapDefaults.Zoom}");
        
        #endregion
    }
    
    private async Task OnMapPanEndAsync(MapPanEndEventArgs args)
    {
        #region get map location
        
        Center = args.Center;
        Extent = args.Extent;
        
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
        {
            cache = await StorageService.GetItemAsync<List<TelerikStop>>(key: "cache") ?? [];
            
            foreach (var item in cache)
                item.Points = item.Points?
                    .Where(predicate: point => point.DepartureDateTime > DateTime.Now)
                    .ToList();
            
            if (cache.Any(predicate: stop => stop.Points.IsNullOrEmpty()))
                cache.RemoveAll(match: stop => stop.Points.IsNullOrEmpty());
        }
        
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
        
        MapData = MapData
            .OrderBy(keySelector: stop => stop.Distance)
            .ThenBy(keySelector: stop => stop.Name)
            .ToList();
        
        #endregion
        
        #region build query data
        
        var query = QueryBuilder.GetStopsFromSearch(
            type: QueryType.StopLocation,
            value: Extent);
        
        var response = await HttpService.GetAsync(requestUri: query);
        
        if (!response.IsSuccessStatusCode)
        {
            query = QueryBuilder.GetStopsFromDatabase(
                type: QueryType.StopLocation,
                value: Extent);
            
            response = await HttpService.GetAsync(requestUri: query);
        }
        
        #endregion
        
        #region build results data
        
        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadFromJsonAsync<List<WebStop>>() ?? [];
            
            foreach (var item in data)
            {
                if (MapData.FirstOrDefault(predicate: stop => stop.Id == item.Id) is not null)
                    MapData.Remove(item: MapData.First(predicate: stop => stop.Id == item.Id));
                
                if (!item.Points.IsNullOrEmpty())
                    MapData.Add(item: MapperService.Map<TelerikStop>(source: item));
            }
        }
        
        foreach (var item in MapData)
            item.Distance = GeoCalculator.GetDistance(
                originLatitude: item.Latitude ?? 0,
                originLongitude: item.Longitude ?? 0,
                destinationLatitude: Center.ElementAt(index: 0),
                destinationLongitude: Center.ElementAt(index: 1),
                distanceUnit: DistanceUnit.Meters);
        
        MapData = MapData
            .OrderBy(keySelector: stop => stop.Distance)
            .ThenBy(keySelector: stop => stop.Name)
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
                data: Extent);
            
            await StorageService.SetItemAsync(
                key: "cache",
                data: MapData.OrderBy(keySelector: stop => stop.Id));
        }
        
        #endregion
        
        #region output console message
        
        if (JavascriptManager is not null)
            await JavascriptManager.InvokeVoidAsync(
                identifier: "writeConsole",
                args: $"stop: map pan {Center.ElementAt(index: 1)}/{Center.ElementAt(index: 0)}");
        
        #endregion
    }
    
    private async Task OnMapZoomEndAsync(MapZoomEndEventArgs args)
    {
        #region get map location
        
        Center = args.Center;
        Extent = args.Extent;
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
        {
            cache = await StorageService.GetItemAsync<List<TelerikStop>>(key: "cache") ?? [];
            
            foreach (var item in cache)
                item.Points = item.Points?
                    .Where(predicate: point => point.DepartureDateTime > DateTime.Now)
                    .ToList();
            
            if (cache.Any(predicate: stop => stop.Points.IsNullOrEmpty()))
                cache.RemoveAll(match: stop => stop.Points.IsNullOrEmpty());
        }
        
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
        
        MapData = MapData
            .OrderBy(keySelector: stop => stop.Distance)
            .ThenBy(keySelector: stop => stop.Name)
            .ToList();
        
        #endregion
        
        #region build query data
        
        var query = QueryBuilder.GetStopsFromSearch(
            type: QueryType.StopLocation,
            value: Extent);
        
        var response = await HttpService.GetAsync(requestUri: query);
        
        if (!response.IsSuccessStatusCode)
        {
            query = QueryBuilder.GetStopsFromDatabase(
                type: QueryType.StopLocation,
                value: Extent);
            
            response = await HttpService.GetAsync(requestUri: query);
        }
        
        #endregion
        
        #region build results data
        
        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadFromJsonAsync<List<WebStop>>() ?? [];
            
            foreach (var item in data)
            {
                if (MapData.FirstOrDefault(predicate: stop => stop.Id == item.Id) is not null)
                    MapData.Remove(item: MapData.First(predicate: stop => stop.Id == item.Id));
                
                if (!item.Points.IsNullOrEmpty())
                    MapData.Add(item: MapperService.Map<TelerikStop>(source: item));
            }
        }
        
        foreach (var item in MapData)
            item.Distance = GeoCalculator.GetDistance(
                originLatitude: item.Latitude ?? 0,
                originLongitude: item.Longitude ?? 0,
                destinationLatitude: Center.ElementAt(index: 0),
                destinationLongitude: Center.ElementAt(index: 1),
                distanceUnit: DistanceUnit.Meters);
        
        MapData = MapData
            .OrderBy(keySelector: stop => stop.Distance)
            .ThenBy(keySelector: stop => stop.Name)
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
                data: Extent);
            
            await StorageService.SetItemAsync(
                key: "cache",
                data: MapData.OrderBy(keySelector: stop => stop.Id));
        }
        
        #endregion
        
        #region output console message
        
        if (JavascriptManager is not null)
            await JavascriptManager.InvokeVoidAsync(
                identifier: "writeConsole",
                args: $"stop: map zoom {Zoom}");
        
        #endregion
    }
    
    private void OnSearchChange(object id)
    {
        #region navigate to stop
        
        var stop = new TelerikStop();
        
        if (SearchData.Any(predicate: item => item.Id!.Equals(value: id as string)))
            stop = SearchData.First(predicate: item => item.Id!.Equals(value: id as string));
        
        Query = string.Empty;
        
        if (stop.Id is not null && stop.Longitude is not null && stop.Latitude is not null)
            NavigationService.NavigateTo(uri: $"/stop/{stop.Id}/{stop.Longitude}/{stop.Latitude}/{TelerikMapDefaults.Zoom}");
        
        #endregion
    }
    
    private async Task OnSearchCloseAsync()
    {
        #region output console message
        
        if (JavascriptManager is not null)
            await JavascriptManager.InvokeVoidAsync(
                identifier: "writeConsole",
                args: "stop: search close");
        
        #endregion
    }
    
    private async Task OnSearchOpenAsync()
    {
        #region output console message
        
        if (JavascriptManager is not null)
            await JavascriptManager.InvokeVoidAsync(
                identifier: "writeConsole",
                args: "stop: search open");
        
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
        {
            cache = await StorageService.GetItemAsync<List<TelerikStop>>(key: "cache") ?? [];
            
            foreach (var item in cache)
                item.Points = item.Points?
                    .Where(predicate: point => point.DepartureDateTime > DateTime.Now)
                    .ToList();
            
            if (cache.Any(predicate: stop => stop.Points.IsNullOrEmpty()))
                cache.RemoveAll(match: stop => stop.Points.IsNullOrEmpty());
        }
        
        SearchData = [];
        
        if (!cache.IsNullOrEmpty())
            SearchData.AddRange(collection: cache);
        
        readEventArgs.Data = SearchData
            .OrderByDescending(keySelector: stop => stop.Name.ContainsIgnoreCase(value: name))
            .ThenByDescending(keySelector: stop => stop.Direction.ContainsIgnoreCase(value: name))
            .ThenBy(keySelector: stop => stop.Name);
        
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
        
        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadFromJsonAsync<List<WebStop>>() ?? [];
            
            foreach (var item in data)
            {
                if (SearchData.FirstOrDefault(predicate: stop => stop.Id == item.Id) is not null)
                    SearchData.Remove(item: SearchData.First(predicate: stop => stop.Id == item.Id));
                
                if (!item.Points.IsNullOrEmpty())
                    SearchData.Add(item: MapperService.Map<TelerikStop>(source: item));
            }
        }
        
        readEventArgs.Data = SearchData
            .OrderByDescending(keySelector: stop => stop.Name.ContainsIgnoreCase(value: name))
            .ThenByDescending(keySelector: stop => stop.Direction.ContainsIgnoreCase(value: name))
            .ThenBy(keySelector: stop => stop.Name);
        
        #endregion
        
        #region clear local storage
        
        await StorageService.ClearAsync();
        
        #endregion
        
        #region save local storage
        
        if (consent)
        {
            await StorageService.SetItemAsync(
                key: "location",
                data: Extent);
            
            await StorageService.SetItemAsync(
                key: "cache",
                data: SearchData.OrderBy(keySelector: stop => stop.Id));
        }
        
        #endregion
        
        #region output console message
        
        if (JavascriptManager is not null)
            await JavascriptManager.InvokeVoidAsync(
                identifier: "writeConsole",
                args: $"stop: search read {name}");
        
        #endregion
    }
    
    async ValueTask IAsyncDisposable.DisposeAsync()
    {
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
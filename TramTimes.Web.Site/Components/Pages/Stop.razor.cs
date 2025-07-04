using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.JSInterop;
using Telerik.Blazor.Components;
using Telerik.DataSource;
using TramTimes.Web.Site.Builders;
using TramTimes.Web.Site.Defaults;
using TramTimes.Web.Site.Models;
using TramTimes.Web.Utilities.Extensions;
using TramTimes.Web.Utilities.Models;

namespace TramTimes.Web.Site.Components.Pages;

public partial class Stop : ComponentBase
{ 
    private List<TelerikStop> MarkerData { get; set; } = [];
    private List<TelerikStop> SearchData { get; set; } = [];
    private TelerikStop StopData { get; set; } = new();
    private double[] Center { get; set; } = [];
    private double[] Extent { get; set; } = [];
    private IJSObjectReference? Manager { get; set; }
    private string? Query { get; set; }
    
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
    }
    
    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();
        
        #region get storage consent
        
        var feature = AccessorService.HttpContext?.Features.Get<ITrackingConsentFeature>();
        var consent = feature?.CanTrack ?? false;
        
        #endregion
        
        #region get map location
        
        double[] location = [];
        
        if (consent)
            location = await StorageService.GetItemAsync<double[]>(key: "location") ?? [];
        
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
        else if (Latitude.HasValue && Longitude.HasValue)
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
        
        #endregion
        
        #region build query data
        
        var query = QueryBuilder.GetIdFromSearch(id: Id);
        var response = await HttpService.GetAsync(requestUri: query);
        
        if (!response.IsSuccessStatusCode)
        {
            query = QueryBuilder.GetIdFromDatabase(id: Id);
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
        
        #region navigate to stop
        
        if (StopData is { Latitude: not null, Longitude: not null })
        {
            if (Math.Abs(value: StopData.Latitude.Value - Latitude.GetValueOrDefault()) > 1e-6)
                NavigationService.NavigateTo(uri: $"/stop/{StopData.Id}/{StopData.Longitude}/{StopData.Latitude}/{Zoom}");
            
            if (Math.Abs(value: StopData.Longitude.Value - Longitude.GetValueOrDefault()) > 1e-6)
                NavigationService.NavigateTo(uri: $"/stop/{StopData.Id}/{StopData.Longitude}/{StopData.Latitude}/{Zoom}");
        }
        
        #endregion
        
        #region get local storage
        
        List<TelerikStop> cache = [];
        
        if (consent)
            cache = await StorageService.GetItemAsync<List<TelerikStop>>(key: "cache") ?? [];
        
        if (!cache.IsNullOrEmpty())
            foreach (var item in cache)
                item.Points = item.Points?
                    .Where(predicate: point => point.DepartureDateTime > DateTime.Now)
                    .ToList();
        
        if (cache.Any(predicate: stop => stop.Points.IsNullOrEmpty()))
            cache.RemoveAll(match: stop => stop.Points.IsNullOrEmpty());
        
        MarkerData = [];
        
        if (!cache.IsNullOrEmpty())
            MarkerData.AddRange(collection: cache);
        
        #endregion
        
        #region build query data
        
        query = QueryBuilder.GetLocationFromSearch(extent: Extent);
        response = await HttpService.GetAsync(requestUri: query);
        
        if (!response.IsSuccessStatusCode)
        {
            query = QueryBuilder.GetLocationFromDatabase(extent: Extent);
            response = await HttpService.GetAsync(requestUri: query);
        }
        
        #endregion
        
        #region build results data
        
        data = [];
        
        if (response.IsSuccessStatusCode)
            data = await response.Content.ReadFromJsonAsync<List<WebStop>>() ?? [];
        
        if (!data.IsNullOrEmpty())
        {
            foreach (var item in data)
            {
                var existing = MarkerData.FirstOrDefault(predicate: stop => stop.Id == item.Id);
                
                if (existing is not null)
                    MarkerData.Remove(item: existing);
                
                if (!item.Points.IsNullOrEmpty())
                    MarkerData.Add(item: MapperService.Map<TelerikStop>(source: item));
            }
        }
        
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
                data: MarkerData.OrderBy(keySelector: stop => stop.Id));
        }
        
        #endregion
        
        #region output console message
        
        if (Manager is not null && Longitude is not null && Latitude is not null)
            await Manager.InvokeVoidAsync(
                identifier: "writeConsole",
                args: $"stop: parameters set {Id}/{Longitude}/{Latitude}");
        
        if (Manager is not null && Longitude is null && Latitude is null)
            await Manager.InvokeVoidAsync(
                identifier: "writeConsole",
                args: $"stop: parameters set {Id}");
        
        #endregion
    }
    
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender: firstRender);
        
        #region create javascript manager
        
        if (firstRender)
        {
            Manager = await JavascriptService.InvokeAsync<IJSObjectReference>(
                identifier: "import",
                args: "./Components/Pages/Stop.razor.js");
            
            await Manager.InvokeVoidAsync(
                identifier: "writeConsole",
                args: "stop: first render");
            
            var feature = AccessorService.HttpContext?.Features.Get<ITrackingConsentFeature>();
            var consent = "unknown";
            
            if (feature is not null)
                consent = feature.CanTrack
                    ? "accept"
                    : "reject";
            
            await Manager.InvokeVoidAsync(
                identifier: "writeConsole",
                args: $"stop: consent {consent}");
        }
        
        #endregion
    }
    
    private void OnChange(object id)
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
    
    private async Task OnClose()
    {
        #region output console message
        
        if (Manager is not null)
            await Manager.InvokeVoidAsync(
                identifier: "writeConsole",
                args: "stop: search close");
        
        #endregion
    }
    
    private void OnMarkerClick(MapMarkerClickEventArgs args)
    {
        #region navigate to stop
        
        if (args.DataItem is TelerikStop stop)
            NavigationService.NavigateTo(uri: $"/stop/{stop.Id}/{stop.Longitude}/{stop.Latitude}/{TelerikMapDefaults.Zoom}");
        
        #endregion
    }
    
    private async Task OnOpen()
    {
        #region output console message
        
        if (Manager is not null)
            await Manager.InvokeVoidAsync(
                identifier: "writeConsole",
                args: "stop: search open");
        
        #endregion
    }
    
    private async Task OnPanEnd(MapPanEndEventArgs args)
    {
        #region get map location
        
        Center = args.Center;
        Extent = args.Extent;
        
        #endregion
        
        #region get storage consent
        
        var feature = AccessorService.HttpContext?.Features.Get<ITrackingConsentFeature>();
        var consent = feature?.CanTrack ?? false;
        
        #endregion
        
        #region get local storage
        
        List<TelerikStop> cache = [];
        
        if (consent)
            cache = await StorageService.GetItemAsync<List<TelerikStop>>(key: "cache") ?? [];
        
        if (!cache.IsNullOrEmpty())
            foreach (var item in cache)
                item.Points = item.Points?
                    .Where(predicate: point => point.DepartureDateTime > DateTime.Now)
                    .ToList();
        
        if (cache.Any(predicate: stop => stop.Points.IsNullOrEmpty()))
            cache.RemoveAll(match: stop => stop.Points.IsNullOrEmpty());
        
        MarkerData = [];
        
        if (!cache.IsNullOrEmpty())
            MarkerData.AddRange(collection: cache);
        
        #endregion
        
        #region build query data
        
        var query = QueryBuilder.GetLocationFromSearch(extent: Extent);
        var response = await HttpService.GetAsync(requestUri: query);
        
        if (!response.IsSuccessStatusCode)
        {
            query = QueryBuilder.GetLocationFromDatabase(extent: Extent);
            response = await HttpService.GetAsync(requestUri: query);
        }
        
        #endregion
        
        #region build results data
        
        List<WebStop> data = [];
        
        if (response.IsSuccessStatusCode)
            data = await response.Content.ReadFromJsonAsync<List<WebStop>>() ?? [];
        
        if (!data.IsNullOrEmpty())
        {
            foreach (var item in data)
            {
                var existing = MarkerData.FirstOrDefault(predicate: stop => stop.Id == item.Id);
                
                if (existing is not null)
                    MarkerData.Remove(item: existing);
                
                if (!item.Points.IsNullOrEmpty())
                    MarkerData.Add(item: MapperService.Map<TelerikStop>(source: item));
            }
        }
        
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
                data: MarkerData.OrderBy(keySelector: stop => stop.Id));
        }
        
        #endregion
        
        #region output console message
        
        if (Manager is not null)
            await Manager.InvokeVoidAsync(
                identifier: "writeConsole",
                args: $"stop: map pan {Center.ElementAt(index: 1)}/{Center.ElementAt(index: 0)}");
        
        #endregion
    }
    
    private async Task OnRead(AutoCompleteReadEventArgs readEventArgs)
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
        
        if (!cache.IsNullOrEmpty())
            foreach (var item in cache)
                item.Points = item.Points?
                    .Where(predicate: point => point.DepartureDateTime > DateTime.Now)
                    .ToList();
        
        if (cache.Any(predicate: stop => stop.Points.IsNullOrEmpty()))
            cache.RemoveAll(match: stop => stop.Points.IsNullOrEmpty());
        
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
        
        var query = QueryBuilder.GetNameFromSearch(name: name);
        var response = await HttpService.GetAsync(requestUri: query);
        
        if (!response.IsSuccessStatusCode)
        {
            query = QueryBuilder.GetNameFromDatabase(name: name);
            response = await HttpService.GetAsync(requestUri: query);
        }
        
        #endregion
        
        #region build results data
        
        List<WebStop> data = [];
        
        if (response.IsSuccessStatusCode)
            data = await response.Content.ReadFromJsonAsync<List<WebStop>>() ?? [];
        
        if (!data.IsNullOrEmpty())
        {
            foreach (var item in data)
            {
                var existing = SearchData.FirstOrDefault(predicate: stop => stop.Id == item.Id);
                
                if (existing is not null)
                    SearchData.Remove(item: existing);
                
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
        
        if (Manager is not null)
            await Manager.InvokeVoidAsync(
                identifier: "writeConsole",
                args: $"stop: search filter {name}");
        
        #endregion
    }
    
    private async Task OnZoomEnd(MapZoomEndEventArgs args)
    {
        #region get map location
        
        Center = args.Center;
        Extent = args.Extent;
        Zoom = args.Zoom;
        
        #endregion
        
        #region get storage consent
        
        var feature = AccessorService.HttpContext?.Features.Get<ITrackingConsentFeature>();
        var consent = feature?.CanTrack ?? false;
        
        #endregion
        
        #region get local storage
        
        List<TelerikStop> cache = [];
        
        if (consent)
            cache = await StorageService.GetItemAsync<List<TelerikStop>>(key: "cache") ?? [];
        
        if (!cache.IsNullOrEmpty())
            foreach (var item in cache)
                item.Points = item.Points?
                    .Where(predicate: point => point.DepartureDateTime > DateTime.Now)
                    .ToList();
        
        if (cache.Any(predicate: stop => stop.Points.IsNullOrEmpty()))
            cache.RemoveAll(match: stop => stop.Points.IsNullOrEmpty());
        
        MarkerData = [];
        
        if (!cache.IsNullOrEmpty())
            MarkerData.AddRange(collection: cache);
        
        #endregion
        
        #region build query data
        
        var query = QueryBuilder.GetLocationFromSearch(extent: Extent);
        var response = await HttpService.GetAsync(requestUri: query);
        
        if (!response.IsSuccessStatusCode)
        {
            query = QueryBuilder.GetLocationFromDatabase(extent: Extent);
            response = await HttpService.GetAsync(requestUri: query);
        }
        
        #endregion
        
        #region build results data
        
        List<WebStop> data = [];
        
        if (response.IsSuccessStatusCode)
            data = await response.Content.ReadFromJsonAsync<List<WebStop>>() ?? [];
        
        if (!data.IsNullOrEmpty())
        {
            foreach (var item in data)
            {
                var existing = MarkerData.FirstOrDefault(predicate: stop => stop.Id == item.Id);
                
                if (existing is not null)
                    MarkerData.Remove(item: existing);
                
                if (!item.Points.IsNullOrEmpty())
                    MarkerData.Add(item: MapperService.Map<TelerikStop>(source: item));
            }
        }
        
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
                data: MarkerData.OrderBy(keySelector: stop => stop.Id));
        }
        
        #endregion
        
        #region output console message
        
        if (Manager is not null)
            await Manager.InvokeVoidAsync(
                identifier: "writeConsole",
                args: $"stop: map zoom {Zoom}");
        
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
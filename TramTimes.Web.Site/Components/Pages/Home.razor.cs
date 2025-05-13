using Telerik.Blazor.Components;
using Telerik.DataSource;
using TramTimes.Web.Site.Builders;
using TramTimes.Web.Site.Defaults;
using TramTimes.Web.Site.Models;
using TramTimes.Web.Utilities.Extensions;
using TramTimes.Web.Utilities.Models;

namespace TramTimes.Web.Site.Components.Pages;

public partial class Home
{
    private double[] Center { get; set; } = TelerikMapDefaults.Center;
    private double[] Extent { get; set; } = TelerikMapDefaults.Extent;
    private double Zoom { get; set; } = TelerikMapDefaults.Zoom;
    
    private List<TelerikStop> MarkerData { get; set; } = [];
    private List<TelerikStop> SearchData { get; set; } = [];
    
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender: firstRender);
        
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
            
            StateHasChanged();
            
            var cache = await StorageService.GetItemAsync<List<TelerikStop>>(key: "cache");
            
            if (!cache.IsNullOrEmpty())
            {
                foreach (var item in cache!)
                    item.Points = item.Points?.Where(predicate: point => point.DepartureDateTime > DateTime.Now).ToList();
            }
            
            if (cache?.Any(predicate: stop => stop.Points.IsNullOrEmpty()) == true)
                cache.RemoveAll(match: stop => stop.Points.IsNullOrEmpty());
            
            MarkerData = [];
            
            if (!cache.IsNullOrEmpty())
                MarkerData.AddRange(collection: cache!);
            
            var query = QueryBuilder.GetLocationFromSearch(extent: Extent);
            var response = await HttpService.GetAsync(requestUri: query);
            
            if (!response.IsSuccessStatusCode)
            {
                query = QueryBuilder.GetLocationFromDatabase(extent: Extent);
                response = await HttpService.GetAsync(requestUri: query);
            }
            
            List<WebStop>? data = [];
            
            if (response.IsSuccessStatusCode)
                data = await response.Content.ReadFromJsonAsync<List<WebStop>>();
            
            if (!data.IsNullOrEmpty())
            {
                foreach (var item in data!)
                {
                    var existing = MarkerData.FirstOrDefault(predicate: stop => stop.Id == item.Id);
                    
                    if (existing is not null)
                        MarkerData.Remove(item: existing);
                    
                    if (!item.Points.IsNullOrEmpty())
                        MarkerData.Add(item: MapperService.Map<TelerikStop>(source: item));
                }
            }
            
            StateHasChanged();
            
            await StorageService.SetItemAsync(
                key: "location",
                data: Extent);
            
            await StorageService.SetItemAsync(
                key: "cache",
                data: MarkerData.OrderBy(keySelector: stop => stop.Id));
        }
    }
    
    private void OnChange(object id)
    {
        if (SearchData.Any(predicate: stop => stop.Id!.Equals(id as string)))
            NavigationService.NavigateTo(uri: $"/stop/{id as string}");
    }
    
    private void OnMarkerClick(MapMarkerClickEventArgs args)
    {
        if (args.DataItem is TelerikStop stop)
            NavigationService.NavigateTo(uri: $"/stop/{stop.Id}");
    }
    
    private async Task OnPanEnd(MapPanEndEventArgs args)
    {
        Center = args.Center;
        Extent = args.Extent;
        
        var cache = await StorageService.GetItemAsync<List<TelerikStop>>(key: "cache");
        
        if (!cache.IsNullOrEmpty())
        {
            foreach (var item in cache!)
                item.Points = item.Points?.Where(predicate: point => point.DepartureDateTime > DateTime.Now).ToList();
        }
        
        if (cache?.Any(predicate: stop => stop.Points.IsNullOrEmpty()) == true)
            cache.RemoveAll(match: stop => stop.Points.IsNullOrEmpty());
        
        MarkerData = [];
        
        if (!cache.IsNullOrEmpty())
            MarkerData.AddRange(collection: cache!);
        
        var query = QueryBuilder.GetLocationFromSearch(extent: Extent);
        var response = await HttpService.GetAsync(requestUri: query);
        
        if (!response.IsSuccessStatusCode)
        {
            query = QueryBuilder.GetLocationFromDatabase(extent: Extent);
            response = await HttpService.GetAsync(requestUri: query);
        }
        
        List<WebStop>? data = [];
        
        if (response.IsSuccessStatusCode)
            data = await response.Content.ReadFromJsonAsync<List<WebStop>>();
        
        if (!data.IsNullOrEmpty())
        {
            foreach (var item in data!)
            {
                var existing = MarkerData.FirstOrDefault(predicate: stop => stop.Id == item.Id);
                
                if (existing is not null)
                    MarkerData.Remove(item: existing);
                
                if (!item.Points.IsNullOrEmpty())
                    MarkerData.Add(item: MapperService.Map<TelerikStop>(source: item));
            }
        }
        
        StateHasChanged();
        
        await StorageService.SetItemAsync(
            key: "location",
            data: Extent);
        
        await StorageService.SetItemAsync(
            key: "cache",
            data: MarkerData.OrderBy(keySelector: stop => stop.Id));
    }
    
    private async Task OnRead(AutoCompleteReadEventArgs readEventArgs)
    {
        IList<IFilterDescriptor>? filterDescriptors = readEventArgs.Request.Filters;
        
        if (filterDescriptors.FirstOrDefault() is not FilterDescriptor filterDescriptor)
            return;
        
        var name = filterDescriptor.Value.ToString() ?? string.Empty;
        var cache = await StorageService.GetItemAsync<List<TelerikStop>>(key: "cache");
        
        if (!cache.IsNullOrEmpty())
        {
            foreach (var item in cache!)
                item.Points = item.Points?.Where(predicate: point => point.DepartureDateTime > DateTime.Now).ToList();
        }
        
        if (cache?.Any(predicate: stop => stop.Points.IsNullOrEmpty()) == true)
            cache.RemoveAll(match: stop => stop.Points.IsNullOrEmpty());
        
        SearchData = [];
        
        if (!cache.IsNullOrEmpty())
            SearchData.AddRange(collection: cache!);
        
        readEventArgs.Data = SearchData
            .OrderByDescending(keySelector: stop => stop.Name.ContainsIgnoreCase(value: name))
            .ThenByDescending(keySelector: stop => stop.Direction.ContainsIgnoreCase(value: name))
            .ThenBy(keySelector: stop => stop.Name);
        
        if (name.Length < TelerikAutoCompleteDefaults.MinQueryLength)
            return;
        
        var query = QueryBuilder.GetNameFromSearch(name: name);
        var response = await HttpService.GetAsync(requestUri: query);
        
        if (!response.IsSuccessStatusCode)
        {
            query = QueryBuilder.GetNameFromDatabase(name: name);
            response = await HttpService.GetAsync(requestUri: query);
        }
        
        List<WebStop>? data = [];
        
        if (response.IsSuccessStatusCode)
            data = await response.Content.ReadFromJsonAsync<List<WebStop>>();
        
        if (!data.IsNullOrEmpty())
        {
            foreach (var item in data!)
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
        
        await StorageService.SetItemAsync(
            key: "location",
            data: Extent);
        
        await StorageService.SetItemAsync(
            key: "cache",
            data: SearchData.OrderBy(keySelector: stop => stop.Id));
    }
    
    private async Task OnZoomEnd(MapZoomEndEventArgs args)
    {
        Center = args.Center;
        Extent = args.Extent;
        Zoom = args.Zoom;
        
        var cache = await StorageService.GetItemAsync<List<TelerikStop>>(key: "cache");
        
        if (!cache.IsNullOrEmpty())
        {
            foreach (var item in cache!)
                item.Points = item.Points?.Where(predicate: point => point.DepartureDateTime > DateTime.Now).ToList();
        }
        
        if (cache?.Any(predicate: stop => stop.Points.IsNullOrEmpty()) == true)
            cache.RemoveAll(match: stop => stop.Points.IsNullOrEmpty());
        
        MarkerData = [];
        
        if (!cache.IsNullOrEmpty())
            MarkerData.AddRange(collection: cache!);
        
        var query = QueryBuilder.GetLocationFromSearch(extent: Extent);
        var response = await HttpService.GetAsync(requestUri: query);
        
        if (!response.IsSuccessStatusCode)
        {
            query = QueryBuilder.GetLocationFromDatabase(extent: Extent);
            response = await HttpService.GetAsync(requestUri: query);
        }
        
        List<WebStop>? data = [];
        
        if (response.IsSuccessStatusCode)
            data = await response.Content.ReadFromJsonAsync<List<WebStop>>();
        
        if (!data.IsNullOrEmpty())
        {
            foreach (var item in data!)
            {
                var existing = MarkerData.FirstOrDefault(predicate: stop => stop.Id == item.Id);
                
                if (existing is not null)
                    MarkerData.Remove(item: existing);
                
                if (!item.Points.IsNullOrEmpty())
                    MarkerData.Add(item: MapperService.Map<TelerikStop>(source: item));
            }
        }
        
        StateHasChanged();
        
        await StorageService.SetItemAsync(
            key: "location",
            data: Extent);
        
        await StorageService.SetItemAsync(
            key: "cache",
            data: MarkerData.OrderBy(keySelector: stop => stop.Id));
    }
}
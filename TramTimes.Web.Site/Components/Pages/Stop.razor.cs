using Geolocation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.JSInterop;
using Polly.Timeout;
using Telerik.Blazor.Components;
using Telerik.DataSource;
using TramTimes.Web.Site.Builders;
using TramTimes.Web.Site.Defaults;
using TramTimes.Web.Site.Models;
using TramTimes.Web.Site.Types;
using TramTimes.Web.Utilities.Extensions;
using TramTimes.Web.Utilities.Models;

namespace TramTimes.Web.Site.Components.Pages;

public partial class Stop : ComponentBase, IAsyncDisposable
{
    private List<TelerikStopPoint> ListData { get; set; } = [];
    private List<TelerikStop> MapData { get; set; } = [];
    private List<TelerikStop> SearchData { get; set; } = [];
    private TelerikStop LastStop { get; set; } = new();
    private TelerikStop NextStop { get; set; } = new();
    private double[] Center { get; set; } = [];
    private IJSObjectReference? JavascriptManager { get; set; }
    private ElementReference? ListElement { get; set; }
    private TelerikListView<TelerikStopPoint>? ListManager { get; set; }
    private TelerikMap? MapManager { get; set; }
    private bool? Disposed { get; set; }
    private string? Query { get; set; }
    private string? Title { get; set; }
    private bool? Hidden { get; set; }

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

        #region set default hidden

        Hidden ??= false;

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

        double[] location = [];

        if (consent)
        {
            var storage = await StorageService.GetAsync<double[]>(key: "location");

            if (storage.Success)
            {
                location = storage.Value ?? [];
            }
        }

        if (!location.IsNullOrEmpty() && !Latitude.HasValue && !Longitude.HasValue)
        {
            Center =
            [
                location.ElementAt(index: 0),
                location.ElementAt(index: 1)
            ];
        }

        #endregion

        # region get local storage

        List<TelerikStop> cache = [];

        if (consent)
        {
            var storage = await StorageService.GetAsync<List<TelerikStop>>(key: "cache");

            if (storage.Success)
            {
                cache = storage.Value ?? [];
            }
        }

        NextStop = cache.FirstOrDefault(predicate: stop => stop.Id == StopId) ?? new TelerikStop { Id = StopId };

        #endregion

        #region navigate to stop

        if (NavigationService.Uri.Equals(value: NavigationService.BaseUri + $"stop/{StopId}"))
        {
            if (NextStop is { Id: not null, Latitude: not null, Longitude: not null })
            {
                NavigationService.NavigateTo(
                    uri: $"/stop/{NextStop.Id}/{NextStop.Longitude}/{NextStop.Latitude}/{Zoom}",
                    replace: true);

                return;
            }
        }

        #endregion

        #region set page title

        if (NextStop.Name is not null)
            Title = $"TramTimes - South Yorkshire - Services from {NextStop.Name}";

        #endregion

        #region clear map data

        MapData = [];

        #endregion

        #region rebind list view

        if (LastStop is { Id: not null } && NextStop is { Id: not null })
            if (LastStop.Id != NextStop.Id)
                ListManager?.Rebind();

        LastStop = NextStop;

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

        HttpResponseMessage? response = null;

        try
        {
            var client = HttpService.CreateClient(name: "search");

            response = await client.GetAsync(requestUri: QueryBuilder.GetStopsFromSearch(
                type: QueryType.StopPoint,
                value: Center));
        }
        catch (TimeoutRejectedException)
        {
            if (JavascriptManager is not null)
                await JavascriptManager.InvokeVoidAsync(
                    identifier: "writeConsole",
                    args: "stop: api timeout");
        }

        if (response?.IsSuccessStatusCode is not true)
        {
            var client = HttpService.CreateClient(name: "database");

            response = await client.GetAsync(requestUri: QueryBuilder.GetStopsFromDatabase(
                type: QueryType.StopPoint,
                value: Center));
        }

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

        #region focus list view

        if (JavascriptManager is not null)
            await JavascriptManager.InvokeVoidAsync(
                identifier: "focusElement",
                args: ListElement);

        #endregion

        #region clear local storage

        await StorageService.DeleteAsync(key: "cache");
        await StorageService.DeleteAsync(key: "location");

        #endregion

        #region save local storage

        if (consent)
            await StorageService.SetAsync(
                key: "location",
                value: Center);

        if (consent)
            await StorageService.SetAsync(
                key: "cache",
                value: MapperService
                    .Map<List<TelerikStop>>(source: data)
                    .Concat(second: cache)
                    .DistinctBy(keySelector: stop => stop.Id)
                    .OrderBy(keySelector: stop => stop.Id));

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
                args: "./Components/Pages/Stop.razor.js");

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
                args: $"stop: consent {consent}");
        }

        #endregion
    }

    private async Task OnListReadAsync(ListViewReadEventArgs readEventArgs)
    {
        #region set hidden toggle

        Hidden = false;

        #endregion

        #region clear list data

        ListData = [];

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

        #region build query data

        HttpResponseMessage? response = null;

        try
        {
            var client = HttpService.CreateClient(name: "cache");

            response = await client.GetAsync(requestUri: QueryBuilder.GetServicesFromCache(
                type: QueryType.StopId,
                value: NextStop.Id ?? StopId));
        }
        catch (TimeoutRejectedException)
        {
            if (JavascriptManager is not null)
                await JavascriptManager.InvokeVoidAsync(
                    identifier: "writeConsole",
                    args: "stop: api timeout");
        }

        if (response?.IsSuccessStatusCode is not true)
        {
            var client = HttpService.CreateClient(name: "database");

            response = await client.GetAsync(requestUri: QueryBuilder.GetServicesFromDatabase(
                type: QueryType.StopId,
                value: NextStop.Id ?? StopId));
        }

        #endregion

        #region set hidden toggle

        Hidden = true;

        #endregion

        #region build results data

        List<WebStopPoint> data = [];

        if (response.IsSuccessStatusCode)
            data = await response.Content.ReadFromJsonAsync<List<WebStopPoint>>() ?? [];

        ListData = MapperService.Map<List<TelerikStopPoint>>(source: data);

        ListData.RemoveAll(match: point => point.DepartureDateTime < currentDateTime);
        ListData.RemoveAll(match: point => point.DepartureDateTime > offsetDateTime);

        readEventArgs.Data = ListData;

        if (ListData.IsNullOrEmpty())
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
                args: $"stop: list read {NextStop.Id ?? StopId}");

        #endregion
    }

    private async Task OnListChangeAsync(
        string? tripId,
        string? stopId) {

        # region check component disposed

        if (Disposed.HasValue && Disposed.Value)
            return;

        #endregion

        #region output console message

        if (JavascriptManager is not null)
            await JavascriptManager.InvokeVoidAsync(
                identifier: "writeConsole",
                args: $"stop: list change {tripId}/{stopId}");

        #endregion

        #region navigate to trip

        if (NavigationService.Uri.Contains(value: $"/trip/{tripId}/{stopId}"))
            return;

        NavigationService.NavigateTo(uri: NextStop is { Latitude: not null, Longitude: not null }
            ? $"/trip/{tripId}/{stopId}/{NextStop.Longitude}/{NextStop.Latitude}/{TelerikMapDefaults.Zoom}"
            : $"/trip/{tripId}/{stopId}/{Center.ElementAt(index: 1)}/{Center.ElementAt(index: 0)}/{TelerikMapDefaults.Zoom}");

        #endregion
    }

    private async Task OnMapMarkerClickAsync(MapMarkerClickEventArgs args)
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
                args: $"stop: map click {stop.Id}");

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
                args: $"stop: map pan {Center.ElementAt(index: 1)}/{Center.ElementAt(index: 0)}");

        #endregion

        #region navigate to home

        NavigationService.NavigateTo(uri: $"/{Center.ElementAt(index: 1)}/{Center.ElementAt(index: 0)}/{Zoom}");

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
                args: $"stop: map zoom {Zoom}");

        #endregion

        #region navigate to home

        NavigationService.NavigateTo(uri: $"/{Center.ElementAt(index: 1)}/{Center.ElementAt(index: 0)}/{Zoom}");

        #endregion
    }

    [JSInvokable]
    public async Task OnScreenResizedAsync()
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
                args: "stop: screen resized");

        #endregion
    }

    private async Task OnSearchBlurAsync()
    {
        # region check component disposed

        if (Disposed.HasValue && Disposed.Value)
            return;

        #endregion

        #region output console message

        if (JavascriptManager is not null)
            await JavascriptManager.InvokeVoidAsync(
                identifier: "writeConsole",
                args: "stop: search blur");

        #endregion
    }

    private async Task OnSearchChangeAsync(object? stopId)
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
                args: $"stop: search change {stop.Id}");

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
                args: "stop: search close");

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

        #region clear search data

        SearchData = [];

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
        {
            var storage = await StorageService.GetAsync<List<TelerikStop>>(key: "cache");

            if (storage.Success)
            {
                cache = storage.Value ?? [];
            }
        }

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

        HttpResponseMessage? response = null;

        try
        {
            var client = HttpService.CreateClient(name: "search");

            response = await client.GetAsync(requestUri: QueryBuilder.GetStopsFromSearch(
                type: QueryType.StopName,
                value: name));
        }
        catch (TimeoutRejectedException)
        {
            if (JavascriptManager is not null)
                await JavascriptManager.InvokeVoidAsync(
                    identifier: "writeConsole",
                    args: "stop: api timeout");
        }

        if (response?.IsSuccessStatusCode is not true)
        {
            var client = HttpService.CreateClient(name: "database");

            response = await client.GetAsync(requestUri: QueryBuilder.GetStopsFromDatabase(
                type: QueryType.StopName,
                value: name));
        }

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

        await StorageService.DeleteAsync(key: "cache");
        await StorageService.DeleteAsync(key: "location");

        #endregion

        #region save local storage

        if (consent)
            await StorageService.SetAsync(
                key: "location",
                value: Center);

        if (consent)
            await StorageService.SetAsync(
                key: "cache",
                value: MapperService
                    .Map<List<TelerikStop>>(source: data)
                    .Concat(second: cache)
                    .DistinctBy(keySelector: stop => stop.Id)
                    .OrderBy(keySelector: stop => stop.Id));

        #endregion

        # region check component disposed

        if (Disposed.HasValue && Disposed.Value)
            return;

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
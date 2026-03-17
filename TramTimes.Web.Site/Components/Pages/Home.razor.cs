using Geolocation;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Polly.Timeout;
using Telerik.Blazor.Components;
using Telerik.DataSource;
using TramTimes.Web.Site.Builders;
using TramTimes.Web.Site.Defaults;
using TramTimes.Web.Site.Extensions;
using TramTimes.Web.Site.Models;
using TramTimes.Web.Site.Types;
using TramTimes.Web.Utilities.Extensions;
using TramTimes.Web.Utilities.Models;

namespace TramTimes.Web.Site.Components.Pages;

public partial class Home : ComponentBase, IAsyncDisposable
{
    private List<TelerikStop> ListData { get; set; } = [];
    private double[] MapCenter { get; set; } = TelerikMapDefaults.Center;
    private List<TelerikStop> MapData { get; set; } = [];
    private CancellationTokenSource? MapSource { get; set; }
    private List<TelerikStop> SearchData { get; set; } = [];
    private CancellationTokenSource? SearchSource { get; set; }
    private IJSObjectReference? JavascriptManager { get; set; }
    private TelerikListView<TelerikStop>? ListManager { get; set; }
    private TelerikMap? MapManager { get; set; }
    private bool? Disposed { get; set; }
    private string? Query { get; set; }
    private string? Title { get; set; }
    private bool? Loading { get; set; }

    private Func<Task>? _onConsentChanged;
    private Action? _onConsentChangedWrapper;
    private Func<Task>? _onLayoutChanged;
    private Action? _onLayoutChangedWrapper;
    private Func<Task>? _onSchemeChanged;
    private Action? _onSchemeChangedWrapper;

    protected override void OnInitialized()
    {
        #region set default query

        Query ??= string.Empty;

        #endregion

        #region set default title

        Title ??= "TramTimes - South Yorkshire";

        #endregion

        #region set default loading

        Loading ??= true;

        #endregion

        #region register event listeners

        _onConsentChanged = OnConsentChangedAsync;
        _onConsentChangedWrapper = () => _onConsentChanged.Invoke();

        ConsentService.OnConsentChanged += StateHasChanged;
        ConsentService.OnConsentChanged += _onConsentChangedWrapper;

        _onLayoutChanged = OnLayoutChangedAsync;
        _onLayoutChangedWrapper = () => _onLayoutChanged.Invoke();

        LayoutService.OnLayoutChanged += StateHasChanged;
        LayoutService.OnLayoutChanged += _onLayoutChangedWrapper;

        _onSchemeChanged = OnSchemeChangedAsync;
        _onSchemeChangedWrapper = () => _onSchemeChanged.Invoke();

        SchemeService.OnSchemeChanged += StateHasChanged;
        SchemeService.OnSchemeChanged += _onSchemeChangedWrapper;

        #endregion
    }

    protected override async Task OnParametersSetAsync()
    {
        #region check component disposed

        if (Disposed.HasValue && Disposed.Value)
            return;

        #endregion

        #region get local location

        if (Latitude.HasValue && Longitude.HasValue)
        {
            MapCenter =
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

        try
        {
            var storage = await StorageService.GetAsync<double[]>(key: "location");

            if (storage is { Success: true, Value.Length: > 0 })
                location = storage.Value;
        }
        catch (Exception)
        {
            location = [];
        }

        if (!location.IsNullOrEmpty() && !Latitude.HasValue && !Longitude.HasValue)
        {
            MapCenter =
            [
                location.ElementAt(index: 0),
                location.ElementAt(index: 1)
            ];
        }

        #endregion

        #region get local cache

        List<TelerikStop> cache = [];

        try
        {
            var storage = await StorageService.GetAsync<List<TelerikStop>>(key: "cache");

            if (storage is { Success: true, Value.Count: > 0 })
                cache = storage.Value.All(predicate: stop => stop.HasAllProperties()) ? storage.Value : [];
        }
        catch (Exception)
        {
            cache = [];
        }

        #endregion

        #region navigate to home

        if (NavigationService.Uri.EqualsIgnoreCase(value: NavigationService.BaseUri))
        {
            NavigationService.NavigateTo(
                uri: $"/{MapCenter.ElementAt(index: 1)}/{MapCenter.ElementAt(index: 0)}/{Zoom}",
                replace: true);

            return;
        }

        #endregion

        #region set loading toggle

        Loading = true;

        #endregion

        #region clear map data

        MapData = [];

        #endregion

        #region rebind list view

        ListManager?.Rebind();

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

        #region build local data

        MapData.AddRange(collection: cache.Select(selector: stop => new TelerikStop
        {
            Id = stop.Id,
            Code = stop.Code,
            Name = stop.Name,
            Latitude = stop.Latitude,
            Longitude = stop.Longitude,
            Platform =  stop.Platform,
            Direction = stop.Direction,
            Location =  stop.Location?.ToArray(),
            Points = stop.Points?.ToList()
        }));

        foreach (var item in MapData)
        {
            item.Distance = GeoCalculator.GetDistance(
                originLatitude: item.Latitude ?? 0,
                originLongitude: item.Longitude ?? 0,
                destinationLatitude: MapCenter.ElementAt(index: 0),
                destinationLongitude: MapCenter.ElementAt(index: 1),
                distanceUnit: DistanceUnit.Meters);

            item.Points = item.Points?
                .Where(predicate: point => point.DepartureDateTime >= currentDateTime)
                .Where(predicate: point => point.DepartureDateTime <= offsetDateTime)
                .ToList();
        }

        MapData.RemoveAll(match: stop => stop.Points?.IsNullOrEmpty() is true);
        MapData.RemoveAll(match: stop => stop.Points?.All(predicate: point => point.DepartureDateTime < currentDateTime) is true);
        MapData.RemoveAll(match: stop => stop.Points?.All(predicate: point => point.DepartureDateTime > offsetDateTime) is true);

        MapData = MapData
            .OrderBy(keySelector: stop => stop.Distance)
            .ToList();

        #endregion

        #region build query data

        HttpResponseMessage? response = null;

        if (MapSource is not null)
            await MapSource.CancelAsync();

        MapSource = new CancellationTokenSource();

        try
        {
            var client = HttpService.CreateClient(name: "search");

            response = await client.GetAsync(
                requestUri: QueryBuilder.GetStopsFromSearch(
                    type: QueryType.StopPoint,
                    value: MapCenter),
                cancellationToken: MapSource.Token);
        }
        catch (OperationCanceledException)
        {
            if (JavascriptManager is not null)
                await JavascriptManager.InvokeVoidAsync(
                    identifier: "writeConsole",
                    args: "home: search cancel");
        }
        catch (TimeoutRejectedException)
        {
            if (JavascriptManager is not null)
                await JavascriptManager.InvokeVoidAsync(
                    identifier: "writeConsole",
                    args: "home: search timeout");
        }

        if (response?.IsSuccessStatusCode is not true)
        {
            try
            {
                var client = HttpService.CreateClient(name: "database");

                response = await client.GetAsync(
                    requestUri: QueryBuilder.GetStopsFromDatabase(
                        type: QueryType.StopPoint,
                        value: MapCenter),
                    cancellationToken: MapSource.Token);
            }
            catch (OperationCanceledException)
            {
                if (JavascriptManager is not null)
                    await JavascriptManager.InvokeVoidAsync(
                        identifier: "writeConsole",
                        args: "home: database cancel");
            }
            catch (TimeoutRejectedException)
            {
                if (JavascriptManager is not null)
                    await JavascriptManager.InvokeVoidAsync(
                        identifier: "writeConsole",
                        args: "home: database timeout");
            }
        }

        #endregion

        #region set loading toggle

        Loading = false;

        #endregion

        #region build remote data

        List<WebStop> data = [];

        if (response?.IsSuccessStatusCode is true)
            data = await response.Content.ReadFromJsonAsync<List<WebStop>>() ?? [];

        var results = MapperService.Map<List<TelerikStop>>(source: data);

        foreach (var item in results)
        {
            var stop = MapData.FirstOrDefault(stop => stop.Id == item.Id);
            stop?.Code = item.Code;
            stop?.Name = item.Name;
            stop?.Latitude = item.Latitude;
            stop?.Longitude = item.Longitude;
            stop?.Platform = item.Platform;
            stop?.Direction = item.Direction;
            stop?.Location = item.Location?.ToArray();
            stop?.Points = item.Points?.ToList();

            if (stop is not null)
                continue;

            MapData.Add(item: new TelerikStop
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
                Latitude = item.Latitude,
                Longitude = item.Longitude,
                Platform =  item.Platform,
                Direction = item.Direction,
                Location =  item.Location?.ToArray(),
                Points = item.Points?.ToList()
            });
        }

        foreach (var item in MapData)
        {
            item.Distance = GeoCalculator.GetDistance(
                originLatitude: item.Latitude ?? 0,
                originLongitude: item.Longitude ?? 0,
                destinationLatitude: MapCenter.ElementAt(index: 0),
                destinationLongitude: MapCenter.ElementAt(index: 1),
                distanceUnit: DistanceUnit.Meters);

            item.Points = item.Points?
                .Where(predicate: point => point.DepartureDateTime >= currentDateTime)
                .Where(predicate: point => point.DepartureDateTime <= offsetDateTime)
                .ToList();
        }

        MapData.RemoveAll(match: stop => stop.Points?.IsNullOrEmpty() is true);
        MapData.RemoveAll(match: stop => stop.Points?.All(predicate: point => point.DepartureDateTime < currentDateTime) is true);
        MapData.RemoveAll(match: stop => stop.Points?.All(predicate: point => point.DepartureDateTime > offsetDateTime) is true);

        MapData = MapData
            .OrderBy(keySelector: stop => stop.Distance)
            .ToList();

        #endregion

        #region rebind list view

        ListManager?.Rebind();

        #endregion

        #region set local cache

        if (ConsentService.Consent is true)
        {
            try
            {
                var storage = await StorageService.SetAsync(
                    key: "cache",
                    value: MapperService
                        .Map<List<TelerikStop>>(source: data)
                        .Concat(second: cache)
                        .DistinctBy(keySelector: stop => stop.Id)
                        .OrderBy(keySelector: stop => stop.Id));

                if (storage is { Success: false })
                    if (JavascriptManager is not null)
                        await JavascriptManager.InvokeVoidAsync(
                            identifier: "writeConsole",
                            args: "home: cache failed");
            }
            catch (Exception)
            {
                if (JavascriptManager is not null)
                    await JavascriptManager.InvokeVoidAsync(
                        identifier: "writeConsole",
                        args: "home: cache failed");
            }
        }

        #endregion

        #region set local location

        if (ConsentService.Consent is true)
        {
            try
            {
                var storage = await StorageService.SetAsync(
                    key: "location",
                    value: MapCenter);

                if (storage is { Success: false })
                    if (JavascriptManager is not null)
                        await JavascriptManager.InvokeVoidAsync(
                            identifier: "writeConsole",
                            args: "home: location failed");
            }
            catch (Exception)
            {
                if (JavascriptManager is not null)
                    await JavascriptManager.InvokeVoidAsync(
                        identifier: "writeConsole",
                        args: "home: location failed");
            }
        }

        #endregion
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        #region check component disposed

        if (Disposed.HasValue && Disposed.Value)
            return;

        #endregion

        #region import javascript manager

        if (firstRender)
        {
            JavascriptManager = await JavascriptService.InvokeAsync<IJSObjectReference>(
                identifier: "import",
                args: "./Components/Pages/Home.razor.js");

            await JavascriptManager.InvokeVoidAsync(
                identifier: "registerBanner",
                args: [
                    Environment.GetEnvironmentVariable(variable: "BANNER_160X300"),
                    Environment.GetEnvironmentVariable(variable: "BANNER_160X600"),
                    Environment.GetEnvironmentVariable(variable: "BANNER_320X50"),
                    Environment.GetEnvironmentVariable(variable: "BANNER_468X60"),
                    Environment.GetEnvironmentVariable(variable: "BANNER_728X90"),
                    ConsentService.Consent
                ]);

            await JavascriptManager.InvokeVoidAsync(
                identifier: "registerResize",
                args: DotNetObjectReference.Create(value: this));
        }

        #endregion
    }

    private async Task OnConsentChangedAsync()
    {
        #region check component disposed

        if (Disposed.HasValue && Disposed.Value)
            return;

        #endregion

        #region import javascript manager

        JavascriptManager = await JavascriptService.InvokeAsync<IJSObjectReference>(
            identifier: "import",
            args: "./Components/Pages/Home.razor.js");

        await JavascriptManager.InvokeVoidAsync(
            identifier: "registerBanner",
            args: [
                Environment.GetEnvironmentVariable(variable: "BANNER_160X300"),
                Environment.GetEnvironmentVariable(variable: "BANNER_160X600"),
                Environment.GetEnvironmentVariable(variable: "BANNER_320X50"),
                Environment.GetEnvironmentVariable(variable: "BANNER_468X60"),
                Environment.GetEnvironmentVariable(variable: "BANNER_728X90"),
                ConsentService.Consent
            ]);

        await JavascriptManager.InvokeVoidAsync(
            identifier: "registerResize",
            args: DotNetObjectReference.Create(value: this));

        #endregion

        #region refresh map view

        MapManager?.Refresh();

        #endregion

        #region output console message

        if (JavascriptManager is not null)
            await JavascriptManager.InvokeVoidAsync(
                identifier: "writeConsole",
                args: $"home: consent changed {(ConsentService.Consent is true ? "accepted" : "rejected")}");

        #endregion
    }

    private async Task OnLayoutChangedAsync()
    {
        #region check component disposed

        if (Disposed.HasValue && Disposed.Value)
            return;

        #endregion

        #region output console message

        if (JavascriptManager is not null)
            await JavascriptManager.InvokeVoidAsync(
                identifier: "writeConsole",
                args: $"home: layout changed {LayoutService.Layout}");

        #endregion
    }

    private async Task OnListReadAsync(ListViewReadEventArgs readEventArgs)
    {
        #region check component disposed

        if (Disposed.HasValue && Disposed.Value)
            return;

        #endregion

        #region clear list data

        ListData = [];

        #endregion

        #region build local data

        ListData.AddRange(collection: MapData.Select(selector: stop => stop));

        #endregion

        #region build remote data

        if (ListData.IsNullOrEmpty() && Loading is false)
            ListData.Add(item: TelerikStopBuilder.Build());

        readEventArgs.Data = ListData.OrderBy(keySelector: stop => stop.Distance);

        if (ListData.FirstOrDefault()?.Id is null)
            return;

        #endregion

        #region output console message

        if (JavascriptManager is not null)
            await JavascriptManager.InvokeVoidAsync(
                identifier: "writeConsole",
                args: $"home: list read {MapCenter.ElementAt(index: 1)}/{MapCenter.ElementAt(index: 0)}");

        #endregion
    }

    private async Task OnListChangeAsync(string? stopId)
    {
        #region check component disposed

        if (Disposed.HasValue && Disposed.Value)
            return;

        #endregion

        #region get context data

        if (stopId is null)
            return;

        #endregion

        #region get stop data

        var stop = new TelerikStop();

        if (MapData.Any(predicate: item => item.Id!.Equals(value: stopId)))
            stop = MapData.First(predicate: item => item.Id!.Equals(value: stopId));

        if (stop.Id is null)
            return;

        #endregion

        #region output console message

        if (JavascriptManager is not null)
            await JavascriptManager.InvokeVoidAsync(
                identifier: "writeConsole",
                args: $"home: list change {stop.Id}");

        #endregion

        #region navigate to stop

        if (NavigationService.Uri.ContainsIgnoreCase(value: $"/stop/{stop.Id}"))
            return;

        NavigationService.NavigateTo(uri: stop.Longitude is not null && stop.Latitude is not null
            ? $"/stop/{stop.Id}/{stop.Longitude}/{stop.Latitude}/{TelerikMapDefaults.Zoom}"
            : $"/stop/{stop.Id}/{MapCenter.ElementAt(index: 1)}/{MapCenter.ElementAt(index: 0)}/{TelerikMapDefaults.Zoom}");

        #endregion
    }

    private async Task OnMapMarkerClickAsync(MapMarkerClickEventArgs args)
    {
        #region check component disposed

        if (Disposed.HasValue && Disposed.Value)
            return;

        #endregion

        #region get stop data

        if (args.DataItem is not TelerikStop stop)
            return;

        if (stop.Id is null)
            return;

        #endregion

        #region output console message

        if (JavascriptManager is not null)
            await JavascriptManager.InvokeVoidAsync(
                identifier: "writeConsole",
                args: $"home: map click {stop.Id}");

        #endregion

        #region navigate to stop

        if (NavigationService.Uri.ContainsIgnoreCase(value: $"/stop/{stop.Id}"))
            return;

        NavigationService.NavigateTo(uri: stop.Longitude is not null && stop.Latitude is not null
            ? $"/stop/{stop.Id}/{stop.Longitude}/{stop.Latitude}/{TelerikMapDefaults.Zoom}"
            : $"/stop/{stop.Id}/{MapCenter.ElementAt(index: 1)}/{MapCenter.ElementAt(index: 0)}/{TelerikMapDefaults.Zoom}");

        #endregion
    }

    private async Task OnMapPanEndAsync(MapPanEndEventArgs args)
    {
        #region check component disposed

        if (Disposed.HasValue && Disposed.Value)
            return;

        #endregion

        #region get map location

        MapCenter = args.Center;

        #endregion

        #region output console message

        if (JavascriptManager is not null)
            await JavascriptManager.InvokeVoidAsync(
                identifier: "writeConsole",
                args: $"home: map pan {MapCenter.ElementAt(index: 1)}/{MapCenter.ElementAt(index: 0)}");

        #endregion

        #region navigate to home

        NavigationService.NavigateTo(
            uri: $"/{MapCenter.ElementAt(index: 1)}/{MapCenter.ElementAt(index: 0)}/{Zoom}",
            replace: true);

        #endregion
    }

    private async Task OnMapZoomEndAsync(MapZoomEndEventArgs args)
    {
        #region check component disposed

        if (Disposed.HasValue && Disposed.Value)
            return;

        #endregion

        #region get map location

        MapCenter = args.Center;
        Zoom = args.Zoom;

        #endregion

        #region output console message

        if (JavascriptManager is not null)
            await JavascriptManager.InvokeVoidAsync(
                identifier: "writeConsole",
                args: $"home: map zoom {Zoom}");

        #endregion

        #region navigate to home

        NavigationService.NavigateTo(
            uri: $"/{MapCenter.ElementAt(index: 1)}/{MapCenter.ElementAt(index: 0)}/{Zoom}",
            replace: true);

        #endregion
    }

    [JSInvokable]
    public async Task OnScreenResizedAsync()
    {
        #region check component disposed

        if (Disposed.HasValue && Disposed.Value)
            return;

        #endregion

        #region refresh map view

        MapManager?.Refresh();

        #endregion

        #region output console message

        if (JavascriptManager is not null)
            await JavascriptManager.InvokeVoidAsync(
                identifier: "writeConsole",
                args: "home: screen resized");

        #endregion
    }

    private async Task OnSearchBlurAsync()
    {
        #region check component disposed

        if (Disposed.HasValue && Disposed.Value)
            return;

        #endregion

        #region output console message

        if (JavascriptManager is not null)
            await JavascriptManager.InvokeVoidAsync(
                identifier: "writeConsole",
                args: "home: search blur");

        #endregion
    }

    private async Task OnSearchChangeAsync(object? stopId)
    {
        #region check component disposed

        if (Disposed.HasValue && Disposed.Value)
            return;

        #endregion

        #region get context data

        Query = string.Empty;

        if (stopId is null)
            return;

        #endregion

        #region get stop data

        var stop = new TelerikStop();

        if (SearchData.Any(predicate: item => item.Id!.Equals(value: stopId as string)))
            stop = SearchData.First(predicate: item => item.Id!.Equals(value: stopId as string));

        if (stop.Id is null)
            return;

        #endregion

        #region output console message

        if (JavascriptManager is not null)
            await JavascriptManager.InvokeVoidAsync(
                identifier: "writeConsole",
                args: $"home: search change {stop.Id}");

        #endregion

        #region navigate to stop

        if (NavigationService.Uri.ContainsIgnoreCase(value: $"/stop/{stop.Id}"))
            return;

        NavigationService.NavigateTo(uri: stop.Longitude is not null && stop.Latitude is not null
            ? $"/stop/{stop.Id}/{stop.Longitude}/{stop.Latitude}/{TelerikMapDefaults.Zoom}"
            : $"/stop/{stop.Id}/{MapCenter.ElementAt(index: 1)}/{MapCenter.ElementAt(index: 0)}/{TelerikMapDefaults.Zoom}");

        #endregion
    }

    private async Task OnSearchCloseAsync()
    {
        #region check component disposed

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
        #region check component disposed

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
        #region check component disposed

        if (Disposed.HasValue && Disposed.Value)
            return;

        #endregion

        #region get search input

        IList<IFilterDescriptor>? filterDescriptors = readEventArgs.Request.Filters;

        if (filterDescriptors.FirstOrDefault() is not FilterDescriptor filterDescriptor)
            return;

        var name = filterDescriptor.Value.ToString() ?? string.Empty;

        #endregion

        #region get local cache

        List<TelerikStop> cache = [];

        try
        {
            var storage = await StorageService.GetAsync<List<TelerikStop>>(key: "cache");

            if (storage is { Success: true, Value.Count: > 0 })
                cache = storage.Value.All(predicate: stop => stop.HasAllProperties()) ? storage.Value : [];
        }
        catch (Exception)
        {
            cache = [];
        }

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

        #region build local data

        SearchData.AddRange(collection: cache.Select(selector: stop => new TelerikStop
        {
            Id = stop.Id,
            Code = stop.Code,
            Name = stop.Name,
            Latitude = stop.Latitude,
            Longitude = stop.Longitude,
            Platform =  stop.Platform,
            Direction = stop.Direction,
            Location =  stop.Location?.ToArray(),
            Points = stop.Points?.ToList()
        }));

        foreach (var item in SearchData)
        {
            item.Distance = GeoCalculator.GetDistance(
                originLatitude: item.Latitude ?? 0,
                originLongitude: item.Longitude ?? 0,
                destinationLatitude: MapCenter.ElementAt(index: 0),
                destinationLongitude: MapCenter.ElementAt(index: 1),
                distanceUnit: DistanceUnit.Meters);

            item.Points = item.Points?
                .Where(predicate: point => point.DepartureDateTime >= currentDateTime)
                .Where(predicate: point => point.DepartureDateTime <= offsetDateTime)
                .ToList();
        }

        SearchData.RemoveAll(match: stop => stop.Points?.IsNullOrEmpty() is true);
        SearchData.RemoveAll(match: stop => stop.Points?.All(predicate: point => point.DepartureDateTime < currentDateTime) is true);
        SearchData.RemoveAll(match: stop => stop.Points?.All(predicate: point => point.DepartureDateTime > offsetDateTime) is true);

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

        if (SearchSource is not null)
            await SearchSource.CancelAsync();

        SearchSource = new CancellationTokenSource();

        try
        {
            var client = HttpService.CreateClient(name: "search");

            response = await client.GetAsync(
                requestUri: QueryBuilder.GetStopsFromSearch(
                    type: QueryType.StopName,
                    value: name),
                cancellationToken: SearchSource.Token);
        }
        catch (OperationCanceledException)
        {
            if (JavascriptManager is not null)
                await JavascriptManager.InvokeVoidAsync(
                    identifier: "writeConsole",
                    args: "home: search cancel");
        }
        catch (TimeoutRejectedException)
        {
            if (JavascriptManager is not null)
                await JavascriptManager.InvokeVoidAsync(
                    identifier: "writeConsole",
                    args: "home: search timeout");
        }

        if (response?.IsSuccessStatusCode is not true)
        {
            try
            {
                var client = HttpService.CreateClient(name: "database");

                response = await client.GetAsync(
                    requestUri: QueryBuilder.GetStopsFromDatabase(
                        type: QueryType.StopName,
                        value: name),
                    cancellationToken: SearchSource.Token);
            }
            catch (OperationCanceledException)
            {
                if (JavascriptManager is not null)
                    await JavascriptManager.InvokeVoidAsync(
                        identifier: "writeConsole",
                        args: "home: database cancel");
            }
            catch (TimeoutRejectedException)
            {
                if (JavascriptManager is not null)
                    await JavascriptManager.InvokeVoidAsync(
                        identifier: "writeConsole",
                        args: "home: database timeout");
            }
        }

        #endregion

        #region build remote data

        List<WebStop> data = [];

        if (response?.IsSuccessStatusCode is true)
            data = await response.Content.ReadFromJsonAsync<List<WebStop>>() ?? [];

        var results = MapperService.Map<List<TelerikStop>>(source: data);

        foreach (var item in results)
        {
            var stop = SearchData.FirstOrDefault(stop => stop.Id == item.Id);
            stop?.Code = item.Code;
            stop?.Name = item.Name;
            stop?.Latitude = item.Latitude;
            stop?.Longitude = item.Longitude;
            stop?.Platform = item.Platform;
            stop?.Direction = item.Direction;
            stop?.Location = item.Location?.ToArray();
            stop?.Points = item.Points?.ToList();

            if (stop is not null)
                continue;

            SearchData.Add(item: new TelerikStop
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
                Latitude = item.Latitude,
                Longitude = item.Longitude,
                Platform =  item.Platform,
                Direction = item.Direction,
                Location =  item.Location?.ToArray(),
                Points = item.Points?.ToList()
            });
        }

        foreach (var item in SearchData)
        {
            item.Distance = GeoCalculator.GetDistance(
                originLatitude: item.Latitude ?? 0,
                originLongitude: item.Longitude ?? 0,
                destinationLatitude: MapCenter.ElementAt(index: 0),
                destinationLongitude: MapCenter.ElementAt(index: 1),
                distanceUnit: DistanceUnit.Meters);

            item.Points = item.Points?
                .Where(predicate: point => point.DepartureDateTime >= currentDateTime)
                .Where(predicate: point => point.DepartureDateTime <= offsetDateTime)
                .ToList();
        }

        SearchData.RemoveAll(match: stop => stop.Points?.IsNullOrEmpty() is true);
        SearchData.RemoveAll(match: stop => stop.Points?.All(predicate: point => point.DepartureDateTime < currentDateTime) is true);
        SearchData.RemoveAll(match: stop => stop.Points?.All(predicate: point => point.DepartureDateTime > offsetDateTime) is true);

        readEventArgs.Data = SearchData
            .OrderByDescending(keySelector: stop => stop.Name.ContainsIgnoreCase(value: name))
            .ThenByDescending(keySelector: stop => stop.Direction.ContainsIgnoreCase(value: name))
            .ThenBy(keySelector: stop => stop.Name)
            .ThenBy(keySelector: stop => stop.Id);

        #endregion

        #region set local cache

        if (ConsentService.Consent is true)
        {
            try
            {
                var storage = await StorageService.SetAsync(
                    key: "cache",
                    value: MapperService
                        .Map<List<TelerikStop>>(source: data)
                        .Concat(second: cache)
                        .DistinctBy(keySelector: stop => stop.Id)
                        .OrderBy(keySelector: stop => stop.Id));

                if (storage is { Success: false })
                    if (JavascriptManager is not null)
                        await JavascriptManager.InvokeVoidAsync(
                            identifier: "writeConsole",
                            args: "home: cache failed");
            }
            catch (Exception)
            {
                if (JavascriptManager is not null)
                    await JavascriptManager.InvokeVoidAsync(
                        identifier: "writeConsole",
                        args: "home: cache failed");
            }
        }

        #endregion

        #region output console message

        if (JavascriptManager is not null)
            await JavascriptManager.InvokeVoidAsync(
                identifier: "writeConsole",
                args: $"home: search read {name}");

        #endregion
    }

    private async Task OnSchemeChangedAsync()
    {
        #region check component disposed

        if (Disposed.HasValue && Disposed.Value)
            return;

        #endregion

        #region set component state

        await JavascriptService.InvokeVoidAsync(
            identifier: "setScheme",
            args: SchemeService.Scheme);

        #endregion

        #region output console message

        if (JavascriptManager is not null)
            await JavascriptManager.InvokeVoidAsync(
                identifier: "writeConsole",
                args: $"home: scheme changed {SchemeService.Scheme}");

        #endregion
    }

    private async Task OnSchemeChangedAsync(bool? match)
    {
        #region check component disposed

        if (Disposed.HasValue && Disposed.Value)
            return;

        #endregion

        #region get component state

        if (JavascriptManager is null)
            return;

        #endregion

        #region get context state

        if (match is null)
            return;

        #endregion

        #region get cookie state

        var scheme = await JavascriptManager.InvokeAsync<string>(
            identifier: "getCookie",
            args: ".AspNet.Preference.Scheme");

        if (scheme is not "dark" and not "light")
            scheme = "system";

        if (scheme is not "system")
            return;

        #endregion

        #region set component state

        SchemeService.Set(scheme: scheme);

        #endregion
    }

    async ValueTask IAsyncDisposable.DisposeAsync()
    {
        #region set component disposed

        Disposed = true;

        #endregion

        #region suppress object finalizer

        GC.SuppressFinalize(obj: this);

        #endregion

        #region cancel pending events

        ConsentService.OnConsentChanged -= StateHasChanged;
        ConsentService.OnConsentChanged -= _onConsentChangedWrapper;

        LayoutService.OnLayoutChanged -= StateHasChanged;
        LayoutService.OnLayoutChanged -= _onLayoutChangedWrapper;

        SchemeService.OnSchemeChanged -= StateHasChanged;
        SchemeService.OnSchemeChanged -= _onSchemeChangedWrapper;

        #endregion

        #region cancel pending requests

        if (MapSource is not null)
            await MapSource.CancelAsync();

        if (SearchSource is not null)
            await SearchSource.CancelAsync();

        #endregion

        #region dispose token sources

        MapSource?.Dispose();
        SearchSource?.Dispose();

        #endregion

        #region dispose javascript manager

        if (JavascriptManager is not null)
            await JavascriptManager.DisposeAsync();

        #endregion
    }
}
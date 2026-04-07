using AutoMapper;
using NextDepartures.Standard;
using NextDepartures.Standard.Types;
using NextDepartures.Storage.Postgres.Aspire;
using Npgsql;
using TramTimes.Web.Utilities.Extensions;
using TramTimes.Web.Utilities.Models;

namespace TramTimes.Web.Site.Services;

public class DatabaseService(
    NpgsqlDataSource dataSource,
    IMapper mapperService) {

    public async Task<List<WebStopPoint>?> GetServicesByStopAsync(
        string id,
        CancellationToken cancellationToken = default) {

        #region build request

        var feed = await Feed.LoadAsync(
            dataStorage: PostgresStorage.Load(dataSource: dataSource),
            cancellationToken: cancellationToken);

        var request = await feed.GetServicesByStopAsync(
            id: id.ToUpperInvariant(),
            target: DateTime.UtcNow,
            offset: TimeSpan.FromMinutes(minutes: -60),
            comparison: ComparisonType.Exact,
            tolerance: TimeSpan.FromHours(value: 12),
            results: 250,
            cancellationToken: cancellationToken);

        if (request.IsNullOrEmpty())
            return null;

        #endregion

        #region build results

        var results = mapperService.Map<List<DatabaseStopPoint>>(
            source: mapperService.Map<List<WebStopPoint>>(
                source: request));

        #endregion

        return mapperService.Map<List<WebStopPoint>>(source: results);
    }

    public async Task<List<WebStopPoint>?> GetServicesByTripAsync(
        string id,
        CancellationToken cancellationToken = default) {

        #region build request

        var feed = await Feed.LoadAsync(
            dataStorage: PostgresStorage.Load(dataSource: dataSource),
            cancellationToken: cancellationToken);

        var request = await feed.GetServicesByTripAsync(
            id: id.ToLowerInvariant(),
            target: DateTime.UtcNow,
            offset: TimeSpan.FromMinutes(minutes: -60),
            comparison: ComparisonType.Exact,
            tolerance: TimeSpan.FromHours(value: 12),
            results: 250,
            cancellationToken: cancellationToken);

        if (request.IsNullOrEmpty())
            return null;

        #endregion

        #region build results

        var results = mapperService.Map<List<DatabaseStopPoint>>(
            source: mapperService.Map<List<WebStopPoint>>(
                source: request));

        #endregion

        return mapperService.Map<List<WebStopPoint>>(source: results);
    }

    public async Task<List<WebStop>?> GetStopsByIdAsync(
        string id,
        CancellationToken cancellationToken = default) {

        #region build request

        var feed = await Feed.LoadAsync(
            dataStorage: PostgresStorage.Load(dataSource: dataSource),
            cancellationToken: cancellationToken);

        var request = await feed.GetStopsByIdAsync(
            id: id.ToUpperInvariant(),
            comparison: ComparisonType.Exact,
            cancellationToken: cancellationToken);

        if (request.IsNullOrEmpty())
            return null;

        #endregion

        #region build results

        var results = mapperService.Map<List<DatabaseStop>>(source: request);

        foreach (var item in results)
            item.Points = mapperService.Map<List<DatabaseStopPoint>>(
                source: mapperService.Map<List<WebStopPoint>>(
                    source: await feed.GetServicesByStopAsync(
                        id: item.Id,
                        target: DateTime.UtcNow,
                        offset: TimeSpan.FromMinutes(minutes: -60),
                        comparison: ComparisonType.Exact,
                        tolerance: TimeSpan.FromHours(value: 12),
                        results: 250,
                        cancellationToken: cancellationToken)));

        #endregion

        return mapperService.Map<List<WebStop>>(source: results);
    }

    public async Task<List<WebStop>?> GetStopsByCodeAsync(
        string code,
        CancellationToken cancellationToken = default) {

        #region build request

        var feed = await Feed.LoadAsync(
            dataStorage: PostgresStorage.Load(dataSource: dataSource),
            cancellationToken: cancellationToken);

        var request = await feed.GetStopsByCodeAsync(
            code: code.ToLowerInvariant(),
            comparison: ComparisonType.Exact,
            cancellationToken: cancellationToken);

        if (request.IsNullOrEmpty())
            return null;

        #endregion

        #region build results

        var results = mapperService.Map<List<DatabaseStop>>(source: request);

        foreach (var item in results)
            item.Points = mapperService.Map<List<DatabaseStopPoint>>(
                source: mapperService.Map<List<WebStopPoint>>(
                    source: await feed.GetServicesByStopAsync(
                        id: item.Id,
                        target: DateTime.UtcNow,
                        offset: TimeSpan.FromMinutes(minutes: -60),
                        comparison: ComparisonType.Exact,
                        tolerance: TimeSpan.FromHours(value: 12),
                        results: 250,
                        cancellationToken: cancellationToken)));

        #endregion

        return mapperService.Map<List<WebStop>>(source: results);
    }

    public async Task<List<WebStop>?> GetStopsByNameAsync(
        string name,
        CancellationToken cancellationToken = default) {

        #region build request

        var feed = await Feed.LoadAsync(
            dataStorage: PostgresStorage.Load(dataSource: dataSource),
            cancellationToken: cancellationToken);

        var request = await feed.GetStopsByNameAsync(
            name: name.ToLowerInvariant(),
            comparison: ComparisonType.Partial,
            cancellationToken: cancellationToken);

        if (request.IsNullOrEmpty())
            return null;

        #endregion

        #region build results

        var results = mapperService.Map<List<DatabaseStop>>(source: request);

        foreach (var item in results)
            item.Points = mapperService.Map<List<DatabaseStopPoint>>(
                source: mapperService.Map<List<WebStopPoint>>(
                    source: await feed.GetServicesByStopAsync(
                        id: item.Id,
                        target: DateTime.UtcNow,
                        offset: TimeSpan.FromMinutes(minutes: -60),
                        comparison: ComparisonType.Exact,
                        tolerance: TimeSpan.FromHours(value: 12),
                        results: 250,
                        cancellationToken: cancellationToken)));

        #endregion

        return mapperService.Map<List<WebStop>>(source: results);
    }

    public async Task<List<WebStop>?> GetStopsByLocationAsync(
        double minLon,
        double minLat,
        double maxLon,
        double maxLat,
        CancellationToken cancellationToken = default) {

        #region build request

        var feed = await Feed.LoadAsync(
            dataStorage: PostgresStorage.Load(dataSource: dataSource),
            cancellationToken: cancellationToken);

        var request = await feed.GetStopsByLocationAsync(
            minimumLongitude: minLon,
            minimumLatitude: minLat,
            maximumLongitude: maxLon,
            maximumLatitude: maxLat,
            comparison: ComparisonType.Partial,
            cancellationToken: cancellationToken);

        if (request.IsNullOrEmpty())
            return null;

        #endregion

        #region build results

        var results = mapperService.Map<List<DatabaseStop>>(source: request);

        foreach (var item in results)
            item.Points = mapperService.Map<List<DatabaseStopPoint>>(
                source: mapperService.Map<List<WebStopPoint>>(
                    source: await feed.GetServicesByStopAsync(
                        id: item.Id,
                        target: DateTime.UtcNow,
                        offset: TimeSpan.FromMinutes(minutes: -60),
                        comparison: ComparisonType.Exact,
                        tolerance: TimeSpan.FromHours(value: 12),
                        results: 250,
                        cancellationToken: cancellationToken)));

        #endregion

        return mapperService.Map<List<WebStop>>(source: results);
    }

    public async Task<List<WebStop>?> GetStopsByPointAsync(
        double lon,
        double lat,
        CancellationToken cancellationToken = default) {

        #region build request

        var feed = await Feed.LoadAsync(
            dataStorage: PostgresStorage.Load(dataSource: dataSource),
            cancellationToken: cancellationToken);

        var request = await feed.GetStopsByPointAsync(
            longitude: lon,
            latitude: lat,
            distance: 1,
            comparison: ComparisonType.Partial,
            cancellationToken: cancellationToken);

        if (request.IsNullOrEmpty())
            return null;

        #endregion

        #region build results

        var results = mapperService.Map<List<DatabaseStop>>(source: request);

        foreach (var item in results)
            item.Points = mapperService.Map<List<DatabaseStopPoint>>(
                source: mapperService.Map<List<WebStopPoint>>(
                    source: await feed.GetServicesByStopAsync(
                        id: item.Id,
                        target: DateTime.UtcNow,
                        offset: TimeSpan.FromMinutes(minutes: -60),
                        comparison: ComparisonType.Exact,
                        tolerance: TimeSpan.FromHours(value: 12),
                        results: 250,
                        cancellationToken: cancellationToken)));

        #endregion

        return mapperService.Map<List<WebStop>>(source: results);
    }
}
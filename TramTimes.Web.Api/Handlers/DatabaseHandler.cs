using AutoMapper;
using NextDepartures.Standard;
using NextDepartures.Standard.Types;
using NextDepartures.Storage.Postgres.Aspire;
using Npgsql;
using TramTimes.Web.Api.Models;
using TramTimes.Web.Utilities.Extensions;
using TramTimes.Web.Utilities.Models;

namespace TramTimes.Web.Api.Handlers;

public static class DatabaseHandler
{
    public static async Task<IResult> GetServicesByStopAsync(
        NpgsqlDataSource dataSource,
        IMapper mapperService,
        string id) {
        
        #region build request
        
        var feed = await Feed.LoadAsync(dataStorage: PostgresStorage.Load(dataSource: dataSource));
        
        var request = await feed.GetServicesByStopAsync(
            id: id.ToUpperInvariant(),
            comparison: ComparisonType.Exact,
            tolerance: TimeSpan.FromMinutes(value: 179));
        
        if (request.IsNullOrEmpty())
            return Results.NotFound();
        
        #endregion
        
        #region build results
        
        var results = mapperService.Map<List<DatabaseStopPoint>>(
            source: mapperService.Map<List<WorkerStopPoint>>(
                source: request));
        
        #endregion
        
        return Results.Json(data: mapperService.Map<List<WebStopPoint>>(source: results));
    }
    
    public static async Task<IResult> GetServicesByTripAsync(
        NpgsqlDataSource dataSource,
        IMapper mapperService,
        string id) {
        
        #region build request
        
        var feed = await Feed.LoadAsync(dataStorage: PostgresStorage.Load(dataSource: dataSource));
        
        var request = await feed.GetServicesByTripAsync(
            id: id.ToLowerInvariant(),
            comparison: ComparisonType.Exact,
            tolerance: TimeSpan.FromMinutes(value: 359));
        
        if (request.IsNullOrEmpty())
            return Results.NotFound();
        
        #endregion
        
        #region build results
        
        var results = mapperService.Map<List<DatabaseStopPoint>>(
            source: mapperService.Map<List<WorkerStopPoint>>(
                source: request));
        
        #endregion
        
        return Results.Json(data: mapperService.Map<List<WebStopPoint>>(source: results));
    }
    
    public static async Task<IResult> GetStopsByIdAsync(
        NpgsqlDataSource dataSource,
        IMapper mapperService,
        string id) {
        
        #region build request
        
        var feed = await Feed.LoadAsync(dataStorage: PostgresStorage.Load(dataSource: dataSource));
        
        var request = await feed.GetStopsByIdAsync(
            id: id.ToUpperInvariant(),
            comparison: ComparisonType.Exact);
        
        if (request.IsNullOrEmpty())
            return Results.NotFound();
        
        #endregion
        
        #region build results
        
        var results = mapperService.Map<List<DatabaseStop>>(source: request);
        
        foreach (var item in results)
            item.Points = mapperService.Map<List<DatabaseStopPoint>>(
                source: mapperService.Map<List<WorkerStopPoint>>(
                    source: await feed.GetServicesByStopAsync(
                        id: item.Id,
                        comparison: ComparisonType.Exact,
                        tolerance: TimeSpan.FromMinutes(value: 179))));
        
        #endregion
        
        return Results.Json(data: mapperService.Map<List<WebStop>>(source: results));
    }
    
    public static async Task<IResult> GetStopsByCodeAsync(
        NpgsqlDataSource dataSource,
        IMapper mapperService,
        string code) {
        
        #region build request
        
        var feed = await Feed.LoadAsync(dataStorage: PostgresStorage.Load(dataSource: dataSource));
        
        var request = await feed.GetStopsByCodeAsync(
            code: code.ToLowerInvariant(),
            comparison: ComparisonType.Exact);
        
        if (request.IsNullOrEmpty())
            return Results.NotFound();
        
        #endregion
        
        #region build results
        
        var results = mapperService.Map<List<DatabaseStop>>(source: request);
        
        foreach (var item in results)
            item.Points = mapperService.Map<List<DatabaseStopPoint>>(
                source: mapperService.Map<List<WorkerStopPoint>>(
                    source: await feed.GetServicesByStopAsync(
                        id: item.Id,
                        comparison: ComparisonType.Exact,
                        tolerance: TimeSpan.FromMinutes(value: 179))));
        
        #endregion
        
        return Results.Json(data: mapperService.Map<List<WebStop>>(source: results));
    }
    
    public static async Task<IResult> GetStopsByNameAsync(
        NpgsqlDataSource dataSource,
        IMapper mapperService,
        string name) {
        
        #region build request
        
        var feed = await Feed.LoadAsync(dataStorage: PostgresStorage.Load(dataSource: dataSource));
        
        var request = await feed.GetStopsByNameAsync(
            name: name.ToLowerInvariant(),
            comparison: ComparisonType.Partial);
        
        if (request.IsNullOrEmpty())
            return Results.NotFound();
        
        #endregion
        
        #region build results
        
        var results = mapperService.Map<List<DatabaseStop>>(source: request);
        
        foreach (var item in results)
            item.Points = mapperService.Map<List<DatabaseStopPoint>>(
                source: mapperService.Map<List<WorkerStopPoint>>(
                    source: await feed.GetServicesByStopAsync(
                        id: item.Id,
                        comparison: ComparisonType.Exact,
                        tolerance: TimeSpan.FromMinutes(value: 179))));
        
        #endregion
        
        return Results.Json(data: mapperService.Map<List<WebStop>>(source: results));
    }
    
    public static async Task<IResult> GetStopsByLocationAsync(
        NpgsqlDataSource dataSource,
        IMapper mapperService,
        double minLon,
        double minLat,
        double maxLon,
        double maxLat) {
        
        #region build request
        
        var feed = await Feed.LoadAsync(dataStorage: PostgresStorage.Load(dataSource: dataSource));
        
        var request = await feed.GetStopsByLocationAsync(
            minimumLongitude: minLon,
            minimumLatitude: minLat,
            maximumLongitude: maxLon,
            maximumLatitude: maxLat,
            comparison: ComparisonType.Partial);
        
        if (request.IsNullOrEmpty())
            return Results.NotFound();
        
        #endregion
        
        #region build results
        
        var results = mapperService.Map<List<DatabaseStop>>(source: request);
        
        foreach (var item in results)
            item.Points = mapperService.Map<List<DatabaseStopPoint>>(
                source: mapperService.Map<List<WorkerStopPoint>>(
                    source: await feed.GetServicesByStopAsync(
                        id: item.Id,
                        comparison: ComparisonType.Exact,
                        tolerance: TimeSpan.FromMinutes(value: 179))));
        
        #endregion
        
        return Results.Json(data: mapperService.Map<List<WebStop>>(source: results));
    }
    
    public static async Task<IResult> GetStopsByPointAsync(
        NpgsqlDataSource dataSource,
        IMapper mapperService,
        double lon,
        double lat) {
        
        #region build request
        
        var feed = await Feed.LoadAsync(dataStorage: PostgresStorage.Load(dataSource: dataSource));
        
        var request = await feed.GetStopsByPointAsync(
            longitude: lon,
            latitude: lat,
            distance: 1,
            comparison: ComparisonType.Partial);
        
        if (request.IsNullOrEmpty())
            return Results.NotFound();
        
        #endregion
        
        #region build results
        
        var results = mapperService.Map<List<DatabaseStop>>(source: request);
        
        foreach (var item in results)
            item.Points = mapperService.Map<List<DatabaseStopPoint>>(
                source: mapperService.Map<List<WorkerStopPoint>>(
                    source: await feed.GetServicesByStopAsync(
                        id: item.Id,
                        comparison: ComparisonType.Exact,
                        tolerance: TimeSpan.FromMinutes(value: 179))));
        
        #endregion
        
        return Results.Json(data: mapperService.Map<List<WebStop>>(source: results));
    }
}
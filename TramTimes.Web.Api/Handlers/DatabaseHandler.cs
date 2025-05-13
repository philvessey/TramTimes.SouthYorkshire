using AutoMapper;
using Geolocation;
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
        
        var feed = await Feed.Load(dataStorage: PostgresStorage.Load(dataSource: dataSource));
        
        var request = await feed.GetServicesByStopAsync(
            id: id.ToUpperInvariant(),
            comparison: ComparisonType.Exact,
            tolerance: TimeSpan.FromMinutes(value: 719));
        
        if (request.IsNullOrEmpty())
            return Results.NotFound();
        
        var response = mapperService.Map<List<DatabaseStopPoint>>(
            source: mapperService.Map<List<WorkerStopPoint>>(
                source: request));
        
        return Results.Json(data: mapperService.Map<List<WebStopPoint>>(source: response));
    }
    
    public static async Task<IResult> GetServicesByTripAsync(
        NpgsqlDataSource dataSource,
        IMapper mapperService,
        string id) {
        
        var feed = await Feed.Load(dataStorage: PostgresStorage.Load(dataSource: dataSource));
        
        var request = await feed.GetServicesByTripAsync(
            id: id.ToUpperInvariant(),
            comparison: ComparisonType.Exact,
            tolerance: TimeSpan.FromMinutes(value: 119));
        
        if (request.IsNullOrEmpty())
            return Results.NotFound();
        
        var response = mapperService.Map<List<DatabaseStopPoint>>(
            source: mapperService.Map<List<WorkerStopPoint>>(
                source: request));
        
        return Results.Json(data: mapperService.Map<List<WebStopPoint>>(source: response));
    }
    
    public static async Task<IResult> GetStopsByIdAsync(
        NpgsqlDataSource dataSource,
        IMapper mapperService,
        string id) {
        
        var feed = await Feed.Load(dataStorage: PostgresStorage.Load(dataSource: dataSource));
        
        var request = await feed.GetStopsByIdAsync(
            id: id.ToUpperInvariant(),
            comparison: ComparisonType.Exact);
        
        if (request.IsNullOrEmpty())
            return Results.NotFound();
        
        var response = mapperService.Map<List<DatabaseStop>>(source: request);
        
        foreach (var stop in response)
        {
            stop.Points = mapperService.Map<List<DatabaseStopPoint>>(
                source: mapperService.Map<List<WorkerStopPoint>>(
                    source: await feed.GetServicesByStopAsync(
                        id: stop.Id,
                        comparison: ComparisonType.Exact,
                        tolerance: TimeSpan.FromMinutes(value: 119))));
        }
        
        return Results.Json(data: mapperService.Map<List<WebStop>>(source: response));
    }
    
    public static async Task<IResult> GetStopsByCodeAsync(
        NpgsqlDataSource dataSource,
        IMapper mapperService,
        string code) {
        
        var feed = await Feed.Load(dataStorage: PostgresStorage.Load(dataSource: dataSource));
        
        var request = await feed.GetStopsByCodeAsync(
            code: code.ToUpperInvariant(),
            comparison: ComparisonType.Exact);
        
        if (request.IsNullOrEmpty())
            return Results.NotFound();
        
        var response = mapperService.Map<List<DatabaseStop>>(source: request);
        
        foreach (var stop in response)
        {
            stop.Points = mapperService.Map<List<DatabaseStopPoint>>(
                source: mapperService.Map<List<WorkerStopPoint>>(
                    source: await feed.GetServicesByStopAsync(
                        id: stop.Id,
                        comparison: ComparisonType.Exact,
                        tolerance: TimeSpan.FromMinutes(value: 119))));
        }
        
        return Results.Json(data: mapperService.Map<List<WebStop>>(source: response));
    }
    
    public static async Task<IResult> GetStopsByNameAsync(
        NpgsqlDataSource dataSource,
        IMapper mapperService,
        string name) {
        
        var feed = await Feed.Load(dataStorage: PostgresStorage.Load(dataSource: dataSource));
        
        var request = await feed.GetStopsByNameAsync(
            name: name.ToUpperInvariant(),
            comparison: ComparisonType.Partial);
        
        if (request.IsNullOrEmpty())
            return Results.NotFound();
        
        var response = mapperService.Map<List<DatabaseStop>>(source: request);
        
        foreach (var stop in response)
        {
            stop.Points = mapperService.Map<List<DatabaseStopPoint>>(
                source: mapperService.Map<List<WorkerStopPoint>>(
                    source: await feed.GetServicesByStopAsync(
                        id: stop.Id,
                        comparison: ComparisonType.Exact,
                        tolerance: TimeSpan.FromMinutes(value: 119))));
        }
        
        return Results.Json(data: mapperService.Map<List<WebStop>>(source: response));
    }
    
    public static async Task<IResult> GetStopsByLocationAsync(
        NpgsqlDataSource dataSource,
        IMapper mapperService,
        double minLon,
        double minLat,
        double maxLon,
        double maxLat) {
        
        var feed = await Feed.Load(dataStorage: PostgresStorage.Load(dataSource: dataSource));
        
        var request = await feed.GetStopsByLocationAsync(
            minimumLongitude: minLon,
            minimumLatitude: minLat,
            maximumLongitude: maxLon,
            maximumLatitude: maxLat,
            comparison: ComparisonType.Partial);
        
        if (request.IsNullOrEmpty())
            return Results.NotFound();
        
        var response = mapperService.Map<List<DatabaseStop>>(source: request);
        
        foreach (var stop in response)
        {
            stop.Distance = GeoCalculator.GetDistance(
                originLatitude: stop.Latitude ?? 0,
                originLongitude: stop.Longitude ?? 0,
                destinationLatitude: (minLat + maxLat) / 2,
                destinationLongitude: (minLon + maxLon) / 2,
                distanceUnit: DistanceUnit.Meters);
            
            stop.Points = mapperService.Map<List<DatabaseStopPoint>>(
                source: mapperService.Map<List<WorkerStopPoint>>(
                    source: await feed.GetServicesByStopAsync(
                        id: stop.Id,
                        comparison: ComparisonType.Exact,
                        tolerance: TimeSpan.FromMinutes(value: 119))));
        }
        
        return Results.Json(data: mapperService.Map<List<WebStop>>(
            source: response.OrderBy(keySelector: stop => stop.Distance)));
    }
}
using System.Text.Json;
using AutoMapper;
using NextDepartures.Standard;
using NextDepartures.Standard.Types;
using NextDepartures.Storage.Postgres.Aspire;
using Npgsql;
using StackExchange.Redis;
using TramTimes.Web.Api.Models;

namespace TramTimes.Web.Api.Builders;

public static class CacheBuilder
{
    public static async Task Build(
        NpgsqlDataSource dataSource,
        IConnectionMultiplexer cacheService,
        IMapper mapperService,
        string id) {
        
        #region get database feed
        
        var databaseFeed = await Feed.LoadAsync(dataStorage: PostgresStorage.Load(dataSource: dataSource));
        
        var databaseResults = await databaseFeed.GetServicesByStopAsync(
            id: id,
            target: DateTime.Now,
            offset: TimeSpan.FromMinutes(minutes: -60),
            comparison: ComparisonType.Exact,
            tolerance: TimeSpan.FromHours(value: 4));
        
        #endregion
        
        #region set cache feed
        
        await cacheService
            .GetDatabase()
            .StringSetAsync(
                key: $"southyorkshire:stop:{id}",
                value: JsonSerializer.Serialize(value: mapperService.Map<List<WorkerStopPoint>>(source: databaseResults)),
                expiry: TimeSpan.FromHours(value: 4));
        
        #endregion
        
        #region get trip feed
        
        var tripFeed = databaseResults
            .Select(selector: s => s.TripId)
            .ToList();
        
        #endregion
        
        #region set cache feed
        
        foreach (var item in tripFeed)
        {
            databaseResults = await databaseFeed.GetServicesByTripAsync(
                id: item,
                target: DateTime.Now,
                offset: TimeSpan.FromMinutes(value: -60),
                comparison: ComparisonType.Exact,
                tolerance: TimeSpan.FromHours(value: 4));
            
            await cacheService
                .GetDatabase()
                .StringSetAsync(
                    key: $"southyorkshire:trip:{item}",
                    value: JsonSerializer.Serialize(value: mapperService.Map<List<WorkerStopPoint>>(source: databaseResults)),
                    expiry: TimeSpan.FromHours(value: 4));
        }
        
        #endregion
    }
}
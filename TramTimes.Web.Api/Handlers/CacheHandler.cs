using System.Text.Json;
using AutoMapper;
using StackExchange.Redis;
using TramTimes.Web.Api.Models;
using TramTimes.Web.Utilities.Models;

namespace TramTimes.Web.Api.Handlers;

public static class CacheHandler
{
    public static async Task<IResult> GetServicesByStopAsync(
        IConnectionMultiplexer cacheService,
        IMapper mapperService,
        string id) {

        #region build request

        var request = await cacheService
            .GetDatabase()
            .StringGetAsync(key: $"southyorkshire:stop:{id.ToUpperInvariant()}");

        if (request.IsNullOrEmpty)
            return Results.NotFound();

        #endregion

        #region build results

        var results = mapperService.Map<List<CacheStopPoint>>(
            source: JsonSerializer.Deserialize<List<WorkerStopPoint>>(
                json: request.ToString()));

        #endregion

        return Results.Json(data: mapperService.Map<List<WebStopPoint>>(source: results));
    }

    public static async Task<IResult> GetServicesByTripAsync(
        IConnectionMultiplexer cacheService,
        IMapper mapperService,
        string id) {

        #region build request

        var request = await cacheService
            .GetDatabase()
            .StringGetAsync(key: $"southyorkshire:trip:{id.ToLowerInvariant()}");

        if (request.IsNullOrEmpty)
            return Results.NotFound();

        #endregion

        #region build results

        var results = mapperService.Map<List<CacheStopPoint>>(
            source: JsonSerializer.Deserialize<List<WorkerStopPoint>>(
                json: request.ToString()));

        #endregion

        return Results.Json(data: mapperService.Map<List<WebStopPoint>>(source: results));
    }
}
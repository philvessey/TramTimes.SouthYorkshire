using System.Text.Json;
using AutoMapper;
using StackExchange.Redis;
using TramTimes.Web.Utilities.Models;

namespace TramTimes.Web.Site.Services;

public class CacheService(
    IConnectionMultiplexer cacheService,
    IMapper mapperService) {

    public async Task<List<WebStopPoint>?> GetServicesByStopAsync(
        string id,
        CancellationToken cancellationToken = default) {

        #region build request

        var request = await cacheService
            .GetDatabase()
            .StringGetAsync(key: $"southyorkshire:stop:{id.ToUpperInvariant()}")
            .WaitAsync(cancellationToken: cancellationToken);

        if (request.IsNullOrEmpty)
            return null;

        #endregion

        #region build results

        var results = mapperService.Map<List<CacheStopPoint>>(
            source: JsonSerializer.Deserialize<List<WebStopPoint>>(
                json: request.ToString()));

        #endregion

        return mapperService.Map<List<WebStopPoint>>(source: results);
    }

    public async Task<List<WebStopPoint>?> GetServicesByTripAsync(
        string id,
        CancellationToken cancellationToken = default) {

        #region build request

        var request = await cacheService
            .GetDatabase()
            .StringGetAsync(key: $"southyorkshire:trip:{id.ToLowerInvariant()}")
            .WaitAsync(cancellationToken: cancellationToken);

        if (request.IsNullOrEmpty)
            return null;

        #endregion

        #region build results

        var results = mapperService.Map<List<CacheStopPoint>>(
            source: JsonSerializer.Deserialize<List<WebStopPoint>>(
                json: request.ToString()));

        #endregion

        return mapperService.Map<List<WebStopPoint>>(source: results);
    }
}
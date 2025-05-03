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
        
        var request = await cacheService.GetDatabase()
            .StringGetAsync(key: id.ToUpperInvariant());
        
        if (request.IsNullOrEmpty)
            return Results.NotFound();
        
        var response = mapperService.Map<List<CacheStopPoint>>(
            source: JsonSerializer.Deserialize<List<WorkerStopPoint>>(
                json: request.ToString()));
        
        return Results.Json(data: mapperService.Map<List<WebStopPoint>>(source: response));
    }
}
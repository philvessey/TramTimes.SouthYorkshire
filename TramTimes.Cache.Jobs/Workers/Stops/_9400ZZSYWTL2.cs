using System.Text.Json;
using AutoMapper;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using NextDepartures.Standard;
using NextDepartures.Standard.Types;
using NextDepartures.Storage.Postgres.Aspire;
using Npgsql;
using Quartz;
using StackExchange.Redis;
using TramTimes.Cache.Jobs.Models;

namespace TramTimes.Cache.Jobs.Workers.Stops;

public class _9400ZZSYWTL2(
    BlobContainerClient blobService,
    NpgsqlDataSource dataSource,
    IConnectionMultiplexer cacheService,
    ILogger<_9400ZZSYWTL2> logger,
    IMapper mapper) : IJob {
    
    public async Task Execute(IJobExecutionContext context)
    {
        var guid = Guid.NewGuid();
        
        var storage = Directory.CreateDirectory(path: Path.Combine(
            path1: Path.GetTempPath(),
            path2: guid.ToString()));
        
        try
        {
            #region get cache feed
            
            var cacheFeed = await cacheService
                .GetDatabase()
                .StringGetAsync(key: "stop:9400ZZSYWTL2");
            
            List<CacheStopPoint> mappedResults = [];
            
            if (!cacheFeed.IsNullOrEmpty)
                mappedResults = mapper.Map<List<CacheStopPoint>>(
                    source: JsonSerializer.Deserialize<List<WorkerStopPoint>>(
                        json: cacheFeed.ToString()));
            
            #endregion
            
            #region check cache feed
            
            if (mappedResults.ElementAtOrDefault(index: 0)?.DepartureDateTime > DateTime.Now)
                return;
            
            #endregion
            
            #region get database feed
            
            var databaseFeed = await Feed.LoadAsync(dataStorage: PostgresStorage.Load(dataSource: dataSource));
            
            var databaseResults = await databaseFeed.GetServicesByStopAsync(
                id: "9400ZZSYWTL2",
                comparison: ComparisonType.Exact,
                tolerance: TimeSpan.FromMinutes(value: 119));
            
            #endregion
            
            #region set cache feed
            
            await cacheService
                .GetDatabase()
                .StringSetAsync(
                    key: "stop:9400ZZSYWTL2",
                    value: JsonSerializer.Serialize(value: mapper.Map<List<WorkerStopPoint>>(source: databaseResults)),
                    expiry: TimeSpan.FromMinutes(value: 119));
            
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
                    comparison: ComparisonType.Exact,
                    tolerance: TimeSpan.FromMinutes(value: 119));
                
                await cacheService
                    .GetDatabase()
                    .StringSetAsync(
                        key: $"trip:{item}",
                        value: JsonSerializer.Serialize(value: mapper.Map<List<WorkerStopPoint>>(source: databaseResults)),
                        expiry: TimeSpan.FromMinutes(value: 119));
            }
            
            #endregion
            
            #region build cache results
            
            var localPath = Path.Combine(
                path1: storage.FullName,
                path2: "9400ZZSYWTL2.json");
            
            await File.WriteAllTextAsync(
                path: localPath,
                contents: JsonSerializer.Serialize(value: mapper.Map<List<WorkerStopPoint>>(source: mappedResults)));
            
            var remotePath = Path.Combine(
                path1: context.FireTimeUtc.DateTime.ToString(format: "yyyyMMddHHmm"),
                path2: "get",
                path3: "9400ZZSYWTL2.json");
            
            await blobService
                .GetBlobClient(blobName: remotePath)
                .UploadAsync(
                    path: localPath,
                    options: new BlobUploadOptions
                    {
                        HttpHeaders = new BlobHttpHeaders
                        {
                            ContentType = "application/json"
                        }
                    });
            
            #endregion
            
            #region build database results
            
            localPath = Path.Combine(
                path1: storage.FullName,
                path2: "9400ZZSYWTL2.json");
            
            await File.WriteAllTextAsync(
                path: localPath,
                contents: JsonSerializer.Serialize(value: mapper.Map<List<WorkerStopPoint>>(source: databaseResults)));
            
            remotePath = Path.Combine(
                path1: context.FireTimeUtc.DateTime.ToString(format: "yyyyMMddHHmm"),
                path2: "set",
                path3: "9400ZZSYWTL2.json");
            
            await blobService
                .GetBlobClient(blobName: remotePath)
                .UploadAsync(
                    path: localPath,
                    options: new BlobUploadOptions
                    {
                        HttpHeaders = new BlobHttpHeaders
                        {
                            ContentType = "application/json"
                        }
                    });
            
            #endregion
            
            #region delete expired blobs
            
            var expiredBlobs = blobService.GetBlobsAsync(prefix: "9400ZZSYWTL2");
            
            await foreach (var item in expiredBlobs)
                if (item.Properties.LastModified < context.FireTimeUtc.DateTime.AddDays(value: -7))
                    await blobService
                        .GetBlobClient(blobName: item.Name)
                        .DeleteAsync();
            
            #endregion
        }
        catch (Exception e)
        {
            logger.LogError(
                message: "Exception: {exception}",
                args: e.ToString());
        }
        finally
        {
            storage.Delete(recursive: true);
        }
    }
}
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

namespace TramTimes.Cache.Jobs.Workers;

public class _9400ZZSYPGR2(
    BlobServiceClient blobService,
    NpgsqlDataSource dataSource,
    IConnectionMultiplexer cacheService,
    ILogger<_9400ZZSYPGR2> logger,
    IMapper mapper) : IJob {
    
    public async Task Execute(IJobExecutionContext context)
    {
        var storage = Directory.CreateDirectory(path: Path.Combine(
            path1: Path.GetTempPath(),
            path2: Guid.NewGuid().ToString()));
        
        try
        {
            #region Delete Expired Blobs
            
            var expiredBlobs = blobService.GetBlobContainerClient(blobContainerName: "cache")
                .GetBlobsAsync(prefix: "9400ZZSYPGR2");
            
            await foreach (var blob in expiredBlobs)
            {
                if (blob.Properties.LastModified < context.FireTimeUtc.DateTime.AddDays(value: -7))
                    await blobService.GetBlobContainerClient(blobContainerName: "cache")
                        .GetBlobClient(blobName: blob.Name)
                        .DeleteAsync();
            }
            
            #endregion
            
            #region Get Cache Feed
            
            var cacheFeed = cacheService.GetDatabase();
            var cacheValue = await cacheFeed.StringGetAsync(key: new RedisKey(key: "9400ZZSYPGR2"));
            
            List<WorkerStopPoint> unmappedResults = [];
            
            if (!cacheValue.IsNullOrEmpty)
            {
                unmappedResults = JsonSerializer.Deserialize<List<WorkerStopPoint>>(json: cacheValue.ToString()) ?? [];
            }
            
            var mappedResults = mapper.Map<List<CacheStopPoint>>(source: unmappedResults);
            
            #endregion
            
            #region Check Cache Feed
            
            if (mappedResults.ElementAtOrDefault(index: 0) is not null &&
                mappedResults.ElementAt(index: 0).DepartureDateTime > DateTime.Now) {
                
                return;
            }
            
            #endregion
            
            #region Get Database Feed
            
            var databaseFeed = await Feed.Load(dataStorage: PostgresStorage.Load(dataSource: dataSource));
            
            var databaseResults = await databaseFeed.GetServicesByStopAsync(
                id: "9400ZZSYPGR2",
                comparison: ComparisonType.Exact,
                tolerance: TimeSpan.FromMinutes(value: 359));
            
            #endregion
            
            #region Set Cache Feed
            
            await cacheFeed.StringSetAsync(
                key: new RedisKey(key: "9400ZZSYPGR2"),
                value: new RedisValue(value: JsonSerializer.Serialize(value: mapper.Map<List<WorkerStopPoint>>(source: databaseResults))),
                expiry: TimeSpan.FromMinutes(value: 359));
            
            #endregion
            
            #region Build Cache Results
            
            var localPath = Path.Combine(
                path1: storage.FullName,
                path2: "9400ZZSYPGR2.json");
            
            await File.WriteAllTextAsync(
                path: localPath,
                contents: JsonSerializer.Serialize(value: mapper.Map<List<WorkerStopPoint>>(source: mappedResults)));
            
            var remotePath = Path.Combine(
                path1: context.FireTimeUtc.DateTime.ToString(format: "yyyyMMddHHmm"),
                path2: "get",
                path3: "9400ZZSYPGR2.json");
            
            await blobService.GetBlobContainerClient(blobContainerName: "cache")
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
            
            #region Build Database Results
            
            localPath = Path.Combine(
                path1: storage.FullName,
                path2: "9400ZZSYPGR2.json");
            
            await File.WriteAllTextAsync(
                path: localPath,
                contents: JsonSerializer.Serialize(value: mapper.Map<List<WorkerStopPoint>>(source: databaseResults)));
            
            remotePath = Path.Combine(
                path1: context.FireTimeUtc.DateTime.ToString(format: "yyyyMMddHHmm"),
                path2: "set",
                path3: "9400ZZSYPGR2.json");
            
            await blobService.GetBlobContainerClient(blobContainerName: "cache")
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
        }
        catch (Exception e)
        {
            logger.LogError(
                message: "Exception thrown: {exception}",
                args: e.Message);
        }
        finally
        {
            storage.Delete(recursive: true);
        }
    }
}
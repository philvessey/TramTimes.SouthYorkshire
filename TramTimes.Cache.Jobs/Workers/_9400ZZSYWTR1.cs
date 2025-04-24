using System.Text.Json;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using NextDepartures.Standard;
using NextDepartures.Standard.Models;
using NextDepartures.Standard.Types;
using NextDepartures.Storage.Postgres.Aspire;
using Npgsql;
using Quartz;
using StackExchange.Redis;

namespace TramTimes.Cache.Jobs.Workers;

public class _9400ZZSYWTR1(
    BlobServiceClient blobService,
    NpgsqlDataSource dataSource,
    IConnectionMultiplexer cacheService,
    ILogger<_9400ZZSYWTR1> logger) : IJob {
    
    public async Task Execute(IJobExecutionContext context)
    {
        var storage = Directory.CreateDirectory(path: Path.Combine(
            path1: Path.GetTempPath(),
            path2: Guid.NewGuid().ToString()));
        
        try
        {
            #region Delete Expired Blobs
            
            var expiredBlobs = blobService.GetBlobContainerClient(blobContainerName: "cache")
                .GetBlobsAsync(prefix: "9400ZZSYWTR1");
            
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
            var cacheValue = await cacheFeed.StringGetAsync(key: new RedisKey(key: "9400ZZSYWTR1"));
            
            List<Service> cacheResults = [];
            
            if (!cacheValue.IsNullOrEmpty)
            {
                cacheResults = JsonSerializer.Deserialize<List<Service>>(json: cacheValue.ToString()) ?? [];
            }
            
            #endregion
            
            #region Check Cache Feed
            
            if (cacheResults.ElementAtOrDefault(index: 0) is not null &&
                cacheResults.ElementAt(index: 0).DepartureDateTime > DateTime.Now) {
                
                return;
            }
            
            #endregion
            
            #region Get Database Feed
            
            var databaseFeed = await Feed.Load(dataStorage: PostgresStorage.Load(dataSource: dataSource));
            
            var databaseResults = await databaseFeed.GetServicesByStopAsync(
                id: "9400ZZSYWTR1",
                comparison: ComparisonType.Exact,
                tolerance: TimeSpan.FromMinutes(value: 59));
            
            #endregion
            
            #region Set Cache Feed
            
            await cacheFeed.StringSetAsync(
                key: new RedisKey(key: "9400ZZSYWTR1"),
                value: new RedisValue(value: JsonSerializer.Serialize(value: databaseResults)),
                expiry: TimeSpan.FromMinutes(value: 59));
            
            #endregion
            
            #region Build Cache Results
            
            var localPath = Path.Combine(
                path1: storage.FullName,
                path2: "9400ZZSYWTR1.json");
            
            await File.WriteAllTextAsync(
                path: localPath,
                contents: JsonSerializer.Serialize(value: cacheResults));
            
            var remotePath = Path.Combine(
                path1: context.FireTimeUtc.DateTime.ToString(format: "yyyyMMddHHmm"),
                path2: "get",
                path3: "9400ZZSYWTR1.json");
            
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
                path2: "9400ZZSYWTR1.json");
            
            await File.WriteAllTextAsync(
                path: localPath,
                contents: JsonSerializer.Serialize(value: databaseResults));
            
            remotePath = Path.Combine(
                path1: context.FireTimeUtc.DateTime.ToString(format: "yyyyMMddHHmm"),
                path2: "set",
                path3: "9400ZZSYWTR1.json");
            
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
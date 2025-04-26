using System.Text.Json;
using AutoMapper;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Elastic.Clients.Elasticsearch;
using NextDepartures.Standard;
using NextDepartures.Standard.Types;
using NextDepartures.Storage.Postgres.Aspire;
using Npgsql;
using Quartz;
using TramTimes.Search.Jobs.Models;

namespace TramTimes.Search.Jobs.Workers;

public class _9400ZZSYMID1(
    BlobServiceClient blobService,
    NpgsqlDataSource dataSource,
    ElasticsearchClient searchService,
    ILogger<_9400ZZSYMID1> logger,
    IMapper mapper) : IJob {
    
    public async Task Execute(IJobExecutionContext context)
    {
        var storage = Directory.CreateDirectory(path: Path.Combine(
            path1: Path.GetTempPath(),
            path2: Guid.NewGuid().ToString()));
        
        try
        {
            #region Delete Expired Blobs
            
            var expiredBlobs = blobService.GetBlobContainerClient(blobContainerName: "search")
                .GetBlobsAsync(prefix: "9400ZZSYMID1");
            
            await foreach (var blob in expiredBlobs)
            {
                if (blob.Properties.LastModified < context.FireTimeUtc.DateTime.AddDays(value: -7))
                    await blobService.GetBlobContainerClient(blobContainerName: "search")
                        .GetBlobClient(blobName: blob.Name)
                        .DeleteAsync();
            }
            
            #endregion
            
            #region Get Search Feed
            
            var searchFeed = await searchService.GetAsync<SearchStop>(
                id: new Id("9400ZZSYMID1"),
                configureRequest: request => request.Index("search"));
            
            var unmappedResults = searchFeed.Source?.Points ?? [];
            
            #endregion
            
            #region Check Search Feed
            
            if (unmappedResults.ElementAtOrDefault(index: 0) is not null &&
                unmappedResults.ElementAt(index: 0).DepartureDateTime > DateTime.Now) {
                
                return;
            }
            
            #endregion
            
            #region Get Database Feed
            
            var databaseFeed = await Feed.Load(dataStorage: PostgresStorage.Load(dataSource: dataSource));
            
            var stopResults = await databaseFeed.GetStopsByIdAsync(
                id: "9400ZZSYMID1",
                comparison: ComparisonType.Exact);
            
            var serviceResults = await databaseFeed.GetServicesByStopAsync(
                id: "9400ZZSYMID1",
                comparison: ComparisonType.Exact,
                tolerance: TimeSpan.FromMinutes(value: 179));
            
            var databaseResults = mapper.Map<List<SearchStop>>(source: stopResults).FirstOrDefault() ?? new SearchStop();
            databaseResults.Points = mapper.Map<List<SearchStopPoint>>(source: serviceResults) ?? [];
            
            #endregion
            
            #region Set Search Feed
            
            await searchService.IndexAsync(
                document: databaseResults,
                configureRequest: request => request.Index("search"));
            
            #endregion
            
            #region Build Search Results
            
            var localPath = Path.Combine(
                path1: storage.FullName,
                path2: "9400ZZSYMID1.json");
            
            await File.WriteAllTextAsync(
                path: localPath,
                contents: JsonSerializer.Serialize(value: mapper.Map<List<WorkerStopPoint>>(source: unmappedResults)));
            
            var remotePath = Path.Combine(
                path1: context.FireTimeUtc.DateTime.ToString(format: "yyyyMMddHHmm"),
                path2: "get",
                path3: "9400ZZSYMID1.json");
            
            await blobService.GetBlobContainerClient(blobContainerName: "search")
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
                path2: "9400ZZSYMID1.json");
            
            await File.WriteAllTextAsync(
                path: localPath,
                contents: JsonSerializer.Serialize(value: mapper.Map<List<WorkerStopPoint>>(source: serviceResults)));
            
            remotePath = Path.Combine(
                path1: context.FireTimeUtc.DateTime.ToString(format: "yyyyMMddHHmm"),
                path2: "set",
                path3: "9400ZZSYMID1.json");
            
            await blobService.GetBlobContainerClient(blobContainerName: "search")
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
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

public class _9400ZZSYMHI2(
    BlobServiceClient blobService,
    NpgsqlDataSource dataSource,
    ElasticsearchClient searchService,
    ILogger<_9400ZZSYMHI2> logger,
    IMapper mapper) : IJob {
    
    public async Task Execute(IJobExecutionContext context)
    {
        var storage = Directory.CreateDirectory(path: Path.Combine(
            path1: Path.GetTempPath(),
            path2: Guid.NewGuid().ToString()));
        
        try
        {
            #region Get Search Feed
            
            var searchFeed = await searchService.GetAsync<SearchStop>(
                id: "9400ZZSYMHI2",
                index: "search");
            
            List<SearchStopPoint> mappedResults = [];
            
            if (searchFeed.Source is not null)
            {
                mappedResults = searchFeed.Source.Points ?? [];
            }
            
            #endregion
            
            #region Check Search Feed
            
            if (mappedResults.ElementAtOrDefault(index: 0) is not null &&
                mappedResults.ElementAt(index: 0).DepartureDateTime > DateTime.Now) {
                
                return;
            }
            
            #endregion
            
            #region Get Database Feed
            
            var databaseFeed = await Feed.Load(dataStorage: PostgresStorage.Load(dataSource: dataSource));
            
            var stopResults = await databaseFeed.GetStopsByIdAsync(
                id: "9400ZZSYMHI2",
                comparison: ComparisonType.Exact);
            
            var serviceResults = await databaseFeed.GetServicesByStopAsync(
                id: "9400ZZSYMHI2",
                comparison: ComparisonType.Exact,
                tolerance: TimeSpan.FromMinutes(value: 119));
            
            var databaseResults = mapper.Map<List<SearchStop>>(source: stopResults).FirstOrDefault() ?? new SearchStop();
            databaseResults.Points = mapper.Map<List<SearchStopPoint>>(source: serviceResults) ?? [];
            
            #endregion
            
            #region Check Database Feed
            
            if (databaseResults is { Latitude: not null, Longitude: not null })
            {
                databaseResults.Location = GeoLocation.LatitudeLongitude(latitudeLongitude: new LatLonGeoLocation 
                {
                    Lat = databaseResults.Latitude.Value,
                    Lon = databaseResults.Longitude.Value
                });
            }
            
            #endregion
            
            #region Set Search Feed
            
            await searchService.IndexAsync(
                document: databaseResults,
                index: "search");
            
            #endregion
            
            #region Build Search Results
            
            var localPath = Path.Combine(
                path1: storage.FullName,
                path2: "9400ZZSYMHI2.json");
            
            await File.WriteAllTextAsync(
                path: localPath,
                contents: JsonSerializer.Serialize(value: mapper.Map<List<WorkerStopPoint>>(source: mappedResults)));
            
            var remotePath = Path.Combine(
                path1: context.FireTimeUtc.DateTime.ToString(format: "yyyyMMddHHmm"),
                path2: "get",
                path3: "9400ZZSYMHI2.json");
            
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
                path2: "9400ZZSYMHI2.json");
            
            await File.WriteAllTextAsync(
                path: localPath,
                contents: JsonSerializer.Serialize(value: mapper.Map<List<WorkerStopPoint>>(source: serviceResults)));
            
            remotePath = Path.Combine(
                path1: context.FireTimeUtc.DateTime.ToString(format: "yyyyMMddHHmm"),
                path2: "set",
                path3: "9400ZZSYMHI2.json");
            
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
            
            #region Delete Expired Blobs
            
            var expiredBlobs = blobService.GetBlobContainerClient(blobContainerName: "search")
                .GetBlobsAsync(prefix: "9400ZZSYMHI2");
            
            await foreach (var item in expiredBlobs)
            {
                if (item.Properties.LastModified < context.FireTimeUtc.DateTime.AddDays(value: -7))
                    await blobService.GetBlobContainerClient(blobContainerName: "search")
                        .GetBlobClient(blobName: item.Name)
                        .DeleteAsync();
            }
            
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
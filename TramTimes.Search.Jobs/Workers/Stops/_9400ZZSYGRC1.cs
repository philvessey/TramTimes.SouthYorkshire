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

namespace TramTimes.Search.Jobs.Workers.Stops;

public class _9400ZZSYGRC1(
    BlobContainerClient blobService,
    NpgsqlDataSource dataSource,
    ElasticsearchClient searchService,
    ILogger<_9400ZZSYGRC1> logger,
    IMapper mapper) : IJob {
    
    public async Task Execute(IJobExecutionContext context)
    {
        var guid = Guid.NewGuid();
        
        var storage = Directory.CreateDirectory(path: Path.Combine(
            path1: Path.GetTempPath(),
            path2: guid.ToString()));
        
        try
        {
            #region get search feed
            
            var searchFeed = await searchService.GetAsync<SearchStop>(
                id: "9400ZZSYGRC1",
                index: "search");
            
            List<SearchStopPoint> mappedResults = [];
            
            if (searchFeed.Source is not null)
                mappedResults = searchFeed.Source.Points ?? [];
            
            #endregion
            
            #region check search feed
            
            if (mappedResults.ElementAtOrDefault(index: 0)?.DepartureDateTime > DateTime.Now)
                return;
            
            #endregion
            
            #region get database feed
            
            var databaseFeed = await Feed.LoadAsync(dataStorage: PostgresStorage.Load(dataSource: dataSource));
            
            var stopResults = await databaseFeed.GetStopsByIdAsync(
                id: "9400ZZSYGRC1",
                comparison: ComparisonType.Exact);
            
            var serviceResults = await databaseFeed.GetServicesByStopAsync(
                id: "9400ZZSYGRC1",
                comparison: ComparisonType.Exact,
                tolerance: TimeSpan.FromMinutes(value: 119));
            
            var databaseResults = mapper.Map<List<SearchStop>>(source: stopResults).FirstOrDefault() ?? new SearchStop();
            databaseResults.Points = mapper.Map<List<SearchStopPoint>>(source: serviceResults) ?? [];
            
            #endregion
            
            #region check database feed
            
            if (databaseResults is { Latitude: not null, Longitude: not null })
                databaseResults.Location = GeoLocation.LatitudeLongitude(latitudeLongitude: new LatLonGeoLocation 
                {
                    Lat = databaseResults.Latitude.Value,
                    Lon = databaseResults.Longitude.Value
                });
            
            #endregion
            
            #region set search feed
            
            await searchService.IndexAsync(
                document: databaseResults,
                index: "search");
            
            #endregion
            
            #region build search results
            
            var localPath = Path.Combine(
                path1: storage.FullName,
                path2: "9400ZZSYGRC1.json");
            
            await File.WriteAllTextAsync(
                path: localPath,
                contents: JsonSerializer.Serialize(value: mapper.Map<List<WorkerStopPoint>>(source: mappedResults)));
            
            var remotePath = Path.Combine(
                path1: context.FireTimeUtc.DateTime.ToString(format: "yyyyMMddHHmm"),
                path2: "get",
                path3: "9400ZZSYGRC1.json");
            
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
                path2: "9400ZZSYGRC1.json");
            
            await File.WriteAllTextAsync(
                path: localPath,
                contents: JsonSerializer.Serialize(value: mapper.Map<List<WorkerStopPoint>>(source: serviceResults)));
            
            remotePath = Path.Combine(
                path1: context.FireTimeUtc.DateTime.ToString(format: "yyyyMMddHHmm"),
                path2: "set",
                path3: "9400ZZSYGRC1.json");
            
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
            
            var expiredBlobs = blobService.GetBlobsAsync(prefix: "9400ZZSYGRC1");
            
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
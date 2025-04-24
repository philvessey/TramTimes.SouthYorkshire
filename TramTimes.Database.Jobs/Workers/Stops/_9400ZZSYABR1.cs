using System.Text.Json;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using NextDepartures.Standard;
using NextDepartures.Standard.Types;
using NextDepartures.Storage.GTFS;
using NextDepartures.Storage.Postgres.Aspire;
using Npgsql;
using Quartz;
using TramTimes.Database.Jobs.Models;

namespace TramTimes.Database.Jobs.Workers.Stops;

public class _9400ZZSYABR1(
    BlobServiceClient blobService,
    NpgsqlDataSource dataSource,
    ILogger<_9400ZZSYABR1> logger) : IJob {
    
    public async Task Execute(IJobExecutionContext context)
    {
        var storage = Directory.CreateDirectory(path: Path.Combine(
            path1: Path.GetTempPath(),
            path2: Guid.NewGuid().ToString()));
        
        try
        {
            #region Get Active Blobs
            
            var activeBlobs = blobService.GetBlobContainerClient(blobContainerName: "database")
                .GetBlobsAsync(prefix: context.FireTimeUtc.Date.ToString(format: "yyyyMMdd") + "/gtfs/");
            
            await foreach (var blob in activeBlobs)
            {
                await blobService.GetBlobContainerClient(blobContainerName: "database")
                    .GetBlobClient(blob.Name)
                    .DownloadToAsync(path: Path.Combine(
                        path1: storage.FullName,
                        path2: Path.GetFileName(path: blob.Name)));
            }
            
            #endregion
            
            #region Get Stop Schedule
            
            var activeSchedule = JsonSerializer.Deserialize<WorkerSchedule>(json: await File.ReadAllTextAsync(path: Path.Combine(
                path1: "Workers",
                path2: "Schedules",
                path3: "_9400ZZSYABR1.json")));
            
            #endregion
            
            #region Get Database Feed
            
            var databaseFeed = await Feed.Load(dataStorage: PostgresStorage.Load(dataSource: dataSource));
            
            var databaseResults = await databaseFeed.GetServicesByStopAsync(
                id: "9400ZZSYABR1",
                target: context.FireTimeUtc.Date,
                offset: TimeSpan.FromHours(value: 12),
                comparison: ComparisonType.Exact,
                tolerance: TimeSpan.FromMinutes(value: 59));
            
            #endregion
            
            #region Get Storage Feed
            
            var storageFeed = await Feed.Load(dataStorage: GtfsStorage.Load(path: storage.FullName));
            
            var storageResults = await storageFeed.GetServicesByStopAsync(
                id: "9400ZZSYABR1",
                target: context.FireTimeUtc.Date,
                offset: TimeSpan.FromHours(value: 12),
                comparison: ComparisonType.Exact,
                tolerance: TimeSpan.FromMinutes(value: 59));
            
            #endregion
            
            #region Check Database Feed
            
            switch (context.FireTimeUtc.Date.DayOfWeek)
            {
                case DayOfWeek.Monday:
                {
                    if (databaseResults.Count != activeSchedule?.Monday?.Count)
                        logger.LogWarning(message: "9400ZZSYABR1: Database service count does not match schedule");
                    
                    break;
                }
                case DayOfWeek.Tuesday:
                {
                    if (databaseResults.Count != activeSchedule?.Tuesday?.Count)
                        logger.LogWarning(message: "9400ZZSYABR1: Database service count does not match schedule");
                    
                    break;
                }
                case DayOfWeek.Wednesday:
                {
                    if (databaseResults.Count != activeSchedule?.Wednesday?.Count)
                        logger.LogWarning(message: "9400ZZSYABR1: Database service count does not match schedule");
                    
                    break;
                }
                case DayOfWeek.Thursday:
                {
                    if (databaseResults.Count != activeSchedule?.Thursday?.Count)
                        logger.LogWarning(message: "9400ZZSYABR1: Database service count does not match schedule");
                    
                    break;
                }
                case DayOfWeek.Friday:
                {
                    if (databaseResults.Count != activeSchedule?.Friday?.Count)
                        logger.LogWarning(message: "9400ZZSYABR1: Database service count does not match schedule");
                    
                    break;
                }
                case DayOfWeek.Saturday:
                {
                    if (databaseResults.Count != activeSchedule?.Saturday?.Count)
                        logger.LogWarning(message: "9400ZZSYABR1: Database service count does not match schedule");
                    
                    break;
                }
                case DayOfWeek.Sunday:
                {
                    if (databaseResults.Count != activeSchedule?.Sunday?.Count)
                        logger.LogWarning(message: "9400ZZSYABR1: Database service count does not match schedule");
                    
                    break;
                }
                default:
                    logger.LogWarning(message: "9400ZZSYABR1: Database service count does not match schedule");
                    
                    break;
            }
            
            #endregion
            
            #region Check Storage Feed
            
            switch (context.FireTimeUtc.Date.DayOfWeek)
            {
                case DayOfWeek.Monday:
                {
                    if (storageResults.Count != activeSchedule?.Monday?.Count)
                        logger.LogWarning(message: "9400ZZSYABR1: Storage service count does not match schedule");
                    
                    break;
                }
                case DayOfWeek.Tuesday:
                {
                    if (storageResults.Count != activeSchedule?.Tuesday?.Count)
                        logger.LogWarning(message: "9400ZZSYABR1: Storage service count does not match schedule");
                    
                    break;
                }
                case DayOfWeek.Wednesday:
                {
                    if (storageResults.Count != activeSchedule?.Wednesday?.Count)
                        logger.LogWarning(message: "9400ZZSYABR1: Storage service count does not match schedule");
                    
                    break;
                }
                case DayOfWeek.Thursday:
                {
                    if (storageResults.Count != activeSchedule?.Thursday?.Count)
                        logger.LogWarning(message: "9400ZZSYABR1: Storage service count does not match schedule");
                    
                    break;
                }
                case DayOfWeek.Friday:
                {
                    if (storageResults.Count != activeSchedule?.Friday?.Count)
                        logger.LogWarning(message: "9400ZZSYABR1: Storage service count does not match schedule");
                    
                    break;
                }
                case DayOfWeek.Saturday:
                {
                    if (storageResults.Count != activeSchedule?.Saturday?.Count)
                        logger.LogWarning(message: "9400ZZSYABR1: Storage service count does not match schedule");
                    
                    break;
                }
                case DayOfWeek.Sunday:
                {
                    if (storageResults.Count != activeSchedule?.Sunday?.Count)
                        logger.LogWarning(message: "9400ZZSYABR1: Storage service count does not match schedule");
                    
                    break;
                }
                default:
                    logger.LogWarning(message: "9400ZZSYABR1: Storage service count does not match schedule");
                    
                    break;
            }
            
            #endregion
            
            #region Process Test Results
            
            List<WorkerStopPoint> testResults = [];
            
            switch (context.FireTimeUtc.Date.DayOfWeek)
            {
                case DayOfWeek.Monday:
                {
                    for (var i = 0; i < activeSchedule?.Monday?.Count; i++)
                    {
                        var value = activeSchedule.Monday.ElementAtOrDefault(index: i) ?? new WorkerStopPoint
                        {
                            DepartureTime = "unknown",
                            RouteName = "unknown"
                        };
                        
                        if (databaseResults.ElementAtOrDefault(index: i) is null ||
                            storageResults.ElementAtOrDefault(index: i) is null ||
                            !databaseResults.ElementAt(index: i).DepartureDateTime.ToString(format: "HH:mm").Equals(value: value.DepartureTime) ||
                            !storageResults.ElementAt(index: i).DepartureDateTime.ToString(format: "HH:mm").Equals(value: value.DepartureTime) ||
                            !databaseResults.ElementAt(index: i).RouteName.Equals(value: value.RouteName) ||
                            !storageResults.ElementAt(index: i).RouteName.Equals(value: value.RouteName)) {
                            
                            testResults.Add(item: value);
                        }
                    }
                    
                    break;
                }
                case DayOfWeek.Tuesday:
                {
                    for (var i = 0; i < activeSchedule?.Tuesday?.Count; i++)
                    {
                        var value = activeSchedule.Tuesday.ElementAtOrDefault(index: i) ?? new WorkerStopPoint
                        {
                            DepartureTime = "unknown",
                            RouteName = "unknown"
                        };
                        
                        if (databaseResults.ElementAtOrDefault(index: i) is null ||
                            storageResults.ElementAtOrDefault(index: i) is null ||
                            !databaseResults.ElementAt(index: i).DepartureDateTime.ToString(format: "HH:mm").Equals(value: value.DepartureTime) ||
                            !storageResults.ElementAt(index: i).DepartureDateTime.ToString(format: "HH:mm").Equals(value: value.DepartureTime) ||
                            !databaseResults.ElementAt(index: i).RouteName.Equals(value: value.RouteName) ||
                            !storageResults.ElementAt(index: i).RouteName.Equals(value: value.RouteName)) {
                            
                            testResults.Add(item: value);
                        }
                    }
                    
                    break;
                }
                case DayOfWeek.Wednesday:
                {
                    for (var i = 0; i < activeSchedule?.Wednesday?.Count; i++)
                    {
                        var value = activeSchedule.Wednesday.ElementAtOrDefault(index: i) ?? new WorkerStopPoint
                        {
                            DepartureTime = "unknown",
                            RouteName = "unknown"
                        };
                        
                        if (databaseResults.ElementAtOrDefault(index: i) is null ||
                            storageResults.ElementAtOrDefault(index: i) is null ||
                            !databaseResults.ElementAt(index: i).DepartureDateTime.ToString(format: "HH:mm").Equals(value: value.DepartureTime) ||
                            !storageResults.ElementAt(index: i).DepartureDateTime.ToString(format: "HH:mm").Equals(value: value.DepartureTime) ||
                            !databaseResults.ElementAt(index: i).RouteName.Equals(value: value.RouteName) ||
                            !storageResults.ElementAt(index: i).RouteName.Equals(value: value.RouteName)) {
                            
                            testResults.Add(item: value);
                        }
                    }
                    
                    break;
                }
                case DayOfWeek.Thursday:
                {
                    for (var i = 0; i < activeSchedule?.Thursday?.Count; i++)
                    {
                        var value = activeSchedule.Thursday.ElementAtOrDefault(index: i) ?? new WorkerStopPoint
                        {
                            DepartureTime = "unknown",
                            RouteName = "unknown"
                        };
                        
                        if (databaseResults.ElementAtOrDefault(index: i) is null ||
                            storageResults.ElementAtOrDefault(index: i) is null ||
                            !databaseResults.ElementAt(index: i).DepartureDateTime.ToString(format: "HH:mm").Equals(value: value.DepartureTime) ||
                            !storageResults.ElementAt(index: i).DepartureDateTime.ToString(format: "HH:mm").Equals(value: value.DepartureTime) ||
                            !databaseResults.ElementAt(index: i).RouteName.Equals(value: value.RouteName) ||
                            !storageResults.ElementAt(index: i).RouteName.Equals(value: value.RouteName)) {
                            
                            testResults.Add(item: value);
                        }
                    }
                    
                    break;
                }
                case DayOfWeek.Friday:
                {
                    for (var i = 0; i < activeSchedule?.Friday?.Count; i++)
                    {
                        var value = activeSchedule.Friday.ElementAtOrDefault(index: i) ?? new WorkerStopPoint
                        {
                            DepartureTime = "unknown",
                            RouteName = "unknown"
                        };
                        
                        if (databaseResults.ElementAtOrDefault(index: i) is null ||
                            storageResults.ElementAtOrDefault(index: i) is null ||
                            !databaseResults.ElementAt(index: i).DepartureDateTime.ToString(format: "HH:mm").Equals(value: value.DepartureTime) ||
                            !storageResults.ElementAt(index: i).DepartureDateTime.ToString(format: "HH:mm").Equals(value: value.DepartureTime) ||
                            !databaseResults.ElementAt(index: i).RouteName.Equals(value: value.RouteName) ||
                            !storageResults.ElementAt(index: i).RouteName.Equals(value: value.RouteName)) {
                            
                            testResults.Add(item: value);
                        }
                    }
                    
                    break;
                }
                case DayOfWeek.Saturday:
                {
                    for (var i = 0; i < activeSchedule?.Saturday?.Count; i++)
                    {
                        var value = activeSchedule.Saturday.ElementAtOrDefault(index: i) ?? new WorkerStopPoint
                        {
                            DepartureTime = "unknown",
                            RouteName = "unknown"
                        };
                        
                        if (databaseResults.ElementAtOrDefault(index: i) is null ||
                            storageResults.ElementAtOrDefault(index: i) is null ||
                            !databaseResults.ElementAt(index: i).DepartureDateTime.ToString(format: "HH:mm").Equals(value: value.DepartureTime) ||
                            !storageResults.ElementAt(index: i).DepartureDateTime.ToString(format: "HH:mm").Equals(value: value.DepartureTime) ||
                            !databaseResults.ElementAt(index: i).RouteName.Equals(value: value.RouteName) ||
                            !storageResults.ElementAt(index: i).RouteName.Equals(value: value.RouteName)) {
                            
                            testResults.Add(item: value);
                        }
                    }
                    
                    break;
                }
                case DayOfWeek.Sunday:
                {
                    for (var i = 0; i < activeSchedule?.Sunday?.Count; i++)
                    {
                        var value = activeSchedule.Sunday.ElementAtOrDefault(index: i) ?? new WorkerStopPoint
                        {
                            DepartureTime = "unknown",
                            RouteName = "unknown"
                        };
                        
                        if (databaseResults.ElementAtOrDefault(index: i) is null ||
                            storageResults.ElementAtOrDefault(index: i) is null ||
                            !databaseResults.ElementAt(index: i).DepartureDateTime.ToString(format: "HH:mm").Equals(value: value.DepartureTime) ||
                            !storageResults.ElementAt(index: i).DepartureDateTime.ToString(format: "HH:mm").Equals(value: value.DepartureTime) ||
                            !databaseResults.ElementAt(index: i).RouteName.Equals(value: value.RouteName) ||
                            !storageResults.ElementAt(index: i).RouteName.Equals(value: value.RouteName)) {
                            
                            testResults.Add(item: value);
                        }
                    }
                    
                    break;
                }
                default:
                    logger.LogWarning(message: "9400ZZSYABR1: Service not found in schedule");
                    
                    break;
            }
            
            #endregion
            
            #region Build Database Results
            
            var localPath = Path.Combine(
                path1: storage.FullName,
                path2: "9400ZZSYABR1.json");
            
            await File.WriteAllTextAsync(
                path: localPath,
                contents: JsonSerializer.Serialize(value: databaseResults));
            
            var remotePath = Path.Combine(
                path1: context.FireTimeUtc.Date.ToString(format: "yyyyMMdd"),
                path2: "record",
                path3: "9400ZZSYABR1.json");
            
            await blobService.GetBlobContainerClient(blobContainerName: "database")
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
            
            #region Build Storage Results
            
            localPath = Path.Combine(
                path1: storage.FullName,
                path2: "9400ZZSYABR1.json");
            
            await File.WriteAllTextAsync(
                path: localPath,
                contents: JsonSerializer.Serialize(value: storageResults));
            
            remotePath = Path.Combine(
                path1: context.FireTimeUtc.Date.ToString(format: "yyyyMMdd"),
                path2: "service",
                path3: "9400ZZSYABR1.json");
            
            await blobService.GetBlobContainerClient(blobContainerName: "database")
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
            
            #region Build Test Results
            
            localPath = Path.Combine(
                path1: storage.FullName,
                path2: "9400ZZSYABR1.json");
            
            await File.WriteAllTextAsync(
                path: localPath,
                contents: JsonSerializer.Serialize(value: testResults));
            
            remotePath = Path.Combine(
                path1: context.FireTimeUtc.Date.ToString(format: "yyyyMMdd"),
                path2: "test",
                path3: "9400ZZSYABR1.json");
            
            await blobService.GetBlobContainerClient(blobContainerName: "database")
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
            
            #region Delete Job
            
            await context.Scheduler.DeleteJob(jobKey: context.JobDetail.Key);
            
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
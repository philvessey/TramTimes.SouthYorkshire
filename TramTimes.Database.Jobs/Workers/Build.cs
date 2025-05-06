using System.IO.Compression;
using System.Xml.Serialization;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Flurl.Http;
using Npgsql;
using Quartz;
using TramTimes.Database.Jobs.Builders;
using TramTimes.Database.Jobs.Extensions;
using TramTimes.Database.Jobs.Models;
using TramTimes.Database.Jobs.Tools;

namespace TramTimes.Database.Jobs.Workers;

public class Build(
    BlobServiceClient blobService,
    NpgsqlDataSource dataSource,
    ILogger<Build> logger) : IJob {
    
    private const string Localities = "https://naptan.api.dft.gov.uk/v1/nptg/localities";
    private const string Stops = "https://naptan.api.dft.gov.uk/v1/access-nodes?dataFormat=csv&atcoAreaCodes=370%2C940";
    
    public async Task Execute(IJobExecutionContext context)
    {
        var storage = Directory.CreateDirectory(path: Path.Combine(
            path1: Path.GetTempPath(),
            path2: Guid.NewGuid().ToString()));
        
        try
        {
            #region Delete Expired Blobs
            
            var expiredBlobs = blobService.GetBlobContainerClient(blobContainerName: "database")
                .GetBlobsAsync();
            
            await foreach (var blob in expiredBlobs)
            {
                if (blob.Properties.LastModified < context.FireTimeUtc.Date.AddDays(value: -28))
                    await blobService.GetBlobContainerClient(blobContainerName: "database")
                        .GetBlobClient(blobName: blob.Name)
                        .DeleteAsync();
            }
            
            #endregion
            
            #region Get Naptan Localities
            
            var remotePath = Path.Combine(
                path1: context.FireTimeUtc.Date.ToString(format: "yyyyMMdd"),
                path2: "raw",
                path3: "localities.csv");
            
            var localPath = Path.Combine(
                path1: storage.FullName,
                path2: "localities.csv");
            
            var blobExists = await blobService.GetBlobContainerClient(blobContainerName: "database")
                .GetBlobClient(blobName: remotePath)
                .ExistsAsync();
            
            if (blobExists)
            {
                await blobService.GetBlobContainerClient(blobContainerName: "database")
                    .GetBlobClient(blobName: remotePath)
                    .DownloadToAsync(path: localPath);
            }
            else
            {
                await Localities.DownloadFileAsync(
                    localFolderPath: storage.FullName,
                    localFileName: "localities.csv");
            }
            
            var localities = await NaptanLocalityTools.GetFromFileAsync(path: localPath);
            
            await blobService.GetBlobContainerClient(blobContainerName: "database")
                .GetBlobClient(blobName: remotePath)
                .UploadAsync(
                    path: localPath,
                    options: new BlobUploadOptions
                    {
                        HttpHeaders = new BlobHttpHeaders
                        {
                            ContentType = "text/csv"
                        }
                    });
            
            #endregion
            
            #region Get Naptan Stops
            
            remotePath = Path.Combine(
                path1: context.FireTimeUtc.Date.ToString(format: "yyyyMMdd"),
                path2: "raw",
                path3: "stops.csv");
            
            localPath = Path.Combine(
                path1: storage.FullName,
                path2: "stops.csv");
            
            blobExists = await blobService.GetBlobContainerClient(blobContainerName: "database")
                .GetBlobClient(blobName: remotePath)
                .ExistsAsync();
            
            if (blobExists)
            {
                await blobService.GetBlobContainerClient(blobContainerName: "database")
                    .GetBlobClient(blobName: remotePath)
                    .DownloadToAsync(path: localPath);
            }
            else
            {
                await Stops.DownloadFileAsync(
                    localFolderPath: storage.FullName,
                    localFileName: "stops.csv");
            }
            
            var stops = await NaptanStopTools.GetFromFileAsync(path: localPath);
            
            await blobService.GetBlobContainerClient(blobContainerName: "database")
                .GetBlobClient(blobName: remotePath)
                .UploadAsync(
                    path: localPath,
                    options: new BlobUploadOptions
                    {
                        HttpHeaders = new BlobHttpHeaders
                        {
                            ContentType = "text/csv"
                        }
                    });
            
            #endregion
            
            #region Get Traveline Data
            
            remotePath = Path.Combine(
                path1: context.FireTimeUtc.Date.ToString(format: "yyyyMMdd"),
                path2: "raw",
                path3: "traveline.zip");
            
            localPath = Path.Combine(
                path1: storage.FullName,
                path2: "traveline.zip");
            
            blobExists = await blobService.GetBlobContainerClient(blobContainerName: "database")
                .GetBlobClient(blobName: remotePath)
                .ExistsAsync();
            
            if (blobExists)
            {
                await blobService.GetBlobContainerClient(blobContainerName: "database")
                    .GetBlobClient(blobName: remotePath)
                    .DownloadToAsync(path: localPath);
            }
            else
            {
                await FtpClientTools.GetFromRemoteAsync(
                    localPath: localPath,
                    remoteFileName: "Y.zip");
            }
            
            await blobService.GetBlobContainerClient(blobContainerName: "database")
                .GetBlobClient(blobName: remotePath)
                .UploadAsync(
                    path: localPath,
                    options: new BlobUploadOptions
                    {
                        HttpHeaders = new BlobHttpHeaders
                        {
                            ContentType = "application/zip"
                        }
                    });
            
            #endregion
            
            #region Process Traveline Data
            
            ZipFile.ExtractToDirectory(
                sourceArchiveFileName: localPath,
                destinationDirectoryName: storage.FullName);
            
            var rawFiles = Directory.GetFiles(path: storage.FullName)
                .Where(predicate: file => file.EndsWith(value: ".xml"))
                .ToArray();
            
            var validFiles = new List<string>();
            var invalidFiles = new List<string>();
            
            foreach (var file in rawFiles)
            {
                var reader = new StreamReader(path: file);
                var contents = await reader.ReadToEndAsync();
                
                if (contents.Contains(value: "ZZSY"))
                {
                    validFiles.Add(item: file);
                }
                else
                {
                    invalidFiles.Add(item: file);
                }
            }
            
            #endregion
            
            #region Process Schedule Data
            
            Dictionary<string, TravelineSchedule> results = [];
            
            foreach (var reader in validFiles.Select(selector: file => new StreamReader(path: file)))
            {
                if (new XmlSerializer(type: typeof(TransXChange)).Deserialize(textReader: reader) is not TransXChange xml)
                    continue;
                
                if (xml.VehicleJourneys?.VehicleJourney is null)
                    continue;
                
                var startDate = await DateTimeTools.GetPeriodStartDateAsync(
                    scheduleDate: context.FireTimeUtc.Date,
                    startDate: xml.Services?.Service?.OperatingPeriod?.StartDate.ToDate());
                
                var endDate = await DateTimeTools.GetPeriodEndDateAsync(
                    scheduleDate: context.FireTimeUtc.Date,
                    endDate: xml.Services?.Service?.OperatingPeriod?.EndDate.ToDate());
                
                if (startDate > endDate)
                    continue;
                
                foreach (var vehicleJourney in xml.VehicleJourneys.VehicleJourney)
                {
                    var calendar = await TravelineCalendarBuilder.BuildAsync(
                        scheduleDate: context.FireTimeUtc.Date,
                        services: xml.Services,
                        vehicleJourney: vehicleJourney,
                        startDate: startDate,
                        endDate: endDate);
                    
                    var journeyPattern = await TransXChangeJourneyPatternTools.GetJourneyPatternAsync(
                        services: xml.Services,
                        reference: vehicleJourney.JourneyPatternRef);
                    
                    var schedule = await TravelineScheduleBuilder.BuildAsync(
                        operators: xml.Operators,
                        services: xml.Services,
                        journeyPattern: journeyPattern,
                        calendar: calendar);
                    
                    var timingLinks = await TransXChangeJourneyPatternTools.GetTimingLinksAsync(
                        patternSections: xml.JourneyPatternSections,
                        references: journeyPattern.JourneyPatternSectionRefs);
                    
                    var departureTime = vehicleJourney.DepartureTime?.ToTime();
                    
                    for (var i = 0; i < timingLinks.Count; i++)
                    {
                        var arrivalTime = departureTime?.Add(ts: await TravelineStopPointTools.GetRunTimeAsync(
                            timingLinks: timingLinks,
                            index: i));
                        
                        departureTime = arrivalTime?.Add(ts: await TravelineStopPointTools.GetWaitTimeAsync(
                            timingLinks: timingLinks,
                            index: i));
                        
                        if (schedule.StopPoints?.Count > 0)
                            schedule.StopPoints.RemoveAt(index: schedule.StopPoints.Count - 1);
                        
                        var stopPoint = await TravelineStopPointBuilder.BuildAsync(
                            localities: localities,
                            stops: stops,
                            stopPoints: xml.StopPoints,
                            reference: timingLinks[i].From?.StopPointRef,
                            activity: i > 0 ? "pickUpAndSetDown" : "pickUp",
                            arrivalTime: arrivalTime,
                            departureTime: departureTime);
                        
                        schedule.StopPoints?.Add(item: stopPoint);
                        
                        stopPoint = await TravelineStopPointBuilder.BuildAsync(
                            localities: localities,
                            stops: stops,
                            stopPoints: xml.StopPoints,
                            reference: timingLinks[i].To?.StopPointRef,
                            activity: i < timingLinks.Count - 1 ? "pickUpAndSetDown" : "setDown",
                            arrivalTime: arrivalTime,
                            departureTime: departureTime);
                        
                        schedule.StopPoints?.Add(item: stopPoint);
                    }
                    
                    var duplicate = await TravelineScheduleTools.GetDuplicateMatchAsync(
                        schedules: results,
                        stopPoints: schedule.StopPoints,
                        runningDates: schedule.Calendar?.RunningDates,
                        supplementRunningDates: schedule.Calendar?.SupplementRunningDates,
                        supplementNonRunningDates: schedule.Calendar?.SupplementNonRunningDates,
                        direction: schedule.Direction,
                        line: schedule.Line);
                    
                    if (duplicate)
                        continue;
                    
                    results.TryAdd(schedule.Id ?? "unknown", schedule);
                }
            }
            
            #endregion
            
            #region Build Database Data
            
            const string sql = "create index gtfs_stop_times_idx on gtfs_stop_times (" +
                               "trip_id, " +
                               "stop_id, " +
                               "pickup_type, " +
                               "arrival_time, " +
                               "departure_time, " +
                               "stop_sequence, " +
                               "stop_headsign, " +
                               "drop_off_type, " +
                               "shape_dist_travelled, " +
                               "timepoint)";
            
            var connection = await dataSource.OpenConnectionAsync();
            
            var command = new NpgsqlCommand(cmdText: "drop index if exists gtfs_stop_times_idx", connection: connection);
            await command.ExecuteNonQueryAsync();
            
            var records = await DatabaseAgencyBuilder.BuildAsync(
                schedules: results,
                dataSource: dataSource);
            
            records += await DatabaseCalendarBuilder.BuildAsync(
                schedules: results,
                dataSource: dataSource);
            
            records += await DatabaseCalendarDateBuilder.BuildAsync(
                schedules: results,
                dataSource: dataSource);
            
            records += await DatabaseRouteBuilder.BuildAsync(
                schedules: results,
                dataSource: dataSource);
            
            records += await DatabaseStopBuilder.BuildAsync(
                schedules: results,
                dataSource: dataSource);
            
            records += await DatabaseStopTimeBuilder.BuildAsync(
                schedules: results,
                dataSource: dataSource);
            
            records += await DatabaseTripBuilder.BuildAsync(
                schedules: results,
                dataSource: dataSource);
            
            command = new NpgsqlCommand(cmdText: sql, connection: connection);
            await command.ExecuteNonQueryAsync();
            
            #endregion
            
            #region Build Storage Data
            
            remotePath = Path.Combine(
                path1: context.FireTimeUtc.Date.ToString(format: "yyyyMMdd"),
                path2: "gtfs",
                path3: "agency.txt");
            
            localPath = await GtfsAgencyBuilder.BuildAsync(
                schedules: results,
                path: storage.FullName);
            
            await blobService.GetBlobContainerClient(blobContainerName: "database")
                .GetBlobClient(blobName: remotePath)
                .UploadAsync(
                    path: localPath,
                    options: new BlobUploadOptions
                    {
                        HttpHeaders = new BlobHttpHeaders
                        {
                            ContentType = "text/plain"
                        }
                    });
            
            remotePath = Path.Combine(
                path1: context.FireTimeUtc.Date.ToString(format: "yyyyMMdd"),
                path2: "gtfs",
                path3: "calendar.txt");
            
            localPath = await GtfsCalendarBuilder.BuildAsync(
                schedules: results,
                path: storage.FullName);
            
            await blobService.GetBlobContainerClient(blobContainerName: "database")
                .GetBlobClient(blobName: remotePath)
                .UploadAsync(
                    path: localPath,
                    options: new BlobUploadOptions
                    {
                        HttpHeaders = new BlobHttpHeaders
                        {
                            ContentType = "text/plain"
                        }
                    });
            
            remotePath = Path.Combine(
                path1: context.FireTimeUtc.Date.ToString(format: "yyyyMMdd"),
                path2: "gtfs",
                path3: "calendar_dates.txt");
            
            localPath = await GtfsCalendarDateBuilder.BuildAsync(
                schedules: results,
                path: storage.FullName);
            
            await blobService.GetBlobContainerClient(blobContainerName: "database")
                .GetBlobClient(blobName: remotePath)
                .UploadAsync(
                    path: localPath,
                    options: new BlobUploadOptions
                    {
                        HttpHeaders = new BlobHttpHeaders
                        {
                            ContentType = "text/plain"
                        }
                    });
            
            remotePath = Path.Combine(
                path1: context.FireTimeUtc.Date.ToString(format: "yyyyMMdd"),
                path2: "gtfs",
                path3: "routes.txt");
            
            localPath = await GtfsRouteBuilder.BuildAsync(
                schedules: results,
                path: storage.FullName);
            
            await blobService.GetBlobContainerClient(blobContainerName: "database")
                .GetBlobClient(blobName: remotePath)
                .UploadAsync(
                    path: localPath,
                    options: new BlobUploadOptions
                    {
                        HttpHeaders = new BlobHttpHeaders
                        {
                            ContentType = "text/plain"
                        }
                    });
            
            remotePath = Path.Combine(
                path1: context.FireTimeUtc.Date.ToString(format: "yyyyMMdd"),
                path2: "gtfs",
                path3: "stops.txt");
            
            localPath = await GtfsStopBuilder.BuildAsync(
                schedules: results,
                path: storage.FullName);
            
            await blobService.GetBlobContainerClient(blobContainerName: "database")
                .GetBlobClient(blobName: remotePath)
                .UploadAsync(
                    path: localPath,
                    options: new BlobUploadOptions
                    {
                        HttpHeaders = new BlobHttpHeaders
                        {
                            ContentType = "text/plain"
                        }
                    });
            
            remotePath = Path.Combine(
                path1: context.FireTimeUtc.Date.ToString(format: "yyyyMMdd"),
                path2: "gtfs",
                path3: "stop_times.txt");
            
            localPath = await GtfsStopTimeBuilder.BuildAsync(
                schedules: results,
                path: storage.FullName);
            
            await blobService.GetBlobContainerClient(blobContainerName: "database")
                .GetBlobClient(blobName: remotePath)
                .UploadAsync(
                    path: localPath,
                    options: new BlobUploadOptions
                    {
                        HttpHeaders = new BlobHttpHeaders
                        {
                            ContentType = "text/plain"
                        }
                    });
            
            remotePath = Path.Combine(
                path1: context.FireTimeUtc.Date.ToString(format: "yyyyMMdd"),
                path2: "gtfs",
                path3: "trips.txt");
            
            localPath = await GtfsTripBuilder.BuildAsync(
                schedules: results,
                path: storage.FullName);
            
            await blobService.GetBlobContainerClient(blobContainerName: "database")
                .GetBlobClient(blobName: remotePath)
                .UploadAsync(
                    path: localPath,
                    options: new BlobUploadOptions
                    {
                        HttpHeaders = new BlobHttpHeaders
                        {
                            ContentType = "text/plain"
                        }
                    });
            
            #endregion
            
            #region Output Log Messages
            
            logger.LogInformation(
                message: "READ: {count} naptan localities",
                args: localities.Count);
            
            logger.LogInformation(
                message: "READ: {count} naptan stops",
                args: stops.Count);
            
            logger.LogInformation(
                message: "READ: {count} transxchange files",
                args: rawFiles.Length);
            
            logger.LogWarning(
                message: "READ: {count} transxchange files invalid",
                args: invalidFiles.Count);
            
            logger.LogInformation(
                message: "READ: {count} transxchange files valid",
                args: validFiles.Count);
            
            logger.LogInformation(
                message: "READ: {count} transxchange schedules",
                args: results.Count);
            
            logger.LogInformation(
                message: "WRITE: {count} database records",
                args: records);
            
            #endregion
            
            #region Schedule Test Job
            
            var jobDetail = JobBuilder.Create<Test>()
                .Build();
            
            var trigger = TriggerBuilder.Create()
                .StartNow()
                .Build();
            
            await context.Scheduler.ScheduleJob(
                jobDetail: jobDetail,
                trigger: trigger);
            
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
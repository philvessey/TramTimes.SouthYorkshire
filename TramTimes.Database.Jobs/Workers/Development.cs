using System.IO.Compression;
using System.Xml.Serialization;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using FluentFTP;
using Flurl.Http;
using Npgsql;
using Quartz;
using TramTimes.Database.Jobs.Builders;
using TramTimes.Database.Jobs.Extensions;
using TramTimes.Database.Jobs.Models;
using TramTimes.Database.Jobs.Tools;

namespace TramTimes.Database.Jobs.Workers;

public class Development(
    BlobContainerClient containerClient,
    NpgsqlDataSource dataSource,
    ILogger<Development> logger) : IJob {
    
    private const string Holidays = "https://date.nager.at/api/v3/NextPublicHolidays/gb";
    private const string Localities = "https://naptan.api.dft.gov.uk/v1/nptg/localities";
    private const string Stops = "https://naptan.api.dft.gov.uk/v1/access-nodes?dataFormat=csv&atcoAreaCodes=370%2C940";
    
    public async Task Execute(IJobExecutionContext context)
    {
        var guid = Guid.NewGuid();
        
        var storage = Directory.CreateDirectory(path: Path.Combine(
            path1: Path.GetTempPath(),
            path2: guid.ToString()));
        
        try
        {
            #region get public holidays
            
            var holidays = await Holidays.GetJsonAsync<List<Holiday>>() ?? [];
            
            #endregion
            
            #region output log messages
            
            logger.LogInformation(
                message: "READ: {count} public holidays",
                args: holidays.Count);
            
            #endregion
            
            #region get naptan localities
            
            var remotePath = Path.Combine(
                path1: "input",
                path2: "localities.csv");
            
            var remoteExists = await containerClient
                .GetBlobClient(blobName: remotePath)
                .ExistsAsync();
            
            if (remoteExists)
                return;
            
            var localPath = await Localities.DownloadFileAsync(
                localFolderPath: storage.FullName,
                localFileName: "localities.csv");
            
            if (localPath is null)
                return;
            
            var localities = NaptanLocalityTools.GetFromFile(path: localPath);
            
            await containerClient
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
            
            #region output log messages
            
            logger.LogInformation(
                message: "READ: {count} naptan localities",
                args: localities.Count);
            
            #endregion
            
            #region get naptan stops
            
            remotePath = Path.Combine(
                path1: "input",
                path2: "stops.csv");
            
            remoteExists = await containerClient
                .GetBlobClient(blobName: remotePath)
                .ExistsAsync();
            
            if (remoteExists)
                return;
            
            localPath = await Stops.DownloadFileAsync(
                localFolderPath: storage.FullName,
                localFileName: "stops.csv");
            
            if (localPath is null)
                return;
            
            var stops = NaptanStopTools.GetFromFile(path: localPath);
            
            await containerClient
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
            
            #region output log messages
            
            logger.LogInformation(
                message: "READ: {count} naptan stops",
                args: stops.Count);
            
            #endregion
            
            #region get traveline data
            
            remotePath = Path.Combine(
                path1: "input",
                path2: "traveline.zip");
            
            remoteExists = await containerClient
                .GetBlobClient(blobName: remotePath)
                .ExistsAsync();
            
            if (remoteExists)
                return;
            
            var status = await FtpClientTools.GetFromRemoteAsync(
                localPath: Path.Combine(
                    path1: storage.FullName,
                    path2: "traveline.zip"),
                remoteFileName: "Y.zip");
            
            if (status is not FtpStatus.Success)
                return;
            
            await containerClient
                .GetBlobClient(blobName: remotePath)
                .UploadAsync(
                    path: Path.Combine(
                        path1: storage.FullName,
                        path2: "traveline.zip"),
                    options: new BlobUploadOptions
                    {
                        HttpHeaders = new BlobHttpHeaders
                        {
                            ContentType = "application/zip"
                        }
                    });
            
            #endregion
            
            #region process traveline data
            
            ZipFile.ExtractToDirectory(
                sourceArchiveFileName: Path.Combine(
                    path1: storage.FullName,
                    path2: "traveline.zip"),
                destinationDirectoryName: storage.FullName);
            
            var rawFiles = Directory
                .GetFiles(path: storage.FullName)
                .Where(predicate: file => file.EndsWith(value: ".xml"))
                .ToArray();
            
            var validFiles = new List<string>();
            var invalidFiles = new List<string>();
            
            foreach (var item in rawFiles)
            {
                var reader = new StreamReader(path: item);
                var contents = await reader.ReadToEndAsync();
                
                if (contents.Contains(value: "ZZSY"))
                    validFiles.Add(item: item);
            }
            
            foreach (var item in rawFiles)
            {
                var reader = new StreamReader(path: item);
                var contents = await reader.ReadToEndAsync();
                
                if (!contents.Contains(value: "ZZSY"))
                    invalidFiles.Add(item: item);
            }
            
            #endregion
            
            #region output log messages
            
            logger.LogInformation(
                message: "READ: {count} transxchange files",
                args: rawFiles.Length);
            
            logger.LogInformation(
                message: "READ: {count} transxchange files valid",
                args: validFiles.Count);
            
            logger.LogInformation(
                message: "READ: {count} transxchange files invalid",
                args: invalidFiles.Count);
            
            #endregion
            
            #region process schedule data
            
            Dictionary<string, TravelineSchedule> results = [];
            
            foreach (var reader in validFiles.Select(selector: file => new StreamReader(path: file)))
            {
                if (new XmlSerializer(type: typeof(TransXChange)).Deserialize(textReader: reader) is not TransXChange xml)
                    continue;
                
                if (xml.VehicleJourneys?.VehicleJourney is null)
                    continue;
                
                var startDate = DateOnlyTools.GetPeriodStartDate(
                    scheduleDate: DateOnly.FromDateTime(dateTime: context.FireTimeUtc.Date),
                    startDate: xml.Services?.Service?.OperatingPeriod?.StartDate.ToDate());
                
                var endDate = DateOnlyTools.GetPeriodEndDate(
                    scheduleDate: DateOnly.FromDateTime(dateTime: context.FireTimeUtc.Date),
                    endDate: xml.Services?.Service?.OperatingPeriod?.EndDate.ToDate());
                
                if (startDate > endDate)
                    continue;
                
                foreach (var item in xml.VehicleJourneys.VehicleJourney)
                {
                    var calendar = TravelineCalendarBuilder.Build(
                        scheduleDate: DateOnly.FromDateTime(dateTime: context.FireTimeUtc.Date),
                        holidays: holidays,
                        services: xml.Services,
                        vehicleJourney: item,
                        startDate: startDate,
                        endDate: endDate);
                    
                    var journeyPattern = TransXChangeJourneyPatternTools.GetJourneyPattern(
                        services: xml.Services,
                        reference: item.JourneyPatternRef);
                    
                    var schedule = TravelineScheduleBuilder.Build(
                        operators: xml.Operators,
                        services: xml.Services,
                        journeyPattern: journeyPattern,
                        calendar: calendar);
                    
                    var timingLinks = TransXChangeJourneyPatternTools.GetTimingLinks(
                        patternSections: xml.JourneyPatternSections,
                        references: journeyPattern.JourneyPatternSectionRefs);
                    
                    var departureTime = item.DepartureTime?.ToTime();
                    
                    for (var i = 0; i < timingLinks.Count; i++)
                    {
                        var arrivalTime = departureTime?.Add(ts: TravelineStopPointTools.GetRunTime(
                            timingLinks: timingLinks,
                            index: i));
                        
                        departureTime = arrivalTime?.Add(ts: TravelineStopPointTools.GetWaitTime(
                            timingLinks: timingLinks,
                            index: i));
                        
                        if (schedule.StopPoints?.Count > 0)
                            schedule.StopPoints.RemoveAt(index: schedule.StopPoints.Count - 1);
                        
                        var stopPoint = TravelineStopPointBuilder.Build(
                            localities: localities,
                            stops: stops,
                            stopPoints: xml.StopPoints,
                            reference: timingLinks.ElementAt(index: i).From?.StopPointRef,
                            activity: i > 0
                                ? "pickUpAndSetDown"
                                : "pickUp",
                            arrivalTime: arrivalTime,
                            departureTime: departureTime);
                        
                        schedule.StopPoints?.Add(item: stopPoint);
                        
                        stopPoint = TravelineStopPointBuilder.Build(
                            localities: localities,
                            stops: stops,
                            stopPoints: xml.StopPoints,
                            reference: timingLinks.ElementAt(index: i).To?.StopPointRef,
                            activity: i < timingLinks.Count - 1
                                ? "pickUpAndSetDown"
                                : "setDown",
                            arrivalTime: arrivalTime,
                            departureTime: departureTime);
                        
                        schedule.StopPoints?.Add(item: stopPoint);
                    }
                    
                    var duplicate = TravelineScheduleTools.GetDuplicateMatch(
                        schedules: results,
                        stopPoints: schedule.StopPoints,
                        runningDates: schedule.Calendar?.RunningDates,
                        supplementRunningDates: schedule.Calendar?.SupplementRunningDates,
                        supplementNonRunningDates: schedule.Calendar?.SupplementNonRunningDates,
                        direction: schedule.Direction,
                        line: schedule.Line);
                    
                    if (duplicate)
                        continue;
                    
                    results.TryAdd(
                        key: schedule.Id ?? "unknown",
                        value: schedule);
                }
            }
            
            #endregion
            
            #region output log messages
            
            logger.LogInformation(
                message: "READ: {count} transxchange schedules",
                args: results.Count);
            
            #endregion
            
            #region build database data
            
            await using var connection = await dataSource.OpenConnectionAsync();
            await using var transaction = await connection.BeginTransactionAsync();
            
            await using var dropCommand = new NpgsqlCommand(
                cmdText: "drop index if exists gtfs_stop_times_idx",
                connection: connection,
                transaction: transaction);
            
            await dropCommand.ExecuteNonQueryAsync();
            
            var records = await DatabaseAgencyBuilder.BuildAsync(
                schedules: results,
                connection: connection,
                transaction: transaction);
            
            records += await DatabaseCalendarBuilder.BuildAsync(
                schedules: results,
                connection: connection,
                transaction: transaction);
            
            records += await DatabaseCalendarDateBuilder.BuildAsync(
                schedules: results,
                connection: connection,
                transaction: transaction);
            
            records += await DatabaseRouteBuilder.BuildAsync(
                schedules: results,
                connection: connection,
                transaction: transaction);
            
            records += await DatabaseStopBuilder.BuildAsync(
                schedules: results,
                connection: connection,
                transaction: transaction);
            
            records += await DatabaseStopTimeBuilder.BuildAsync(
                schedules: results,
                connection: connection,
                transaction: transaction);
            
            records += await DatabaseTripBuilder.BuildAsync(
                schedules: results,
                connection: connection,
                transaction: transaction);
            
            await using var createCommand = new NpgsqlCommand(
                cmdText: "create index gtfs_stop_times_idx on gtfs_stop_times (" +
                         "trip_id, " +
                         "stop_id, " +
                         "pickup_type, " +
                         "arrival_time, " +
                         "departure_time, " +
                         "stop_sequence, " +
                         "stop_headsign, " +
                         "drop_off_type, " +
                         "shape_dist_travelled, " +
                         "timepoint)",
                connection: connection,
                transaction: transaction);
            
            await createCommand.ExecuteNonQueryAsync();
            await transaction.CommitAsync();
            
            #endregion
            
            #region output log messages
            
            logger.LogInformation(
                message: "WRITE: {count} database records",
                args: records);
            
            #endregion
            
            #region build storage data
            
            remotePath = Path.Combine(
                path1: "output",
                path2: "agency.txt");
            
            localPath = await GtfsAgencyBuilder.BuildAsync(
                schedules: results,
                path: storage.FullName);
            
            await containerClient
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
                path1: "output",
                path2: "calendar.txt");
            
            localPath = await GtfsCalendarBuilder.BuildAsync(
                schedules: results,
                path: storage.FullName);
            
            await containerClient
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
                path1: "output",
                path2: "calendar_dates.txt");
            
            localPath = await GtfsCalendarDateBuilder.BuildAsync(
                schedules: results,
                path: storage.FullName);
            
            await containerClient
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
                path1: "output",
                path2: "routes.txt");
            
            localPath = await GtfsRouteBuilder.BuildAsync(
                schedules: results,
                path: storage.FullName);
            
            await containerClient
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
                path1: "output",
                path2: "stops.txt");
            
            localPath = await GtfsStopBuilder.BuildAsync(
                schedules: results,
                path: storage.FullName);
            
            await containerClient
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
                path1: "output",
                path2: "stop_times.txt");
            
            localPath = await GtfsStopTimeBuilder.BuildAsync(
                schedules: results,
                path: storage.FullName);
            
            await containerClient
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
                path1: "output",
                path2: "trips.txt");
            
            localPath = await GtfsTripBuilder.BuildAsync(
                schedules: results,
                path: storage.FullName);
            
            await containerClient
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
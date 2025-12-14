using System.IO.Compression;
using System.Xml.Serialization;
using FluentFTP;
using FluentFTP.Exceptions;
using Flurl.Http;
using Npgsql;
using Polly;
using Polly.Retry;
using TramTimes.Database.Jobs.Builders;
using TramTimes.Database.Jobs.Extensions;
using TramTimes.Database.Jobs.Models;
using TramTimes.Database.Jobs.Tools;

namespace TramTimes.Database.Jobs.Services;

public class BuildService : IHostedService
{
    private readonly NpgsqlDataSource _service;
    private readonly ILogger<BuildService> _logger;
    private readonly IHostApplicationLifetime _host;
    private readonly DirectoryInfo _storage;
    private readonly AsyncRetryPolicy _result;

    private const string Holidays = "https://date.nager.at/api/v3/NextPublicHolidays/gb";
    private const string Localities = "https://naptan.api.dft.gov.uk/v1/nptg/localities";
    private const string Stops = "https://naptan.api.dft.gov.uk/v1/access-nodes?dataFormat=csv&atcoAreaCodes=370%2C940";

    public BuildService(
        NpgsqlDataSource service,
        ILogger<BuildService> logger,
        IHostApplicationLifetime host) {

        #region inject services

        _service = service;
        _logger = logger;
        _host = host;

        #endregion

        #region build storage

        var guid = Guid.NewGuid();

        _storage = Directory.CreateDirectory(path: Path.Combine(
            path1: Path.GetTempPath(),
            path2: guid.ToString()));

        #endregion

        #region build result

        _result = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(
                retryCount: 5,
                sleepDurationProvider: i => TimeSpan.FromSeconds(value: Math.Pow(
                    x: 5,
                    y: i)));

        #endregion
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        #region build task

        await _result.ExecuteAsync(action: async () =>
        {
            var holidays = await Holidays.GetJsonAsync<List<Holiday>>(cancellationToken: cancellationToken) ?? [];

            var localPath = await Localities.DownloadFileAsync(
                localFolderPath: _storage.FullName,
                localFileName: "localities.csv",
                cancellationToken: cancellationToken);

            if (localPath is null)
                throw new FtpException(message: "Failed to download Naptan localities");

            var localities = NaptanLocalityTools.GetFromFile(path: localPath);

            localPath = await Stops.DownloadFileAsync(
                localFolderPath: _storage.FullName,
                localFileName: "stops.csv",
                cancellationToken: cancellationToken);

            if (localPath is null)
                throw new FtpException(message: "Failed to download Naptan stops");

            var stops = NaptanStopTools.GetFromFile(path: localPath);

            var status = await FtpClientTools.GetFromRemoteAsync(
                localPath: Path.Combine(
                    path1: _storage.FullName,
                    path2: "traveline.zip"),
                remoteFileName: "Y.zip");

            if (status is not FtpStatus.Success)
                throw new FtpException(message: "Failed to download Traveline data");

            await ZipFile.ExtractToDirectoryAsync(
                sourceArchiveFileName: Path.Combine(
                    path1: _storage.FullName,
                    path2: "traveline.zip"),
                destinationDirectoryName: _storage.FullName,
                cancellationToken: cancellationToken);

            var rawFiles = Directory
                .GetFiles(path: _storage.FullName)
                .Where(predicate: file => file.EndsWith(value: ".xml"))
                .ToArray();

            var workingFiles = new List<string>();

            foreach (var item in rawFiles)
            {
                var reader = new StreamReader(path: item);
                var contents = await reader.ReadToEndAsync(cancellationToken: cancellationToken);

                if (contents.Contains(value: "ZZSY"))
                    workingFiles.Add(item: item);
            }

            Dictionary<string, TravelineSchedule> results = [];

            foreach (var reader in workingFiles.Select(selector: file => new StreamReader(path: file)))
            {
                if (new XmlSerializer(type: typeof(TransXChange)).Deserialize(textReader: reader) is not TransXChange xml)
                    continue;

                if (xml.VehicleJourneys?.VehicleJourney is null)
                    continue;

                var startDate = DateOnlyTools.GetPeriodStartDate(
                    scheduleDate: DateOnly.FromDateTime(dateTime: DateTime.UtcNow.Date),
                    startDate: xml.Services?.Service?.OperatingPeriod?.StartDate.ToDate());

                var endDate = DateOnlyTools.GetPeriodEndDate(
                    scheduleDate: DateOnly.FromDateTime(dateTime: DateTime.UtcNow.Date),
                    endDate: xml.Services?.Service?.OperatingPeriod?.EndDate.ToDate());

                if (startDate > endDate)
                    continue;

                foreach (var item in xml.VehicleJourneys.VehicleJourney)
                {
                    var calendar = TravelineCalendarBuilder.Build(
                        scheduleDate: DateOnly.FromDateTime(dateTime: DateTime.UtcNow.Date),
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

            await using var connection = await _service.OpenConnectionAsync(cancellationToken: cancellationToken);
            await using var transaction = await connection.BeginTransactionAsync(cancellationToken: cancellationToken);

            await using var dropCommand = new NpgsqlCommand(
                cmdText: "drop index if exists gtfs_stop_times_idx",
                connection: connection,
                transaction: transaction);

            await dropCommand.ExecuteNonQueryAsync(cancellationToken: cancellationToken);

            await DatabaseAgencyBuilder.BuildAsync(
                schedules: results,
                connection: connection,
                transaction: transaction);

            await DatabaseCalendarBuilder.BuildAsync(
                schedules: results,
                connection: connection,
                transaction: transaction);

            await DatabaseCalendarDateBuilder.BuildAsync(
                schedules: results,
                connection: connection,
                transaction: transaction);

            await DatabaseRouteBuilder.BuildAsync(
                schedules: results,
                connection: connection,
                transaction: transaction);

            await DatabaseStopBuilder.BuildAsync(
                schedules: results,
                connection: connection,
                transaction: transaction);

            await DatabaseStopTimeBuilder.BuildAsync(
                schedules: results,
                connection: connection,
                transaction: transaction);

            await DatabaseTripBuilder.BuildAsync(
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

            await createCommand.ExecuteNonQueryAsync(cancellationToken: cancellationToken);
            await transaction.CommitAsync(cancellationToken: cancellationToken);

            if (_logger.IsEnabled(logLevel: LogLevel.Information))
                _logger.LogInformation(
                    message: "Build service health status: {status}",
                    args: "Green");

            _host.StopApplication();
        });

        #endregion
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        #region build task

        return Task.CompletedTask;

        #endregion
    }
}
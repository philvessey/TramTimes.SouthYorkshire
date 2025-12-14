using Npgsql;
using NpgsqlTypes;
using TramTimes.Database.Jobs.Models;
using TramTimes.Database.Jobs.Tools;

namespace TramTimes.Database.Jobs.Builders;

public static class DatabaseStopBuilder
{
    public static async Task<ulong> BuildAsync(
        Dictionary<string, TravelineSchedule> schedules,
        NpgsqlConnection connection,
        NpgsqlTransaction transaction) {

        #region build stops

        var stops = DatabaseStopTools.GetFromSchedules(schedules: schedules);

        #endregion

        #region create table

        await using var createCommand = new NpgsqlCommand(
            cmdText: "create table if not exists gtfs_stops (" +
                     "stop_id character varying(255) primary key, " +
                     "stop_code character varying(255), " +
                     "stop_name character varying(255), " +
                     "stop_desc character varying(255), " +
                     "stop_lat real, " +
                     "stop_lon real, " +
                     "zone_id character varying(255), " +
                     "stop_url character varying(255), " +
                     "location_type character varying(255), " +
                     "parent_station character varying(255), " +
                     "stop_timezone character varying(255), " +
                     "wheelchair_boarding character varying(255), " +
                     "level_id character varying(255), " +
                     "platform_code character varying(255))",
            connection: connection,
            transaction: transaction);

        await createCommand.ExecuteNonQueryAsync();

        #endregion

        #region truncate table

        await using var truncateCommand = new NpgsqlCommand(
            cmdText: "truncate table gtfs_stops",
            connection: connection,
            transaction: transaction);

        await truncateCommand.ExecuteNonQueryAsync();

        #endregion

        #region create importer

        await using var importer = await connection.BeginBinaryImportAsync(
            copyFromCommand: "copy gtfs_stops (" +
                             "stop_id, " +
                             "stop_code, " +
                             "stop_name, " +
                             "stop_desc, " +
                             "stop_lat, " +
                             "stop_lon, " +
                             "zone_id, " +
                             "stop_url, " +
                             "location_type, " +
                             "parent_station, " +
                             "stop_timezone, " +
                             "wheelchair_boarding, " +
                             "level_id, " +
                             "platform_code) " +
                             "from stdin (format binary)");

        #endregion

        #region build results

        foreach (var item in stops.Values)
        {
            await importer.StartRowAsync();

            await importer.WriteAsync(
                value: item.StopId,
                npgsqlDbType: NpgsqlDbType.Varchar);

            await importer.WriteAsync(
                value: item.StopCode,
                npgsqlDbType: NpgsqlDbType.Varchar);

            await importer.WriteAsync(
                value: item.StopName,
                npgsqlDbType: NpgsqlDbType.Varchar);

            await importer.WriteAsync(
                value: item.StopDesc,
                npgsqlDbType: NpgsqlDbType.Varchar);

            await importer.WriteAsync(
                value: item.StopLat,
                npgsqlDbType: NpgsqlDbType.Real);

            await importer.WriteAsync(
                value: item.StopLon,
                npgsqlDbType: NpgsqlDbType.Real);

            await importer.WriteAsync(
                value: item.ZoneId,
                npgsqlDbType: NpgsqlDbType.Varchar);

            await importer.WriteAsync(
                value: item.StopUrl,
                npgsqlDbType: NpgsqlDbType.Varchar);

            await importer.WriteAsync(
                value: item.LocationType,
                npgsqlDbType: NpgsqlDbType.Varchar);

            await importer.WriteAsync(
                value: item.ParentStation,
                npgsqlDbType: NpgsqlDbType.Varchar);

            await importer.WriteAsync(
                value: item.StopTimezone,
                npgsqlDbType: NpgsqlDbType.Varchar);

            await importer.WriteAsync(
                value: item.WheelchairBoarding,
                npgsqlDbType: NpgsqlDbType.Varchar);

            await importer.WriteAsync(
                value: item.LevelId,
                npgsqlDbType: NpgsqlDbType.Varchar);

            await importer.WriteAsync(
                value: item.PlatformCode,
                npgsqlDbType: NpgsqlDbType.Varchar);
        }

        var results = await importer.CompleteAsync();

        #endregion

        return results;
    }
}
using Npgsql;
using NpgsqlTypes;
using TramTimes.Database.Jobs.Models;
using TramTimes.Database.Jobs.Tools;

namespace TramTimes.Database.Jobs.Builders;

public static class DatabaseStopTimeBuilder
{
    public static async Task<ulong> BuildAsync(
        Dictionary<string, TravelineSchedule> schedules,
        NpgsqlConnection connection,
        NpgsqlTransaction transaction) {

        #region build stop times

        var stopTimes = DatabaseStopTimeTools.GetFromSchedules(schedules: schedules);

        #endregion

        #region create table

        await using var createCommand = new NpgsqlCommand(
            cmdText: "create table if not exists gtfs_stop_times (" +
                     "trip_id character varying(255) not null, " +
                     "arrival_time character varying(255), " +
                     "departure_time character varying(255), " +
                     "stop_id character varying(255), " +
                     "stop_sequence smallint not null, " +
                     "stop_headsign character varying(255), " +
                     "pickup_type character varying(255), " +
                     "drop_off_type character varying(255), " +
                     "shape_dist_travelled real, " +
                     "timepoint smallint)",
            connection: connection,
            transaction: transaction);

        await createCommand.ExecuteNonQueryAsync();

        #endregion

        #region truncate table

        await using var truncateCommand = new NpgsqlCommand(
            cmdText: "truncate table gtfs_stop_times",
            connection: connection,
            transaction: transaction);

        await truncateCommand.ExecuteNonQueryAsync();

        #endregion

        #region create importer

        await using var importer = await connection.BeginBinaryImportAsync(
            copyFromCommand: "copy gtfs_stop_times (" +
                             "trip_id, " +
                             "arrival_time, " +
                             "departure_time, " +
                             "stop_id, " +
                             "stop_sequence, " +
                             "stop_headsign, " +
                             "pickup_type, " +
                             "drop_off_type, " +
                             "shape_dist_travelled, " +
                             "timepoint) " +
                             "from stdin (format binary)");

        #endregion

        #region build results

        foreach (var item in stopTimes.Values)
        {
            await importer.StartRowAsync();

            await importer.WriteAsync(
                value: item.trip_id,
                npgsqlDbType: NpgsqlDbType.Varchar);

            await importer.WriteAsync(
                value: item.arrival_time,
                npgsqlDbType: NpgsqlDbType.Varchar);

            await importer.WriteAsync(
                value: item.departure_time,
                npgsqlDbType: NpgsqlDbType.Varchar);

            await importer.WriteAsync(
                value: item.stop_id,
                npgsqlDbType: NpgsqlDbType.Varchar);

            await importer.WriteAsync(
                value: item.stop_sequence,
                npgsqlDbType: NpgsqlDbType.Smallint);

            await importer.WriteAsync(
                value: item.stop_headsign,
                npgsqlDbType: NpgsqlDbType.Varchar);

            await importer.WriteAsync(
                value: item.pickup_type,
                npgsqlDbType: NpgsqlDbType.Varchar);

            await importer.WriteAsync(
                value: item.drop_off_type,
                npgsqlDbType: NpgsqlDbType.Varchar);

            await importer.WriteAsync(
                value: item.shape_dist_traveled,
                npgsqlDbType: NpgsqlDbType.Real);

            await importer.WriteAsync(
                value: item.timepoint,
                npgsqlDbType: NpgsqlDbType.Smallint);
        }

        var results = await importer.CompleteAsync();

        #endregion

        return results;
    }
}
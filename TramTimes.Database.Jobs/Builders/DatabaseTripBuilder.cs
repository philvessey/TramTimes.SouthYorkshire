using Npgsql;
using NpgsqlTypes;
using TramTimes.Database.Jobs.Models;
using TramTimes.Database.Jobs.Tools;

namespace TramTimes.Database.Jobs.Builders;

public static class DatabaseTripBuilder
{
    public static async Task<ulong> BuildAsync(
        Dictionary<string, TravelineSchedule> schedules,
        NpgsqlConnection connection,
        NpgsqlTransaction transaction) {

        #region build trips

        var trips = DatabaseTripTools.GetFromSchedules(schedules: schedules);

        #endregion

        #region create table

        await using var createCommand = new NpgsqlCommand(
            cmdText: "create table if not exists gtfs_trips (" +
                     "route_id character varying(255) not null, " +
                     "service_id character varying(255) not null, " +
                     "trip_id character varying(255) primary key, " +
                     "trip_headsign character varying(255), " +
                     "trip_short_name character varying(255), " +
                     "direction_id smallint, " +
                     "block_id character varying(255), " +
                     "shape_id character varying(255), " +
                     "wheelchair_accessible character varying(255), " +
                     "bikes_allowed character varying(255))",
            connection: connection,
            transaction: transaction);

        await createCommand.ExecuteNonQueryAsync();

        #endregion

        #region truncate table

        await using var truncateCommand = new NpgsqlCommand(
            cmdText: "truncate table gtfs_trips",
            connection: connection,
            transaction: transaction);

        await truncateCommand.ExecuteNonQueryAsync();

        #endregion

        #region create importer

        await using var importer = await connection.BeginBinaryImportAsync(
            copyFromCommand: "copy gtfs_trips (" +
                             "route_id, " +
                             "service_id, " +
                             "trip_id, " +
                             "trip_headsign, " +
                             "trip_short_name, " +
                             "direction_id, " +
                             "block_id, " +
                             "shape_id, " +
                             "wheelchair_accessible, " +
                             "bikes_allowed) " +
                             "from stdin (format binary)");

        #endregion

        #region build results

        foreach (var item in trips.Values)
        {
            await importer.StartRowAsync();

            await importer.WriteAsync(
                value: item.route_id,
                npgsqlDbType: NpgsqlDbType.Varchar);

            await importer.WriteAsync(
                value: item.service_id,
                npgsqlDbType: NpgsqlDbType.Varchar);

            await importer.WriteAsync(
                value: item.trip_id,
                npgsqlDbType: NpgsqlDbType.Varchar);

            await importer.WriteAsync(
                value: item.trip_headsign,
                npgsqlDbType: NpgsqlDbType.Varchar);

            await importer.WriteAsync(
                value: item.trip_short_name,
                npgsqlDbType: NpgsqlDbType.Varchar);

            await importer.WriteAsync(
                value: item.direction_id,
                npgsqlDbType: NpgsqlDbType.Smallint);

            await importer.WriteAsync(
                value: item.block_id,
                npgsqlDbType: NpgsqlDbType.Varchar);

            await importer.WriteAsync(
                value: item.shape_id,
                npgsqlDbType: NpgsqlDbType.Varchar);

            await importer.WriteAsync(
                value: item.wheelchair_accessible,
                npgsqlDbType: NpgsqlDbType.Varchar);

            await importer.WriteAsync(
                value: item.bikes_allowed,
                npgsqlDbType: NpgsqlDbType.Varchar);
        }

        var results = await importer.CompleteAsync();

        #endregion

        return results;
    }
}
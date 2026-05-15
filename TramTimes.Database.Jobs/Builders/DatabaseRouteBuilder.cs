using Npgsql;
using NpgsqlTypes;
using TramTimes.Database.Jobs.Models;
using TramTimes.Database.Jobs.Tools;

namespace TramTimes.Database.Jobs.Builders;

public static class DatabaseRouteBuilder
{
    public static async Task<ulong> BuildAsync(
        Dictionary<string, TravelineSchedule> schedules,
        NpgsqlConnection connection,
        NpgsqlTransaction transaction) {

        #region build routes

        var routes = DatabaseRouteTools.GetFromSchedules(schedules: schedules);

        #endregion

        #region create table

        await using var createCommand = new NpgsqlCommand(
            cmdText: "create table if not exists gtfs_routes (" +
                     "route_id character varying(255) primary key, " +
                     "agency_id character varying(255), " +
                     "route_short_name character varying(255), " +
                     "route_long_name character varying(255), " +
                     "route_desc character varying(255), " +
                     "route_type smallint not null, " +
                     "route_url character varying(255), " +
                     "route_color character varying(255), " +
                     "route_text_color character varying(255), " +
                     "route_sort_order smallint)",
            connection: connection,
            transaction: transaction);

        await createCommand.ExecuteNonQueryAsync();

        #endregion

        #region truncate table

        await using var truncateCommand = new NpgsqlCommand(
            cmdText: "truncate table gtfs_routes",
            connection: connection,
            transaction: transaction);

        await truncateCommand.ExecuteNonQueryAsync();

        #endregion

        #region create importer

        await using var importer = await connection.BeginBinaryImportAsync(
            copyFromCommand: "copy gtfs_routes (" +
                             "route_id, " +
                             "agency_id, " +
                             "route_short_name, " +
                             "route_long_name, " +
                             "route_desc, " +
                             "route_type, " +
                             "route_url, " +
                             "route_color, " +
                             "route_text_color, " +
                             "route_sort_order) " +
                             "from stdin (format binary)");

        #endregion

        #region build results

        foreach (var item in routes.Values)
        {
            await importer.StartRowAsync();

            await importer.WriteAsync(
                value: item.route_id,
                npgsqlDbType: NpgsqlDbType.Varchar);

            await importer.WriteAsync(
                value: item.agency_id,
                npgsqlDbType: NpgsqlDbType.Varchar);

            await importer.WriteAsync(
                value: item.route_short_name,
                npgsqlDbType: NpgsqlDbType.Varchar);

            await importer.WriteAsync(
                value: item.route_long_name,
                npgsqlDbType: NpgsqlDbType.Varchar);

            await importer.WriteAsync(
                value: item.route_desc,
                npgsqlDbType: NpgsqlDbType.Varchar);

            await importer.WriteAsync(
                value: item.route_type,
                npgsqlDbType: NpgsqlDbType.Smallint);

            await importer.WriteAsync(
                value: item.route_url,
                npgsqlDbType: NpgsqlDbType.Varchar);

            await importer.WriteAsync(
                value: item.route_color,
                npgsqlDbType: NpgsqlDbType.Varchar);

            await importer.WriteAsync(
                value: item.route_text_color,
                npgsqlDbType: NpgsqlDbType.Varchar);

            await importer.WriteAsync(
                value: item.route_sort_order,
                npgsqlDbType: NpgsqlDbType.Smallint);
        }

        var results = await importer.CompleteAsync();

        #endregion

        return results;
    }
}
using Npgsql;
using NpgsqlTypes;
using TramTimes.Database.Jobs.Models;
using TramTimes.Database.Jobs.Tools;

namespace TramTimes.Database.Jobs.Builders;

public static class DatabaseCalendarBuilder
{
    public static async Task<ulong> BuildAsync(
        Dictionary<string, TravelineSchedule> schedules,
        NpgsqlConnection connection,
        NpgsqlTransaction transaction) {

        #region build calendars

        var calendars = DatabaseCalendarTools.GetFromSchedules(schedules: schedules);

        #endregion

        #region create table

        await using var createCommand = new NpgsqlCommand(
            cmdText: "create table if not exists gtfs_calendar (" +
                     "service_id character varying(255) primary key, " +
                     "monday smallint not null, " +
                     "tuesday smallint not null, " +
                     "wednesday smallint not null, " +
                     "thursday smallint not null, " +
                     "friday smallint not null, " +
                     "saturday smallint not null, " +
                     "sunday smallint not null, " +
                     "start_date date not null, " +
                     "end_date date not null)",
            connection: connection,
            transaction: transaction);

        await createCommand.ExecuteNonQueryAsync();

        #endregion

        #region truncate table

        await using var truncateCommand = new NpgsqlCommand(
            cmdText: "truncate table gtfs_calendar",
            connection: connection,
            transaction: transaction);

        await truncateCommand.ExecuteNonQueryAsync();

        #endregion

        #region create importer

        await using var importer = await connection.BeginBinaryImportAsync(
            copyFromCommand: "copy gtfs_calendar (" +
                             "service_id, " +
                             "monday, " +
                             "tuesday, " +
                             "wednesday, " +
                             "thursday, " +
                             "friday, " +
                             "saturday, " +
                             "sunday, " +
                             "start_date, " +
                             "end_date) " +
                             "from stdin (format binary)");

        #endregion

        #region build results

        foreach (var item in calendars.Values)
        {
            await importer.StartRowAsync();

            await importer.WriteAsync(
                value: item.ServiceId,
                npgsqlDbType: NpgsqlDbType.Varchar);

            await importer.WriteAsync(
                value: item.Monday,
                npgsqlDbType: NpgsqlDbType.Smallint);

            await importer.WriteAsync(
                value: item.Tuesday,
                npgsqlDbType: NpgsqlDbType.Smallint);

            await importer.WriteAsync(
                value: item.Wednesday,
                npgsqlDbType: NpgsqlDbType.Smallint);

            await importer.WriteAsync(
                value: item.Thursday,
                npgsqlDbType: NpgsqlDbType.Smallint);

            await importer.WriteAsync(
                value: item.Friday,
                npgsqlDbType: NpgsqlDbType.Smallint);

            await importer.WriteAsync(
                value: item.Saturday,
                npgsqlDbType: NpgsqlDbType.Smallint);

            await importer.WriteAsync(
                value: item.Sunday,
                npgsqlDbType: NpgsqlDbType.Smallint);

            await importer.WriteAsync(
                value: item.StartDate,
                npgsqlDbType: NpgsqlDbType.Date);

            await importer.WriteAsync(
                value: item.EndDate,
                npgsqlDbType: NpgsqlDbType.Date);
        }

        var results = await importer.CompleteAsync();

        #endregion

        return results;
    }
}
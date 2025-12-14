using Npgsql;
using NpgsqlTypes;
using TramTimes.Database.Jobs.Models;
using TramTimes.Database.Jobs.Tools;

namespace TramTimes.Database.Jobs.Builders;

public static class DatabaseCalendarDateBuilder
{
    public static async Task<ulong> BuildAsync(
        Dictionary<string, TravelineSchedule> schedules,
        NpgsqlConnection connection,
        NpgsqlTransaction transaction) {

        #region build calendar dates

        var calendarDates = DatabaseCalendarDateTools.GetFromSchedules(schedules: schedules);

        #endregion

        #region create table

        await using var createCommand = new NpgsqlCommand(
            cmdText: "create table if not exists gtfs_calendar_dates (" +
                     "service_id character varying(255) not null, " +
                     "exception_date date not null, " +
                     "exception_type smallint not null)",
            connection: connection,
            transaction: transaction);

        await createCommand.ExecuteNonQueryAsync();

        #endregion

        #region truncate table

        await using var truncateCommand = new NpgsqlCommand(
            cmdText: "truncate table gtfs_calendar_dates",
            connection: connection,
            transaction: transaction);

        await truncateCommand.ExecuteNonQueryAsync();

        #endregion

        #region create importer

        await using var importer = await connection.BeginBinaryImportAsync(
            copyFromCommand: "copy gtfs_calendar_dates (" +
                             "service_id, " +
                             "exception_date, " +
                             "exception_type) " +
                             "from stdin (format binary)");

        #endregion

        #region build results

        foreach (var item in calendarDates.Values)
        {
            await importer.StartRowAsync();

            await importer.WriteAsync(
                value: item.ServiceId,
                npgsqlDbType: NpgsqlDbType.Varchar);

            await importer.WriteAsync(
                value: item.ExceptionDate,
                npgsqlDbType: NpgsqlDbType.Date);

            await importer.WriteAsync(
                value: item.ExceptionType,
                npgsqlDbType: NpgsqlDbType.Smallint);
        }

        var results = await importer.CompleteAsync();

        #endregion

        return results;
    }
}
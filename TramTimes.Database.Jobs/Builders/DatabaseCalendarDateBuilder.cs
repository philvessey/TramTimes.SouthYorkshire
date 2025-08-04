using Npgsql;
using NpgsqlTypes;
using TramTimes.Database.Jobs.Models;
using TramTimes.Database.Jobs.Tools;

namespace TramTimes.Database.Jobs.Builders;

public static class DatabaseCalendarDateBuilder
{
    public static async Task<ulong> BuildAsync(
        Dictionary<string, TravelineSchedule> schedules,
        NpgsqlConnection connection) {
        
        #region build calendar dates
        
        var calendarDates = DatabaseCalendarDateTools.GetFromSchedules(schedules: schedules);
        
        #endregion
        
        #region create table
        
        var command = new NpgsqlCommand(
            cmdText: "create table if not exists gtfs_calendar_dates (" +
                     "service_id character varying(255) not null, " +
                     "exception_date date not null, " +
                     "exception_type smallint not null)",
            connection: connection);
        
        await command.ExecuteNonQueryAsync();
        
        #endregion
        
        #region truncate table
        
        command = new NpgsqlCommand(
            cmdText: "truncate table gtfs_calendar_dates",
            connection: connection);
        
        await command.ExecuteNonQueryAsync();
        
        #endregion
        
        #region create importer
        
        var importer = await connection.BeginBinaryImportAsync(copyFromCommand: "copy gtfs_calendar_dates (" +
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
        await importer.CloseAsync();
        
        #endregion
        
        return results;
    }
}
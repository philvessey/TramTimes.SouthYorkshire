using Npgsql;
using NpgsqlTypes;
using TramTimes.Database.Jobs.Models;
using TramTimes.Database.Jobs.Tools;

namespace TramTimes.Database.Jobs.Builders;

public static class DatabaseCalendarDateBuilder
{
    public static async Task<ulong> BuildAsync(
        Dictionary<string, TravelineSchedule> schedules,
        NpgsqlDataSource dataSource) {
        
        var calendarDates = await DatabaseCalendarDateTools.GetFromSchedulesAsync(schedules: schedules);
        
        const string sql = "copy gtfs_calendar_dates (" +
                           "service_id, " +
                           "exception_date, " +
                           "exception_type)";
        
        var connection = await dataSource.OpenConnectionAsync();
        
        var command = new NpgsqlCommand(
            cmdText: "truncate table gtfs_calendar_dates",
            connection: connection);
        
        await command.ExecuteNonQueryAsync();
        
        var importer = await connection.BeginBinaryImportAsync(copyFromCommand: $"{sql} from stdin (format binary)");
        
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
        
        return await Task.FromResult(result: await importer.CompleteAsync());
    }
}
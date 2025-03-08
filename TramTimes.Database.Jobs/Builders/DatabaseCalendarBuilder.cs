using Npgsql;
using NpgsqlTypes;
using TramTimes.Database.Jobs.Models;
using TramTimes.Database.Jobs.Tools;

namespace TramTimes.Database.Jobs.Builders;

public static class DatabaseCalendarBuilder
{
    public static async Task<ulong> BuildAsync(
        Dictionary<string, TravelineSchedule> schedules,
        NpgsqlDataSource dataSource) {
        
        var calendars = await DatabaseCalendarTools.GetFromSchedulesAsync(schedules: schedules);
        
        const string sql = "copy gtfs_calendar (" +
                           "service_id, " +
                           "monday, " +
                           "tuesday, " +
                           "wednesday, " +
                           "thursday, " +
                           "friday, " +
                           "saturday, " +
                           "sunday, " +
                           "start_date, " +
                           "end_date)";
        
        var connection = await dataSource.OpenConnectionAsync();
        
        var command = new NpgsqlCommand(cmdText: "truncate table gtfs_calendar", connection: connection);
        await command.ExecuteNonQueryAsync();
        
        var importer = await connection.BeginBinaryImportAsync(copyFromCommand: $"{sql} from stdin (format binary)");
        
        foreach (var value in calendars.Values)
        {
            await importer.StartRowAsync();
            
            await importer.WriteAsync(
                value: value.ServiceId,
                npgsqlDbType: NpgsqlDbType.Varchar);
            
            await importer.WriteAsync(
                value: value.Monday,
                npgsqlDbType: NpgsqlDbType.Smallint);
            
            await importer.WriteAsync(
                value: value.Tuesday,
                npgsqlDbType: NpgsqlDbType.Smallint);
            
            await importer.WriteAsync(
                value: value.Wednesday,
                npgsqlDbType: NpgsqlDbType.Smallint);
            
            await importer.WriteAsync(
                value: value.Thursday,
                npgsqlDbType: NpgsqlDbType.Smallint);
            
            await importer.WriteAsync(
                value: value.Friday,
                npgsqlDbType: NpgsqlDbType.Smallint);
            
            await importer.WriteAsync(
                value: value.Saturday,
                npgsqlDbType: NpgsqlDbType.Smallint);
            
            await importer.WriteAsync(
                value: value.Sunday,
                npgsqlDbType: NpgsqlDbType.Smallint);
            
            await importer.WriteAsync(
                value: value.StartDate,
                npgsqlDbType: NpgsqlDbType.Date);
            
            await importer.WriteAsync(
                value: value.EndDate,
                npgsqlDbType: NpgsqlDbType.Date);
        }
        
        return await Task.FromResult(result: await importer.CompleteAsync());
    }
}
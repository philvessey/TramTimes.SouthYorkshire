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
        
        var command = new NpgsqlCommand(
            cmdText: "truncate table gtfs_calendar",
            connection: connection);
        
        await command.ExecuteNonQueryAsync();
        
        var importer = await connection.BeginBinaryImportAsync(copyFromCommand: $"{sql} from stdin (format binary)");
        
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
        
        return await Task.FromResult(result: await importer.CompleteAsync());
    }
}
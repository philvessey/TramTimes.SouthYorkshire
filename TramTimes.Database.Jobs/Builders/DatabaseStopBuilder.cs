using Npgsql;
using NpgsqlTypes;
using TramTimes.Database.Jobs.Models;
using TramTimes.Database.Jobs.Tools;

namespace TramTimes.Database.Jobs.Builders;

public static class DatabaseStopBuilder
{
    public static async Task<ulong> BuildAsync(
        Dictionary<string, TravelineSchedule> schedules,
        NpgsqlDataSource dataSource) {
        
        var stops = await DatabaseStopTools.GetFromSchedulesAsync(schedules: schedules);
        
        const string sql = "copy gtfs_stops (" +
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
                           "platform_code)";
        
        var connection = await dataSource.OpenConnectionAsync();
        
        var command = new NpgsqlCommand(cmdText: "truncate table gtfs_stops", connection: connection);
        await command.ExecuteNonQueryAsync();
        
        var importer = await connection.BeginBinaryImportAsync(copyFromCommand: $"{sql} from stdin (format binary)");
        
        foreach (var value in stops.Values)
        {
            await importer.StartRowAsync();
            
            await importer.WriteAsync(
                value: value.StopId,
                npgsqlDbType: NpgsqlDbType.Varchar);
            
            await importer.WriteAsync(
                value: value.StopCode,
                npgsqlDbType: NpgsqlDbType.Varchar);
            
            await importer.WriteAsync(
                value: value.StopName,
                npgsqlDbType: NpgsqlDbType.Varchar);
            
            await importer.WriteAsync(
                value: value.StopDesc,
                npgsqlDbType: NpgsqlDbType.Varchar);
            
            await importer.WriteAsync(
                value: value.StopLat,
                npgsqlDbType: NpgsqlDbType.Real);
            
            await importer.WriteAsync(
                value: value.StopLon,
                npgsqlDbType: NpgsqlDbType.Real);
            
            await importer.WriteAsync(
                value: value.ZoneId,
                npgsqlDbType: NpgsqlDbType.Varchar);
            
            await importer.WriteAsync(
                value: value.StopUrl,
                npgsqlDbType: NpgsqlDbType.Varchar);
            
            await importer.WriteAsync(
                value: value.LocationType,
                npgsqlDbType: NpgsqlDbType.Smallint);
            
            await importer.WriteAsync(
                value: value.ParentStation,
                npgsqlDbType: NpgsqlDbType.Varchar);
            
            await importer.WriteAsync(
                value: value.StopTimezone,
                npgsqlDbType: NpgsqlDbType.Varchar);
            
            await importer.WriteAsync(
                value: value.WheelchairBoarding,
                npgsqlDbType: NpgsqlDbType.Smallint);
            
            await importer.WriteAsync(
                value: value.LevelId,
                npgsqlDbType: NpgsqlDbType.Varchar);
            
            await importer.WriteAsync(
                value: value.PlatformCode,
                npgsqlDbType: NpgsqlDbType.Varchar);
        }
        
        return await Task.FromResult(result: await importer.CompleteAsync());
    }
}
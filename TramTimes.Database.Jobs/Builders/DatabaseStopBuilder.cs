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
        
        var command = new NpgsqlCommand(
            cmdText: "truncate table gtfs_stops",
            connection: connection);
        
        await command.ExecuteNonQueryAsync();
        
        var importer = await connection.BeginBinaryImportAsync(copyFromCommand: $"{sql} from stdin (format binary)");
        
        foreach (var item in stops.Values)
        {
            await importer.StartRowAsync();
            
            await importer.WriteAsync(
                value: item.StopId,
                npgsqlDbType: NpgsqlDbType.Varchar);
            
            await importer.WriteAsync(
                value: item.StopCode,
                npgsqlDbType: NpgsqlDbType.Varchar);
            
            await importer.WriteAsync(
                value: item.StopName,
                npgsqlDbType: NpgsqlDbType.Varchar);
            
            await importer.WriteAsync(
                value: item.StopDesc,
                npgsqlDbType: NpgsqlDbType.Varchar);
            
            await importer.WriteAsync(
                value: item.StopLat,
                npgsqlDbType: NpgsqlDbType.Real);
            
            await importer.WriteAsync(
                value: item.StopLon,
                npgsqlDbType: NpgsqlDbType.Real);
            
            await importer.WriteAsync(
                value: item.ZoneId,
                npgsqlDbType: NpgsqlDbType.Varchar);
            
            await importer.WriteAsync(
                value: item.StopUrl,
                npgsqlDbType: NpgsqlDbType.Varchar);
            
            await importer.WriteAsync(
                value: item.LocationType,
                npgsqlDbType: NpgsqlDbType.Varchar);
            
            await importer.WriteAsync(
                value: item.ParentStation,
                npgsqlDbType: NpgsqlDbType.Varchar);
            
            await importer.WriteAsync(
                value: item.StopTimezone,
                npgsqlDbType: NpgsqlDbType.Varchar);
            
            await importer.WriteAsync(
                value: item.WheelchairBoarding,
                npgsqlDbType: NpgsqlDbType.Varchar);
            
            await importer.WriteAsync(
                value: item.LevelId,
                npgsqlDbType: NpgsqlDbType.Varchar);
            
            await importer.WriteAsync(
                value: item.PlatformCode,
                npgsqlDbType: NpgsqlDbType.Varchar);
        }
        
        return await Task.FromResult(result: await importer.CompleteAsync());
    }
}
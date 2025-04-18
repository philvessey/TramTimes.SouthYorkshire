using Npgsql;
using NpgsqlTypes;
using TramTimes.Database.Jobs.Models;
using TramTimes.Database.Jobs.Tools;

namespace TramTimes.Database.Jobs.Builders;

public static class DatabaseTripBuilder
{
    public static async Task<ulong> BuildAsync(
        Dictionary<string, TravelineSchedule> schedules,
        NpgsqlDataSource dataSource) {
        
        var trips = await DatabaseTripTools.GetFromSchedulesAsync(schedules: schedules);
        
        const string sql = "copy gtfs_trips (" +
                           "route_id, " +
                           "service_id, " +
                           "trip_id, " +
                           "trip_headsign, " +
                           "trip_short_name, " +
                           "direction_id, " +
                           "block_id, " +
                           "shape_id, " +
                           "wheelchair_accessible, " +
                           "bikes_allowed)";
        
        var connection = await dataSource.OpenConnectionAsync();
        
        var command = new NpgsqlCommand(cmdText: "truncate table gtfs_trips", connection: connection);
        await command.ExecuteNonQueryAsync();
        
        var importer = await connection.BeginBinaryImportAsync(copyFromCommand: $"{sql} from stdin (format binary)");
        
        foreach (var value in trips.Values)
        {
            await importer.StartRowAsync();
            
            await importer.WriteAsync(
                value: value.RouteId,
                npgsqlDbType: NpgsqlDbType.Varchar);
            
            await importer.WriteAsync(
                value: value.ServiceId,
                npgsqlDbType: NpgsqlDbType.Varchar);
            
            await importer.WriteAsync(
                value: value.TripId,
                npgsqlDbType: NpgsqlDbType.Varchar);
            
            await importer.WriteAsync(
                value: value.TripHeadsign,
                npgsqlDbType: NpgsqlDbType.Varchar);
            
            await importer.WriteAsync(
                value: value.TripShortName,
                npgsqlDbType: NpgsqlDbType.Varchar);
            
            await importer.WriteAsync(
                value: value.DirectionId,
                npgsqlDbType: NpgsqlDbType.Smallint);
            
            await importer.WriteAsync(
                value: value.BlockId,
                npgsqlDbType: NpgsqlDbType.Varchar);
            
            await importer.WriteAsync(
                value: value.ShapeId,
                npgsqlDbType: NpgsqlDbType.Varchar);
            
            await importer.WriteAsync(
                value: value.WheelchairAccessible,
                npgsqlDbType: NpgsqlDbType.Varchar);
            
            await importer.WriteAsync(
                value: value.BikesAllowed,
                npgsqlDbType: NpgsqlDbType.Varchar);
        }
        
        return await Task.FromResult(result: await importer.CompleteAsync());
    }
}
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
        
        #region build trips
        
        var trips = DatabaseTripTools.GetFromSchedules(schedules: schedules);
        
        #endregion
        
        #region build results
        
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
        
        await using var connection = await dataSource.OpenConnectionAsync();
        
        var command = new NpgsqlCommand(
            cmdText: "truncate table gtfs_trips",
            connection: connection);
        
        await command.ExecuteNonQueryAsync();
        
        var importer = await connection.BeginBinaryImportAsync(copyFromCommand: $"{sql} from stdin (format binary)");
        
        foreach (var item in trips.Values)
        {
            await importer.StartRowAsync();
            
            await importer.WriteAsync(
                value: item.RouteId,
                npgsqlDbType: NpgsqlDbType.Varchar);
            
            await importer.WriteAsync(
                value: item.ServiceId,
                npgsqlDbType: NpgsqlDbType.Varchar);
            
            await importer.WriteAsync(
                value: item.TripId,
                npgsqlDbType: NpgsqlDbType.Varchar);
            
            await importer.WriteAsync(
                value: item.TripHeadsign,
                npgsqlDbType: NpgsqlDbType.Varchar);
            
            await importer.WriteAsync(
                value: item.TripShortName,
                npgsqlDbType: NpgsqlDbType.Varchar);
            
            await importer.WriteAsync(
                value: item.DirectionId,
                npgsqlDbType: NpgsqlDbType.Smallint);
            
            await importer.WriteAsync(
                value: item.BlockId,
                npgsqlDbType: NpgsqlDbType.Varchar);
            
            await importer.WriteAsync(
                value: item.ShapeId,
                npgsqlDbType: NpgsqlDbType.Varchar);
            
            await importer.WriteAsync(
                value: item.WheelchairAccessible,
                npgsqlDbType: NpgsqlDbType.Varchar);
            
            await importer.WriteAsync(
                value: item.BikesAllowed,
                npgsqlDbType: NpgsqlDbType.Varchar);
        }
        
        var results = await importer.CompleteAsync();
        
        await importer.CloseAsync();
        await connection.CloseAsync();
        
        #endregion
        
        return results;
    }
}
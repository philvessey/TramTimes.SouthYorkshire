using Npgsql;
using NpgsqlTypes;
using TramTimes.Database.Jobs.Models;
using TramTimes.Database.Jobs.Tools;

namespace TramTimes.Database.Jobs.Builders;

public static class DatabaseStopTimeBuilder
{
    public static async Task<ulong> BuildAsync(
        Dictionary<string, TravelineSchedule> schedules,
        NpgsqlDataSource dataSource) {
        
        #region build stop times
        
        var stopTimes = DatabaseStopTimeTools.GetFromSchedules(schedules: schedules);
        
        #endregion
        
        #region build results
        
        const string sql = "copy gtfs_stop_times (" +
                           "trip_id, " +
                           "arrival_time, " +
                           "departure_time, " +
                           "stop_id, " +
                           "stop_sequence, " +
                           "stop_headsign, " +
                           "pickup_type, " +
                           "drop_off_type, " +
                           "shape_dist_travelled, " +
                           "timepoint)";
        
        await using var connection = await dataSource.OpenConnectionAsync();
        
        var command = new NpgsqlCommand(
            cmdText: "truncate table gtfs_stop_times",
            connection: connection);
        
        await command.ExecuteNonQueryAsync();
        
        var importer = await connection.BeginBinaryImportAsync(copyFromCommand: $"{sql} from stdin (format binary)");
        
        foreach (var item in stopTimes.Values)
        {
            await importer.StartRowAsync();
            
            await importer.WriteAsync(
                value: item.TripId,
                npgsqlDbType: NpgsqlDbType.Varchar);
            
            await importer.WriteAsync(
                value: item.ArrivalTime,
                npgsqlDbType: NpgsqlDbType.Varchar);
            
            await importer.WriteAsync(
                value: item.DepartureTime,
                npgsqlDbType: NpgsqlDbType.Varchar);
            
            await importer.WriteAsync(
                value: item.StopId,
                npgsqlDbType: NpgsqlDbType.Varchar);
            
            await importer.WriteAsync(
                value: item.StopSequence,
                npgsqlDbType: NpgsqlDbType.Smallint);
            
            await importer.WriteAsync(
                value: item.StopHeadsign,
                npgsqlDbType: NpgsqlDbType.Varchar);
            
            await importer.WriteAsync(
                value: item.PickupType,
                npgsqlDbType: NpgsqlDbType.Varchar);
            
            await importer.WriteAsync(
                value: item.DropOffType,
                npgsqlDbType: NpgsqlDbType.Varchar);
            
            await importer.WriteAsync(
                value: item.ShapeDistTraveled,
                npgsqlDbType: NpgsqlDbType.Real);
            
            await importer.WriteAsync(
                value: item.Timepoint,
                npgsqlDbType: NpgsqlDbType.Smallint);
        }
        
        var results = await importer.CompleteAsync();
        
        await importer.CloseAsync();
        await connection.CloseAsync();
        
        #endregion
        
        return results;
    }
}
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
        
        var stopTimes = await DatabaseStopTimeTools.GetFromSchedulesAsync(schedules: schedules);
        
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
        
        var connection = await dataSource.OpenConnectionAsync();
        
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
        
        return await Task.FromResult(result: await importer.CompleteAsync());
    }
}
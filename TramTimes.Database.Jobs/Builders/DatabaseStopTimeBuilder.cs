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
        
        var command = new NpgsqlCommand(cmdText: "truncate table gtfs_stop_times", connection: connection);
        await command.ExecuteNonQueryAsync();
        
        var importer = await connection.BeginBinaryImportAsync(copyFromCommand: $"{sql} from stdin (format binary)");
        
        foreach (var value in stopTimes.Values)
        {
            await importer.StartRowAsync();
            
            await importer.WriteAsync(
                value: value.TripId,
                npgsqlDbType: NpgsqlDbType.Varchar);
            
            await importer.WriteAsync(
                value: value.ArrivalTime,
                npgsqlDbType: NpgsqlDbType.Varchar);
            
            await importer.WriteAsync(
                value: value.DepartureTime,
                npgsqlDbType: NpgsqlDbType.Varchar);
            
            await importer.WriteAsync(
                value: value.StopId,
                npgsqlDbType: NpgsqlDbType.Varchar);
            
            await importer.WriteAsync(
                value: value.StopSequence,
                npgsqlDbType: NpgsqlDbType.Smallint);
            
            await importer.WriteAsync(
                value: value.StopHeadsign,
                npgsqlDbType: NpgsqlDbType.Varchar);
            
            await importer.WriteAsync(
                value: value.PickupType,
                npgsqlDbType: NpgsqlDbType.Smallint);
            
            await importer.WriteAsync(
                value: value.DropOffType,
                npgsqlDbType: NpgsqlDbType.Smallint);
            
            await importer.WriteAsync(
                value: value.ShapeDistTraveled,
                npgsqlDbType: NpgsqlDbType.Real);
            
            await importer.WriteAsync(
                value: value.Timepoint,
                npgsqlDbType: NpgsqlDbType.Smallint);
        }
        
        return await Task.FromResult(result: await importer.CompleteAsync());
    }
}
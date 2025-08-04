using Npgsql;
using NpgsqlTypes;
using TramTimes.Database.Jobs.Models;
using TramTimes.Database.Jobs.Tools;

namespace TramTimes.Database.Jobs.Builders;

public static class DatabaseTripBuilder
{
    public static async Task<ulong> BuildAsync(
        Dictionary<string, TravelineSchedule> schedules,
        NpgsqlConnection connection) {
        
        #region build trips
        
        var trips = DatabaseTripTools.GetFromSchedules(schedules: schedules);
        
        #endregion
        
        #region create table
        
        var command = new NpgsqlCommand(
            cmdText: "create table if not exists gtfs_trips (" +
                     "route_id character varying(255) not null, " +
                     "service_id character varying(255) not null, " +
                     "trip_id character varying(255) primary key, " +
                     "trip_headsign character varying(255), " +
                     "trip_short_name character varying(255), " +
                     "direction_id smallint, " +
                     "block_id character varying(255), " +
                     "shape_id character varying(255), " +
                     "wheelchair_accessible character varying(255), " +
                     "bikes_allowed character varying(255))",
            connection: connection);
        
        await command.ExecuteNonQueryAsync();
        
        #endregion
        
        #region truncate table
        
        command = new NpgsqlCommand(
            cmdText: "truncate table gtfs_trips",
            connection: connection);
        
        await command.ExecuteNonQueryAsync();
        
        #endregion
        
        #region create importer
        
        var importer = await connection.BeginBinaryImportAsync(copyFromCommand: "copy gtfs_trips (" +
                                                                                "route_id, " +
                                                                                "service_id, " +
                                                                                "trip_id, " +
                                                                                "trip_headsign, " +
                                                                                "trip_short_name, " +
                                                                                "direction_id, " +
                                                                                "block_id, " +
                                                                                "shape_id, " +
                                                                                "wheelchair_accessible, " +
                                                                                "bikes_allowed) " +
                                                                                "from stdin (format binary)");
        
        #endregion
        
        #region build results
        
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
        
        #endregion
        
        return results;
    }
}
using Npgsql;
using NpgsqlTypes;
using TramTimes.Database.Jobs.Models;
using TramTimes.Database.Jobs.Tools;

namespace TramTimes.Database.Jobs.Builders;

public static class DatabaseRouteBuilder
{
    public static async Task<ulong> BuildAsync(
        Dictionary<string, TravelineSchedule> schedules,
        NpgsqlConnection connection) {
        
        #region build routes
        
        var routes = DatabaseRouteTools.GetFromSchedules(schedules: schedules);
        
        #endregion
        
        #region create table
        
        var command = new NpgsqlCommand(
            cmdText: "create table if not exists gtfs_routes (" +
                     "route_id character varying(255) primary key, " +
                     "agency_id character varying(255), " +
                     "route_short_name character varying(255), " +
                     "route_long_name character varying(255), " +
                     "route_desc character varying(255), " +
                     "route_type smallint not null, " +
                     "route_url character varying(255), " +
                     "route_color character varying(255), " +
                     "route_text_color character varying(255), " +
                     "route_sort_order smallint)",
            connection: connection);
        
        await command.ExecuteNonQueryAsync();
        
        #endregion
        
        #region truncate table
        
        command = new NpgsqlCommand(
            cmdText: "truncate table gtfs_routes",
            connection: connection);
        
        await command.ExecuteNonQueryAsync();
        
        #endregion
        
        #region create importer
        
        var importer = await connection.BeginBinaryImportAsync(copyFromCommand: "copy gtfs_routes (" +
                                                                                "route_id, " +
                                                                                "agency_id, " +
                                                                                "route_short_name, " +
                                                                                "route_long_name, " +
                                                                                "route_desc, " +
                                                                                "route_type, " +
                                                                                "route_url, " +
                                                                                "route_color, " +
                                                                                "route_text_color, " +
                                                                                "route_sort_order) " +
                                                                                "from stdin (format binary)");
        
        #endregion
        
        #region build results
        
        foreach (var item in routes.Values)
        {
            await importer.StartRowAsync();
            
            await importer.WriteAsync(
                value: item.RouteId,
                npgsqlDbType: NpgsqlDbType.Varchar);
            
            await importer.WriteAsync(
                value: item.AgencyId,
                npgsqlDbType: NpgsqlDbType.Varchar);
            
            await importer.WriteAsync(
                value: item.RouteShortName,
                npgsqlDbType: NpgsqlDbType.Varchar);
            
            await importer.WriteAsync(
                value: item.RouteLongName,
                npgsqlDbType: NpgsqlDbType.Varchar);
            
            await importer.WriteAsync(
                value: item.RouteDesc,
                npgsqlDbType: NpgsqlDbType.Varchar);
            
            await importer.WriteAsync(
                value: item.RouteType,
                npgsqlDbType: NpgsqlDbType.Smallint);
            
            await importer.WriteAsync(
                value: item.RouteUrl,
                npgsqlDbType: NpgsqlDbType.Varchar);
            
            await importer.WriteAsync(
                value: item.RouteColor,
                npgsqlDbType: NpgsqlDbType.Varchar);
            
            await importer.WriteAsync(
                value: item.RouteTextColor,
                npgsqlDbType: NpgsqlDbType.Varchar);
            
            await importer.WriteAsync(
                value: item.RouteSortOrder,
                npgsqlDbType: NpgsqlDbType.Smallint);
        }
        
        var results = await importer.CompleteAsync();
        await importer.CloseAsync();
        
        #endregion
        
        return results;
    }
}
using Npgsql;
using NpgsqlTypes;
using TramTimes.Database.Jobs.Models;
using TramTimes.Database.Jobs.Tools;

namespace TramTimes.Database.Jobs.Builders;

public static class DatabaseRouteBuilder
{
    public static async Task<ulong> BuildAsync(
        Dictionary<string, TravelineSchedule> schedules,
        NpgsqlDataSource dataSource) {
        
        #region build routes
        
        var routes = DatabaseRouteTools.GetFromSchedules(schedules: schedules);
        
        #endregion
        
        #region build results
        
        const string sql = "copy gtfs_routes (" +
                           "route_id, " +
                           "agency_id, " +
                           "route_short_name, " +
                           "route_long_name, " +
                           "route_desc, " +
                           "route_type, " +
                           "route_url, " +
                           "route_color, " +
                           "route_text_color, " +
                           "route_sort_order)";
        
        await using var connection = await dataSource.OpenConnectionAsync();
        
        var command = new NpgsqlCommand(
            cmdText: "truncate table gtfs_routes",
            connection: connection);
        
        await command.ExecuteNonQueryAsync();
        
        var importer = await connection.BeginBinaryImportAsync(copyFromCommand: $"{sql} from stdin (format binary)");
        
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
        await connection.CloseAsync();
        
        #endregion
        
        return results;
    }
}
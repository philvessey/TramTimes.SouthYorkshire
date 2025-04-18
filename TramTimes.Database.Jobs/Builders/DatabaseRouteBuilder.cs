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
        
        var routes = await DatabaseRouteTools.GetFromSchedulesAsync(schedules: schedules);
        
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
        
        var connection = await dataSource.OpenConnectionAsync();
        
        var command = new NpgsqlCommand(cmdText: "truncate table gtfs_routes", connection: connection);
        await command.ExecuteNonQueryAsync();
        
        var importer = await connection.BeginBinaryImportAsync(copyFromCommand: $"{sql} from stdin (format binary)");
        
        foreach (var value in routes.Values)
        {
            await importer.StartRowAsync();
            
            await importer.WriteAsync(
                value: value.RouteId,
                npgsqlDbType: NpgsqlDbType.Varchar);
            
            await importer.WriteAsync(
                value: value.AgencyId,
                npgsqlDbType: NpgsqlDbType.Varchar);
            
            await importer.WriteAsync(
                value: value.RouteShortName,
                npgsqlDbType: NpgsqlDbType.Varchar);
            
            await importer.WriteAsync(
                value: value.RouteLongName,
                npgsqlDbType: NpgsqlDbType.Varchar);
            
            await importer.WriteAsync(
                value: value.RouteDesc,
                npgsqlDbType: NpgsqlDbType.Varchar);
            
            await importer.WriteAsync(
                value: value.RouteType,
                npgsqlDbType: NpgsqlDbType.Smallint);
            
            await importer.WriteAsync(
                value: value.RouteUrl,
                npgsqlDbType: NpgsqlDbType.Varchar);
            
            await importer.WriteAsync(
                value: value.RouteColor,
                npgsqlDbType: NpgsqlDbType.Varchar);
            
            await importer.WriteAsync(
                value: value.RouteTextColor,
                npgsqlDbType: NpgsqlDbType.Varchar);
            
            await importer.WriteAsync(
                value: value.RouteSortOrder,
                npgsqlDbType: NpgsqlDbType.Smallint);
        }
        
        return await Task.FromResult(result: await importer.CompleteAsync());
    }
}
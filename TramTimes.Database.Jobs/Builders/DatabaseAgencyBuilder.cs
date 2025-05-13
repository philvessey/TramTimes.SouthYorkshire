using Npgsql;
using NpgsqlTypes;
using TramTimes.Database.Jobs.Models;
using TramTimes.Database.Jobs.Tools;

namespace TramTimes.Database.Jobs.Builders;

public static class DatabaseAgencyBuilder
{
    public static async Task<ulong> BuildAsync(
        Dictionary<string, TravelineSchedule> schedules,
        NpgsqlDataSource dataSource) {
        
        var agencies = await DatabaseAgencyTools.GetFromSchedulesAsync(schedules: schedules);
        
        const string sql = "copy gtfs_agency (" +
                           "agency_id, " +
                           "agency_name, " +
                           "agency_url, " +
                           "agency_timezone, " +
                           "agency_lang, " +
                           "agency_phone, " +
                           "agency_fare_url, " +
                           "agency_email)";
        
        var connection = await dataSource.OpenConnectionAsync();
        
        var command = new NpgsqlCommand(
            cmdText: "truncate table gtfs_agency",
            connection: connection);
        
        await command.ExecuteNonQueryAsync();
        
        var importer = await connection.BeginBinaryImportAsync(copyFromCommand: $"{sql} from stdin (format binary)");
        
        foreach (var item in agencies.Values)
        {
            await importer.StartRowAsync();
            
            await importer.WriteAsync(
                value: item.AgencyId,
                npgsqlDbType: NpgsqlDbType.Varchar);
            
            await importer.WriteAsync(
                value: item.AgencyName,
                npgsqlDbType: NpgsqlDbType.Varchar);
            
            await importer.WriteAsync(
                value: item.AgencyUrl,
                npgsqlDbType: NpgsqlDbType.Varchar);
            
            await importer.WriteAsync(
                value: item.AgencyTimezone,
                npgsqlDbType: NpgsqlDbType.Varchar);
            
            await importer.WriteAsync(
                value: item.AgencyLang,
                npgsqlDbType: NpgsqlDbType.Varchar);
            
            await importer.WriteAsync(
                value: item.AgencyPhone,
                npgsqlDbType: NpgsqlDbType.Varchar);
            
            await importer.WriteAsync(
                value: item.AgencyFareUrl,
                npgsqlDbType: NpgsqlDbType.Varchar);
            
            await importer.WriteAsync(
                value: item.AgencyEmail,
                npgsqlDbType: NpgsqlDbType.Varchar);
        }
        
        return await Task.FromResult(result: await importer.CompleteAsync());
    }
}
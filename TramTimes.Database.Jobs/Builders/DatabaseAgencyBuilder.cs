using Npgsql;
using NpgsqlTypes;
using TramTimes.Database.Jobs.Models;
using TramTimes.Database.Jobs.Tools;

namespace TramTimes.Database.Jobs.Builders;

public static class DatabaseAgencyBuilder
{
    public static async Task<ulong> BuildAsync(
        Dictionary<string, TravelineSchedule> schedules,
        NpgsqlConnection connection) {
        
        #region build agencies
        
        var agencies = DatabaseAgencyTools.GetFromSchedules(schedules: schedules);
        
        #endregion
        
        #region create table
        
        var command = new NpgsqlCommand(
            cmdText: "create table if not exists gtfs_agency (" +
                     "agency_id character varying(255), " +
                     "agency_name character varying(255) not null, " +
                     "agency_url character varying(255) not null, " +
                     "agency_timezone character varying(255) not null, " +
                     "agency_lang character varying(255), " +
                     "agency_phone character varying(255), " +
                     "agency_fare_url character varying(255), " +
                     "agency_email character varying(255))",
            connection: connection);
        
        await command.ExecuteNonQueryAsync();
        
        #endregion
        
        #region truncate table
        
        command = new NpgsqlCommand(
            cmdText: "truncate table gtfs_agency",
            connection: connection);
        
        await command.ExecuteNonQueryAsync();
        
        #endregion
        
        #region create importer
        
        var importer = await connection.BeginBinaryImportAsync(copyFromCommand: "copy gtfs_agency (" +
                                                                                "agency_id, " +
                                                                                "agency_name, " +
                                                                                "agency_url, " +
                                                                                "agency_timezone, " +
                                                                                "agency_lang, " +
                                                                                "agency_phone, " +
                                                                                "agency_fare_url, " +
                                                                                "agency_email) " +
                                                                                "from stdin (format binary)");
        
        #endregion
        
        #region build results
        
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
        
        var results = await importer.CompleteAsync();
        await importer.CloseAsync();
        
        #endregion
        
        return results;
    }
}
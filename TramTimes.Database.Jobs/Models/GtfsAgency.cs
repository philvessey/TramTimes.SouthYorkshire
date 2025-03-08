using CsvHelper.Configuration.Attributes;
using JetBrains.Annotations;

namespace TramTimes.Database.Jobs.Models;

public class GtfsAgency
{
    [UsedImplicitly]
    [Name(name: "agency_id")]
    public string? AgencyId { get; set; }
    
    [UsedImplicitly]
    [Name(name: "agency_name")]
    public string? AgencyName { get; set; }
    
    [UsedImplicitly]
    [Name(name: "agency_url")]
    public string? AgencyUrl { get; set; }
    
    [UsedImplicitly]
    [Name(name: "agency_timezone")]
    public string? AgencyTimezone { get; set; }
    
    [UsedImplicitly]
    [Name(name: "agency_lang")]
    public string? AgencyLang { get; set; }
    
    [UsedImplicitly]
    [Name(name: "agency_phone")]
    public string? AgencyPhone { get; set; }
    
    [UsedImplicitly]
    [Name(name: "agency_fare_url")]
    public string? AgencyFareUrl { get; set; }
    
    [UsedImplicitly]
    [Name(name: "agency_email")]
    public string? AgencyEmail { get; set; }
}
using CsvHelper.Configuration.Attributes;
using JetBrains.Annotations;

namespace TramTimes.Database.Jobs.Models;

public class NaptanStop
{
    [UsedImplicitly]
    [Name(name: "ATCOCode")]
    public string? AtcoCode { get; set; }
    
    [UsedImplicitly]
    [Name(name: "NaptanCode")]
    public string? NaptanCode { get; set; }
    
    [UsedImplicitly]
    [Name(name: "PlateCode")]
    public string? PlateCode { get; set; }
    
    [UsedImplicitly]
    [Name(name: "CleardownCode")]
    public string? CleardownCode { get; set; }
    
    [UsedImplicitly]
    [Name(name: "CommonName")]
    public string? CommonName { get; set; }
    
    [UsedImplicitly]
    [Name(name: "CommonNameLang")]
    public string? CommonNameLang { get; set; }
    
    [UsedImplicitly]
    [Name(name: "ShortCommonName")]
    public string? ShortCommonName { get; set; }
    
    [UsedImplicitly]
    [Name(name: "ShortCommonNameLang")]
    public string? ShortCommonNameLang { get; set; }
    
    [UsedImplicitly]
    [Name(name: "Landmark")]
    public string? Landmark { get; set; }
    
    [UsedImplicitly]
    [Name(name: "LandmarkLang")]
    public string? LandmarkLang { get; set; }
    
    [UsedImplicitly]
    [Name(name: "Street")]
    public string? Street { get; set; }
    
    [UsedImplicitly]
    [Name(name: "StreetLang")]
    public string? StreetLang { get; set; }
    
    [UsedImplicitly]
    [Name(name: "Crossing")]
    public string? Crossing { get; set; }
    
    [UsedImplicitly]
    [Name(name: "CrossingLang")]
    public string? CrossingLang { get; set; }
    
    [UsedImplicitly]
    [Name(name: "Indicator")]
    public string? Indicator { get; set; }
    
    [UsedImplicitly]
    [Name(name: "IndicatorLang")]
    public string? IndicatorLang { get; set; }
    
    [UsedImplicitly]
    [Name(name: "Bearing")]
    public string? Bearing { get; set; }
    
    [UsedImplicitly]
    [Name(name: "NptgLocalityCode")]
    public string? NptgLocalityCode { get; set; }
    
    [UsedImplicitly]
    [Name(name: "LocalityName")]
    public string? LocalityName { get; set; }
    
    [UsedImplicitly]
    [Name(name: "ParentLocalityName")]
    public string? ParentLocalityName { get; set; }
    
    [UsedImplicitly]
    [Name(name: "GrandParentLocalityName")]
    public string? GrandParentLocalityName { get; set; }
    
    [UsedImplicitly]
    [Name(name: "Town")]
    public string? Town { get; set; }
    
    [UsedImplicitly]
    [Name(name: "TownLang")]
    public string? TownLang { get; set; }
    
    [UsedImplicitly]
    [Name(name: "Suburb")]
    public string? Suburb { get; set; }
    
    [UsedImplicitly]
    [Name(name: "SuburbLang")]
    public string? SuburbLang { get; set; }
    
    [UsedImplicitly]
    [Name(name: "LocalityCentre")]
    public string? LocalityCentre { get; set; }
    
    [UsedImplicitly]
    [Name(name: "GridType")]
    public string? GridType { get; set; }
    
    [UsedImplicitly]
    [Name(name: "Easting")]
    public string? Easting { get; set; }
    
    [UsedImplicitly]
    [Name(name: "Northing")]
    public string? Northing { get; set; }
    
    [UsedImplicitly]
    [Name(name: "Longitude")]
    public string? Longitude { get; set; }
    
    [UsedImplicitly]
    [Name(name: "Latitude")]
    public string? Latitude { get; set; }
    
    [UsedImplicitly]
    [Name(name: "StopType")]
    public string? StopType { get; set; }
    
    [UsedImplicitly]
    [Name(name: "BusStopType")]
    public string? BusStopType { get; set; }
    
    [UsedImplicitly]
    [Name(name: "TimingStatus")]
    public string? TimingStatus { get; set; }
    
    [UsedImplicitly]
    [Name(name: "DefaultWaitTime")]
    public string? DefaultWaitTime { get; set; }
    
    [UsedImplicitly]
    [Name(name: "Notes")]
    public string? Notes { get; set; }
    
    [UsedImplicitly]
    [Name(name: "NotesLang")]
    public string? NotesLang { get; set; }
    
    [UsedImplicitly]
    [Name(name: "AdministrativeAreaCode")]
    public string? AdministrativeAreaCode { get; set; }
}
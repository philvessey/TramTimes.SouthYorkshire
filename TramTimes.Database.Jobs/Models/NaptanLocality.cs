using CsvHelper.Configuration.Attributes;
using JetBrains.Annotations;

namespace TramTimes.Database.Jobs.Models;

public class NaptanLocality
{
    [UsedImplicitly]
    [Name(name: "NptgLocalityCode")]
    public string? NptgLocalityCode { get; set; }
    
    [UsedImplicitly]
    [Name(name: "LocalityName")]
    public string? LocalityName { get; set; }
    
    [UsedImplicitly]
    [Name(name: "LocalityNameLang")]
    public string? LocalityNameLang { get; set; }
    
    [UsedImplicitly]
    [Name(name: "ShortName")]
    public string? ShortName { get; set; }
    
    [UsedImplicitly]
    [Name(name: "ShortNameLang")]
    public string? ShortNameLang { get; set; }
    
    [UsedImplicitly]
    [Name(name: "QualifierName")]
    public string? QualifierName { get; set; }
    
    [UsedImplicitly]
    [Name(name: "QualifierNameLang")]
    public string? QualifierNameLang { get; set; }
    
    [UsedImplicitly]
    [Name(name: "QualifierLocalityRef")]
    public string? QualifierLocalityRef { get; set; }
    
    [UsedImplicitly]
    [Name(name: "QualifierDistrictRef")]
    public string? QualifierDistrictRef { get; set; }
    
    [UsedImplicitly]
    [Name(name: "ParentLocalityName")]
    public string? ParentLocalityName { get; set; }
    
    [UsedImplicitly]
    [Name(name: "ParentLocalityNameLang")]
    public string? ParentLocalityNameLang { get; set; }
    
    [UsedImplicitly]
    [Name(name: "AdministrativeAreaCode")]
    public string? AdministrativeAreaCode { get; set; }
    
    [UsedImplicitly]
    [Name(name: "NptgDistrictCode")]
    public string? NptgDistrictCode { get; set; }
    
    [UsedImplicitly]
    [Name(name: "SourceLocalityType")]
    public string? SourceLocalityType { get; set; }
    
    [UsedImplicitly]
    [Name(name: "GridType")]
    public string? GridType { get; set; }
    
    [UsedImplicitly]
    [Name(name: "Easting")]
    public string? Easting { get; set; }
    
    [UsedImplicitly]
    [Name(name: "Northing")]
    public string? Northing { get; set; }
}
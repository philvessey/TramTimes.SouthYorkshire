using JetBrains.Annotations;

namespace TramTimes.Database.Jobs.Models;

public class NaptanLocality
{
    [UsedImplicitly] public string? NptgLocalityCode { get; set; }
    [UsedImplicitly] public string? LocalityName { get; set; }
    [UsedImplicitly] public string? LocalityNameLang { get; set; }
    [UsedImplicitly] public string? ShortName { get; set; }
    [UsedImplicitly] public string? ShortNameLang { get; set; }
    [UsedImplicitly] public string? QualifierName { get; set; }
    [UsedImplicitly] public string? QualifierNameLang { get; set; }
    [UsedImplicitly] public string? QualifierLocalityRef { get; set; }
    [UsedImplicitly] public string? QualifierDistrictRef { get; set; }
    [UsedImplicitly] public string? ParentLocalityName { get; set; }
    [UsedImplicitly] public string? ParentLocalityNameLang { get; set; }
    [UsedImplicitly] public string? AdministrativeAreaCode { get; set; }
    [UsedImplicitly] public string? NptgDistrictCode { get; set; }
    [UsedImplicitly] public string? SourceLocalityType { get; set; }
    [UsedImplicitly] public string? GridType { get; set; }
    [UsedImplicitly] public string? Easting { get; set; }
    [UsedImplicitly] public string? Northing { get; set; }
}
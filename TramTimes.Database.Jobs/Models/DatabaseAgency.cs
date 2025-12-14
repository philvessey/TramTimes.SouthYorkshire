using JetBrains.Annotations;

namespace TramTimes.Database.Jobs.Models;

public class DatabaseAgency
{
    [UsedImplicitly] public string? AgencyId { get; set; }
    [UsedImplicitly] public string? AgencyName { get; set; }
    [UsedImplicitly] public string? AgencyUrl { get; set; }
    [UsedImplicitly] public string? AgencyTimezone { get; set; }
    [UsedImplicitly] public string? AgencyLang { get; set; }
    [UsedImplicitly] public string? AgencyPhone { get; set; }
    [UsedImplicitly] public string? AgencyFareUrl { get; set; }
    [UsedImplicitly] public string? AgencyEmail { get; set; }
}
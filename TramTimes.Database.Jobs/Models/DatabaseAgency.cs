// ReSharper disable all

using JetBrains.Annotations;

namespace TramTimes.Database.Jobs.Models;

public class DatabaseAgency
{
    [UsedImplicitly] public string? agency_id { get; set; }
    [UsedImplicitly] public string? agency_name { get; set; }
    [UsedImplicitly] public string? agency_url { get; set; }
    [UsedImplicitly] public string? agency_timezone { get; set; }
    [UsedImplicitly] public string? agency_lang { get; set; }
    [UsedImplicitly] public string? agency_phone { get; set; }
    [UsedImplicitly] public string? agency_fare_url { get; set; }
    [UsedImplicitly] public string? agency_email { get; set; }
}
using CsvHelper.Configuration.Attributes;
using JetBrains.Annotations;

namespace TramTimes.Database.Jobs.Models;

public class GtfsCalendar
{
    [UsedImplicitly]
    [Name(name: "service_id")]
    public string? ServiceId { get; set; }

    [UsedImplicitly]
    [Name(name: "monday")]
    public string? Monday { get; set; }

    [UsedImplicitly]
    [Name(name: "tuesday")]
    public string? Tuesday { get; set; }

    [UsedImplicitly]
    [Name(name: "wednesday")]
    public string? Wednesday { get; set; }

    [UsedImplicitly]
    [Name(name: "thursday")]
    public string? Thursday { get; set; }

    [UsedImplicitly]
    [Name(name: "friday")]
    public string? Friday { get; set; }

    [UsedImplicitly]
    [Name(name: "saturday")]
    public string? Saturday { get; set; }

    [UsedImplicitly]
    [Name(name: "sunday")]
    public string? Sunday { get; set; }

    [UsedImplicitly]
    [Name(name: "start_date")]
    public string? StartDate { get; set; }

    [UsedImplicitly]
    [Name(name: "end_date")]
    public string? EndDate { get; set; }
}
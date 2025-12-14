using CsvHelper.Configuration.Attributes;
using JetBrains.Annotations;

namespace TramTimes.Database.Jobs.Models;

public class GtfsCalendarDate
{
    [UsedImplicitly]
    [Name(name: "service_id")]
    public string? ServiceId { get; set; }

    [UsedImplicitly]
    [Name(name: "date")]
    public string? ExceptionDate { get; set; }

    [UsedImplicitly]
    [Name(name: "exception_type")]
    public string? ExceptionType { get; set; }
}
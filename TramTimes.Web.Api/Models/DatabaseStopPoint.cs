using JetBrains.Annotations;

namespace TramTimes.Web.Api.Models;

public class DatabaseStopPoint
{
    [UsedImplicitly] public DateTime? DepartureDateTime { get; set; }
    [UsedImplicitly] public string? DestinationName { get; set; }
    [UsedImplicitly] public string? RouteName { get; set; }
    [UsedImplicitly] public string? StopId { get; set; }
    [UsedImplicitly] public string? StopName { get; set; }
    [UsedImplicitly] public string? TripId { get; set; }
}
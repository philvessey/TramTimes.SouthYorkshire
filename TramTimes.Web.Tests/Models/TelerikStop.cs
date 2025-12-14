using JetBrains.Annotations;

namespace TramTimes.Web.Tests.Models;

public class TelerikStop
{
    [UsedImplicitly] public string? Id { get; set; }
    [UsedImplicitly] public string? Code { get; set; }
    [UsedImplicitly] public string? Name { get; set; }
    [UsedImplicitly] public double? Latitude { get; set; }
    [UsedImplicitly] public double? Longitude { get; set; }
    [UsedImplicitly] public string? Platform { get; set; }
    [UsedImplicitly] public string? Direction { get; set; }
    [UsedImplicitly] public double? Distance { get; set; }
    [UsedImplicitly] public double[]? Location { get; set; }
    [UsedImplicitly] public List<TelerikStopPoint>? Points { get; set; }
}
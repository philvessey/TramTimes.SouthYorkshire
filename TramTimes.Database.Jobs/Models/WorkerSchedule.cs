using System.Text.Json.Serialization;
using JetBrains.Annotations;

namespace TramTimes.Database.Jobs.Models;

public class WorkerSchedule
{
    [UsedImplicitly]
    [JsonPropertyName(name: "monday")]
    public List<WorkerStopPoint>? Monday { get; set; }

    [UsedImplicitly]
    [JsonPropertyName(name: "tuesday")]
    public List<WorkerStopPoint>? Tuesday { get; set; }

    [UsedImplicitly]
    [JsonPropertyName(name: "wednesday")]
    public List<WorkerStopPoint>? Wednesday { get; set; }

    [UsedImplicitly]
    [JsonPropertyName(name: "thursday")]
    public List<WorkerStopPoint>? Thursday { get; set; }

    [UsedImplicitly]
    [JsonPropertyName(name: "friday")]
    public List<WorkerStopPoint>? Friday { get; set; }

    [UsedImplicitly]
    [JsonPropertyName(name: "saturday")]
    public List<WorkerStopPoint>? Saturday { get; set; }

    [UsedImplicitly]
    [JsonPropertyName(name: "sunday")]
    public List<WorkerStopPoint>? Sunday { get; set; }
}
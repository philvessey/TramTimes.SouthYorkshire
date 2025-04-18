using System.Text.Json.Serialization;
using JetBrains.Annotations;

namespace TramTimes.Database.Jobs.Models;

public class WorkerSchedule
{
    [UsedImplicitly]
    [JsonPropertyName("monday")]
    public List<WorkerStopPoint>? Monday { get; set; }
    
    [UsedImplicitly]
    [JsonPropertyName("tuesday")]
    public List<WorkerStopPoint>? Tuesday { get; set; }
    
    [UsedImplicitly]
    [JsonPropertyName("wednesday")]
    public List<WorkerStopPoint>? Wednesday { get; set; }
    
    [UsedImplicitly]
    [JsonPropertyName("thursday")]
    public List<WorkerStopPoint>? Thursday { get; set; }
    
    [UsedImplicitly]
    [JsonPropertyName("friday")]
    public List<WorkerStopPoint>? Friday { get; set; }
    
    [UsedImplicitly]
    [JsonPropertyName("saturday")]
    public List<WorkerStopPoint>? Saturday { get; set; }
    
    [UsedImplicitly]
    [JsonPropertyName("sunday")]
    public List<WorkerStopPoint>? Sunday { get; set; }
}
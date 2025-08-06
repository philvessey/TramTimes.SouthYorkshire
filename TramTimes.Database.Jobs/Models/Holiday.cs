using System.Text.Json.Serialization;
using JetBrains.Annotations;

namespace TramTimes.Database.Jobs.Models;

public class Holiday
{
    [UsedImplicitly]
    [JsonPropertyName(name: "date")]
    public DateOnly? Date { get; set; }
    
    [UsedImplicitly]
    [JsonPropertyName(name: "localName")]
    public string? LocalName { get; set; }
    
    [UsedImplicitly]
    [JsonPropertyName(name: "global")]
    public bool? Global { get; set; }
    
    [UsedImplicitly]
    [JsonPropertyName(name: "counties")]
    public string[]? Counties { get; set; }
}
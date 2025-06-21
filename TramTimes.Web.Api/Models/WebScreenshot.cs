using System.Text.Json.Serialization;
using JetBrains.Annotations;

namespace TramTimes.Web.Api.Models;

public class WebScreenshot
{
    [UsedImplicitly]
    [JsonPropertyName(name: "endpoint")]
    public string? Endpoint { get; set; }
}
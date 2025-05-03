using Elastic.Clients.Elasticsearch;
using JetBrains.Annotations;

namespace TramTimes.Web.Api.Models;

public class SearchStop
{
    [UsedImplicitly]
    public string? Id { get; set; }
    
    [UsedImplicitly]
    public string? Code { get; set; }
    
    [UsedImplicitly]
    public string? Name { get; set; }
    
    [UsedImplicitly]
    public double? Latitude { get; set; }
    
    [UsedImplicitly]
    public double? Longitude { get; set; }
    
    [UsedImplicitly]
    public GeoLocation? Location { get; set; }
    
    [UsedImplicitly]
    public List<SearchStopPoint>? Points { get; set; }
}
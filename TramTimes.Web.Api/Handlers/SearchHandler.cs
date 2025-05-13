using AutoMapper;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using TramTimes.Web.Api.Models;
using TramTimes.Web.Api.Tools;
using TramTimes.Web.Utilities.Extensions;
using TramTimes.Web.Utilities.Models;

namespace TramTimes.Web.Api.Handlers;

public static class SearchHandler
{
    public static async Task<IResult> GetStopsByIdAsync(
        ElasticsearchClient searchService,
        IMapper mapperService,
        string id) {
        
        var request = new SearchRequest(indices: "search")
        {
            Query = new TermQuery(field: new Field(name: "id"))
            {
                Value = id.ToUpperInvariant()
            }
        };
        
        var response = await searchService.SearchAsync<SearchStop>(request: request);
        
        if (!response.IsValidResponse || response.Documents.IsNullOrEmpty())
            return Results.NotFound();
        
        return Results.Json(data: mapperService.Map<List<WebStop>>(
            source: response.Documents
                .OrderBy(keySelector: stop => stop.Name)
                .ThenBy(keySelector: stop => stop.Id)));
    }
    
    public static async Task<IResult> GetStopsByCodeAsync(
        ElasticsearchClient searchService,
        IMapper mapperService,
        string code) {
        
        var request = new SearchRequest(indices: "search")
        {
            Query = new TermQuery(field: new Field(name: "code"))
            {
                Value = code.ToUpperInvariant()
            }
        };
        
        var response = await searchService.SearchAsync<SearchStop>(request: request);
        
        if (!response.IsValidResponse || response.Documents.IsNullOrEmpty())
            return Results.NotFound();
        
        return Results.Json(data: mapperService.Map<List<WebStop>>(
            source: response.Documents
                .OrderBy(keySelector: stop => stop.Name)
                .ThenBy(keySelector: stop => stop.Id)));
    }
    
    public static async Task<IResult> GetStopsByNameAsync(
        ElasticsearchClient searchService,
        IMapper mapperService,
        string name) {
        
        var request = new SearchRequest(indices: "search")
        {
            Query = new WildcardQuery(field: new Field(name: "name"))
            {
                CaseInsensitive = true,
                Value = $"*{name.ToLowerInvariant()}*"
            }
        };
        
        var response = await searchService.SearchAsync<SearchStop>(request: request);
        
        if (!response.IsValidResponse || response.Documents.IsNullOrEmpty())
            return Results.NotFound();
        
        return Results.Json(data: mapperService.Map<List<WebStop>>(
            source: response.Documents
                .OrderBy(keySelector: stop => stop.Name)
                .ThenBy(keySelector: stop => stop.Id)));
    }
    
    public static async Task<IResult> GetStopsByLocationAsync(
        ElasticsearchClient searchService,
        IMapper mapperService,
        double minLon,
        double minLat,
        double maxLon,
        double maxLat) {
        
        var request = new SearchRequest(indices: "search")
        {
            Query = new GeoBoundingBoxQuery
            {
                Field = new Field(name: "location"),
                BoundingBox = GeoBounds.TopRightBottomLeft(topRightBottomLeft: new TopRightBottomLeftGeoBounds
                {
                    BottomLeft = GeoLocation.LatitudeLongitude(latitudeLongitude: new LatLonGeoLocation
                    {
                        Lat = minLat,
                        Lon = minLon
                    }),
                    TopRight = GeoLocation.LatitudeLongitude(latitudeLongitude: new LatLonGeoLocation
                    {
                        Lat = maxLat,
                        Lon = maxLon
                    })
                })
            },
            Sort = SearchTools.SortDistance(location: new LatLonGeoLocation
            {
                Lat = (minLat + maxLat) / 2,
                Lon = (minLon + maxLon) / 2
            })
        };
        
        var response = await searchService.SearchAsync<SearchStop>(request: request);
        
        if (!response.IsValidResponse || response.Documents.IsNullOrEmpty())
            return Results.NotFound();
        
        return Results.Json(data: mapperService.Map<List<WebStop>>(source: response.Documents));
    }
}
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
        
        #region build request
        
        var request = new SearchRequest(indices: "southyorkshire")
        {
            Query = new TermQuery(field: new Field(name: "id"))
            {
                Value = id.ToUpperInvariant()
            },
            Size = 1000,
            Sort = SearchTools.SortByNameThenById()
        };
        
        #endregion
        
        #region build results
        
        var results = await searchService.SearchAsync<SearchStop>(request: request);
        
        if (!results.IsValidResponse || results.Documents.IsNullOrEmpty())
            return Results.NotFound();
        
        #endregion
        
        return Results.Json(data: mapperService.Map<List<WebStop>>(source: results.Documents));
    }
    
    public static async Task<IResult> GetStopsByCodeAsync(
        ElasticsearchClient searchService,
        IMapper mapperService,
        string code) {
        
        #region build request
        
        var request = new SearchRequest(indices: "southyorkshire")
        {
            Query = new TermQuery(field: new Field(name: "code"))
            {
                Value = code.ToLowerInvariant()
            },
            Size = 1000,
            Sort = SearchTools.SortByNameThenById()
        };
        
        #endregion
        
        #region build results
        
        var results = await searchService.SearchAsync<SearchStop>(request: request);
        
        if (!results.IsValidResponse || results.Documents.IsNullOrEmpty())
            return Results.NotFound();
        
        #endregion
        
        return Results.Json(data: mapperService.Map<List<WebStop>>(source: results.Documents));
    }
    
    public static async Task<IResult> GetStopsByNameAsync(
        ElasticsearchClient searchService,
        IMapper mapperService,
        string name) {
        
        #region build request
        
        var request = new SearchRequest(indices: "southyorkshire")
        {
            Query = new WildcardQuery(field: new Field(name: "name"))
            {
                CaseInsensitive = true,
                Value = $"*{name.ToLowerInvariant()}*"
            },
            Size = 1000,
            Sort = SearchTools.SortByNameThenById()
        };
        
        #endregion
        
        #region build results
        
        var results = await searchService.SearchAsync<SearchStop>(request: request);
        
        if (!results.IsValidResponse || results.Documents.IsNullOrEmpty())
            return Results.NotFound();
        
        #endregion
        
        return Results.Json(data: mapperService.Map<List<WebStop>>(source: results.Documents));
    }
    
    public static async Task<IResult> GetStopsByLocationAsync(
        ElasticsearchClient searchService,
        IMapper mapperService,
        double minLon,
        double minLat,
        double maxLon,
        double maxLat) {
        
        #region build request
        
        var request = new SearchRequest(indices: "southyorkshire")
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
            Size = 1000,
            Sort = SearchTools.SortByDistance(location: new LatLonGeoLocation
            {
                Lat = (minLat + maxLat) / 2,
                Lon = (minLon + maxLon) / 2
            })
        };
        
        #endregion
        
        #region build results
        
        var results = await searchService.SearchAsync<SearchStop>(request: request);
        
        if (!results.IsValidResponse || results.Documents.IsNullOrEmpty())
            return Results.NotFound();
        
        #endregion
        
        return Results.Json(data: mapperService.Map<List<WebStop>>(source: results.Documents));
    }
    
    public static async Task<IResult> GetStopsByPointAsync(
        ElasticsearchClient searchService,
        IMapper mapperService,
        double lon,
        double lat) {
        
        #region build request
        
        var request = new SearchRequest(indices: "southyorkshire")
        {
            Query = new GeoDistanceQuery
            {
                Field = new Field(name: "location"),
                Distance = "1km",
                Location = GeoLocation.LatitudeLongitude(latitudeLongitude: new LatLonGeoLocation
                {
                    Lat = lat,
                    Lon = lon
                })
            },
            Size = 1000,
            Sort = SearchTools.SortByDistance(location: new LatLonGeoLocation
            {
                Lat = lat,
                Lon = lon
            })
        };
        
        #endregion
        
        #region build results
        
        var results = await searchService.SearchAsync<SearchStop>(request: request);
        
        if (!results.IsValidResponse || results.Documents.IsNullOrEmpty())
            return Results.NotFound();
        
        #endregion
        
        return Results.Json(data: mapperService.Map<List<WebStop>>(source: results.Documents));
    }
}
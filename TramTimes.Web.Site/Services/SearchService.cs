using AutoMapper;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using TramTimes.Web.Utilities.Extensions;
using TramTimes.Web.Utilities.Models;
using TramTimes.Web.Utilities.Tools;

namespace TramTimes.Web.Site.Services;

public class SearchService(
    ElasticsearchClient searchService,
    IMapper mapperService) {

    public async Task<List<WebStop>?> GetStopsByIdAsync(
        string id,
        CancellationToken cancellationToken = default) {

        #region build request

        var request = new SearchRequest
        {
            Indices = "southyorkshire",
            Query = new TermQuery
            {
                Field = "id",
                Value = id.ToUpperInvariant()
            },
            Size = 1000,
            Sort = SearchTools.SortByNameThenById()
        };

        #endregion

        #region build results

        var results = await searchService.SearchAsync<SearchStop>(
            request: request,
            cancellationToken: cancellationToken);

        if (!results.IsValidResponse || results.Documents.IsNullOrEmpty())
            return null;

        #endregion

        return mapperService.Map<List<WebStop>>(source: results.Documents);
    }

    public async Task<List<WebStop>?> GetStopsByCodeAsync(
        string code,
        CancellationToken cancellationToken = default) {

        #region build request

        var request = new SearchRequest
        {
            Indices = "southyorkshire",
            Query = new TermQuery
            {
                Field = "code",
                Value = code.ToLowerInvariant()
            },
            Size = 1000,
            Sort = SearchTools.SortByNameThenById()
        };

        #endregion

        #region build results

        var results = await searchService.SearchAsync<SearchStop>(
            request: request,
            cancellationToken: cancellationToken);

        if (!results.IsValidResponse || results.Documents.IsNullOrEmpty())
            return null;

        #endregion

        return mapperService.Map<List<WebStop>>(source: results.Documents);
    }

    public async Task<List<WebStop>?> GetStopsByNameAsync(
        string name,
        CancellationToken cancellationToken = default) {

        #region build request

        var request = new SearchRequest
        {
            Indices = "southyorkshire",
            Query = new WildcardQuery
            {
                Field = "name",
                CaseInsensitive = true,
                Value = $"*{name.ToLowerInvariant()}*"
            },
            Size = 1000,
            Sort = SearchTools.SortByNameThenById()
        };

        #endregion

        #region build results

        var results = await searchService.SearchAsync<SearchStop>(
            request: request,
            cancellationToken: cancellationToken);

        if (!results.IsValidResponse || results.Documents.IsNullOrEmpty())
            return null;

        #endregion

        return mapperService.Map<List<WebStop>>(source: results.Documents);
    }

    public async Task<List<WebStop>?> GetStopsByLocationAsync(
        double minLon,
        double minLat,
        double maxLon,
        double maxLat,
        CancellationToken cancellationToken = default) {

        #region build request

        var request = new SearchRequest
        {
            Indices = "southyorkshire",
            Query = new GeoBoundingBoxQuery
            {
                Field = "location",
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

        var results = await searchService.SearchAsync<SearchStop>(
            request: request,
            cancellationToken: cancellationToken);

        if (!results.IsValidResponse || results.Documents.IsNullOrEmpty())
            return null;

        #endregion

        return mapperService.Map<List<WebStop>>(source: results.Documents);
    }

    public async Task<List<WebStop>?> GetStopsByPointAsync(
        double lon,
        double lat,
        CancellationToken cancellationToken = default) {

        #region build request

        var request = new SearchRequest
        {
            Indices = "southyorkshire",
            Query = new GeoDistanceQuery
            {
                Field = "location",
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

        var results = await searchService.SearchAsync<SearchStop>(
            request: request,
            cancellationToken: cancellationToken);

        if (!results.IsValidResponse || results.Documents.IsNullOrEmpty())
            return null;

        #endregion

        return mapperService.Map<List<WebStop>>(source: results.Documents);
    }
}
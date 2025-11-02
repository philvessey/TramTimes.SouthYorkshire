using AutoMapper;
using Elastic.Clients.Elasticsearch;
using NextDepartures.Standard;
using NextDepartures.Standard.Types;
using NextDepartures.Storage.Postgres.Aspire;
using Npgsql;
using TramTimes.Web.Api.Models;

namespace TramTimes.Web.Api.Builders;

public static class IndexBuilder
{
    public static async Task Build(
        NpgsqlDataSource dataSource,
        ElasticsearchClient searchService,
        IMapper mapperService,
        string id) {
        
        #region get database feed
        
        var databaseFeed = await Feed.LoadAsync(dataStorage: PostgresStorage.Load(dataSource: dataSource));
        
        var stopResults = await databaseFeed.GetStopsByIdAsync(
            id: id,
            comparison: ComparisonType.Exact);
        
        var serviceResults = await databaseFeed.GetServicesByStopAsync(
            id: id,
            comparison: ComparisonType.Exact,
            tolerance: TimeSpan.FromMinutes(value: 179));
        
        var databaseResults = mapperService.Map<List<SearchStop>>(source: stopResults).FirstOrDefault() ?? new SearchStop();
        databaseResults.Points = mapperService.Map<List<SearchStopPoint>>(source: serviceResults) ?? [];
        
        #endregion
        
        #region check database feed
        
        if (databaseResults is { Latitude: not null, Longitude: not null })
            databaseResults.Location = GeoLocation.LatitudeLongitude(latitudeLongitude: new LatLonGeoLocation 
            {
                Lat = databaseResults.Latitude.Value,
                Lon = databaseResults.Longitude.Value
            });
        
        #endregion
        
        #region set search feed
        
        await searchService.IndexAsync(request: new IndexRequest<SearchStop>
        {
            Document = databaseResults,
            Id = databaseResults.Id ?? id,
            Index = "southyorkshire"
        });
        
        #endregion
    }
}
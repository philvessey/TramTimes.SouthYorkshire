using AutoMapper;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Mapping;
using Npgsql;
using StackExchange.Redis;
using TramTimes.Web.Api.Builders;
using TramTimes.Web.Api.Models;
using TramTimes.Web.Utilities.Extensions;

namespace TramTimes.Web.Api.Handlers;

public static class WebHandler
{
    public static async Task<IResult> PostCacheByBuildAsync(
        NpgsqlDataSource dataSource,
        IConnectionMultiplexer cacheService,
        IMapper mapperService) {
        
        #region flush keys
        
        await cacheService
            .GetDatabase()
            .ExecuteAsync(command: "flushdb");
        
        #endregion
        
        #region arbourthorne road
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYABR1");
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYABR2");
        
        #endregion
        
        #region attercliffe
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYATT1");
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYATT2");
        
        #endregion
        
        #region bamforth street
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYBAM1");
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYBAM2");
        
        #endregion
        
        #region birley moor road
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYBMR1");
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYBMR2");
        
        #endregion
        
        #region birley lane
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYBRL1");
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYBRL2");
        
        #endregion
        
        #region castle square
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYCAS1");
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYCAS2");
        
        #endregion
        
        #region cathedral
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYCAT1");
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYCAT2");
        
        #endregion
        
        #region cricket inn road
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYCIR1");
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYCIR2");
        
        #endregion
        
        #region carbrook
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYCRB1");
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYCRB2");
        
        #endregion
        
        #region crystal peaks
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYCRY1");
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYCRY2");
        
        #endregion
        
        #region city hall
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYCYH1");
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYCYH2");
        
        #endregion
        
        #region drake house lane
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYDHL1");
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYDHL2");
        
        #endregion
        
        #region donetsk way
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYDON1");
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYDON2");
        
        #endregion
        
        #region arena / olympic legacy park
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYDVS1");
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYDVS2");
        
        #endregion
        
        #region fitzalan sq - ponds forge
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYFIZ1");
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYFIZ2");
        
        #endregion
        
        #region gleadless townend
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYGLE1");
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYGLE2");
        
        #endregion
        
        #region granville rd - sheffield college
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYGRC1");
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYGRC2");
        
        #endregion
        
        #region hackenthorpe
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYHAK1");
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYHAK2");
        
        #endregion
        
        #region herdings park
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYHDP1");
        
        #endregion
        
        #region halfway
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYHFW1");
        
        #endregion
        
        #region hillsborough
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYHIL1");
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYHIL2");
        
        #endregion
        
        #region hillsborough park
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYHLP1");
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYHLP2");
        
        #endregion
        
        #region herdings - leighton road
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYHLR1");
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYHLR2");
        
        #endregion
        
        #region hollinsend
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYHOL1");
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYHOL2");
        
        #endregion
        
        #region hyde park
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYHYP1");
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYHYP2");
        
        #endregion
        
        #region infirmary road
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYINF1");
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYINF2");
        
        #endregion
        
        #region leppings lane
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYLEP1");
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYLEP2");
        
        #endregion
        
        #region langsett - primrose view
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYLPH1");
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYLPH2");
        
        #endregion
        
        #region malin bridge
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYMAL1");
        
        #endregion
        
        #region meadowhall interchange
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYMHI1");
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYMHI2");
        
        #endregion
        
        #region meadowhall south - tinsley
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYMHS1");
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYMHS2");
        
        #endregion
        
        #region middlewood
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYMID1");
        
        #endregion
        
        #region manor top
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYMRT1");
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYMRT2");
        
        #endregion
        
        #region moss way
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYMWY1");
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYMWY2");
        
        #endregion
        
        #region netherthorpe road
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYNET1");
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYNET2");
        
        #endregion
        
        #region nunnery square
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYNUN1");
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYNUN2");
        
        #endregion
        
        #region parkgate
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYPAR1");
        
        #endregion
        
        #region park grange croft
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYPGC1");
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYPGC2");
        
        #endregion
        
        #region park grange
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYPGR1");
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYPGR2");
        
        #endregion
        
        #region rotherham station
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYRTH1");
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYRTH2");
        
        #endregion
        
        #region kelham island
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYSHL1");
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYSHL2");
        
        #endregion
        
        #region sheffield stn - hallam uni
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYSHU1");
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYSHU2");
        
        #endregion
        
        #region spring lane
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYSPL1");
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYSPL2");
        
        #endregion
        
        #region university of sheffield
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYUNI1");
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYUNI2");
        
        #endregion
        
        #region valley centertainment
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYVEN1");
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYVEN2");
        
        #endregion
        
        #region woodbourn road
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYWBN1");
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYWBN2");
        
        #endregion
        
        #region westfield
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYWTF1");
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYWTF2");
        
        #endregion
        
        #region white lane
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYWTL1");
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYWTL2");
        
        #endregion
        
        #region waterthorpe
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYWTR1");
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYWTR2");
        
        #endregion
        
        #region west street
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYWTS1");
        
        await CacheBuilder.Build(
            dataSource: dataSource,
            cacheService: cacheService,
            mapperService: mapperService,
            id: "9400ZZSYWTS2");
        
        #endregion
        
        return Results.Ok();
    }
    
    public static async Task<IResult> PostCacheByDeleteAsync(
        NpgsqlDataSource dataSource,
        IConnectionMultiplexer cacheService,
        IMapper mapperService) {
        
        #region flush keys
        
        await cacheService
            .GetDatabase()
            .ExecuteAsync(command: "flushdb");
        
        #endregion
        
        return Results.Ok();
    }
    
    public static async Task<IResult> PostIndexByBuildAsync(
        NpgsqlDataSource dataSource,
        ElasticsearchClient searchService,
        IMapper mapperService) {
        
        #region delete index
        
        await searchService.Indices.DeleteAsync(indices: "search");
        
        #endregion
        
        #region create index
        
        await searchService.Indices.CreateAsync<SearchStop>(
            index: "search",
            configureRequest: request => request
                .Mappings(configure: map => map
                    .Properties(properties: new Properties<SearchStop>
                    {
                        { "code", new KeywordProperty() },
                        { "id", new KeywordProperty() },
                        { "latitude", new DoubleNumberProperty() },
                        { "location", new GeoPointProperty() },
                        { "longitude", new DoubleNumberProperty() },
                        { "name", new KeywordProperty() },
                        { "platform", new TextProperty() },
                        { "points", new ObjectProperty() }
                    })));
        
        #endregion
        
        #region arbourthorne road
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYABR1");
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYABR2");
        
        #endregion
        
        #region attercliffe
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYATT1");
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYATT2");
        
        #endregion
        
        #region bamforth street
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYBAM1");
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYBAM2");
        
        #endregion
        
        #region birley moor road
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYBMR1");
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYBMR2");
        
        #endregion
        
        #region birley lane
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYBRL1");
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYBRL2");
        
        #endregion
        
        #region castle square
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYCAS1");
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYCAS2");
        
        #endregion
        
        #region cathedral
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYCAT1");
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYCAT2");
        
        #endregion
        
        #region cricket inn road
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYCIR1");
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYCIR2");
        
        #endregion
        
        #region carbrook
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYCRB1");
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYCRB2");
        
        #endregion
        
        #region crystal peaks
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYCRY1");
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYCRY2");
        
        #endregion
        
        #region city hall
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYCYH1");
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYCYH2");
        
        #endregion
        
        #region drake house lane
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYDHL1");
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYDHL2");
        
        #endregion
        
        #region donetsk way
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYDON1");
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYDON2");
        
        #endregion
        
        #region arena / olympic legacy park
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYDVS1");
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYDVS2");
        
        #endregion
        
        #region fitzalan sq - ponds forge
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYFIZ1");
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYFIZ2");
        
        #endregion
        
        #region gleadless townend
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYGLE1");
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYGLE2");
        
        #endregion
        
        #region granville rd - sheffield college
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYGRC1");
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYGRC2");
        
        #endregion
        
        #region hackenthorpe
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYHAK1");
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYHAK2");
        
        #endregion
        
        #region herdings park
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYHDP1");
        
        #endregion
        
        #region halfway
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYHFW1");
        
        #endregion
        
        #region hillsborough
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYHIL1");
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYHIL2");
        
        #endregion
        
        #region hillsborough park
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYHLP1");
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYHLP2");
        
        #endregion
        
        #region herdings - leighton road
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYHLR1");
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYHLR2");
        
        #endregion
        
        #region hollinsend
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYHOL1");
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYHOL2");
        
        #endregion
        
        #region hyde park
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYHYP1");
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYHYP2");
        
        #endregion
        
        #region infirmary road
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYINF1");
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYINF2");
        
        #endregion
        
        #region leppings lane
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYLEP1");
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYLEP2");
        
        #endregion
        
        #region langsett - primrose view
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYLPH1");
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYLPH2");
        
        #endregion
        
        #region malin bridge
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYMAL1");
        
        #endregion
        
        #region meadowhall interchange
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYMHI1");
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYMHI2");
        
        #endregion
        
        #region meadowhall south - tinsley
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYMHS1");
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYMHS2");
        
        #endregion
        
        #region middlewood
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYMID1");
        
        #endregion
        
        #region manor top
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYMRT1");
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYMRT2");
        
        #endregion
        
        #region moss way
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYMWY1");
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYMWY2");
        
        #endregion
        
        #region netherthorpe road
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYNET1");
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYNET2");
        
        #endregion
        
        #region nunnery square
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYNUN1");
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYNUN2");
        
        #endregion
        
        #region parkgate
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYPAR1");
        
        #endregion
        
        #region park grange croft
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYPGC1");
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYPGC2");
        
        #endregion
        
        #region park grange
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYPGR1");
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYPGR2");
        
        #endregion
        
        #region rotherham station
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYRTH1");
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYRTH2");
        
        #endregion
        
        #region kelham island
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYSHL1");
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYSHL2");
        
        #endregion
        
        #region sheffield stn - hallam uni
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYSHU1");
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYSHU2");
        
        #endregion
        
        #region spring lane
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYSPL1");
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYSPL2");
        
        #endregion
        
        #region university of sheffield
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYUNI1");
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYUNI2");
        
        #endregion
        
        #region valley centertainment
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYVEN1");
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYVEN2");
        
        #endregion
        
        #region woodbourn road
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYWBN1");
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYWBN2");
        
        #endregion
        
        #region westfield
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYWTF1");
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYWTF2");
        
        #endregion
        
        #region white lane
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYWTL1");
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYWTL2");
        
        #endregion
        
        #region waterthorpe
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYWTR1");
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYWTR2");
        
        #endregion
        
        #region west street
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYWTS1");
        
        await IndexBuilder.Build(
            dataSource: dataSource,
            searchService: searchService,
            mapperService: mapperService,
            id: "9400ZZSYWTS2");
        
        #endregion
        
        return Results.Ok();
    }
    
    public static async Task<IResult> PostIndexByDeleteAsync(
        NpgsqlDataSource dataSource,
        ElasticsearchClient searchService,
        IMapper mapperService) {
        
        #region delete index
        
        await searchService.Indices.DeleteAsync(indices: "search");
        
        #endregion
        
        #region create index
        
        await searchService.Indices.CreateAsync<SearchStop>(
            index: "search",
            configureRequest: request => request
                .Mappings(configure: map => map
                    .Properties(properties: new Properties<SearchStop>
                    {
                        { "code", new KeywordProperty() },
                        { "id", new KeywordProperty() },
                        { "latitude", new DoubleNumberProperty() },
                        { "location", new GeoPointProperty() },
                        { "longitude", new DoubleNumberProperty() },
                        { "name", new KeywordProperty() },
                        { "platform", new TextProperty() },
                        { "points", new ObjectProperty() }
                    })));
        
        #endregion
        
        return Results.Ok();
    }
    
    public static async Task<IResult> PostScreenshotByFileAsync(
        BlobContainerClient blobService,
        HttpRequest request) {
        
        #region check headers
        
        var contentLength = request.Headers.ContentLength;
        var contentType = request.Headers.ContentType;
        
        if (contentLength is null || string.IsNullOrEmpty(value: contentType.FirstOrDefault()))
            return Results.BadRequest();
        
        if (contentLength is 0 || contentType.FirstOrDefault() is not "image/png")
            return Results.NotFound();
        
        #endregion
        
        #region get headers
        
        request.Headers.TryGetValue(key: "Custom-Name", out var customHeaders);
        var customName = customHeaders.FirstOrDefault();
        
        if (string.IsNullOrEmpty(value: customName))
            return Results.BadRequest();
        
        if (!customName.EndsWithIgnoreCase(value: ".png"))
            return Results.NotFound();
        
        #endregion
        
        #region upload file
        
        var blobClient = blobService.GetBlobClient(blobName: $"web/{customName}");
        
        await blobClient.UploadAsync(
            content: request.Body,
            options: new BlobUploadOptions
            {
                HttpHeaders = new BlobHttpHeaders
                {
                    ContentType = "image/png"
                }
            });
        
        #endregion
        
        return Results.Ok();
    }
}
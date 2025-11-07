using AutoMapper;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.IndexManagement;
using Elastic.Clients.Elasticsearch.Mapping;
using Npgsql;
using StackExchange.Redis;
using TramTimes.Web.Api.Builders;
using TramTimes.Web.Utilities.Extensions;

namespace TramTimes.Web.Api.Handlers;

public static class WebHandler
{
    public static async Task<IResult> PostCacheByBuildAsync(
        NpgsqlDataSource dataSource,
        IConnectionMultiplexer cacheService,
        IMapper mapperService) {
        
        #region get matched keys
        
        var keys = cacheService
            .GetServer(endpoint: cacheService
                .GetEndPoints()
                .First())
            .Keys(pattern: "southyorkshire:*")
            .ToArray();
        
        #endregion
        
        #region delete matched keys
        
        await cacheService
            .GetDatabase()
            .KeyDeleteAsync(keys: keys);
        
        #endregion
        
        #region build stops
        
        var stops = StopBuilder.Build(path: Path.Combine(
            path1: "Data",
            path2: "stops.txt"));
        
        #endregion
        
        #region build jobs
        
        foreach (var item in stops)
            await CacheBuilder.Build(
                dataSource: dataSource,
                cacheService: cacheService,
                mapperService: mapperService,
                id: item);
        
        #endregion
        
        return Results.Ok();
    }
    
    public static async Task<IResult> PostCacheByDeleteAsync(
        NpgsqlDataSource dataSource,
        IConnectionMultiplexer cacheService,
        IMapper mapperService) {
        
        #region get matched keys
        
        var keys = cacheService
            .GetServer(endpoint: cacheService
                .GetEndPoints()
                .First())
            .Keys(pattern: "southyorkshire:*")
            .ToArray();
        
        #endregion
        
        #region delete matched keys
        
        await cacheService
            .GetDatabase()
            .KeyDeleteAsync(keys: keys);
        
        #endregion
        
        return Results.Ok();
    }
    
    public static async Task<IResult> PostIndexByBuildAsync(
        NpgsqlDataSource dataSource,
        ElasticsearchClient searchService,
        IMapper mapperService) {
        
        #region delete index
        
        await searchService.Indices.DeleteAsync(request: new DeleteIndexRequest
        {
            Indices = "southyorkshire"
        });
        
        #endregion
        
        #region create index
        
        await searchService.Indices.CreateAsync(request: new CreateIndexRequest
        {
            Index = "southyorkshire",
            Mappings = new TypeMapping
            {
                Properties = new Properties
                {
                    { "code", new KeywordProperty() },
                    { "id", new KeywordProperty() },
                    { "latitude", new DoubleNumberProperty() },
                    { "location", new GeoPointProperty() },
                    { "longitude", new DoubleNumberProperty() },
                    { "name", new KeywordProperty() },
                    { "platform", new TextProperty() },
                    { "points", new ObjectProperty() }
                }
            }
        });
        
        #endregion
        
        #region build stops
        
        var stops = StopBuilder.Build(path: Path.Combine(
            path1: "Data",
            path2: "stops.txt"));
        
        #endregion
        
        #region build jobs
        
        foreach (var item in stops)
            await IndexBuilder.Build(
                dataSource: dataSource,
                searchService: searchService,
                mapperService: mapperService,
                id: item);
        
        #endregion
        
        return Results.Ok();
    }
    
    public static async Task<IResult> PostIndexByDeleteAsync(
        NpgsqlDataSource dataSource,
        ElasticsearchClient searchService,
        IMapper mapperService) {
        
        #region delete index
        
        await searchService.Indices.DeleteAsync(request: new DeleteIndexRequest
        {
            Indices = "southyorkshire"
        });
        
        #endregion
        
        #region create index
        
        await searchService.Indices.CreateAsync(request: new CreateIndexRequest
        {
            Index = "southyorkshire",
            Mappings = new TypeMapping
            {
                Properties = new Properties
                {
                    { "code", new KeywordProperty() },
                    { "id", new KeywordProperty() },
                    { "latitude", new DoubleNumberProperty() },
                    { "location", new GeoPointProperty() },
                    { "longitude", new DoubleNumberProperty() },
                    { "name", new KeywordProperty() },
                    { "platform", new TextProperty() },
                    { "points", new ObjectProperty() }
                }
            }
        });
        
        #endregion
        
        return Results.Ok();
    }
    
    public static async Task<IResult> PostScreenshotByFileAsync(
        BlobContainerClient containerClient,
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
        
        await containerClient
            .GetBlobClient(blobName: $"screenshots/{customName}")
            .UploadAsync(
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
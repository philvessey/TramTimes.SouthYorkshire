using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Mapping;
using Polly;
using Polly.Retry;
using TramTimes.Web.Api.Models;

namespace TramTimes.Web.Api.Services;

public class SearchService : IHostedService
{
    private readonly ElasticsearchClient _service;
    private readonly ILogger<SearchService> _logger;
    private readonly AsyncRetryPolicy _result;
    
    public SearchService(
        ElasticsearchClient service,
        ILogger<SearchService> logger) {
        
        #region inject servics
        
        _service = service;
        _logger = logger;
        
        #endregion
        
        #region build result
        
        _result = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(
                retryCount: 5,
                sleepDurationProvider: i => TimeSpan.FromSeconds(value: Math.Pow(
                    x: 5,
                    y: i)));
        
        #endregion
    }
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        #region build task
        
        await _result.ExecuteAsync(action: async () =>
        {
            await _service.Indices.DeleteAsync(
                indices: "southyorkshire",
                cancellationToken: cancellationToken);
            
            await _service.Indices.CreateAsync<SearchStop>(
                index: "southyorkshire",
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
                        })),
                cancellationToken: cancellationToken);
            
            _logger.LogInformation(
                message: "Search service health status: {status}",
                args: "Green");
        });
        
        #endregion
    }
    
    public Task StopAsync(CancellationToken cancellationToken)
    {
        #region build task
        
        return Task.CompletedTask;
        
        #endregion
    }
}
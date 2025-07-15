using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Mapping;
using Polly;
using Polly.Retry;
using TramTimes.Search.Jobs.Models;

namespace TramTimes.Search.Jobs.Services;

public class StartupService : IHostedService
{
    private readonly ElasticsearchClient _service;
    private readonly ILogger<StartupService> _logger;
    private readonly AsyncRetryPolicy _result;
    
    public StartupService(
        ElasticsearchClient service,
        ILogger<StartupService> logger) {
        
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
            var pingResponse = await _service.PingAsync(cancellationToken: cancellationToken);
            
            if (!pingResponse.IsValidResponse)
                _logger.LogError(
                    message: "Service ping status: {status}",
                    args: pingResponse.IsSuccess());
            
            if (!pingResponse.IsValidResponse)
                throw new Exception(message: $"Service ping status: {pingResponse.IsSuccess()}");
            
            _logger.LogInformation(
                message: "Service ping status: {status}",
                args: pingResponse.IsSuccess());
            
            var healthResponse = await _service.Cluster.HealthAsync(cancellationToken: cancellationToken);
            
            if (!healthResponse.IsValidResponse)
                _logger.LogError(
                    message: "Service cluster status: {status}",
                    args: healthResponse.Status);
            
            if (!healthResponse.IsValidResponse)
                throw new Exception(message: $"Service cluster status: {healthResponse.Status}");
            
            _logger.LogInformation(
                message: "Service cluster status: {status}",
                args: healthResponse.Status);
            
            await _service.Indices.DeleteAsync(
                indices: "search",
                cancellationToken: cancellationToken);
            
            var createResponse = await _service.Indices.CreateAsync<SearchStop>(
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
                        })),
                cancellationToken: cancellationToken);
            
            if (!createResponse.IsValidResponse)
                _logger.LogError(
                    message: "Service index status: {status}",
                    args: createResponse.IsSuccess());
            
            if (!createResponse.IsValidResponse)
                throw new Exception(message: $"Service index status: {createResponse.IsSuccess()}");
            
            _logger.LogInformation(
                message: "Service index status: {status}",
                args: createResponse.IsSuccess());
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
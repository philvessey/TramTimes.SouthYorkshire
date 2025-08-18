using Polly;
using Polly.Retry;
using StackExchange.Redis;

namespace TramTimes.Cache.Jobs.Services;

public class CacheService : IHostedService
{
    private readonly IConnectionMultiplexer _service;
    private readonly ILogger<CacheService> _logger;
    private readonly AsyncRetryPolicy _result;
    
    public CacheService(
        IConnectionMultiplexer service,
        ILogger<CacheService> logger) {
        
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
            var response = await _service
                .GetDatabase()
                .ExecuteAsync(command: "info");
            
            if (response.IsNull)
                _logger.LogError(
                    message: "Cache service health status: {status}",
                    args: "Red");
            
            if (response.IsNull)
                throw new Exception(message: "Cache service health status: Red");
            
            _logger.LogInformation(
                message: "Cache service health status: {status}",
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
using Polly;
using Polly.Retry;
using StackExchange.Redis;

namespace TramTimes.Web.Api.Services;

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
            await _service
                .GetDatabase()
                .ExecuteAsync(command: "info");
            
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
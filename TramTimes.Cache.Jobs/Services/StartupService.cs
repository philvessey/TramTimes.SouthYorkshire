using Polly;
using Polly.Retry;
using StackExchange.Redis;

namespace TramTimes.Cache.Jobs.Services;

public class StartupService : IHostedService
{
    private readonly IConnectionMultiplexer _service;
    private readonly ILogger<StartupService> _logger;
    private readonly AsyncRetryPolicy _result;
    
    public StartupService(
        IConnectionMultiplexer service,
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
            var pingResponse = await _service
                .GetDatabase()
                .PingAsync();
            
            if (pingResponse == TimeSpan.Zero)
                _logger.LogError(
                    message: "Service ping status: {status}",
                    args: "False");
            
            if (pingResponse == TimeSpan.Zero)
                throw new Exception(message: "Service ping status: False");
            
            _logger.LogInformation(
                message: "Service ping status: {status}",
                args: "True");
            
            var healthResponse = await _service
                .GetDatabase()
                .ExecuteAsync(command: "info");
            
            if (healthResponse.IsNull)
                _logger.LogError(
                    message: "Service database status: {status}",
                    args: "Yellow");
            
            if (healthResponse.IsNull)
                throw new Exception(message: "Service database status: Yellow");
            
            _logger.LogInformation(
                message: "Service database status: {status}",
                args: "Green");
            
            var deleteResponse = await _service
                .GetDatabase()
                .ExecuteAsync(command: "flushdb");
            
            if (deleteResponse.IsNull)
                _logger.LogError(
                    message: "Service cache status: {status}",
                    args: "False");
            
            if (deleteResponse.IsNull)
                throw new Exception(message: "Service cache status: False");
            
            _logger.LogInformation(
                message: "Service cache status: {status}",
                args: "True");
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
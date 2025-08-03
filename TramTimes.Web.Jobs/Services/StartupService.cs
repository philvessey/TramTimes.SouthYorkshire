using Azure.Storage.Blobs;
using Polly;
using Polly.Retry;

namespace TramTimes.Web.Jobs.Services;

public class StartupService : IHostedService
{
    private readonly BlobContainerClient _service;
    private readonly ILogger<StartupService> _logger;
    private readonly AsyncRetryPolicy _result;
    
    public StartupService(
        BlobContainerClient service,
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
            var response = await _service.ExistsAsync(cancellationToken: cancellationToken);
            
            if (!response)
                _logger.LogError(
                    message: "Service health status: {status}",
                    args: "Red");
            
            if (!response)
                throw new Exception(message: "Service health status: Red");
            
            _logger.LogInformation(
                message: "Service health status: {status}",
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
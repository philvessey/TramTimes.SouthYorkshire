using Azure.Storage.Blobs;
using Polly;
using Polly.Retry;

namespace TramTimes.Web.Api.Services;

public class StorageService : IHostedService
{
    private readonly BlobContainerClient _service;
    private readonly ILogger<StorageService> _logger;
    private readonly AsyncRetryPolicy _result;
    
    public StorageService(
        BlobContainerClient service,
        ILogger<StorageService> logger) {
        
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
            await _service.CreateIfNotExistsAsync(cancellationToken: cancellationToken);
            
            _logger.LogInformation(
                message: "Storage service health status: {status}",
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
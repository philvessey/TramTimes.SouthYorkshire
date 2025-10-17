using Azure.Storage.Blobs;
using Polly;
using Polly.Retry;

namespace TramTimes.Database.Jobs.Services;

public class BlobService : IHostedService
{
    private readonly BlobContainerClient _client;
    private readonly ILogger<BlobService> _logger;
    private readonly AsyncRetryPolicy _result;
    
    public BlobService(
        BlobContainerClient client,
        ILogger<BlobService> logger) {
        
        #region inject services
        
        _client = client;
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
            await _client.CreateIfNotExistsAsync(cancellationToken: cancellationToken);
            
            _logger.LogInformation(
                message: "Blob service health status: {status}",
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
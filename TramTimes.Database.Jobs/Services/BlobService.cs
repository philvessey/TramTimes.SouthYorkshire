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
                sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(value: Math.Pow(
                    x: retryAttempt,
                    y: 2)),
                retryCount: 4);

        #endregion
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        #region build task

        await _result.ExecuteAsync(action: async () =>
        {
            await _client.CreateIfNotExistsAsync(cancellationToken: cancellationToken);

            if (_logger.IsEnabled(logLevel: LogLevel.Information))
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
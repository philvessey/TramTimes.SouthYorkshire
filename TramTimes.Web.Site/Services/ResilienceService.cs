using Microsoft.Extensions.Http.Resilience;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;

namespace TramTimes.Web.Site.Services;

public class ResilienceService : IHostedService
{
    private readonly IOptionsMonitor<HttpStandardResilienceOptions> _options;
    private readonly ILogger<ResilienceService> _logger;
    private readonly AsyncRetryPolicy _result;

    public ResilienceService(
        IOptionsMonitor<HttpStandardResilienceOptions> options,
        ILogger<ResilienceService> logger) {

        #region inject services

        _options = options;
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

        await _result.ExecuteAsync(action: () =>
        {
            if (_logger.IsEnabled(logLevel: LogLevel.Information))
                _logger.LogInformation(
                    message: "AttemptTimeout: {timeout}",
                    args: _options.Get(name: "api").AttemptTimeout.Timeout);

            if (_logger.IsEnabled(logLevel: LogLevel.Information))
                _logger.LogInformation(
                    message: "TotalRequestTimeout: {timeout}",
                    args: _options.Get(name: "api").TotalRequestTimeout.Timeout);

            if (_logger.IsEnabled(logLevel: LogLevel.Information))
                _logger.LogInformation(
                    message: "MaxRetryAttempts: {attempts}",
                    args: _options.Get(name: "api").Retry.MaxRetryAttempts);

            if (_logger.IsEnabled(logLevel: LogLevel.Information))
                _logger.LogInformation(
                    message: "BackoffType: {type}",
                    args: _options.Get(name: "api").Retry.BackoffType);

            return Task.CompletedTask;
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
using Polly;
using Polly.Retry;
using Quartz;
using TramTimes.Cache.Jobs.Builders;
using TramTimes.Cache.Jobs.Workers.Stops;

namespace TramTimes.Cache.Jobs.Services;

public class UpdateService : IHostedService
{
    private readonly IServiceProvider _service;
    private readonly ILogger<UpdateService> _logger;
    private readonly IHostApplicationLifetime _host;
    private readonly AsyncRetryPolicy _result;

    public UpdateService(
        IServiceProvider service,
        ILogger<UpdateService> logger,
        IHostApplicationLifetime host) {

        #region inject servics

        _service = service;
        _logger = logger;
        _host = host;

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
            using var scope = _service.CreateScope();

            var stops = StopBuilder.Build(path: Path.Combine(
                path1: "Data",
                path2: "stops.txt"));

            foreach (var item in stops)
            {
                var serviceType = Type.GetType(typeName: $"TramTimes.Cache.Jobs.Workers.Stops._{item}") ?? typeof(_9400ZZSYZZZ1);

                if (scope.ServiceProvider.GetRequiredService(serviceType: serviceType) is not IJob job)
                    continue;

                await job.Execute(context: null!);
            }

            if (_logger.IsEnabled(logLevel: LogLevel.Information))
                _logger.LogInformation(
                    message: "Update service health status: {status}",
                    args: "Green");

            _host.StopApplication();
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
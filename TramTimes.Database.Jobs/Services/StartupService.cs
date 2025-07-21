using Npgsql;
using Polly;
using Polly.Retry;

namespace TramTimes.Database.Jobs.Services;

public class StartupService : IHostedService
{
    private readonly NpgsqlDataSource _service;
    private readonly ILogger<StartupService> _logger;
    private readonly AsyncRetryPolicy _result;
    
    public StartupService(
        NpgsqlDataSource service,
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
            await using var connection = await _service.OpenConnectionAsync(cancellationToken: cancellationToken);
            
            var command = new NpgsqlCommand(
                cmdText: "select 1 from pg_tables limit 1",
                connection: connection);
            
            var pingResponse = await command.ExecuteScalarAsync(cancellationToken: cancellationToken);
            
            await command.DisposeAsync();
            await connection.CloseAsync();
            
            if (pingResponse is null)
                _logger.LogError(
                    message: "Service ping status: {status}",
                    args: "False");
            
            if (pingResponse is null)
                throw new Exception(message: "Service ping status: False");
            
            _logger.LogInformation(
                message: "Service ping status: {status}",
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
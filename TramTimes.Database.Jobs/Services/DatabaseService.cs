using Npgsql;
using Polly;
using Polly.Retry;

namespace TramTimes.Database.Jobs.Services;

public class DatabaseService : IHostedService
{
    private readonly NpgsqlDataSource _service;
    private readonly ILogger<DatabaseService> _logger;
    private readonly AsyncRetryPolicy _result;
    
    public DatabaseService(
        NpgsqlDataSource service,
        ILogger<DatabaseService> logger) {
        
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
            
            var response = await command.ExecuteScalarAsync(cancellationToken: cancellationToken);
            
            await command.DisposeAsync();
            await connection.CloseAsync();
            
            if (response is null)
                _logger.LogError(
                    message: "Database service health status: {status}",
                    args: "Red");
            
            if (response is null)
                throw new Exception(message: "Database service health status: Red");
            
            _logger.LogInformation(
                message: "Database service health status: {status}",
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
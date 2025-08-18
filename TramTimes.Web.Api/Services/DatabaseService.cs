using Npgsql;
using Polly;
using Polly.Retry;

namespace TramTimes.Web.Api.Services;

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
            
            await using var command = new NpgsqlCommand(
                cmdText: await File.ReadAllTextAsync(
                    path: "Scripts/init.sql",
                    cancellationToken: cancellationToken),
                connection: connection);
            
            await command.ExecuteNonQueryAsync(cancellationToken: cancellationToken);
            
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
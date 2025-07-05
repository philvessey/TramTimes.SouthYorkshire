using Microsoft.Extensions.Diagnostics.HealthChecks;
using StackExchange.Redis;

namespace TramTimes.Web.Api.Checks;

public class CacheCheck(IConfiguration configuration) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default) {
        
        #region build connection
        
        var connectionString = configuration.GetConnectionString(name: "cache");
        
        if (string.IsNullOrEmpty(value: connectionString))
            return HealthCheckResult.Unhealthy();
        
        #endregion
        
        #region build result
        
        try
        {
            var options = ConfigurationOptions.Parse(configuration: connectionString);
            options.ConnectTimeout = 2000;
            
            await using var connection = await ConnectionMultiplexer.ConnectAsync(configuration: options);
            var database = connection.GetDatabase();
            
            var result = await database.PingAsync();
            
            return result != TimeSpan.Zero
                ? HealthCheckResult.Healthy()
                : HealthCheckResult.Unhealthy();
        }
        catch (Exception)
        {
            return HealthCheckResult.Unhealthy();
        }
        
        #endregion
    }
}
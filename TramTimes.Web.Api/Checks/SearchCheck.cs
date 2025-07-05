using Microsoft.Extensions.Diagnostics.HealthChecks;
using Elastic.Clients.Elasticsearch;

namespace TramTimes.Web.Api.Checks;

public class SearchCheck(IConfiguration configuration) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default) {
        
        #region build connection
        
        var connectionString = configuration.GetConnectionString(name: "search");
        
        if (string.IsNullOrEmpty(value: connectionString))
            return HealthCheckResult.Unhealthy();
        
        #endregion
        
        #region build result
        
        try
        {
            var settings = new ElasticsearchClientSettings(uri: new Uri(uriString: connectionString));
            var client = new ElasticsearchClient(elasticsearchClientSettings: settings);
            
            var result = await client.PingAsync(cancellationToken: cancellationToken);
            
            return result.IsSuccess()
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
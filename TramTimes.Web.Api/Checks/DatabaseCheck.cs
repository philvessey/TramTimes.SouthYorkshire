using Microsoft.Extensions.Diagnostics.HealthChecks;
using Npgsql;

namespace TramTimes.Web.Api.Checks;

public class DatabaseCheck(IConfiguration configuration) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default) {

        #region build connection

        var connectionString = configuration.GetConnectionString(name: "database");

        if (string.IsNullOrEmpty(value: connectionString))
            return HealthCheckResult.Unhealthy();

        #endregion

        #region build result

        try
        {
            await using var connection = new NpgsqlConnection(connectionString: connectionString);
            await connection.OpenAsync(cancellationToken: cancellationToken);

            await using var command = new NpgsqlCommand(
                cmdText: "select 1 from pg_indexes where indexname = 'gtfs_stop_times_idx' limit 1",
                connection: connection);

            var result = await command.ExecuteScalarAsync(cancellationToken: cancellationToken);

            return result is not null
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
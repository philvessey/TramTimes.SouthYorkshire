using Quartz;
using StackExchange.Redis;

namespace TramTimes.Cache.Jobs.Workers;

public class Testing(
    IConnectionMultiplexer cacheService,
    ILogger<Testing> logger) : IJob {

    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            #region get matched keys

            var keys = cacheService
                .GetServer(endpoint: cacheService
                    .GetEndPoints()
                    .First())
                .Keys(pattern: "southyorkshire:*")
                .ToArray();

            #endregion

            #region delete matched keys

            await cacheService
                .GetDatabase()
                .KeyDeleteAsync(keys: keys);

            #endregion
        }
        catch (Exception e)
        {
            if (logger.IsEnabled(logLevel: LogLevel.Error))
                logger.LogError(
                    message: "Exception: {exception}",
                    args: e.ToString());
        }
    }
}
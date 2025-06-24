using Quartz;
using StackExchange.Redis;

namespace TramTimes.Cache.Jobs.Workers;

public class Clean(
    IConnectionMultiplexer cacheService,
    ILogger<Clean> logger) : IJob {
    
    public async Task Execute(IJobExecutionContext context)
    {
        var guid = Guid.NewGuid();
        
        var storage = Directory.CreateDirectory(path: Path.Combine(
            path1: Path.GetTempPath(),
            path2: guid.ToString()));
        
        try
        {
            #region flush database keys
            
            await cacheService
                .GetDatabase()
                .ExecuteAsync(command: "flushdb");
            
            #endregion
        }
        catch (Exception e)
        {
            logger.LogError(
                message: "Exception: {exception}",
                args: e.ToString());
        }
        finally
        {
            storage.Delete(recursive: true);
        }
    }
}
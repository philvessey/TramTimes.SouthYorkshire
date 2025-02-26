using Quartz;

namespace TramTimes.Database.Jobs;

public class Worker(ILogger<Worker> logger) : IJob
{
    public Task Execute(IJobExecutionContext context)
    {
        logger.LogInformation(message: "{Job} job executing, triggered by {Trigger}", context.JobDetail.Key, context.Trigger.Key);
        
        return Task.CompletedTask;
    }
}
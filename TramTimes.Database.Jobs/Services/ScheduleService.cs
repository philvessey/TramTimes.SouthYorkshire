using Quartz;
using TramTimes.Database.Jobs.Workers;

namespace TramTimes.Database.Jobs.Services;

public static class ScheduleService
{
    public static HostApplicationBuilder AddScheduleDefaults(this HostApplicationBuilder builder)
    {
        builder.Services.AddQuartz(configure: quartz =>
        {
            var initKey = new JobKey(name: "Init");
            var cronKey = new JobKey(name: "Cron");
            
            quartz.AddJob<Build>(jobKey: initKey)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: initKey);
                    trigger.StartNow();
                });
            
            quartz.AddJob<Build>(jobKey: cronKey)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: cronKey);
                    trigger.WithCronSchedule(cronExpression: "0 0 4 * * ?");
                });
        });
        
        builder.Services.AddQuartzHostedService(configure: options =>
        {
            options.WaitForJobsToComplete = true;
        });
        
        return builder;
    }
}
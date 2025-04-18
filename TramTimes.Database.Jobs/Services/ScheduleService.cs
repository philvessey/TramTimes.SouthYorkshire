using Quartz;
using TramTimes.Database.Jobs.Workers;

namespace TramTimes.Database.Jobs.Services;

public static class ScheduleService
{
    public static HostApplicationBuilder AddScheduleDefaults(this HostApplicationBuilder builder)
    {
        var initJobKey = new JobKey(name: "InitJob");
        var cronJobKey = new JobKey(name: "CronJob");
        
        builder.Services.AddQuartz(configure: quartz =>
        {
            quartz.AddJob<Build>(jobKey: initJobKey)
                .AddTrigger(configure: trigger =>
                {
                    trigger.WithIdentity(name: "InitTrigger");
                    trigger.ForJob(jobKey: initJobKey);
                    trigger.StartNow();
                });
            
            quartz.AddJob<Build>(jobKey: cronJobKey)
                .AddTrigger(configure: trigger =>
                {
                    trigger.WithIdentity(name: "CronTrigger");
                    trigger.ForJob(jobKey: cronJobKey);
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
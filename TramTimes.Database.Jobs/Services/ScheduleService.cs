using Quartz;
using TramTimes.Database.Jobs.Workers;

namespace TramTimes.Database.Jobs.Services;

public static class ScheduleService
{
    public static HostApplicationBuilder AddScheduleDefaults(this HostApplicationBuilder builder)
    {
        builder.Services.AddQuartz(configure: quartz =>
        {
            #region initialize database
            
            var init = new JobKey(name: "init");
            var cron = new JobKey(name: "cron");
            
            quartz
                .AddJob<Build>(jobKey: init)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: init);
                    trigger.StartNow();
                    trigger.WithDescription(description: "Startup job to initialize the database.");
                });
            
            quartz
                .AddJob<Build>(jobKey: cron)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: cron);
                    trigger.StartNow();
                    trigger.WithCronSchedule(cronExpression: "0 15 3 ? * *");
                });
            
            #endregion
        });
        
        builder.Services.AddQuartzHostedService(configure: options =>
        {
            options.WaitForJobsToComplete = true;
        });
        
        return builder;
    }
}
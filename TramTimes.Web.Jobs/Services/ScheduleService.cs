using Quartz;
using TramTimes.Web.Jobs.Workers;

namespace TramTimes.Web.Jobs.Services;

public static class ScheduleService
{
    public static HostApplicationBuilder AddScheduleDefaults(this HostApplicationBuilder builder)
    {
        builder.Services.AddQuartz(configure: quartz =>
        {
            #region clean container
            
            var init = new JobKey(name: "Init");
            var cron = new JobKey(name: "Cron");
            
            quartz
                .AddJob<Clean>(jobKey: init)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: init);
                    trigger.StartNow();
                    trigger.WithDescription(description: "Startup job to clean the container.");
                });
            
            quartz
                .AddJob<Clean>(jobKey: cron)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: cron);
                    trigger.StartNow();
                    trigger.WithCronSchedule(cronExpression: "0 30 3 * * ?");
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
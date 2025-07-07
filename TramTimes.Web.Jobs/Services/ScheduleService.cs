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
            
            var clean = new JobKey(name: "Clean");
            
            quartz
                .AddJob<Clean>(jobKey: clean)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: clean);
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
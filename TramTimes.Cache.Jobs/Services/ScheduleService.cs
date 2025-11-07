using Quartz;
using TramTimes.Cache.Jobs.Builders;
using TramTimes.Cache.Jobs.Workers;
using TramTimes.Cache.Jobs.Workers.Stops;

namespace TramTimes.Cache.Jobs.Services;

public static class ScheduleService
{
    private static readonly string Context = Environment.GetEnvironmentVariable(variable: "ASPIRE_CONTEXT") ?? "Development";
    
    public static HostApplicationBuilder AddScheduleDefaults(this HostApplicationBuilder builder)
    {
        builder.Services.AddQuartz(configure: quartz =>
        {
            #region build pool
            
            quartz.UseDefaultThreadPool(maxConcurrency: 5);
            
            #endregion
            
            #region build cache
            
            if (Context is "Production")
            {
                var clean = new JobKey(name: "clean");
                
                quartz
                    .AddJob<Production>(jobKey: clean, configure: job =>
                    {
                        job.WithDescription(description: "Cleans old entries from the cache.");
                    })
                    .AddTrigger(configure: trigger =>
                    {
                        trigger.ForJob(jobKey: clean);
                        trigger.WithIdentity(name: "clean-trigger");
                        trigger.WithCronSchedule(cronExpression: "0 0 3 ? * *");
                    });
            }
            
            #endregion
            
            #region build stops
            
            var stops = StopBuilder.Build(path: Path.Combine(
                path1: "Data",
                path2: "stops.txt"));
            
            #endregion
            
            #region check context
            
            if (Context is "Testing")
                return;
            
            #endregion
            
            #region build jobs
            
            foreach (var item in stops)
            {
                var jobKey = new JobKey(name: item);
                var jobType = Type.GetType(typeName: $"TramTimes.Cache.Jobs.Workers.Stops._{item}") ?? typeof(_9400ZZSYZZZ1);
                
                quartz
                    .AddJob(jobType: jobType, jobKey: jobKey, configure: job =>
                    {
                        job.WithDescription(description: $"Fetches and caches tram times for stop {item}.");
                    })
                    .AddTrigger(configure: trigger =>
                    {
                        trigger.ForJob(jobKey: jobKey);
                        trigger.WithIdentity(name: $"{item}-trigger-peak");
                        trigger.StartAt(startTimeUtc: DateTimeOffset.UtcNow.AddMinutes(minutes: 5));
                        trigger.WithCronSchedule(cronExpression: "0 0/10 6-9,16-19 ? * 1-5");
                    })
                    .AddTrigger(configure: trigger =>
                    {
                        trigger.ForJob(jobKey: jobKey);
                        trigger.WithIdentity(name: $"{item}-trigger-offpeak");
                        trigger.StartAt(startTimeUtc: DateTimeOffset.UtcNow.AddMinutes(minutes: 5));
                        trigger.WithCronSchedule(cronExpression: "0 0/20 10-15,20-23 ? * 1-5");
                    })
                    .AddTrigger(configure: trigger =>
                    {
                        trigger.ForJob(jobKey: jobKey);
                        trigger.WithIdentity(name: $"{item}-trigger-weekend");
                        trigger.StartAt(startTimeUtc: DateTimeOffset.UtcNow.AddMinutes(minutes: 5));
                        trigger.WithCronSchedule(cronExpression: "0 0/30 6-23 ? * 6-7");
                    })
                    .AddTrigger(configure: trigger =>
                    {
                        trigger.ForJob(jobKey: jobKey);
                        trigger.WithIdentity(name: $"{item}-trigger-night");
                        trigger.StartAt(startTimeUtc: DateTimeOffset.UtcNow.AddMinutes(minutes: 5));
                        trigger.WithCronSchedule(cronExpression: "0 0/60 0-2,4-5 ? * *");
                    });
            }
            
            #endregion
        });
        
        builder.Services.AddQuartzHostedService(configure: options =>
        {
            options.WaitForJobsToComplete = true;
        });
        
        return builder;
    }
}
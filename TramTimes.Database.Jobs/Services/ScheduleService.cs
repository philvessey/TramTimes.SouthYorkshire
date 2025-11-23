using Quartz;
using TramTimes.Database.Jobs.Builders;
using TramTimes.Database.Jobs.Workers;
using TramTimes.Database.Jobs.Workers.Stops;

namespace TramTimes.Database.Jobs.Services;

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
            
            #region build database
            
            switch (Context)
            {
                case "Development":
                {
                    var development = new JobKey(name: "development");
                    
                    quartz
                        .AddJob<Development>(jobKey: development, configure: job =>
                        {
                            job.WithDescription(description: "Development job to build the database.");
                        })
                        .AddTrigger(configure: trigger =>
                        {
                            trigger.ForJob(jobKey: development);
                            trigger.WithIdentity(name: "development-trigger");
                            trigger.StartNow();
                        });
                    
                    break;
                }
                case "Testing":
                {
                    var testing = new JobKey(name: "testing");
                    
                    quartz
                        .AddJob<Testing>(jobKey: testing, configure: job =>
                        {
                            job.WithDescription(description: "Testing job to build the database.");
                        })
                        .AddTrigger(configure: trigger =>
                        {
                            trigger.ForJob(jobKey: testing);
                            trigger.WithIdentity(name: "testing-trigger");
                            trigger.StartNow();
                        });
                    
                    break;
                }
            }
            
            #endregion
            
            #region build stops
            
            var stops = LocalStopBuilder.Build(path: Path.Combine(
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
                var jobType = Type.GetType(typeName: $"TramTimes.Database.Jobs.Workers.Stops._{item}") ?? typeof(_9400ZZSYZZZ1);
                
                quartz
                    .AddJob(jobType: jobType, jobKey: jobKey, configure: job =>
                    {
                        job.WithDescription(description: $"Verifies schedules for stop {item}.");
                    })
                    .AddTrigger(configure: trigger =>
                    {
                        trigger.ForJob(jobKey: jobKey);
                        trigger.WithIdentity(name: $"{item}-trigger");
                        trigger.StartAt(startTimeUtc: DateTimeOffset.UtcNow.AddMinutes(minutes: 5));
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
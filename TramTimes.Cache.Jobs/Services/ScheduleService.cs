using Quartz;
using TramTimes.Cache.Jobs.Builders;
using TramTimes.Cache.Jobs.Workers;
using TramTimes.Cache.Jobs.Workers.Stops;

namespace TramTimes.Cache.Jobs.Services;

public static class ScheduleService
{
    private static readonly string _context = Environment.GetEnvironmentVariable(variable: "ASPIRE_CONTEXT") ?? "Development";

    public static HostApplicationBuilder AddScheduleDefaults(this HostApplicationBuilder builder)
    {
        builder.Services.AddQuartz(configure: quartz =>
        {
            #region build pool

            quartz.UseDefaultThreadPool(maxConcurrency: 5);

            #endregion

            #region build cache

            switch (_context)
            {
                case "Development":
                {
                    var development = new JobKey(name: "development");

                    quartz
                        .AddJob<Development>(jobKey: development, configure: job =>
                        {
                            job.WithDescription(description: "Development job to build the cache.");
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
                            job.WithDescription(description: "Testing job to build the cache.");
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

            var stops = StopBuilder.Build(path: Path.Combine(
                path1: "Data",
                path2: "stops.txt"));

            #endregion

            #region check context

            if (_context is "Testing")
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
                        trigger.WithCronSchedule(cronExpression: "0 0/10 7-8,16-17 ? * 1-5");
                    })
                    .AddTrigger(configure: trigger =>
                    {
                        trigger.ForJob(jobKey: jobKey);
                        trigger.WithIdentity(name: $"{item}-trigger-offpeak");
                        trigger.StartAt(startTimeUtc: DateTimeOffset.UtcNow.AddMinutes(minutes: 5));
                        trigger.WithCronSchedule(cronExpression: "0 0/20 9-15,18-23 ? * 1-5");
                    })
                    .AddTrigger(configure: trigger =>
                    {
                        trigger.ForJob(jobKey: jobKey);
                        trigger.WithIdentity(name: $"{item}-trigger-weekend");
                        trigger.StartAt(startTimeUtc: DateTimeOffset.UtcNow.AddMinutes(minutes: 5));
                        trigger.WithCronSchedule(cronExpression: "0 0/30 7-23 ? * 6-7");
                    })
                    .AddTrigger(configure: trigger =>
                    {
                        trigger.ForJob(jobKey: jobKey);
                        trigger.WithIdentity(name: $"{item}-trigger-night");
                        trigger.StartAt(startTimeUtc: DateTimeOffset.UtcNow.AddMinutes(minutes: 5));
                        trigger.WithCronSchedule(cronExpression: "0 0/30 0-1,4-6 ? * *");
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
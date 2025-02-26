using Quartz;
using TramTimes.Database.Jobs;

var builder = Host.CreateApplicationBuilder(args: args);
builder.AddServiceDefaults();

var initJobKey = new JobKey(name: "InitJob");
var cronJobKey = new JobKey(name: "CronJob");

builder.Services.AddQuartz(configure: quartz =>
{
    quartz.AddJob<Worker>(jobKey: initJobKey)
        .AddTrigger(configure: trigger =>
        {
            trigger.WithIdentity(name: "InitTrigger");
            trigger.ForJob(jobKey: initJobKey)
                .StartNow();
        });
    
    quartz.AddJob<Worker>(jobKey: cronJobKey)
        .AddTrigger(configure: trigger =>
        {
            trigger.WithIdentity(name: "CronTrigger");
            trigger.ForJob(jobKey: cronJobKey)
                .WithCronSchedule(cronExpression: Environment.GetEnvironmentVariable(variable: "SCHEDULE") ?? "0 0 3 * * ?");
        });
});

builder.Services.AddQuartzHostedService(configure: options =>
{
    options.WaitForJobsToComplete = true;
});

builder.Build().Run();
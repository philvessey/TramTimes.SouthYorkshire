using Azure.Storage.Blobs;
using Quartz;
using TramTimes.Database.Jobs.Workers;

var builder = Host.CreateApplicationBuilder(args: args);
builder.AddServiceDefaults();

builder.AddAzureBlobClient(connectionName: "storage-blobs");
builder.AddNpgsqlDataSource(connectionName: "database");

var initJobKey = new JobKey(name: "InitJob");
var cronJobKey = new JobKey(name: "CronJob");

var cronExpression = Environment.GetEnvironmentVariable(variable: "CRON_SCHEDULE") ?? "0 0 4 * * ?";

builder.Services.AddQuartz(configure: quartz =>
{
    quartz.AddJob<Build>(jobKey: initJobKey)
        .AddTrigger(configure: trigger =>
        {
            trigger.WithIdentity(name: "InitTrigger");
            trigger.ForJob(jobKey: initJobKey)
                .StartNow();
        });
    
    quartz.AddJob<Build>(jobKey: cronJobKey)
        .AddTrigger(configure: trigger =>
        {
            trigger.WithIdentity(name: "CronTrigger");
            trigger.ForJob(jobKey: cronJobKey)
                .WithCronSchedule(cronExpression: cronExpression);
        });
});

builder.Services.AddQuartzHostedService(configure: options =>
{
    options.WaitForJobsToComplete = true;
});

var app = builder.Build();
var scope = app.Services.CreateScope();

await scope.ServiceProvider.GetRequiredService<BlobServiceClient>()
    .GetBlobContainerClient(blobContainerName: "database")
    .CreateIfNotExistsAsync();

app.Run();
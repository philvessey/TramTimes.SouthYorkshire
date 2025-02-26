using Microsoft.Extensions.Hosting;

var builder = DistributedApplication.CreateBuilder(args: args);

var postgres = builder.AddPostgres(name: "postgres")
    .WithEnvironment(name: "POSTGRES_DB", value: "database")
    .WithBindMount(source: "postgres", target: "/docker-entrypoint-initdb.d")
    .WithPgWeb();

postgres.WithLifetime(lifetime: builder.Environment.IsDevelopment()
    ? ContainerLifetime.Session
    : ContainerLifetime.Persistent);

if (builder.ExecutionContext.IsRunMode)
{
    postgres.WithDataVolume();
}

var database = postgres.AddDatabase(name: "database");

builder.AddProject<Projects.TramTimes_Database_Jobs>(name: "database-jobs")
    .WithEnvironment(name: "SCHEDULE", parameter: builder.AddParameter(name: "database-schedule", secret: false))
    .WithReference(source: database)
    .WaitFor(dependency: database);

builder.Build().Run();
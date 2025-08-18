using TramTimes.Aspire.Host.Builders;

var builder = DistributedApplication.CreateBuilder(args: args);

#region build storage

var storage = builder.BuildStorage();

#endregion

#region build database

var databaseResources = builder.BuildDatabase(storage: storage);

#endregion

#region build cache

var cacheResources = builder.BuildCache(
    storage: storage,
    server: databaseResources.PostgresServer ?? throw new InvalidOperationException("Postgres server not initialized."),
    database: databaseResources.PostgresDatabase ?? throw new InvalidOperationException("Postgres database not initialized."));

#endregion

#region build search

var searchResources = builder.BuildSearch(
    storage: storage,
    server: databaseResources.PostgresServer ?? throw new InvalidOperationException("Postgres server not initialized."),
    database: databaseResources.PostgresDatabase ?? throw new InvalidOperationException("Postgres database not initialized."));

#endregion

#region build web

builder.BuildWeb(
    storage: storage,
    server: databaseResources.PostgresServer ?? throw new InvalidOperationException("Postgres server not initialized."),
    database: databaseResources.PostgresDatabase ?? throw new InvalidOperationException("Postgres database not initialized."),
    cache: cacheResources.Redis ?? throw new InvalidOperationException("Redis not initialized."),
    search: searchResources.Elasticsearch ?? throw new InvalidOperationException("Elasticsearch not initialized."));

#endregion

#region build application

var application = builder.Build();

#endregion

application.Run();
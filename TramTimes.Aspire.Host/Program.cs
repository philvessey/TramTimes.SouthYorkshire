using TramTimes.Aspire.Host.Builders;

var builder = DistributedApplication.CreateBuilder(args: args);

#region build storage

var storage = builder.BuildStorage();

#endregion

#region build database

var database = builder.BuildDatabase(storage: storage);

#endregion

#region build cache

var cacheResources = builder.BuildCache(
    storage: storage,
    database: database);

#endregion

#region build search

var searchResources = builder.BuildSearch(
    storage: storage,
    database: database);

#endregion

#region build web

builder.BuildWeb(
    storage: storage,
    database: database,
    cache: cacheResources.Redis ?? throw new InvalidOperationException("Redis not initialized."),
    search: searchResources.Elasticsearch ?? throw new InvalidOperationException("Elasticsearch not initialized."));

#endregion

#region build application

var application = builder.Build();

#endregion

application.Run();
using TramTimes.Aspire.Host.Builders;

var builder = DistributedApplication.CreateBuilder(args: args);

#region build storage

var storageResources = builder.BuildStorage();

#endregion

#region build database

var databaseResources = builder.BuildDatabase(
    storage: storageResources.Azure ?? throw new InvalidOperationException("Azure storage not initialized."),
    container: storageResources.Database ?? throw new InvalidOperationException("Database container not initialized."));

#endregion

#region build cache

var cacheResources = builder.BuildCache(
    storage: storageResources.Azure ?? throw new InvalidOperationException("Azure storage not initialized."),
    container: storageResources.Cache ?? throw new InvalidOperationException("Cache container not initialized."),
    server: databaseResources.Postgres ?? throw new InvalidOperationException("Postgres server not initialized."),
    database: databaseResources.Database ?? throw new InvalidOperationException("Postgres database not initialized."));

#endregion

#region build search

var searchResources = builder.BuildSearch(
    storage: storageResources.Azure ?? throw new InvalidOperationException("Azure storage not initialized."),
    container: storageResources.Search ?? throw new InvalidOperationException("Search container not initialized."),
    server: databaseResources.Postgres ?? throw new InvalidOperationException("Postgres server not initialized."),
    database: databaseResources.Database ?? throw new InvalidOperationException("Postgres database not initialized."));

#endregion

#region build web

builder.BuildWeb(
    storage: storageResources.Azure ?? throw new InvalidOperationException("Azure storage not initialized."),
    container: storageResources.Web ?? throw new InvalidOperationException("Web container not initialized."),
    server: databaseResources.Postgres ?? throw new InvalidOperationException("Postgres server not initialized."),
    database: databaseResources.Database ?? throw new InvalidOperationException("Postgres database not initialized."),
    cache: cacheResources.Redis ?? throw new InvalidOperationException("Redis cache not initialized."),
    search: searchResources.Elastic ?? throw new InvalidOperationException("Elastic search not initialized."));

#endregion

#region build application

var application = builder.Build();

#endregion

application.Run();
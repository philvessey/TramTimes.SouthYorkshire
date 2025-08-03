using TramTimes.Aspire.Host.Builders;

var builder = DistributedApplication.CreateBuilder(args: args);

#region build storage

var storageResources = builder.BuildStorage();

#endregion

#region build database

var databaseResources = builder.BuildDatabase(
    storage: storageResources.AzureStorage ?? throw new InvalidOperationException("Azure storage not initialized."),
    container: storageResources.AzureBlobStorageContainer ?? throw new InvalidOperationException("Azure blob storage container not initialized."));

#endregion

#region build cache

var cacheResources = builder.BuildCache(
    storage: storageResources.AzureStorage ?? throw new InvalidOperationException("Azure storage not initialized."),
    container: storageResources.AzureBlobStorageContainer ?? throw new InvalidOperationException("Azure blob storage container not initialized."),
    server: databaseResources.Postgres ?? throw new InvalidOperationException("Postgres server not initialized."),
    database: databaseResources.Database ?? throw new InvalidOperationException("Postgres database not initialized."));

#endregion

#region build search

var searchResources = builder.BuildSearch(
    storage: storageResources.AzureStorage ?? throw new InvalidOperationException("Azure storage not initialized."),
    container: storageResources.AzureBlobStorageContainer ?? throw new InvalidOperationException("Azure blob storage container not initialized."),
    server: databaseResources.Postgres ?? throw new InvalidOperationException("Postgres server not initialized."),
    database: databaseResources.Database ?? throw new InvalidOperationException("Postgres database not initialized."));

#endregion

#region build web

builder.BuildWeb(
    storage: storageResources.AzureStorage ?? throw new InvalidOperationException("Azure storage not initialized."),
    container: storageResources.AzureBlobStorageContainer ?? throw new InvalidOperationException("Azure blob storage container not initialized."),
    server: databaseResources.Postgres ?? throw new InvalidOperationException("Postgres server not initialized."),
    database: databaseResources.Database ?? throw new InvalidOperationException("Postgres database not initialized."),
    cache: cacheResources.Redis ?? throw new InvalidOperationException("Redis cache not initialized."),
    search: searchResources.Elastic ?? throw new InvalidOperationException("Elastic search not initialized."));

#endregion

#region build application

var application = builder.Build();

#endregion

application.Run();
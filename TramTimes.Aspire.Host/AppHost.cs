using TramTimes.Aspire.Host.Builders;

var builder = DistributedApplication.CreateBuilder(args: args);

#region build container

builder.BuildContainer();

#endregion

#region build storage

var storage = builder.BuildStorage();

#endregion

#region build database

var database = builder.BuildDatabase(storage: storage);

#endregion

#region build cache

var cache = builder.BuildCache(database: database);

#endregion

#region build search

var search = builder.BuildSearch(database: database);

#endregion

#region build revenue

var revenue = builder.BuildRevenue();

#endregion

#region build web

builder.BuildWeb(
    storage: storage,
    database: database,
    cache: cache,
    search: search,
    revenue: revenue);

#endregion

#region build application

var application = builder.Build();

#endregion

application.Run();
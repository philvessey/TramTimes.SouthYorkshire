using TramTimes.Aspire.Host.Builders;

var builder = DistributedApplication.CreateBuilder(args: args);

#region build license

var license = builder.BuildLicense();

#endregion

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

var cache = builder.BuildCache(
    license: license,
    database: database);

#endregion

#region build search

var search = builder.BuildSearch(
    license: license,
    database: database);

#endregion

#region build revenue

var revenue = builder.BuildRevenue();

#endregion

#region build web

builder.BuildWeb(
    license: license,
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
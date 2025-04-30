using TramTimes.Aspire.Host.Services;

var builder = DistributedApplication.CreateBuilder(args: args);

builder.AddStorage(
    storage: out var storage,
    blobs: out var blobs);

builder.AddDatabase(
    storage: storage,
    blobs: blobs,
    server: out var server,
    database: out var database);

builder.AddCache(
    blobs: blobs,
    server: server,
    database: database);

builder.AddSearch(
    blobs: blobs,
    server: server,
    database: database);

var application = builder.Build();
application.Run();
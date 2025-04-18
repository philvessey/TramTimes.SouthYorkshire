using TramTimes.Aspire.Host.Services;

var builder = DistributedApplication.CreateBuilder(args: args);

builder.AddStorage(
    storage: out var storage,
    blobs: out var blobs);

builder.AddDatabase(
    storage: storage,
    blobs: blobs);

var application = builder.Build();
application.Run();
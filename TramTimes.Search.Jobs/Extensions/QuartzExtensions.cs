// ReSharper disable all

using Quartz;
using TramTimes.Search.Jobs.Services;
using TramTimes.Search.Jobs.Workers.Stops;

namespace TramTimes.Search.Jobs.Extensions;

public static class QuartzExtensions
{
    public static IServiceCollection AddHostedJobs(this IServiceCollection baseCollection)
    {
        #region build results

        var results = typeof(_9400ZZSYZZZ1).Assembly.GetTypes().Where(type =>
            typeof(IJob).IsAssignableFrom(c: type) && type is { IsAbstract: false, IsInterface: false });

        #endregion

        #region build services

        foreach (var item in results)
            baseCollection.AddScoped(serviceType: item);

        baseCollection.AddHostedService<UpdateService>();

        #endregion

        return baseCollection;
    }
}
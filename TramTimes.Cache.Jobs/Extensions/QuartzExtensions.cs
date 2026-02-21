// ReSharper disable all

using Quartz;
using TramTimes.Cache.Jobs.Services;
using TramTimes.Cache.Jobs.Workers.Stops;

namespace TramTimes.Cache.Jobs.Extensions;

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
// ReSharper disable all

using TramTimes.Aspire.Host.Parameters;
using TramTimes.Aspire.Host.Resources;

namespace TramTimes.Aspire.Host.Builders;

public static class RevenueBuilder
{
    private static readonly string _context = Environment.GetEnvironmentVariable(variable: "ASPIRE_CONTEXT") ?? "Development";

    public static RevenueResources BuildRevenue(this IDistributedApplicationBuilder builder)
    {
        #region build resources

        var revenue = new RevenueResources();

        #endregion

        #region add parameters

        revenue.Parameters = new RevenueParameters();

        if (builder.ExecutionContext.IsPublishMode)
            revenue.Parameters._160x300 = builder.AddParameter(
                name: "revenue-160x300",
                secret: true);

        if (builder.ExecutionContext.IsPublishMode)
            revenue.Parameters._160x600 = builder.AddParameter(
                name: "revenue-160x600",
                secret: true);

        if (builder.ExecutionContext.IsPublishMode)
            revenue.Parameters._320x50 = builder.AddParameter(
                name: "revenue-320x50",
                secret: true);

        if (builder.ExecutionContext.IsPublishMode)
            revenue.Parameters._468x60 = builder.AddParameter(
                name: "revenue-468x60",
                secret: true);

        if (builder.ExecutionContext.IsPublishMode)
            revenue.Parameters._728x90 = builder.AddParameter(
                name: "revenue-728x90",
                secret: true);

        #endregion

        return revenue;
    }
}
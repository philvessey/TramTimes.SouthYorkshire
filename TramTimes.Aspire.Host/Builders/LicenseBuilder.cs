// ReSharper disable all

using TramTimes.Aspire.Host.Parameters;
using TramTimes.Aspire.Host.Resources;

namespace TramTimes.Aspire.Host.Builders;

public static class LicenseBuilder
{
    private static readonly string _context = Environment.GetEnvironmentVariable(variable: "ASPIRE_CONTEXT") ?? "Development";

    public static LicenseResources BuildLicense(this IDistributedApplicationBuilder builder)
    {
        #region build resources

        var license = new LicenseResources();

        #endregion

        #region add parameters

        license.Parameters = new LicenseParameters();

        if (builder.ExecutionContext.IsRunMode)
            license.Parameters.Automapper = builder
                .AddParameter(
                    name: "license-automapper",
                    secret: true)
                .WithDescription(description: "Automapper license key.");

        if (builder.ExecutionContext.IsPublishMode)
            license.Parameters.Automapper = builder.AddParameter(
                name: "license-automapper",
                secret: true);

        if (builder.ExecutionContext.IsRunMode)
            license.Parameters.Telerik = builder
                .AddParameter(
                    name: "license-telerik",
                    secret: true)
                .WithDescription(description: "Telerik license key.");

        if (builder.ExecutionContext.IsPublishMode)
            license.Parameters.Telerik = builder.AddParameter(
                name: "license-telerik",
                secret: true);

        #endregion

        return license;
    }
}
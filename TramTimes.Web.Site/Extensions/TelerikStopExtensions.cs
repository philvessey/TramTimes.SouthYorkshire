using TramTimes.Web.Site.Models;

namespace TramTimes.Web.Site.Extensions;

public static class TelerikStopExtensions
{
    public static bool HasAllProperties(this TelerikStop baseStop)
    {
        #region build result

        var result = baseStop is
        {
            Id: not null,
            Code: not null,
            Name: not null,
            Latitude: not null,
            Longitude: not null,
            Platform: not null,
            Direction: not null,
            Location: not null,
            Points: not null
        };

        #endregion

        return result;
    }

    public static bool HasRequiredProperties(this TelerikStop baseStop)
    {
        #region build result

        var result = baseStop is
        {
            Id: not null,
            Name: not null,
            Latitude: not null,
            Longitude: not null,
            Platform: not null,
            Direction: not null,
            Location: not null,
            Points: not null
        };

        #endregion

        return result;
    }
}
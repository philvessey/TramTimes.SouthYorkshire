using TramTimes.Web.Site.Models;

namespace TramTimes.Web.Site.Extensions;

public static class TelerikStopPointExtensions
{
    public static bool HasAllProperties(this TelerikStopPoint baseStopPoint)
    {
        #region build result

        var result = baseStopPoint is
        {
            DepartureDateTime: not null,
            DestinationName: not null,
            DestinationDirection: not null,
            RouteName: not null,
            StopId: not null,
            StopName: not null,
            StopDirection: not null,
            TripId: not null
        };

        #endregion

        return result;
    }

    public static bool HasRequiredProperties(this TelerikStopPoint baseStopPoint)
    {
        #region build result

        var result = baseStopPoint is
        {
            DepartureDateTime: not null,
            DestinationName: not null,
            DestinationDirection: not null,
            RouteName: not null,
            StopId: not null,
            StopName: not null,
            StopDirection: not null,
            TripId: not null
        };

        #endregion

        return result;
    }
}
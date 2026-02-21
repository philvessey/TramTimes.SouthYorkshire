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

    public static bool HasNoProperties(this TelerikStopPoint baseStopPoint)
    {
        #region build result

        var result = baseStopPoint is
        {
            DepartureDateTime: null,
            DestinationName: null,
            DestinationDirection: null,
            RouteName: null,
            StopId: null,
            StopName: null,
            StopDirection: null,
            TripId: null
        };

        #endregion

        return result;
    }
}
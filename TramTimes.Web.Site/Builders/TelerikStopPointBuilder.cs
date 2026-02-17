using TramTimes.Web.Site.Models;

namespace TramTimes.Web.Site.Builders;

public static class TelerikStopPointBuilder
{
    public static TelerikStopPoint Build()
    {
        #region build result

        var result = new TelerikStopPoint
        {
            DepartureDateTime = DateTime.Now,
            DestinationName = "No Services Found",
            DestinationDirection = "Unknown",
            StopName = "No Services Found",
            StopDirection = "Unknown"
        };

        #endregion

        return result;
    }
}
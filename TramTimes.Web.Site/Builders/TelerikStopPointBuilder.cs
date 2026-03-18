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
            DestinationDirection = "Within Two Hours",
            StopName = "No Services Found",
            StopDirection = "Within Two Hours"
        };

        #endregion

        return result;
    }
}
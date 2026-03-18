using TramTimes.Web.Site.Models;

namespace TramTimes.Web.Site.Builders;

public static class TelerikStopBuilder
{
    public static TelerikStop Build()
    {
        #region build result

        var result = new TelerikStop
        {
            Name = "No Stops Were Found",
            Direction = "Adjust Map Position To Search",
            Distance = 0,
            Points = Enumerable
                .Range(start: 0, count: 5)
                .Select(selector: count => new TelerikStopPoint { DepartureDateTime = DateTime.Now.AddMinutes(value: count * 10) })
                .ToList()
        };

        #endregion

        return result;
    }
}
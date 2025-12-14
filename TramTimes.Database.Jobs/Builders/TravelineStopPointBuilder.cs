using TramTimes.Database.Jobs.Models;

namespace TramTimes.Database.Jobs.Builders;

public static class TravelineStopPointBuilder
{
    public static TravelineStopPoint Build(
        Dictionary<string, NaptanLocality> localities,
        Dictionary<string, NaptanStop> stops,
        TransXChangeStopPoints? stopPoints,
        string? reference,
        string? activity,
        TimeSpan? arrivalTime,
        TimeSpan? departureTime) {

        #region build unknown

        var unknown = new TravelineStopPoint { AtcoCode = reference ?? "unknown" };

        #endregion

        #region build result

        var result = new TravelineStopPoint
        {
            AtcoCode = reference,
            Activity = activity,
            ArrivalTime = arrivalTime,
            DepartureTime = departureTime
        };

        if (!arrivalTime.HasValue || !departureTime.HasValue)
            return unknown;

        result.NaptanStop = NaptanStopBuilder.Build(
            stops: stops,
            reference: reference);

        result.TravelineStop = TravelineStopBuilder.Build(
            localities: localities,
            stopPoints: stopPoints,
            reference: reference);

        #endregion

        return result;
    }
}
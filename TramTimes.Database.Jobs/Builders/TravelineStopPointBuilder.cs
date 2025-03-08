using TramTimes.Database.Jobs.Models;

namespace TramTimes.Database.Jobs.Builders;

public static class TravelineStopPointBuilder
{
    public static async Task<TravelineStopPoint> BuildAsync(
        Dictionary<string, NaptanLocality> localities,
        Dictionary<string, NaptanStop> stops,
        TransXChangeStopPoints? stopPoints,
        string? reference,
        string? activity,
        TimeSpan? arrivalTime,
        TimeSpan? departureTime) {
        
        return await Task.FromResult(result: new TravelineStopPoint
        {
            AtcoCode = reference,
            Activity = activity,
            ArrivalTime = arrivalTime,
            DepartureTime = departureTime,
            
            NaptanStop = await NaptanStopBuilder.BuildAsync(
                stops: stops,
                reference: reference),
            
            TravelineStop = await TravelineStopBuilder.BuildAsync(
                localities: localities,
                stopPoints: stopPoints,
                reference: reference)
        });
    }
}
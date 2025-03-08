using TramTimes.Database.Jobs.Models;

namespace TramTimes.Database.Jobs.Builders;

public static class TravelineScheduleBuilder
{
    public static async Task<TravelineSchedule> BuildAsync(
        TransXChangeOperators? operators,
        TransXChangeServices? services,
        TransXChangeJourneyPattern? journeyPattern,
        TravelineCalendar? calendar) {
        
        var value = new TravelineSchedule
        {
            Id = Guid.NewGuid().ToString(),
            Description = services?.Service?.Description?.Trim(),
            Direction = journeyPattern?.Direction == "inbound" ? "1" : "0",
            Line = services?.Service?.Lines?.Line?.LineName,
            Mode = "0",
            OperatorCode = operators?.Operator?.NationalOperatorCode,
            OperatorName = operators?.Operator?.TradingName,
            OperatorPhone = operators?.Operator?.ContactTelephoneNumber?.TelNationalNumber,
            ServiceCode = services?.Service?.ServiceCode,
            Calendar = calendar,
            StopPoints = []
        };
        
        if (string.IsNullOrWhiteSpace(value: value.OperatorName))
            value.OperatorName = operators?.Operator?.OperatorNameOnLicence;
        
        if (string.IsNullOrWhiteSpace(value: value.OperatorName))
            value.OperatorName = operators?.Operator?.OperatorShortName;
        
        if (string.IsNullOrWhiteSpace(value: value.OperatorPhone))
            value.OperatorPhone = operators?.Operator?.EnquiryTelephoneNumber?.TelNationalNumber;
        
        return await Task.FromResult(result: value);
    }
}
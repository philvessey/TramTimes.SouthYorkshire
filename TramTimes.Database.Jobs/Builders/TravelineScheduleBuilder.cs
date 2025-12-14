using TramTimes.Database.Jobs.Models;
using TramTimes.Database.Jobs.Tools;

namespace TramTimes.Database.Jobs.Builders;

public static class TravelineScheduleBuilder
{
    public static TravelineSchedule Build(
        TransXChangeOperators? operators,
        TransXChangeServices? services,
        TransXChangeJourneyPattern? journeyPattern,
        TravelineCalendar? calendar) {

        #region build unknown

        var guid = Guid.NewGuid();

        var unknown = new TravelineSchedule { Id = guid.ToString() };

        #endregion

        #region build result

        var result = new TravelineSchedule
        {
            Id = guid.ToString(),
            Description = services?.Service?.Description?.Trim(),
            Direction = TravelineScheduleTools.GetServiceDirection(direction: journeyPattern?.Direction),
            Line = services?.Service?.Lines?.Line?.LineName,
            Mode = "0",
            OperatorCode = operators?.Operator?.NationalOperatorCode,
            OperatorName = operators?.Operator?.TradingName,
            OperatorPhone = operators?.Operator?.ContactTelephoneNumber?.TelNationalNumber,
            ServiceCode = services?.Service?.ServiceCode,
            Calendar = calendar,
            StopPoints = []
        };

        if (calendar is null)
            return unknown;

        if (string.IsNullOrWhiteSpace(value: result.OperatorName))
            result.OperatorName = operators?.Operator?.OperatorNameOnLicence;

        if (string.IsNullOrWhiteSpace(value: result.OperatorName))
            result.OperatorName = operators?.Operator?.OperatorShortName;

        if (string.IsNullOrWhiteSpace(value: result.OperatorPhone))
            result.OperatorPhone = operators?.Operator?.EnquiryTelephoneNumber?.TelNationalNumber;

        #endregion

        return result;
    }
}
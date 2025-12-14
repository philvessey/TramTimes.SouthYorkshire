using TramTimes.Database.Jobs.Models;

namespace TramTimes.Database.Jobs.Tools;

public static class HolidayTools
{
    public static Holiday GetNewYearsDay(
        DateOnly? startDate,
        DateOnly? endDate) {

        #region check valid input

        if (!startDate.HasValue || !endDate.HasValue)
            return new Holiday();

        #endregion

        #region build result

        var result = new Holiday
        {
            Date = new DateOnly(
                year: endDate.Value > startDate.Value
                    ? endDate.Value.Year
                    : startDate.Value.Year,
                month: 1,
                day: 1)
        };

        #endregion

        return result;
    }

    public static Holiday GetNewYearsDayHoliday(
        List<Holiday> holidays,
        DateOnly? startDate,
        DateOnly? endDate) {

        #region check valid input

        if (!startDate.HasValue || !endDate.HasValue)
            return new Holiday();

        #endregion

        #region build result

        var result = holidays.FirstOrDefault(predicate: holiday =>
            holiday is { LocalName: "New Year's Day", Counties: not null } &&
            holiday.Counties.Contains(value: "GB-ENG")) ?? new Holiday();

        #endregion

        return result;
    }

    public static Holiday GetGoodFriday(
        List<Holiday> holidays,
        DateOnly? startDate,
        DateOnly? endDate) {

        #region check valid input

        if (!startDate.HasValue || !endDate.HasValue)
            return new Holiday();

        #endregion

        #region build result

        var result = holidays.FirstOrDefault(predicate: holiday =>
            holiday is { LocalName: "Good Friday", Global: true }) ?? new Holiday();

        #endregion

        return result;
    }

    public static Holiday GetEasterMonday(
        List<Holiday> holidays,
        DateOnly? startDate,
        DateOnly? endDate) {

        #region check valid input

        if (!startDate.HasValue || !endDate.HasValue)
            return new Holiday();

        #endregion

        #region build result

        var result = holidays.FirstOrDefault(predicate: holiday =>
            holiday is { LocalName: "Easter Monday", Counties: not null } &&
            holiday.Counties.Contains(value: "GB-ENG")) ?? new Holiday();

        #endregion

        return result;
    }

    public static Holiday GetMayDay(
        List<Holiday> holidays,
        DateOnly? startDate,
        DateOnly? endDate) {

        #region check valid input

        if (!startDate.HasValue || !endDate.HasValue)
            return new Holiday();

        #endregion

        #region build result

        var result = holidays.FirstOrDefault(predicate: holiday =>
            holiday is { LocalName: "Early May Bank Holiday", Global: true }) ?? new Holiday();

        #endregion

        return result;
    }

    public static Holiday GetSpringBank(
        List<Holiday> holidays,
        DateOnly? startDate,
        DateOnly? endDate) {

        #region check valid input

        if (!startDate.HasValue || !endDate.HasValue)
            return new Holiday();

        #endregion

        #region build result

        var result = holidays.FirstOrDefault(predicate: holiday =>
            holiday is { LocalName: "Spring Bank Holiday", Global: true }) ?? new Holiday();

        #endregion

        return result;
    }

    public static Holiday GetLateSummerBankHolidayNotScotland(
        List<Holiday> holidays,
        DateOnly? startDate,
        DateOnly? endDate) {

        #region check valid input

        if (!startDate.HasValue || !endDate.HasValue)
            return new Holiday();

        #endregion

        #region build result

        var result = holidays.FirstOrDefault(predicate: holiday =>
            holiday is { LocalName: "Summer Bank Holiday", Counties: not null } &&
            holiday.Counties.Contains(value: "GB-ENG")) ?? new Holiday();

        #endregion

        return result;
    }

    public static Holiday GetChristmasEve(
        DateOnly? startDate,
        DateOnly? endDate) {

        #region check valid input

        if (!startDate.HasValue || !endDate.HasValue)
            return new Holiday();

        #endregion

        #region build result

        var result = new Holiday
        {
            Date = new DateOnly(
                year: endDate.Value > startDate.Value
                    ? endDate.Value.Year
                    : startDate.Value.Year,
                month: 12,
                day: 24)
        };

        #endregion

        return result;
    }

    public static Holiday GetChristmasDay(
        DateOnly? startDate,
        DateOnly? endDate) {

        #region check valid input

        if (!startDate.HasValue || !endDate.HasValue)
            return new Holiday();

        #endregion

        #region build result

        var result = new Holiday
        {
            Date = new DateOnly(
                year: endDate.Value > startDate.Value
                    ? endDate.Value.Year
                    : startDate.Value.Year,
                month: 12,
                day: 25)
        };

        #endregion

        return result;
    }

    public static Holiday GetChristmasDayHoliday(
        List<Holiday> holidays,
        DateOnly? startDate,
        DateOnly? endDate) {

        #region check valid input

        if (!startDate.HasValue || !endDate.HasValue)
            return new Holiday();

        #endregion

        #region build result

        var result = holidays.FirstOrDefault(predicate: holiday =>
            holiday is { LocalName: "Christmas Day", Global: true }) ?? new Holiday();

        #endregion

        return result;
    }

    public static Holiday GetBoxingDay(
        DateOnly? startDate,
        DateOnly? endDate) {

        #region check valid input

        if (!startDate.HasValue || !endDate.HasValue)
            return new Holiday();

        #endregion

        #region build result

        var result = new Holiday
        {
            Date = new DateOnly(
                year: endDate.Value > startDate.Value
                    ? endDate.Value.Year
                    : startDate.Value.Year,
                month: 12,
                day: 26)
        };

        #endregion

        return result;
    }

    public static Holiday GetBoxingDayHoliday(
        List<Holiday> holidays,
        DateOnly? startDate,
        DateOnly? endDate) {

        #region check valid input

        if (!startDate.HasValue || !endDate.HasValue)
            return new Holiday();

        #endregion

        #region build result

        var result = holidays.FirstOrDefault(predicate: holiday =>
            holiday is { LocalName: "Boxing Day", Global: true }) ?? new Holiday();

        #endregion

        return result;
    }

    public static Holiday GetNewYearsEve(
        DateOnly? startDate,
        DateOnly? endDate) {

        #region check valid input

        if (!startDate.HasValue || !endDate.HasValue)
            return new Holiday();

        #endregion

        #region build result

        var result = new Holiday
        {
            Date = new DateOnly(
                year: endDate.Value > startDate.Value
                    ? endDate.Value.Year
                    : startDate.Value.Year,
                month: 12,
                day: 31)
        };

        #endregion

        return result;
    }
}
using TramTimes.Database.Jobs.Models;

namespace TramTimes.Database.Jobs.Tools;

public static class TransXChangeDaysOfNonOperationTools
{
    public static List<DateOnly> GetAllHolidays(
        List<Holiday> holidays,
        TransXChangeDaysOfNonOperation? daysOfNonOperation,
        DateOnly? startDate,
        DateOnly? endDate) {

        List<Holiday> results = [];

        #region check valid input

        if (!startDate.HasValue || !endDate.HasValue)
            return [];

        #endregion

        #region get all bank holidays

        if (daysOfNonOperation?.AllBankHolidays is not null)
        {
            results.Add(item: HolidayTools.GetNewYearsDay(
                startDate: startDate,
                endDate: endDate));

            results.Add(item: HolidayTools.GetNewYearsDayHoliday(
                holidays: holidays,
                startDate: startDate,
                endDate: endDate));

            results.Add(item: HolidayTools.GetGoodFriday(
                holidays: holidays,
                startDate: startDate,
                endDate: endDate));

            results.Add(item: HolidayTools.GetEasterMonday(
                holidays: holidays,
                startDate: startDate,
                endDate: endDate));

            results.Add(item: HolidayTools.GetMayDay(
                holidays: holidays,
                startDate: startDate,
                endDate: endDate));

            results.Add(item: HolidayTools.GetSpringBank(
                holidays: holidays,
                startDate: startDate,
                endDate: endDate));

            results.Add(item: HolidayTools.GetLateSummerBankHolidayNotScotland(
                holidays: holidays,
                startDate: startDate,
                endDate: endDate));

            results.Add(item: HolidayTools.GetChristmasDay(
                startDate: startDate,
                endDate: endDate));

            results.Add(item: HolidayTools.GetChristmasDayHoliday(
                holidays: holidays,
                startDate: startDate,
                endDate: endDate));

            results.Add(item: HolidayTools.GetBoxingDay(
                startDate: startDate,
                endDate: endDate));

            results.Add(item: HolidayTools.GetBoxingDayHoliday(
                holidays: holidays,
                startDate: startDate,
                endDate: endDate));
        }

        #endregion

        #region get all holidays except christmas

        if (daysOfNonOperation?.AllHolidaysExceptChristmas is not null)
        {
            results.Add(item: HolidayTools.GetNewYearsDay(
                startDate: startDate,
                endDate: endDate));

            results.Add(item: HolidayTools.GetGoodFriday(
                holidays: holidays,
                startDate: startDate,
                endDate: endDate));

            results.Add(item: HolidayTools.GetEasterMonday(
                holidays: holidays,
                startDate: startDate,
                endDate: endDate));

            results.Add(item: HolidayTools.GetMayDay(
                holidays: holidays,
                startDate: startDate,
                endDate: endDate));

            results.Add(item: HolidayTools.GetSpringBank(
                holidays: holidays,
                startDate: startDate,
                endDate: endDate));

            results.Add(item: HolidayTools.GetLateSummerBankHolidayNotScotland(
                holidays: holidays,
                startDate: startDate,
                endDate: endDate));
        }

        #endregion

        #region get christmas

        if (daysOfNonOperation?.Christmas is not null)
        {
            results.Add(item: HolidayTools.GetChristmasDay(
                startDate: startDate,
                endDate: endDate));

            results.Add(item: HolidayTools.GetBoxingDay(
                startDate: startDate,
                endDate: endDate));
        }

        #endregion

        #region get displacement holidays

        if (daysOfNonOperation?.DisplacementHolidays is not null)
        {
            results.Add(item: HolidayTools.GetNewYearsDayHoliday(
                holidays: holidays,
                startDate: startDate,
                endDate: endDate));

            results.Add(item: HolidayTools.GetChristmasDayHoliday(
                holidays: holidays,
                startDate: startDate,
                endDate: endDate));

            results.Add(item: HolidayTools.GetBoxingDayHoliday(
                holidays: holidays,
                startDate: startDate,
                endDate: endDate));
        }

        #endregion

        #region get early run off

        if (daysOfNonOperation?.EarlyRunOff is not null)
        {
            results.Add(item: HolidayTools.GetChristmasEve(
                startDate: startDate,
                endDate: endDate));

            results.Add(item: HolidayTools.GetNewYearsEve(
                startDate: startDate,
                endDate: endDate));
        }

        #endregion

        #region get holiday mondays

        if (daysOfNonOperation?.HolidayMondays is not null)
        {
            results.Add(item: HolidayTools.GetEasterMonday(
                holidays: holidays,
                startDate: startDate,
                endDate: endDate));

            results.Add(item: HolidayTools.GetMayDay(
                holidays: holidays,
                startDate: startDate,
                endDate: endDate));

            results.Add(item: HolidayTools.GetSpringBank(
                holidays: holidays,
                startDate: startDate,
                endDate: endDate));

            results.Add(item: HolidayTools.GetLateSummerBankHolidayNotScotland(
                holidays: holidays,
                startDate: startDate,
                endDate: endDate));
        }

        #endregion

        #region get holidays

        if (daysOfNonOperation?.Holidays is not null)
        {
            results.Add(item: HolidayTools.GetNewYearsDay(
                startDate: startDate,
                endDate: endDate));

            results.Add(item: HolidayTools.GetGoodFriday(
                holidays: holidays,
                startDate: startDate,
                endDate: endDate));
        }

        #endregion

        #region get new years day

        if (daysOfNonOperation?.NewYearsDay is not null)
            results.Add(item: HolidayTools.GetNewYearsDay(
                startDate: startDate,
                endDate: endDate));

        #endregion

        #region get new years day holiday

        if (daysOfNonOperation?.NewYearsDayHoliday is not null)
            results.Add(item: HolidayTools.GetNewYearsDayHoliday(
                holidays: holidays,
                startDate: startDate,
                endDate: endDate));

        #endregion

        #region get good friday

        if (daysOfNonOperation?.GoodFriday is not null)
            results.Add(item: HolidayTools.GetGoodFriday(
                holidays: holidays,
                startDate: startDate,
                endDate: endDate));

        #endregion

        #region get easter monday

        if (daysOfNonOperation?.EasterMonday is not null)
            results.Add(item: HolidayTools.GetEasterMonday(
                holidays: holidays,
                startDate: startDate,
                endDate: endDate));

        #endregion

        #region get may day

        if (daysOfNonOperation?.MayDay is not null)
            results.Add(item: HolidayTools.GetMayDay(
                holidays: holidays,
                startDate: startDate,
                endDate: endDate));

        #endregion

        #region get spring bank

        if (daysOfNonOperation?.SpringBank is not null)
            results.Add(item: HolidayTools.GetSpringBank(
                holidays: holidays,
                startDate: startDate,
                endDate: endDate));

        #endregion

        #region get late summer bank holiday not scotland

        if (daysOfNonOperation?.LateSummerBankHolidayNotScotland is not null)
            results.Add(item: HolidayTools.GetLateSummerBankHolidayNotScotland(
                holidays: holidays,
                startDate: startDate,
                endDate: endDate));

        #endregion

        #region get christmas eve

        if (daysOfNonOperation?.ChristmasEve is not null)
            results.Add(item: HolidayTools.GetChristmasEve(
                startDate: startDate,
                endDate: endDate));

        #endregion

        #region get christmas day

        if (daysOfNonOperation?.ChristmasDay is not null)
            results.Add(item: HolidayTools.GetChristmasDay(
                startDate: startDate,
                endDate: endDate));

        #endregion

        #region get christmas day holiday

        if (daysOfNonOperation?.ChristmasDayHoliday is not null)
            results.Add(item: HolidayTools.GetChristmasDayHoliday(
                holidays: holidays,
                startDate: startDate,
                endDate: endDate));

        #endregion

        #region get boxing day

        if (daysOfNonOperation?.BoxingDay is not null)
            results.Add(item: HolidayTools.GetBoxingDay(
                startDate: startDate,
                endDate: endDate));

        #endregion

        #region get boxing day holiday

        if (daysOfNonOperation?.BoxingDayHoliday is not null)
            results.Add(item: HolidayTools.GetBoxingDayHoliday(
                holidays: holidays,
                startDate: startDate,
                endDate: endDate));

        #endregion

        #region get new years eve

        if (daysOfNonOperation?.NewYearsEve is not null)
            results.Add(item: HolidayTools.GetNewYearsEve(
                startDate: startDate,
                endDate: endDate));

        #endregion

        return results
            .Where(holiday => holiday.Date >= startDate && holiday.Date <= endDate)
            .Select(holiday => holiday.Date)
            .Where(date => date.HasValue)
            .Select(date => date!.Value)
            .Distinct()
            .OrderBy(date => date)
            .ToList();
    }
}
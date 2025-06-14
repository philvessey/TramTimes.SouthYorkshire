using Nager.Date;
using Nager.Date.Models;
using TramTimes.Database.Jobs.Models;

namespace TramTimes.Database.Jobs.Tools;

public static class TransXChangeDaysOfOperationTools
{
    private static readonly string HolidaySystemKey = Environment.GetEnvironmentVariable(variable: "LICENSE_KEY") ?? string.Empty;
    
    public static List<DateTime> GetAllHolidays(
        TransXChangeDaysOfOperation? daysOfOperation,
        DateTime? startDate,
        DateTime? endDate) {
        
        List<Holiday> results = [];
        
        #region check license key
        
        if (string.IsNullOrEmpty(value: HolidaySystem.LicenseKey))
            HolidaySystem.LicenseKey = HolidaySystemKey;
        
        #endregion
        
        #region check valid input
        
        if (!startDate.HasValue || !endDate.HasValue)
            return [];
        
        #endregion
        
        #region get all bank holidays
        
        if (daysOfOperation?.AllBankHolidays is not null)
        {
            results.Add(item: HolidayTools.GetNewYearsDay(
                startDate: startDate,
                endDate: endDate));
            
            results.Add(item: HolidayTools.GetNewYearsDayHoliday(
                startDate: startDate,
                endDate: endDate));
            
            results.Add(item: HolidayTools.GetGoodFriday(
                startDate: startDate,
                endDate: endDate));
            
            results.Add(item: HolidayTools.GetEasterMonday(
                startDate: startDate,
                endDate: endDate));
            
            results.Add(item: HolidayTools.GetMayDay(
                startDate: startDate,
                endDate: endDate));
            
            results.Add(item: HolidayTools.GetSpringBank(
                startDate: startDate,
                endDate: endDate));
            
            results.Add(item: HolidayTools.GetLateSummerBankHolidayNotScotland(
                startDate: startDate,
                endDate: endDate));
            
            results.Add(item: HolidayTools.GetChristmasDay(
                startDate: startDate,
                endDate: endDate));
            
            results.Add(item: HolidayTools.GetChristmasDayHoliday(
                startDate: startDate,
                endDate: endDate));
            
            results.Add(item: HolidayTools.GetBoxingDay(
                startDate: startDate,
                endDate: endDate));
            
            results.Add(item: HolidayTools.GetBoxingDayHoliday(
                startDate: startDate,
                endDate: endDate));
        }
        
        #endregion
        
        #region get all holidays except christmas
        
        if (daysOfOperation?.AllHolidaysExceptChristmas is not null)
        {
            results.Add(item: HolidayTools.GetNewYearsDay(
                startDate: startDate,
                endDate: endDate));
            
            results.Add(item: HolidayTools.GetGoodFriday(
                startDate: startDate,
                endDate: endDate));
            
            results.Add(item: HolidayTools.GetEasterMonday(
                startDate: startDate,
                endDate: endDate));
            
            results.Add(item: HolidayTools.GetMayDay(
                startDate: startDate,
                endDate: endDate));
            
            results.Add(item: HolidayTools.GetSpringBank(
                startDate: startDate,
                endDate: endDate));
            
            results.Add(item: HolidayTools.GetLateSummerBankHolidayNotScotland(
                startDate: startDate,
                endDate: endDate));
        }
        
        #endregion
        
        #region get christmas
        
        if (daysOfOperation?.Christmas is not null)
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
        
        if (daysOfOperation?.DisplacementHolidays is not null)
        {
            results.Add(item: HolidayTools.GetNewYearsDayHoliday(
                startDate: startDate,
                endDate: endDate));
            
            results.Add(item: HolidayTools.GetChristmasDayHoliday(
                startDate: startDate,
                endDate: endDate));
            
            results.Add(item: HolidayTools.GetBoxingDayHoliday(
                startDate: startDate,
                endDate: endDate));
        }
        
        #endregion
        
        #region get early run off
        
        if (daysOfOperation?.EarlyRunOff is not null)
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
        
        if (daysOfOperation?.HolidayMondays is not null)
        {
            results.Add(item: HolidayTools.GetEasterMonday(
                startDate: startDate,
                endDate: endDate));
            
            results.Add(item: HolidayTools.GetMayDay(
                startDate: startDate,
                endDate: endDate));
            
            results.Add(item: HolidayTools.GetSpringBank(
                startDate: startDate,
                endDate: endDate));
            
            results.Add(item: HolidayTools.GetLateSummerBankHolidayNotScotland(
                startDate: startDate,
                endDate: endDate));
        }
        
        #endregion
        
        #region get holidays
        
        if (daysOfOperation?.Holidays is not null)
        {
            results.Add(item: HolidayTools.GetNewYearsDay(
                startDate: startDate,
                endDate: endDate));
            
            results.Add(item: HolidayTools.GetGoodFriday(
                startDate: startDate,
                endDate: endDate));
        }
        
        #endregion
        
        #region get new years day
        
        if (daysOfOperation?.NewYearsDay is not null)
            results.Add(item: HolidayTools.GetNewYearsDay(
                startDate: startDate,
                endDate: endDate));
        
        #endregion
        
        #region get new years day holiday
        
        if (daysOfOperation?.NewYearsDayHoliday is not null)
            results.Add(item: HolidayTools.GetNewYearsDayHoliday(
                startDate: startDate,
                endDate: endDate));
        
        #endregion
        
        #region get good friday
        
        if (daysOfOperation?.GoodFriday is not null)
            results.Add(item: HolidayTools.GetGoodFriday(
                startDate: startDate,
                endDate: endDate));
        
        #endregion
        
        #region get easter monday
        
        if (daysOfOperation?.EasterMonday is not null)
            results.Add(item: HolidayTools.GetEasterMonday(
                startDate: startDate,
                endDate: endDate));
        
        #endregion
        
        #region get may day
        
        if (daysOfOperation?.MayDay is not null)
            results.Add(item: HolidayTools.GetMayDay(
                startDate: startDate,
                endDate: endDate));
        
        #endregion
        
        #region get spring bank
        
        if (daysOfOperation?.SpringBank is not null)
            results.Add(item: HolidayTools.GetSpringBank(
                startDate: startDate,
                endDate: endDate));
        
        #endregion
        
        #region get late summer bank holiday not scotland
        
        if (daysOfOperation?.LateSummerBankHolidayNotScotland is not null)
            results.Add(item: HolidayTools.GetLateSummerBankHolidayNotScotland(
                startDate: startDate,
                endDate: endDate));
        
        #endregion
        
        #region get christmas eve
        
        if (daysOfOperation?.ChristmasEve is not null)
            results.Add(item: HolidayTools.GetChristmasEve(
                startDate: startDate,
                endDate: endDate));
        
        #endregion
        
        #region get christmas day
        
        if (daysOfOperation?.ChristmasDay is not null)
            results.Add(item: HolidayTools.GetChristmasDay(
                startDate: startDate,
                endDate: endDate));
        
        #endregion
        
        #region get christmas day holiday
        
        if (daysOfOperation?.ChristmasDayHoliday is not null)
            results.Add(item: HolidayTools.GetChristmasDayHoliday(
                startDate: startDate,
                endDate: endDate));
        
        #endregion
        
        #region get boxing day
        
        if (daysOfOperation?.BoxingDay is not null)
            results.Add(item: HolidayTools.GetBoxingDay(
                startDate: startDate,
                endDate: endDate));
        
        #endregion
        
        #region get boxing day holiday
        
        if (daysOfOperation?.BoxingDayHoliday is not null)
            results.Add(item: HolidayTools.GetBoxingDayHoliday(
                startDate: startDate,
                endDate: endDate));
        
        #endregion
        
        #region get new years eve
        
        if (daysOfOperation?.NewYearsEve is not null)
            results.Add(item: HolidayTools.GetNewYearsEve(
                startDate: startDate,
                endDate: endDate));
        
        #endregion
        
        return results
            .Where(predicate: holiday => holiday.Date >= startDate && holiday.Date <= endDate)
            .Select(selector: holiday => holiday.Date)
            .Distinct()
            .OrderBy(keySelector: date => date)
            .ToList();
    }
}
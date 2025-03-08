using Nager.Date;
using Nager.Date.Models;
using TramTimes.Database.Jobs.Models;

namespace TramTimes.Database.Jobs.Tools;

public static class TransXChangeDaysOfOperationTools
{
    private static readonly string HolidaySystemKey = Environment.GetEnvironmentVariable(variable: "LICENSE_KEY") ?? string.Empty;
    
    public static async Task<List<DateTime>> GetAllHolidaysAsync(
        TransXChangeDaysOfOperation? daysOfOperation,
        DateTime? startDate,
        DateTime? endDate) {
        
        if (string.IsNullOrEmpty(value: HolidaySystem.LicenseKey))
            HolidaySystem.LicenseKey = HolidaySystemKey;
        
        if (!startDate.HasValue || !endDate.HasValue)
            return await Task.FromResult(result: new List<DateTime>());
        
        List<Holiday> results = [];
        
        if (daysOfOperation?.AllBankHolidays != null)
        {
            results.Add(item: await HolidayTools.GetNewYearsDayAsync(
                startDate: startDate,
                endDate: endDate));
            
            results.Add(item: await HolidayTools.GetNewYearsDayHolidayAsync(
                startDate: startDate,
                endDate: endDate));
            
            results.Add(item: await HolidayTools.GetGoodFridayAsync(
                startDate: startDate,
                endDate: endDate));
            
            results.Add(item: await HolidayTools.GetEasterMondayAsync(
                startDate: startDate,
                endDate: endDate));
            
            results.Add(item: await HolidayTools.GetMayDayAsync(
                startDate: startDate,
                endDate: endDate));
            
            results.Add(item: await HolidayTools.GetSpringBankAsync(
                startDate: startDate,
                endDate: endDate));
            
            results.Add(item: await HolidayTools.GetLateSummerBankHolidayNotScotlandAsync(
                startDate: startDate,
                endDate: endDate));
            
            results.Add(item: await HolidayTools.GetChristmasDayAsync(
                startDate: startDate,
                endDate: endDate));
            
            results.Add(item: await HolidayTools.GetChristmasDayHolidayAsync(
                startDate: startDate,
                endDate: endDate));
            
            results.Add(item: await HolidayTools.GetBoxingDayAsync(
                startDate: startDate,
                endDate: endDate));
            
            results.Add(item: await HolidayTools.GetBoxingDayHolidayAsync(
                startDate: startDate,
                endDate: endDate));
        }
        
        if (daysOfOperation?.AllHolidaysExceptChristmas != null)
        {
            results.Add(item: await HolidayTools.GetNewYearsDayAsync(
                startDate: startDate,
                endDate: endDate));
            
            results.Add(item: await HolidayTools.GetGoodFridayAsync(
                startDate: startDate,
                endDate: endDate));
            
            results.Add(item: await HolidayTools.GetEasterMondayAsync(
                startDate: startDate,
                endDate: endDate));
            
            results.Add(item: await HolidayTools.GetMayDayAsync(
                startDate: startDate,
                endDate: endDate));
            
            results.Add(item: await HolidayTools.GetSpringBankAsync(
                startDate: startDate,
                endDate: endDate));
            
            results.Add(item: await HolidayTools.GetLateSummerBankHolidayNotScotlandAsync(
                startDate: startDate,
                endDate: endDate));
        }
        
        if (daysOfOperation?.Christmas != null)
        {
            results.Add(item: await HolidayTools.GetChristmasDayAsync(
                startDate: startDate,
                endDate: endDate));
            
            results.Add(item: await HolidayTools.GetBoxingDayAsync(
                startDate: startDate,
                endDate: endDate));
        }
        
        if (daysOfOperation?.DisplacementHolidays != null)
        {
            results.Add(item: await HolidayTools.GetNewYearsDayHolidayAsync(
                startDate: startDate,
                endDate: endDate));
            
            results.Add(item: await HolidayTools.GetChristmasDayHolidayAsync(
                startDate: startDate,
                endDate: endDate));
            
            results.Add(item: await HolidayTools.GetBoxingDayHolidayAsync(
                startDate: startDate,
                endDate: endDate));
        }
        
        if (daysOfOperation?.EarlyRunOff != null)
        {
            results.Add(item: await HolidayTools.GetChristmasEveAsync(
                startDate: startDate,
                endDate: endDate));
            
            results.Add(item: await HolidayTools.GetNewYearsEveAsync(
                startDate: startDate,
                endDate: endDate));
        }
        
        if (daysOfOperation?.HolidayMondays != null)
        {
            results.Add(item: await HolidayTools.GetEasterMondayAsync(
                startDate: startDate,
                endDate: endDate));
            
            results.Add(item: await HolidayTools.GetMayDayAsync(
                startDate: startDate,
                endDate: endDate));
            
            results.Add(item: await HolidayTools.GetSpringBankAsync(
                startDate: startDate,
                endDate: endDate));
            
            results.Add(item: await HolidayTools.GetLateSummerBankHolidayNotScotlandAsync(
                startDate: startDate,
                endDate: endDate));
        }
        
        if (daysOfOperation?.Holidays != null)
        {
            results.Add(item: await HolidayTools.GetNewYearsDayAsync(
                startDate: startDate,
                endDate: endDate));
            
            results.Add(item: await HolidayTools.GetGoodFridayAsync(
                startDate: startDate,
                endDate: endDate));
        }
        
        if (daysOfOperation?.NewYearsDay != null)
            results.Add(item: await HolidayTools.GetNewYearsDayAsync(
                startDate: startDate,
                endDate: endDate));
        
        if (daysOfOperation?.NewYearsDayHoliday != null)
            results.Add(item: await HolidayTools.GetNewYearsDayHolidayAsync(
                startDate: startDate,
                endDate: endDate));
        
        if (daysOfOperation?.GoodFriday != null)
            results.Add(item: await HolidayTools.GetGoodFridayAsync(
                startDate: startDate,
                endDate: endDate));
        
        if (daysOfOperation?.EasterMonday != null)
            results.Add(item: await HolidayTools.GetEasterMondayAsync(
                startDate: startDate,
                endDate: endDate));
        
        if (daysOfOperation?.MayDay != null)
            results.Add(item: await HolidayTools.GetMayDayAsync(
                startDate: startDate,
                endDate: endDate));
        
        if (daysOfOperation?.SpringBank != null)
            results.Add(item: await HolidayTools.GetSpringBankAsync(
                startDate: startDate,
                endDate: endDate));
        
        if (daysOfOperation?.LateSummerBankHolidayNotScotland != null)
            results.Add(item: await HolidayTools.GetLateSummerBankHolidayNotScotlandAsync(
                startDate: startDate,
                endDate: endDate));
        
        if (daysOfOperation?.ChristmasEve != null)
            results.Add(item: await HolidayTools.GetChristmasEveAsync(
                startDate: startDate,
                endDate: endDate));
        
        if (daysOfOperation?.ChristmasDay != null)
            results.Add(item: await HolidayTools.GetChristmasDayAsync(
                startDate: startDate,
                endDate: endDate));
        
        if (daysOfOperation?.ChristmasDayHoliday != null)
            results.Add(item: await HolidayTools.GetChristmasDayHolidayAsync(
                startDate: startDate,
                endDate: endDate));
        
        if (daysOfOperation?.BoxingDay != null)
            results.Add(item: await HolidayTools.GetBoxingDayAsync(
                startDate: startDate,
                endDate: endDate));
        
        if (daysOfOperation?.BoxingDayHoliday != null)
            results.Add(item: await HolidayTools.GetBoxingDayHolidayAsync(
                startDate: startDate,
                endDate: endDate));
        
        if (daysOfOperation?.NewYearsEve != null)
            results.Add(item: await HolidayTools.GetNewYearsEveAsync(
                startDate: startDate,
                endDate: endDate));
        
        return await Task.FromResult(result: results
            .Where(predicate: holiday => holiday.Date >= startDate && holiday.Date <= endDate)
            .Select(selector: holiday => holiday.Date)
            .Distinct()
            .OrderBy(keySelector: date => date)
            .ToList());
    }
}
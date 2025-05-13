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
        
        if (daysOfOperation?.AllBankHolidays is not null)
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
        
        if (daysOfOperation?.AllHolidaysExceptChristmas is not null)
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
        
        if (daysOfOperation?.Christmas is not null)
        {
            results.Add(item: await HolidayTools.GetChristmasDayAsync(
                startDate: startDate,
                endDate: endDate));
            
            results.Add(item: await HolidayTools.GetBoxingDayAsync(
                startDate: startDate,
                endDate: endDate));
        }
        
        if (daysOfOperation?.DisplacementHolidays is not null)
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
        
        if (daysOfOperation?.EarlyRunOff is not null)
        {
            results.Add(item: await HolidayTools.GetChristmasEveAsync(
                startDate: startDate,
                endDate: endDate));
            
            results.Add(item: await HolidayTools.GetNewYearsEveAsync(
                startDate: startDate,
                endDate: endDate));
        }
        
        if (daysOfOperation?.HolidayMondays is not null)
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
        
        if (daysOfOperation?.Holidays is not null)
        {
            results.Add(item: await HolidayTools.GetNewYearsDayAsync(
                startDate: startDate,
                endDate: endDate));
            
            results.Add(item: await HolidayTools.GetGoodFridayAsync(
                startDate: startDate,
                endDate: endDate));
        }
        
        if (daysOfOperation?.NewYearsDay is not null)
            results.Add(item: await HolidayTools.GetNewYearsDayAsync(
                startDate: startDate,
                endDate: endDate));
        
        if (daysOfOperation?.NewYearsDayHoliday is not null)
            results.Add(item: await HolidayTools.GetNewYearsDayHolidayAsync(
                startDate: startDate,
                endDate: endDate));
        
        if (daysOfOperation?.GoodFriday is not null)
            results.Add(item: await HolidayTools.GetGoodFridayAsync(
                startDate: startDate,
                endDate: endDate));
        
        if (daysOfOperation?.EasterMonday is not null)
            results.Add(item: await HolidayTools.GetEasterMondayAsync(
                startDate: startDate,
                endDate: endDate));
        
        if (daysOfOperation?.MayDay is not null)
            results.Add(item: await HolidayTools.GetMayDayAsync(
                startDate: startDate,
                endDate: endDate));
        
        if (daysOfOperation?.SpringBank is not null)
            results.Add(item: await HolidayTools.GetSpringBankAsync(
                startDate: startDate,
                endDate: endDate));
        
        if (daysOfOperation?.LateSummerBankHolidayNotScotland is not null)
            results.Add(item: await HolidayTools.GetLateSummerBankHolidayNotScotlandAsync(
                startDate: startDate,
                endDate: endDate));
        
        if (daysOfOperation?.ChristmasEve is not null)
            results.Add(item: await HolidayTools.GetChristmasEveAsync(
                startDate: startDate,
                endDate: endDate));
        
        if (daysOfOperation?.ChristmasDay is not null)
            results.Add(item: await HolidayTools.GetChristmasDayAsync(
                startDate: startDate,
                endDate: endDate));
        
        if (daysOfOperation?.ChristmasDayHoliday is not null)
            results.Add(item: await HolidayTools.GetChristmasDayHolidayAsync(
                startDate: startDate,
                endDate: endDate));
        
        if (daysOfOperation?.BoxingDay is not null)
            results.Add(item: await HolidayTools.GetBoxingDayAsync(
                startDate: startDate,
                endDate: endDate));
        
        if (daysOfOperation?.BoxingDayHoliday is not null)
            results.Add(item: await HolidayTools.GetBoxingDayHolidayAsync(
                startDate: startDate,
                endDate: endDate));
        
        if (daysOfOperation?.NewYearsEve is not null)
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
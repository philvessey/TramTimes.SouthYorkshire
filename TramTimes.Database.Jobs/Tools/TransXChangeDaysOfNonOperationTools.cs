using Nager.Date;
using Nager.Date.Models;
using TramTimes.Database.Jobs.Models;

namespace TramTimes.Database.Jobs.Tools;

public static class TransXChangeDaysOfNonOperationTools
{
    private static readonly string HolidaySystemKey = Environment.GetEnvironmentVariable(variable: "LICENSE_KEY") ?? string.Empty;
    
    public static async Task<List<DateTime>> GetAllHolidaysAsync(
        TransXChangeDaysOfNonOperation? daysOfNonOperation,
        DateTime? startDate,
        DateTime? endDate) {
        
        if (string.IsNullOrEmpty(value: HolidaySystem.LicenseKey))
            HolidaySystem.LicenseKey = HolidaySystemKey;
        
        if (!startDate.HasValue || !endDate.HasValue)
            return await Task.FromResult(result: new List<DateTime>());
        
        List<Holiday> results = [];
        
        if (daysOfNonOperation?.AllBankHolidays is not null)
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
        
        if (daysOfNonOperation?.AllHolidaysExceptChristmas is not null)
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
        
        if (daysOfNonOperation?.Christmas is not null)
        {
            results.Add(item: await HolidayTools.GetChristmasDayAsync(
                startDate: startDate,
                endDate: endDate));
            
            results.Add(item: await HolidayTools.GetBoxingDayAsync(
                startDate: startDate,
                endDate: endDate));
        }
        
        if (daysOfNonOperation?.DisplacementHolidays is not null)
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
        
        if (daysOfNonOperation?.EarlyRunOff is not null)
        {
            results.Add(item: await HolidayTools.GetChristmasEveAsync(
                startDate: startDate,
                endDate: endDate));
            
            results.Add(item: await HolidayTools.GetNewYearsEveAsync(
                startDate: startDate,
                endDate: endDate));
        }
        
        if (daysOfNonOperation?.HolidayMondays is not null)
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
        
        if (daysOfNonOperation?.Holidays is not null)
        {
            results.Add(item: await HolidayTools.GetNewYearsDayAsync(
                startDate: startDate,
                endDate: endDate));
            
            results.Add(item: await HolidayTools.GetGoodFridayAsync(
                startDate: startDate,
                endDate: endDate));
        }
        
        if (daysOfNonOperation?.NewYearsDay is not null)
            results.Add(item: await HolidayTools.GetNewYearsDayAsync(
                startDate: startDate,
                endDate: endDate));
        
        if (daysOfNonOperation?.NewYearsDayHoliday is not null)
            results.Add(item: await HolidayTools.GetNewYearsDayHolidayAsync(
                startDate: startDate,
                endDate: endDate));
        
        if (daysOfNonOperation?.GoodFriday is not null)
            results.Add(item: await HolidayTools.GetGoodFridayAsync(
                startDate: startDate,
                endDate: endDate));
        
        if (daysOfNonOperation?.EasterMonday is not null)
            results.Add(item: await HolidayTools.GetEasterMondayAsync(
                startDate: startDate,
                endDate: endDate));
        
        if (daysOfNonOperation?.MayDay is not null)
            results.Add(item: await HolidayTools.GetMayDayAsync(
                startDate: startDate,
                endDate: endDate));
        
        if (daysOfNonOperation?.SpringBank is not null)
            results.Add(item: await HolidayTools.GetSpringBankAsync(
                startDate: startDate,
                endDate: endDate));
        
        if (daysOfNonOperation?.LateSummerBankHolidayNotScotland is not null)
            results.Add(item: await HolidayTools.GetLateSummerBankHolidayNotScotlandAsync(
                startDate: startDate,
                endDate: endDate));
        
        if (daysOfNonOperation?.ChristmasEve is not null)
            results.Add(item: await HolidayTools.GetChristmasEveAsync(
                startDate: startDate,
                endDate: endDate));
        
        if (daysOfNonOperation?.ChristmasDay is not null)
            results.Add(item: await HolidayTools.GetChristmasDayAsync(
                startDate: startDate,
                endDate: endDate));
        
        if (daysOfNonOperation?.ChristmasDayHoliday is not null)
            results.Add(item: await HolidayTools.GetChristmasDayHolidayAsync(
                startDate: startDate,
                endDate: endDate));
        
        if (daysOfNonOperation?.BoxingDay is not null)
            results.Add(item: await HolidayTools.GetBoxingDayAsync(
                startDate: startDate,
                endDate: endDate));
        
        if (daysOfNonOperation?.BoxingDayHoliday is not null)
            results.Add(item: await HolidayTools.GetBoxingDayHolidayAsync(
                startDate: startDate,
                endDate: endDate));
        
        if (daysOfNonOperation?.NewYearsEve is not null)
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
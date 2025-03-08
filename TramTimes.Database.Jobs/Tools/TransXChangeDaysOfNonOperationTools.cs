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
        
        if (daysOfNonOperation?.AllBankHolidays != null)
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
        
        if (daysOfNonOperation?.AllHolidaysExceptChristmas != null)
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
        
        if (daysOfNonOperation?.Christmas != null)
        {
            results.Add(item: await HolidayTools.GetChristmasDayAsync(
                startDate: startDate,
                endDate: endDate));
            
            results.Add(item: await HolidayTools.GetBoxingDayAsync(
                startDate: startDate,
                endDate: endDate));
        }
        
        if (daysOfNonOperation?.DisplacementHolidays != null)
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
        
        if (daysOfNonOperation?.EarlyRunOff != null)
        {
            results.Add(item: await HolidayTools.GetChristmasEveAsync(
                startDate: startDate,
                endDate: endDate));
            
            results.Add(item: await HolidayTools.GetNewYearsEveAsync(
                startDate: startDate,
                endDate: endDate));
        }
        
        if (daysOfNonOperation?.HolidayMondays != null)
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
        
        if (daysOfNonOperation?.Holidays != null)
        {
            results.Add(item: await HolidayTools.GetNewYearsDayAsync(
                startDate: startDate,
                endDate: endDate));
            
            results.Add(item: await HolidayTools.GetGoodFridayAsync(
                startDate: startDate,
                endDate: endDate));
        }
        
        if (daysOfNonOperation?.NewYearsDay != null)
            results.Add(item: await HolidayTools.GetNewYearsDayAsync(
                startDate: startDate,
                endDate: endDate));
        
        if (daysOfNonOperation?.NewYearsDayHoliday != null)
            results.Add(item: await HolidayTools.GetNewYearsDayHolidayAsync(
                startDate: startDate,
                endDate: endDate));
        
        if (daysOfNonOperation?.GoodFriday != null)
            results.Add(item: await HolidayTools.GetGoodFridayAsync(
                startDate: startDate,
                endDate: endDate));
        
        if (daysOfNonOperation?.EasterMonday != null)
            results.Add(item: await HolidayTools.GetEasterMondayAsync(
                startDate: startDate,
                endDate: endDate));
        
        if (daysOfNonOperation?.MayDay != null)
            results.Add(item: await HolidayTools.GetMayDayAsync(
                startDate: startDate,
                endDate: endDate));
        
        if (daysOfNonOperation?.SpringBank != null)
            results.Add(item: await HolidayTools.GetSpringBankAsync(
                startDate: startDate,
                endDate: endDate));
        
        if (daysOfNonOperation?.LateSummerBankHolidayNotScotland != null)
            results.Add(item: await HolidayTools.GetLateSummerBankHolidayNotScotlandAsync(
                startDate: startDate,
                endDate: endDate));
        
        if (daysOfNonOperation?.ChristmasEve != null)
            results.Add(item: await HolidayTools.GetChristmasEveAsync(
                startDate: startDate,
                endDate: endDate));
        
        if (daysOfNonOperation?.ChristmasDay != null)
            results.Add(item: await HolidayTools.GetChristmasDayAsync(
                startDate: startDate,
                endDate: endDate));
        
        if (daysOfNonOperation?.ChristmasDayHoliday != null)
            results.Add(item: await HolidayTools.GetChristmasDayHolidayAsync(
                startDate: startDate,
                endDate: endDate));
        
        if (daysOfNonOperation?.BoxingDay != null)
            results.Add(item: await HolidayTools.GetBoxingDayAsync(
                startDate: startDate,
                endDate: endDate));
        
        if (daysOfNonOperation?.BoxingDayHoliday != null)
            results.Add(item: await HolidayTools.GetBoxingDayHolidayAsync(
                startDate: startDate,
                endDate: endDate));
        
        if (daysOfNonOperation?.NewYearsEve != null)
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
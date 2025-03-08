using Nager.Date;
using Nager.Date.Models;

namespace TramTimes.Database.Jobs.Tools;

public static class HolidayTools
{
    public static async Task<Holiday> GetNewYearsDayAsync(
        DateTime? startDate,
        DateTime? endDate) {
        
        if (!startDate.HasValue || !endDate.HasValue)
            return await Task.FromResult(result: new Holiday());
        
        var year = new DateTime(ticks: Math.Max(val1: startDate.Value.Ticks, val2: endDate.Value.Ticks)).Year;
        
        return await Task.FromResult(result: new Holiday
        {
            Date = new DateTime(
                year: year,
                month: 1,
                day: 1),
            
            LocalName = "New Year's Day",
            EnglishName = "New Year's Day",
            CountryCode = CountryCode.GB
        });
    }
    
    public static async Task<Holiday> GetNewYearsDayHolidayAsync(
        DateTime? startDate,
        DateTime? endDate) {
        
        if (!startDate.HasValue || !endDate.HasValue)
            return await Task.FromResult(result: new Holiday());
        
        return await Task.FromResult(result: HolidaySystem
            .GetHolidays(
                startDate: startDate.Value,
                endDate: endDate.Value,
                countryCode: CountryCode.GB)
            .FirstOrDefault(predicate: holiday =>
                holiday is { LocalName: "New Year's Day", SubdivisionCodes: not null } &&
                holiday.SubdivisionCodes.Contains(value: "GB-ENG")) ?? new Holiday());
    }
    
    public static async Task<Holiday> GetGoodFridayAsync(
        DateTime? startDate,
        DateTime? endDate) {
        
        if (!startDate.HasValue || !endDate.HasValue)
            return await Task.FromResult(result: new Holiday());
        
        return await Task.FromResult(result: HolidaySystem
            .GetHolidays(
                startDate: startDate.Value,
                endDate: endDate.Value,
                countryCode: CountryCode.GB)
            .FirstOrDefault(predicate: holiday =>
                holiday is { LocalName: "Good Friday", NationalHoliday: true }) ?? new Holiday());
    }
    
    public static async Task<Holiday> GetEasterMondayAsync(
        DateTime? startDate,
        DateTime? endDate) {
        
        if (!startDate.HasValue || !endDate.HasValue)
            return await Task.FromResult(result: new Holiday());
        
        return await Task.FromResult(result: HolidaySystem
            .GetHolidays(
                startDate: startDate.Value,
                endDate: endDate.Value,
                countryCode: CountryCode.GB)
            .FirstOrDefault(predicate: holiday =>
                holiday is { LocalName: "Easter Monday", SubdivisionCodes: not null } &&
                holiday.SubdivisionCodes.Contains(value: "GB-ENG")) ?? new Holiday());
    }
    
    public static async Task<Holiday> GetMayDayAsync(
        DateTime? startDate,
        DateTime? endDate) {
        
        if (!startDate.HasValue || !endDate.HasValue)
            return await Task.FromResult(result: new Holiday());
        
        return await Task.FromResult(result: HolidaySystem
            .GetHolidays(
                startDate: startDate.Value,
                endDate: endDate.Value,
                countryCode: CountryCode.GB)
            .FirstOrDefault(predicate: holiday =>
                holiday is { LocalName: "Early May Bank Holiday", NationalHoliday: true }) ?? new Holiday());
    }
    
    public static async Task<Holiday> GetSpringBankAsync(
        DateTime? startDate,
        DateTime? endDate) {
        
        if (!startDate.HasValue || !endDate.HasValue)
            return await Task.FromResult(result: new Holiday());
        
        return await Task.FromResult(result: HolidaySystem
            .GetHolidays(
                startDate: startDate.Value,
                endDate: endDate.Value,
                countryCode: CountryCode.GB)
            .FirstOrDefault(predicate: holiday =>
                holiday is { LocalName: "Spring Bank Holiday", NationalHoliday: true }) ?? new Holiday());
    }
    
    public static async Task<Holiday> GetLateSummerBankHolidayNotScotlandAsync(
        DateTime? startDate,
        DateTime? endDate) {
        
        if (!startDate.HasValue || !endDate.HasValue)
            return await Task.FromResult(result: new Holiday());
        
        return await Task.FromResult(result: HolidaySystem
            .GetHolidays(
                startDate: startDate.Value,
                endDate: endDate.Value,
                countryCode: CountryCode.GB)
            .FirstOrDefault(predicate: holiday =>
                holiday is { LocalName: "Summer Bank Holiday", SubdivisionCodes: not null } &&
                holiday.SubdivisionCodes.Contains(value: "GB-ENG")) ?? new Holiday());
    }
    
    public static async Task<Holiday> GetChristmasEveAsync(
        DateTime? startDate,
        DateTime? endDate) {
        
        if (!startDate.HasValue || !endDate.HasValue)
            return await Task.FromResult(result: new Holiday());
        
        var year = new DateTime(ticks: Math.Max(val1: startDate.Value.Ticks, val2: endDate.Value.Ticks)).Year;
        
        return await Task.FromResult(result: new Holiday
        {
            Date = new DateTime(
                year: year,
                month: 12,
                day: 24),
            
            LocalName = "Christmas Eve",
            EnglishName = "Christmas Eve",
            CountryCode = CountryCode.GB
        });
    }
    
    public static async Task<Holiday> GetChristmasDayAsync(
        DateTime? startDate,
        DateTime? endDate) {
        
        if (!startDate.HasValue || !endDate.HasValue)
            return await Task.FromResult(result: new Holiday());
        
        var year = new DateTime(ticks: Math.Max(val1: startDate.Value.Ticks, val2: endDate.Value.Ticks)).Year;
        
        return await Task.FromResult(result: new Holiday
        {
            Date = new DateTime(
                year: year,
                month: 12,
                day: 25),
            
            LocalName = "Christmas Day",
            EnglishName = "Christmas Day",
            CountryCode = CountryCode.GB
        });
    }
    
    public static async Task<Holiday> GetChristmasDayHolidayAsync(
        DateTime? startDate,
        DateTime? endDate) {
        
        if (!startDate.HasValue || !endDate.HasValue)
            return await Task.FromResult(result: new Holiday());
        
        return await Task.FromResult(result: HolidaySystem
            .GetHolidays(
                startDate: startDate.Value,
                endDate: endDate.Value,
                countryCode: CountryCode.GB)
            .FirstOrDefault(predicate: holiday =>
                holiday is { LocalName: "Christmas Day", NationalHoliday: true }) ?? new Holiday());
    }
    
    public static async Task<Holiday> GetBoxingDayAsync(
        DateTime? startDate,
        DateTime? endDate) {
        
        if (!startDate.HasValue || !endDate.HasValue)
            return await Task.FromResult(result: new Holiday());
        
        var year = new DateTime(ticks: Math.Max(val1: startDate.Value.Ticks, val2: endDate.Value.Ticks)).Year;
        
        return await Task.FromResult(result: new Holiday
        {
            Date = new DateTime(
                year: year,
                month: 12,
                day: 26),
            
            LocalName = "Boxing Day",
            EnglishName = "St. Stephen's Day",
            CountryCode = CountryCode.GB
        });
    }
    
    public static async Task<Holiday> GetBoxingDayHolidayAsync(
        DateTime? startDate,
        DateTime? endDate) {
        
        if (!startDate.HasValue || !endDate.HasValue)
            return await Task.FromResult(result: new Holiday());
        
        return await Task.FromResult(result: HolidaySystem
            .GetHolidays(
                startDate: startDate.Value,
                endDate: endDate.Value,
                countryCode: CountryCode.GB)
            .FirstOrDefault(predicate: holiday =>
                holiday is { LocalName: "Boxing Day", NationalHoliday: true }) ?? new Holiday());
    }
    
    public static async Task<Holiday> GetNewYearsEveAsync(
        DateTime? startDate,
        DateTime? endDate) {
        
        if (!startDate.HasValue || !endDate.HasValue)
            return await Task.FromResult(result: new Holiday());
        
        var year = new DateTime(ticks: Math.Max(val1: startDate.Value.Ticks, val2: endDate.Value.Ticks)).Year;
        
        return await Task.FromResult(result: new Holiday
        {
            Date = new DateTime(
                year: year,
                month: 12,
                day: 31),
            
            LocalName = "New Year's Eve",
            EnglishName = "New Year's Eve",
            CountryCode = CountryCode.GB
        });
    }
}
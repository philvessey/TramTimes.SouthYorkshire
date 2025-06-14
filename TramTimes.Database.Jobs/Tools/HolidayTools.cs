using Nager.Date;
using Nager.Date.Models;

namespace TramTimes.Database.Jobs.Tools;

public static class HolidayTools
{
    public static Holiday GetNewYearsDay(
        DateTime? startDate,
        DateTime? endDate) {
        
        #region check valid input
        
        if (!startDate.HasValue || !endDate.HasValue)
            return new Holiday();
        
        #endregion
        
        #region build result
        
        var date = new DateTime(ticks: Math.Max(
            val1: startDate.Value.Ticks,
            val2: endDate.Value.Ticks));
        
        var result = new Holiday
        {
            Date = new DateTime(
                year: date.Year,
                month: 1,
                day: 1),
            
            LocalName = "New Year's Day",
            EnglishName = "New Year's Day",
            CountryCode = CountryCode.GB
        };
        
        #endregion
        
        return result;
    }
    
    public static Holiday GetNewYearsDayHoliday(
        DateTime? startDate,
        DateTime? endDate) {
        
        #region check valid input
        
        if (!startDate.HasValue || !endDate.HasValue)
            return new Holiday();
        
        #endregion
        
        #region build result
        
        var result = HolidaySystem
            .GetHolidays(
                startDate: startDate.Value,
                endDate: endDate.Value,
                countryCode: CountryCode.GB)
            .FirstOrDefault(predicate: holiday =>
                holiday is { LocalName: "New Year's Day", SubdivisionCodes: not null } &&
                holiday.SubdivisionCodes.Contains(value: "GB-ENG")) ?? new Holiday();
        
        #endregion
        
        return result;
    }
    
    public static Holiday GetGoodFriday(
        DateTime? startDate,
        DateTime? endDate) {
        
        #region check valid input
        
        if (!startDate.HasValue || !endDate.HasValue)
            return new Holiday();
        
        #endregion
        
        #region build result
        
        var result = HolidaySystem
            .GetHolidays(
                startDate: startDate.Value,
                endDate: endDate.Value,
                countryCode: CountryCode.GB)
            .FirstOrDefault(predicate: holiday =>
                holiday is { LocalName: "Good Friday", NationalHoliday: true }) ?? new Holiday();
        
        #endregion
        
        return result;
    }
    
    public static Holiday GetEasterMonday(
        DateTime? startDate,
        DateTime? endDate) {
        
        #region check valid input
        
        if (!startDate.HasValue || !endDate.HasValue)
            return new Holiday();
        
        #endregion
        
        #region build result
        
        var result = HolidaySystem
            .GetHolidays(
                startDate: startDate.Value,
                endDate: endDate.Value,
                countryCode: CountryCode.GB)
            .FirstOrDefault(predicate: holiday =>
                holiday is { LocalName: "Easter Monday", SubdivisionCodes: not null } &&
                holiday.SubdivisionCodes.Contains(value: "GB-ENG")) ?? new Holiday();
        
        #endregion
        
        return result;
    }
    
    public static Holiday GetMayDay(
        DateTime? startDate,
        DateTime? endDate) {
        
        #region check valid input
        
        if (!startDate.HasValue || !endDate.HasValue)
            return new Holiday();
        
        #endregion
        
        #region build result
        
        var result = HolidaySystem
            .GetHolidays(
                startDate: startDate.Value,
                endDate: endDate.Value,
                countryCode: CountryCode.GB)
            .FirstOrDefault(predicate: holiday =>
                holiday is { LocalName: "Early May Bank Holiday", NationalHoliday: true }) ?? new Holiday();
        
        #endregion
        
        return result;
    }
    
    public static Holiday GetSpringBank(
        DateTime? startDate,
        DateTime? endDate) {
        
        #region check valid input
        
        if (!startDate.HasValue || !endDate.HasValue)
            return new Holiday();
        
        #endregion
        
        #region build result
        
        var result = HolidaySystem
            .GetHolidays(
                startDate: startDate.Value,
                endDate: endDate.Value,
                countryCode: CountryCode.GB)
            .FirstOrDefault(predicate: holiday =>
                holiday is { LocalName: "Spring Bank Holiday", NationalHoliday: true }) ?? new Holiday();
        
        #endregion
        
        return result;
    }
    
    public static Holiday GetLateSummerBankHolidayNotScotland(
        DateTime? startDate,
        DateTime? endDate) {
        
        #region check valid input
        
        if (!startDate.HasValue || !endDate.HasValue)
            return new Holiday();
        
        #endregion
        
        #region build result
        
        var result = HolidaySystem
            .GetHolidays(
                startDate: startDate.Value,
                endDate: endDate.Value,
                countryCode: CountryCode.GB)
            .FirstOrDefault(predicate: holiday =>
                holiday is { LocalName: "Summer Bank Holiday", SubdivisionCodes: not null } &&
                holiday.SubdivisionCodes.Contains(value: "GB-ENG")) ?? new Holiday();
        
        #endregion
        
        return result;
    }
    
    public static Holiday GetChristmasEve(
        DateTime? startDate,
        DateTime? endDate) {
        
        #region check valid input
        
        if (!startDate.HasValue || !endDate.HasValue)
            return new Holiday();
        
        #endregion
        
        #region build result
        
        var date = new DateTime(ticks: Math.Max(
            val1: startDate.Value.Ticks,
            val2: endDate.Value.Ticks));
        
        var result = new Holiday
        {
            Date = new DateTime(
                year: date.Year,
                month: 12,
                day: 24),
            
            LocalName = "Christmas Eve",
            EnglishName = "Christmas Eve",
            CountryCode = CountryCode.GB
        };
        
        #endregion
        
        return result;
    }
    
    public static Holiday GetChristmasDay(
        DateTime? startDate,
        DateTime? endDate) {
        
        #region check valid input
        
        if (!startDate.HasValue || !endDate.HasValue)
            return new Holiday();
        
        #endregion
        
        #region build result
        
        var date = new DateTime(ticks: Math.Max(
            val1: startDate.Value.Ticks,
            val2: endDate.Value.Ticks));
        
        var result = new Holiday
        {
            Date = new DateTime(
                year: date.Year,
                month: 12,
                day: 25),
            
            LocalName = "Christmas Day",
            EnglishName = "Christmas Day",
            CountryCode = CountryCode.GB
        };
        
        #endregion
        
        return result;
    }
    
    public static Holiday GetChristmasDayHoliday(
        DateTime? startDate,
        DateTime? endDate) {
        
        #region check valid input
        
        if (!startDate.HasValue || !endDate.HasValue)
            return new Holiday();
        
        #endregion
        
        #region build result
        
        var result = HolidaySystem
            .GetHolidays(
                startDate: startDate.Value,
                endDate: endDate.Value,
                countryCode: CountryCode.GB)
            .FirstOrDefault(predicate: holiday =>
                holiday is { LocalName: "Christmas Day", NationalHoliday: true }) ?? new Holiday();
        
        #endregion
        
        return result;
    }
    
    public static Holiday GetBoxingDay(
        DateTime? startDate,
        DateTime? endDate) {
        
        #region check valid input
        
        if (!startDate.HasValue || !endDate.HasValue)
            return new Holiday();
        
        #endregion
        
        #region build result
        
        var date = new DateTime(ticks: Math.Max(
            val1: startDate.Value.Ticks,
            val2: endDate.Value.Ticks));
        
        var result = new Holiday
        {
            Date = new DateTime(
                year: date.Year,
                month: 12,
                day: 26),
            
            LocalName = "Boxing Day",
            EnglishName = "St. Stephen's Day",
            CountryCode = CountryCode.GB
        };
        
        #endregion
        
        return result;
    }
    
    public static Holiday GetBoxingDayHoliday(
        DateTime? startDate,
        DateTime? endDate) {
        
        #region check valid input
        
        if (!startDate.HasValue || !endDate.HasValue)
            return new Holiday();
        
        #endregion
        
        #region build result
        
        var result = HolidaySystem
            .GetHolidays(
                startDate: startDate.Value,
                endDate: endDate.Value,
                countryCode: CountryCode.GB)
            .FirstOrDefault(predicate: holiday =>
                holiday is { LocalName: "Boxing Day", NationalHoliday: true }) ?? new Holiday();
        
        #endregion
        
        return result;
    }
    
    public static Holiday GetNewYearsEve(
        DateTime? startDate,
        DateTime? endDate) {
        
        #region check valid input
        
        if (!startDate.HasValue || !endDate.HasValue)
            return new Holiday();
        
        #endregion
        
        #region build result
        
        var date = new DateTime(ticks: Math.Max(
            val1: startDate.Value.Ticks,
            val2: endDate.Value.Ticks));
        
        var result = new Holiday
        {
            Date = new DateTime(
                year: date.Year,
                month: 12,
                day: 31),
            
            LocalName = "New Year's Eve",
            EnglishName = "New Year's Eve",
            CountryCode = CountryCode.GB
        };
        
        #endregion
        
        return result;
    }
}
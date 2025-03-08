namespace TramTimes.Database.Jobs.Tools;

public static class DateTimeTools
{
    public static async Task<DateTime> GetPeriodStartDateAsync(
        DateTime scheduleDate,
        DateTime? startDate) {
        
        if (!startDate.HasValue || startDate == DateTime.MinValue || startDate <= scheduleDate)
            return await Task.FromResult(result: scheduleDate);
        
        var daysDifference = startDate.Value.Subtract(value: scheduleDate).TotalDays;
        
        return await Task.FromResult(result: daysDifference < 27 ? startDate.Value : DateTime.MaxValue);
    }
    
    public static async Task<DateTime> GetPeriodEndDateAsync(
        DateTime scheduleDate,
        DateTime? endDate) {
        
        if (!endDate.HasValue || endDate == DateTime.MinValue)
            return await Task.FromResult(result: scheduleDate.AddDays(value: 27));
        
        if (endDate < scheduleDate)
            return await Task.FromResult(result: DateTime.MinValue);
        
        if (endDate == scheduleDate)
            return await Task.FromResult(result: scheduleDate);
        
        var daysDifference = endDate.Value.Subtract(value: scheduleDate).TotalDays;
        
        return await Task.FromResult(result: daysDifference > 27 ? scheduleDate.AddDays(value: 27) : endDate.Value);
    }
    
    public static async Task<DateTime> GetProfileStartDateAsync(
        DateTime scheduleDate,
        DateTime? startDate) {
        
        if (!startDate.HasValue || startDate == DateTime.MinValue || startDate < scheduleDate)
            return await Task.FromResult(result: DateTime.MaxValue);
        
        if (startDate == scheduleDate)
            return await Task.FromResult(result: startDate.Value);
        
        var daysDifference = startDate.Value.Subtract(value: scheduleDate).TotalDays;
        
        return await Task.FromResult(result: daysDifference < 27 ? startDate.Value : DateTime.MaxValue);
    }
    
    public static async Task<DateTime> GetProfileEndDateAsync(
        DateTime scheduleDate,
        DateTime? endDate) {
        
        if (!endDate.HasValue || endDate == DateTime.MinValue || endDate < scheduleDate)
            return await Task.FromResult(result: DateTime.MinValue);
        
        if (endDate == scheduleDate)
            return await Task.FromResult(result: endDate.Value);
        
        var daysDifference = endDate.Value.Subtract(value: scheduleDate).TotalDays;
        
        return await Task.FromResult(result: daysDifference < 27 ? endDate.Value : DateTime.MinValue);
    }
}
namespace TramTimes.Database.Jobs.Tools;

public static class DateTimeTools
{
    public static DateTime GetPeriodStartDate(
        DateTime scheduleDate,
        DateTime? startDate) {
        
        #region check valid input
        
        if (!startDate.HasValue || startDate == DateTime.MinValue || startDate <= scheduleDate)
            return scheduleDate;
        
        #endregion
        
        #region build result
        
        var result = startDate.Value.Subtract(value: scheduleDate).TotalDays < 27
            ? startDate.Value
            : DateTime.MaxValue;
        
        #endregion
        
        return result;
    }
    
    public static DateTime GetPeriodEndDate(
        DateTime scheduleDate,
        DateTime? endDate) {
        
        #region check valid input
        
        if (!endDate.HasValue || endDate == DateTime.MinValue)
            return scheduleDate.AddDays(value: 27);
        
        if (endDate < scheduleDate)
            return DateTime.MinValue;
        
        if (endDate == scheduleDate)
            return scheduleDate;
        
        #endregion
        
        #region build result
        
        var result = endDate.Value.Subtract(value: scheduleDate).TotalDays > 27
            ? scheduleDate.AddDays(value: 27)
            : endDate.Value;
        
        #endregion
        
        return result;
    }
    
    public static DateTime GetProfileStartDate(
        DateTime scheduleDate,
        DateTime? startDate) {
        
        #region check valid input
        
        if (!startDate.HasValue || startDate == DateTime.MinValue || startDate < scheduleDate)
            return DateTime.MaxValue;
        
        if (startDate == scheduleDate)
            return startDate.Value;
        
        #endregion
        
        #region build result
        
        var result = startDate.Value.Subtract(value: scheduleDate).TotalDays < 27
            ? startDate.Value
            : DateTime.MaxValue;
        
        #endregion
        
        return result;
    }
    
    public static DateTime GetProfileEndDate(
        DateTime scheduleDate,
        DateTime? endDate) {
        
        #region check valid input
        
        if (!endDate.HasValue || endDate == DateTime.MinValue || endDate < scheduleDate)
            return DateTime.MinValue;
        
        if (endDate == scheduleDate)
            return endDate.Value;
        
        #endregion
        
        #region build result
        
        var result = endDate.Value.Subtract(value: scheduleDate).TotalDays < 27
            ? endDate.Value
            : DateTime.MinValue;
        
        #endregion
        
        return result;
    }
}
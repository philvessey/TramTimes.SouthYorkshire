namespace TramTimes.Database.Jobs.Tools;

public static class DateOnlyTools
{
    public static DateOnly GetPeriodStartDate(
        DateOnly scheduleDate,
        DateOnly? startDate) {
        
        #region check valid input
        
        if (!startDate.HasValue || startDate == DateOnly.MinValue || startDate <= scheduleDate)
            return scheduleDate;
        
        #endregion
        
        #region build result
        
        var result = startDate.Value.ToDateTime(time: TimeOnly.MinValue).Subtract(value: scheduleDate.ToDateTime(time: TimeOnly.MinValue)).TotalDays < 27
            ? startDate.Value
            : DateOnly.MaxValue;
        
        #endregion
        
        return result;
    }
    
    public static DateOnly GetPeriodEndDate(
        DateOnly scheduleDate,
        DateOnly? endDate) {
        
        #region check valid input
        
        if (!endDate.HasValue || endDate == DateOnly.MinValue)
            return scheduleDate.AddDays(value: 27);
        
        if (endDate < scheduleDate)
            return DateOnly.MinValue;
        
        if (endDate == scheduleDate)
            return scheduleDate;
        
        #endregion
        
        #region build result
        
        var result = endDate.Value.ToDateTime(time: TimeOnly.MinValue).Subtract(value: scheduleDate.ToDateTime(time: TimeOnly.MinValue)).TotalDays > 27
            ? scheduleDate.AddDays(value: 27)
            : endDate.Value;
        
        #endregion
        
        return result;
    }
    
    public static DateOnly GetProfileStartDate(
        DateOnly scheduleDate,
        DateOnly? startDate) {
        
        #region check valid input
        
        if (!startDate.HasValue || startDate == DateOnly.MinValue || startDate < scheduleDate)
            return DateOnly.MaxValue;
        
        if (startDate == scheduleDate)
            return startDate.Value;
        
        #endregion
        
        #region build result
        
        var result = startDate.Value.ToDateTime(time: TimeOnly.MinValue).Subtract(value: scheduleDate.ToDateTime(time: TimeOnly.MinValue)).TotalDays < 27
            ? startDate.Value
            : DateOnly.MaxValue;
        
        #endregion
        
        return result;
    }
    
    public static DateOnly GetProfileEndDate(
        DateOnly scheduleDate,
        DateOnly? endDate) {
        
        #region check valid input
        
        if (!endDate.HasValue || endDate == DateOnly.MinValue || endDate < scheduleDate)
            return DateOnly.MinValue;
        
        if (endDate == scheduleDate)
            return endDate.Value;
        
        #endregion
        
        #region build result
        
        var result = endDate.Value.ToDateTime(time: TimeOnly.MinValue).Subtract(value: scheduleDate.ToDateTime(time: TimeOnly.MinValue)).TotalDays < 27
            ? endDate.Value
            : DateOnly.MinValue;
        
        #endregion
        
        return result;
    }
}
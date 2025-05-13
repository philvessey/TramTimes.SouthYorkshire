using TramTimes.Database.Jobs.Models;
using TramTimes.Database.Jobs.Tools;

namespace TramTimes.Database.Jobs.Builders;

public static class TravelineCalendarBuilder
{
    public static async Task<TravelineCalendar> BuildAsync(
        DateTime scheduleDate,
        TransXChangeServices? services,
        TransXChangeVehicleJourney? vehicleJourney,
        DateTime? startDate,
        DateTime? endDate) {
        
        if (!startDate.HasValue || !endDate.HasValue)
        {
            return await Task.FromResult(result: new TravelineCalendar
            {
                Monday = false,
                Tuesday = false,
                Wednesday = false,
                Thursday = false,
                Friday = false,
                Saturday = false,
                Sunday = false
            });
        }
        
        TravelineCalendar value = new()
        {
            Monday = false,
            Tuesday = false,
            Wednesday = false,
            Thursday = false,
            Friday = false,
            Saturday = false,
            Sunday = false,
            StartDate = startDate,
            EndDate = endDate
        };
        
        var operatingProfile = vehicleJourney?.OperatingProfile ?? services?.Service?.OperatingProfile;
        
        if (operatingProfile?.RegularDayType?.DaysOfWeek?.MondayToFriday is not null)
        {
            value.Monday = true;
            value.Tuesday = true;
            value.Wednesday = true;
            value.Thursday = true;
            value.Friday = true;
        }
        
        if (operatingProfile?.RegularDayType?.DaysOfWeek?.MondayToSaturday is not null)
        {
            value.Monday = true;
            value.Tuesday = true;
            value.Wednesday = true;
            value.Thursday = true;
            value.Friday = true;
            value.Saturday = true;
        }
        
        if (operatingProfile?.RegularDayType?.DaysOfWeek?.MondayToSunday is not null)
        {
            value.Monday = true;
            value.Tuesday = true;
            value.Wednesday = true;
            value.Thursday = true;
            value.Friday = true;
            value.Saturday = true;
            value.Sunday = true;
        }
        
        if (operatingProfile?.RegularDayType?.DaysOfWeek?.Weekend is not null)
        {
            value.Saturday = true;
            value.Sunday = true;
        }
        
        if (operatingProfile?.RegularDayType?.DaysOfWeek?.NotMonday is not null)
        {
            value.Tuesday = true;
            value.Wednesday = true;
            value.Thursday = true;
            value.Friday = true;
            value.Saturday = true;
            value.Sunday = true;
        }
        
        if (operatingProfile?.RegularDayType?.DaysOfWeek?.NotTuesday is not null)
        {
            value.Monday = true;
            value.Wednesday = true;
            value.Thursday = true;
            value.Friday = true;
            value.Saturday = true;
            value.Sunday = true;
        }
        
        if (operatingProfile?.RegularDayType?.DaysOfWeek?.NotTuesday is not null)
        {
            value.Monday = true;
            value.Wednesday = true;
            value.Thursday = true;
            value.Friday = true;
            value.Saturday = true;
            value.Sunday = true;
        }
        
        if (operatingProfile?.RegularDayType?.DaysOfWeek?.NotWednesday is not null)
        {
            value.Monday = true;
            value.Tuesday = true;
            value.Thursday = true;
            value.Friday = true;
            value.Saturday = true;
            value.Sunday = true;
        }
        
        if (operatingProfile?.RegularDayType?.DaysOfWeek?.NotWednesday is not null)
        {
            value.Monday = true;
            value.Tuesday = true;
            value.Thursday = true;
            value.Friday = true;
            value.Saturday = true;
            value.Sunday = true;
        }
        
        if (operatingProfile?.RegularDayType?.DaysOfWeek?.NotWednesday is not null)
        {
            value.Monday = true;
            value.Tuesday = true;
            value.Thursday = true;
            value.Friday = true;
            value.Saturday = true;
            value.Sunday = true;
        }
        
        if (operatingProfile?.RegularDayType?.DaysOfWeek?.NotWednesday is not null)
        {
            value.Monday = true;
            value.Tuesday = true;
            value.Thursday = true;
            value.Friday = true;
            value.Saturday = true;
            value.Sunday = true;
        }
        
        if (operatingProfile?.RegularDayType?.DaysOfWeek?.NotThursday is not null)
        {
            value.Monday = true;
            value.Tuesday = true;
            value.Wednesday = true;
            value.Friday = true;
            value.Saturday = true;
            value.Sunday = true;
        }
        
        if (operatingProfile?.RegularDayType?.DaysOfWeek?.NotFriday is not null)
        {
            value.Monday = true;
            value.Tuesday = true;
            value.Wednesday = true;
            value.Thursday = true;
            value.Saturday = true;
            value.Sunday = true;
        }
        
        if (operatingProfile?.RegularDayType?.DaysOfWeek?.NotSaturday is not null)
        {
            value.Monday = true;
            value.Tuesday = true;
            value.Wednesday = true;
            value.Thursday = true;
            value.Friday = true;
            value.Sunday = true;
        }
        
        if (operatingProfile?.RegularDayType?.DaysOfWeek?.NotSunday is not null)
        {
            value.Monday = true;
            value.Tuesday = true;
            value.Wednesday = true;
            value.Thursday = true;
            value.Friday = true;
            value.Saturday = true;
        }
        
        if (operatingProfile?.RegularDayType?.DaysOfWeek?.Monday is not null)
            value.Monday = true;
        
        if (operatingProfile?.RegularDayType?.DaysOfWeek?.Tuesday is not null)
            value.Tuesday = true;
        
        if (operatingProfile?.RegularDayType?.DaysOfWeek?.Wednesday is not null)
            value.Wednesday = true;
        
        if (operatingProfile?.RegularDayType?.DaysOfWeek?.Thursday is not null)
            value.Thursday = true;
        
        if (operatingProfile?.RegularDayType?.DaysOfWeek?.Friday is not null)
            value.Friday = true;
        
        if (operatingProfile?.RegularDayType?.DaysOfWeek?.Saturday is not null)
            value.Saturday = true;
        
        if (operatingProfile?.RegularDayType?.DaysOfWeek?.Sunday is not null)
            value.Sunday = true;
        
        value.RunningDates = await TravelineCalendarRunningDateTools.GetAllDatesAsync(
            startDate: value.StartDate,
            endDate: value.EndDate,
            monday: value.Monday,
            tuesday: value.Tuesday,
            wednesday: value.Wednesday,
            thursday: value.Thursday,
            friday: value.Friday,
            saturday: value.Saturday,
            sunday: value.Sunday);
        
        value.SupplementRunningDates = await TravelineCalendarSupplementRunningDateTools.GetAllDatesAsync(
            scheduleDate: scheduleDate,
            operatingProfile: operatingProfile,
            startDate: value.StartDate,
            endDate: value.EndDate,
            monday: value.Monday,
            tuesday: value.Tuesday,
            wednesday: value.Wednesday,
            thursday: value.Thursday,
            friday: value.Friday,
            saturday: value.Saturday,
            sunday: value.Sunday,
            dates: value.RunningDates);
        
        value.SupplementNonRunningDates = await TravelineCalendarSupplementNonRunningDateTools.GetAllDatesAsync(
            scheduleDate: scheduleDate,
            operatingProfile: operatingProfile,
            startDate: value.StartDate,
            endDate: value.EndDate,
            monday: value.Monday,
            tuesday: value.Tuesday,
            wednesday: value.Wednesday,
            thursday: value.Thursday,
            friday: value.Friday,
            saturday: value.Saturday,
            sunday: value.Sunday,
            dates: value.RunningDates);
        
        return await Task.FromResult(result: value);
    }
}
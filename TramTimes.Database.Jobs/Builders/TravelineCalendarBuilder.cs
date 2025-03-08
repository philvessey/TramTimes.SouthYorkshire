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
        
        if (operatingProfile?.RegularDayType?.DaysOfWeek?.MondayToFriday != null)
        {
            value.Monday = true;
            value.Tuesday = true;
            value.Wednesday = true;
            value.Thursday = true;
            value.Friday = true;
        }
        
        if (operatingProfile?.RegularDayType?.DaysOfWeek?.MondayToSaturday != null)
        {
            value.Monday = true;
            value.Tuesday = true;
            value.Wednesday = true;
            value.Thursday = true;
            value.Friday = true;
            value.Saturday = true;
        }
        
        if (operatingProfile?.RegularDayType?.DaysOfWeek?.MondayToSunday != null)
        {
            value.Monday = true;
            value.Tuesday = true;
            value.Wednesday = true;
            value.Thursday = true;
            value.Friday = true;
            value.Saturday = true;
            value.Sunday = true;
        }
        
        if (operatingProfile?.RegularDayType?.DaysOfWeek?.Weekend != null)
        {
            value.Saturday = true;
            value.Sunday = true;
        }
        
        if (operatingProfile?.RegularDayType?.DaysOfWeek?.NotMonday != null)
        {
            value.Tuesday = true;
            value.Wednesday = true;
            value.Thursday = true;
            value.Friday = true;
            value.Saturday = true;
            value.Sunday = true;
        }
        
        if (operatingProfile?.RegularDayType?.DaysOfWeek?.NotTuesday != null)
        {
            value.Monday = true;
            value.Wednesday = true;
            value.Thursday = true;
            value.Friday = true;
            value.Saturday = true;
            value.Sunday = true;
        }
        
        if (operatingProfile?.RegularDayType?.DaysOfWeek?.NotTuesday != null)
        {
            value.Monday = true;
            value.Wednesday = true;
            value.Thursday = true;
            value.Friday = true;
            value.Saturday = true;
            value.Sunday = true;
        }
        
        if (operatingProfile?.RegularDayType?.DaysOfWeek?.NotWednesday != null)
        {
            value.Monday = true;
            value.Tuesday = true;
            value.Thursday = true;
            value.Friday = true;
            value.Saturday = true;
            value.Sunday = true;
        }
        
        if (operatingProfile?.RegularDayType?.DaysOfWeek?.NotWednesday != null)
        {
            value.Monday = true;
            value.Tuesday = true;
            value.Thursday = true;
            value.Friday = true;
            value.Saturday = true;
            value.Sunday = true;
        }
        
        if (operatingProfile?.RegularDayType?.DaysOfWeek?.NotWednesday != null)
        {
            value.Monday = true;
            value.Tuesday = true;
            value.Thursday = true;
            value.Friday = true;
            value.Saturday = true;
            value.Sunday = true;
        }
        
        if (operatingProfile?.RegularDayType?.DaysOfWeek?.NotWednesday != null)
        {
            value.Monday = true;
            value.Tuesday = true;
            value.Thursday = true;
            value.Friday = true;
            value.Saturday = true;
            value.Sunday = true;
        }
        
        if (operatingProfile?.RegularDayType?.DaysOfWeek?.NotThursday != null)
        {
            value.Monday = true;
            value.Tuesday = true;
            value.Wednesday = true;
            value.Friday = true;
            value.Saturday = true;
            value.Sunday = true;
        }
        
        if (operatingProfile?.RegularDayType?.DaysOfWeek?.NotFriday != null)
        {
            value.Monday = true;
            value.Tuesday = true;
            value.Wednesday = true;
            value.Thursday = true;
            value.Saturday = true;
            value.Sunday = true;
        }
        
        if (operatingProfile?.RegularDayType?.DaysOfWeek?.NotSaturday != null)
        {
            value.Monday = true;
            value.Tuesday = true;
            value.Wednesday = true;
            value.Thursday = true;
            value.Friday = true;
            value.Sunday = true;
        }
        
        if (operatingProfile?.RegularDayType?.DaysOfWeek?.NotSunday != null)
        {
            value.Monday = true;
            value.Tuesday = true;
            value.Wednesday = true;
            value.Thursday = true;
            value.Friday = true;
            value.Saturday = true;
        }
        
        if (operatingProfile?.RegularDayType?.DaysOfWeek?.Monday != null)
            value.Monday = true;
        
        if (operatingProfile?.RegularDayType?.DaysOfWeek?.Tuesday != null)
            value.Tuesday = true;
        
        if (operatingProfile?.RegularDayType?.DaysOfWeek?.Wednesday != null)
            value.Wednesday = true;
        
        if (operatingProfile?.RegularDayType?.DaysOfWeek?.Thursday != null)
            value.Thursday = true;
        
        if (operatingProfile?.RegularDayType?.DaysOfWeek?.Friday != null)
            value.Friday = true;
        
        if (operatingProfile?.RegularDayType?.DaysOfWeek?.Saturday != null)
            value.Saturday = true;
        
        if (operatingProfile?.RegularDayType?.DaysOfWeek?.Sunday != null)
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
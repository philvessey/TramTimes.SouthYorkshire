using TramTimes.Database.Jobs.Models;
using TramTimes.Database.Jobs.Tools;

namespace TramTimes.Database.Jobs.Builders;

public static class TravelineCalendarBuilder
{
    public static TravelineCalendar Build(
        DateOnly scheduleDate,
        List<Holiday> holidays,
        TransXChangeServices? services,
        TransXChangeVehicleJourney? vehicleJourney,
        DateOnly? startDate,
        DateOnly? endDate) {

        #region build unknown

        TravelineCalendar unknown = new()
        {
            Monday = false,
            Tuesday = false,
            Wednesday = false,
            Thursday = false,
            Friday = false,
            Saturday = false,
            Sunday = false
        };

        #endregion

        #region build result

        TravelineCalendar result = new()
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

        if (!startDate.HasValue || !endDate.HasValue)
            return unknown;

        var operatingProfile = vehicleJourney?.OperatingProfile ?? services?.Service?.OperatingProfile;

        if (operatingProfile?.RegularDayType?.DaysOfWeek?.MondayToFriday is not null)
        {
            result.Monday = true;
            result.Tuesday = true;
            result.Wednesday = true;
            result.Thursday = true;
            result.Friday = true;
        }

        if (operatingProfile?.RegularDayType?.DaysOfWeek?.MondayToSaturday is not null)
        {
            result.Monday = true;
            result.Tuesday = true;
            result.Wednesday = true;
            result.Thursday = true;
            result.Friday = true;
            result.Saturday = true;
        }

        if (operatingProfile?.RegularDayType?.DaysOfWeek?.MondayToSunday is not null)
        {
            result.Monday = true;
            result.Tuesday = true;
            result.Wednesday = true;
            result.Thursday = true;
            result.Friday = true;
            result.Saturday = true;
            result.Sunday = true;
        }

        if (operatingProfile?.RegularDayType?.DaysOfWeek?.Weekend is not null)
        {
            result.Saturday = true;
            result.Sunday = true;
        }

        if (operatingProfile?.RegularDayType?.DaysOfWeek?.NotMonday is not null)
        {
            result.Tuesday = true;
            result.Wednesday = true;
            result.Thursday = true;
            result.Friday = true;
            result.Saturday = true;
            result.Sunday = true;
        }

        if (operatingProfile?.RegularDayType?.DaysOfWeek?.NotTuesday is not null)
        {
            result.Monday = true;
            result.Wednesday = true;
            result.Thursday = true;
            result.Friday = true;
            result.Saturday = true;
            result.Sunday = true;
        }

        if (operatingProfile?.RegularDayType?.DaysOfWeek?.NotTuesday is not null)
        {
            result.Monday = true;
            result.Wednesday = true;
            result.Thursday = true;
            result.Friday = true;
            result.Saturday = true;
            result.Sunday = true;
        }

        if (operatingProfile?.RegularDayType?.DaysOfWeek?.NotWednesday is not null)
        {
            result.Monday = true;
            result.Tuesday = true;
            result.Thursday = true;
            result.Friday = true;
            result.Saturday = true;
            result.Sunday = true;
        }

        if (operatingProfile?.RegularDayType?.DaysOfWeek?.NotWednesday is not null)
        {
            result.Monday = true;
            result.Tuesday = true;
            result.Thursday = true;
            result.Friday = true;
            result.Saturday = true;
            result.Sunday = true;
        }

        if (operatingProfile?.RegularDayType?.DaysOfWeek?.NotWednesday is not null)
        {
            result.Monday = true;
            result.Tuesday = true;
            result.Thursday = true;
            result.Friday = true;
            result.Saturday = true;
            result.Sunday = true;
        }

        if (operatingProfile?.RegularDayType?.DaysOfWeek?.NotWednesday is not null)
        {
            result.Monday = true;
            result.Tuesday = true;
            result.Thursday = true;
            result.Friday = true;
            result.Saturday = true;
            result.Sunday = true;
        }

        if (operatingProfile?.RegularDayType?.DaysOfWeek?.NotThursday is not null)
        {
            result.Monday = true;
            result.Tuesday = true;
            result.Wednesday = true;
            result.Friday = true;
            result.Saturday = true;
            result.Sunday = true;
        }

        if (operatingProfile?.RegularDayType?.DaysOfWeek?.NotFriday is not null)
        {
            result.Monday = true;
            result.Tuesday = true;
            result.Wednesday = true;
            result.Thursday = true;
            result.Saturday = true;
            result.Sunday = true;
        }

        if (operatingProfile?.RegularDayType?.DaysOfWeek?.NotSaturday is not null)
        {
            result.Monday = true;
            result.Tuesday = true;
            result.Wednesday = true;
            result.Thursday = true;
            result.Friday = true;
            result.Sunday = true;
        }

        if (operatingProfile?.RegularDayType?.DaysOfWeek?.NotSunday is not null)
        {
            result.Monday = true;
            result.Tuesday = true;
            result.Wednesday = true;
            result.Thursday = true;
            result.Friday = true;
            result.Saturday = true;
        }

        if (operatingProfile?.RegularDayType?.DaysOfWeek?.Monday is not null)
            result.Monday = true;

        if (operatingProfile?.RegularDayType?.DaysOfWeek?.Tuesday is not null)
            result.Tuesday = true;

        if (operatingProfile?.RegularDayType?.DaysOfWeek?.Wednesday is not null)
            result.Wednesday = true;

        if (operatingProfile?.RegularDayType?.DaysOfWeek?.Thursday is not null)
            result.Thursday = true;

        if (operatingProfile?.RegularDayType?.DaysOfWeek?.Friday is not null)
            result.Friday = true;

        if (operatingProfile?.RegularDayType?.DaysOfWeek?.Saturday is not null)
            result.Saturday = true;

        if (operatingProfile?.RegularDayType?.DaysOfWeek?.Sunday is not null)
            result.Sunday = true;

        result.RunningDates = TravelineCalendarRunningDateTools.GetAllDates(
            startDate: result.StartDate,
            endDate: result.EndDate,
            monday: result.Monday,
            tuesday: result.Tuesday,
            wednesday: result.Wednesday,
            thursday: result.Thursday,
            friday: result.Friday,
            saturday: result.Saturday,
            sunday: result.Sunday);

        result.SupplementRunningDates = TravelineCalendarSupplementRunningDateTools.GetAllDates(
            scheduleDate: scheduleDate,
            holidays: holidays,
            operatingProfile: operatingProfile,
            startDate: result.StartDate,
            endDate: result.EndDate,
            monday: result.Monday,
            tuesday: result.Tuesday,
            wednesday: result.Wednesday,
            thursday: result.Thursday,
            friday: result.Friday,
            saturday: result.Saturday,
            sunday: result.Sunday,
            dates: result.RunningDates);

        result.SupplementNonRunningDates = TravelineCalendarSupplementNonRunningDateTools.GetAllDates(
            scheduleDate: scheduleDate,
            holidays: holidays,
            operatingProfile: operatingProfile,
            startDate: result.StartDate,
            endDate: result.EndDate,
            monday: result.Monday,
            tuesday: result.Tuesday,
            wednesday: result.Wednesday,
            thursday: result.Thursday,
            friday: result.Friday,
            saturday: result.Saturday,
            sunday: result.Sunday,
            dates: result.RunningDates);

        #endregion

        return result;
    }
}
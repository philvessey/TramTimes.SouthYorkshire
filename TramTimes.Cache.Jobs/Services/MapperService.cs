using System.Globalization;
using AutoMapper;
using NextDepartures.Standard.Models;
using TramTimes.Cache.Jobs.Models;

namespace TramTimes.Cache.Jobs.Services;

public class MapperService : Profile
{
    private readonly TimeZoneInfo _timezone = TimeZoneInfo.FindSystemTimeZoneById(id: "Europe/London");

    public MapperService()
    {
        #region service -> worker stop point

        CreateMap<Service, WorkerStopPoint>()
            .ForMember(
                destinationMember: point => point.DepartureDateTime,
                memberOptions: member => member.MapFrom(mapExpression: service =>
                    TimeZoneInfo
                        .ConvertTimeToUtc(
                            dateTime: service.DepartureDateTime ?? DateTime.UtcNow,
                            sourceTimeZone: _timezone)
                        .ToString(provider: CultureInfo.InvariantCulture)));

        #endregion

        #region worker stop point -> cache stop point

        CreateMap<WorkerStopPoint, CacheStopPoint>()
            .ForMember(
                destinationMember: point => point.DepartureDateTime,
                memberOptions: member => member.MapFrom(mapExpression: point =>
                    DateTime.Parse(
                        s: string.IsNullOrEmpty(value: point.DepartureDateTime)
                            ? DateTime.UtcNow.ToString(provider: CultureInfo.InvariantCulture)
                            : point.DepartureDateTime,
                        provider: CultureInfo.InvariantCulture)));

        #endregion
    }
}
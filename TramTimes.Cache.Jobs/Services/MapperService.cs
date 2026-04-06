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
                            dateTime: service.DepartureDateTime,
                            sourceTimeZone: _timezone)
                        .ToString(provider: CultureInfo.InvariantCulture)));

        #endregion

        #region worker stop point -> cache stop point

        CreateMap<WorkerStopPoint, CacheStopPoint>()
            .ForMember(
                destinationMember: point => point.DepartureDateTime,
                memberOptions: member => member.MapFrom(mapExpression: point =>
                    DateTime.Parse(
                        s: point.DepartureDateTime ?? string.Empty,
                        provider: CultureInfo.InvariantCulture)));

        #endregion
    }
}
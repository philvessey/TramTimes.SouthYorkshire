using AutoMapper;
using GTFS.Entities;
using NextDepartures.Standard.Models;
using TramTimes.Search.Jobs.Models;

namespace TramTimes.Search.Jobs.Services;

public class MapperService : Profile
{
    private readonly TimeZoneInfo _timezone = TimeZoneInfo.FindSystemTimeZoneById(id: "Europe/London");

    public MapperService()
    {
        #region service -> search stop point

        CreateMap<Service, SearchStopPoint>()
            .ForMember(
                destinationMember: point => point.DepartureDateTime,
                memberOptions: member => member.MapFrom(mapExpression: service =>
                    TimeZoneInfo.ConvertTimeToUtc(
                        dateTime: service.DepartureDateTime,
                        sourceTimeZone: _timezone)));

        #endregion

        #region stop -> search stop

        CreateMap<Stop, SearchStop>()
            .ForMember(
                destinationMember: stop => stop.Platform,
                memberOptions: member => member.MapFrom(mapExpression: stop => stop.PlatformCode));

        #endregion
    }
}
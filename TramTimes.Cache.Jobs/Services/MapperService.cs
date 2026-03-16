using System.Globalization;
using AutoMapper;
using GTFS.Entities;
using NextDepartures.Standard.Models;
using TramTimes.Cache.Jobs.Models;

namespace TramTimes.Cache.Jobs.Services;

public class MapperService : Profile
{
    public MapperService()
    {
        #region service -> cache stop point

        CreateMap<Service, CacheStopPoint>();
        CreateMap<CacheStopPoint, Service>();

        #endregion

        #region service -> worker stop point

        CreateMap<Service, WorkerStopPoint>()
            .ForMember(
                destinationMember: point => point.DepartureDateTime,
                memberOptions: member => member.MapFrom(mapExpression: service =>
                    service.DepartureDateTime.ToString(CultureInfo.InvariantCulture)));

        CreateMap<WorkerStopPoint, Service>()
            .ForMember(
                destinationMember: service => service.DepartureDateTime,
                memberOptions: member => member.MapFrom(mapExpression: point =>
                    DateTime.Parse(point.DepartureDateTime ?? string.Empty, CultureInfo.InvariantCulture)));

        #endregion

        #region cache stop -> stop

        CreateMap<CacheStop, Stop>()
            .ForMember(
                destinationMember: stop => stop.PlatformCode,
                memberOptions: member => member.MapFrom(mapExpression: stop => stop.Platform));

        CreateMap<Stop, CacheStop>()
            .ForMember(
                destinationMember: stop => stop.Platform,
                memberOptions: member => member.MapFrom(mapExpression: stop => stop.PlatformCode));

        #endregion

        #region cache stop point -> worker stop point

        CreateMap<CacheStopPoint, WorkerStopPoint>()
            .ForMember(
                destinationMember: point => point.DepartureDateTime,
                memberOptions: member => member.MapFrom(mapExpression: point =>
                    point.DepartureDateTime!.Value.ToString(CultureInfo.InvariantCulture)));

        CreateMap<WorkerStopPoint, CacheStopPoint>()
            .ForMember(
                destinationMember: point => point.DepartureDateTime,
                memberOptions: member => member.MapFrom(mapExpression: point =>
                    DateTime.Parse(point.DepartureDateTime ?? string.Empty, CultureInfo.InvariantCulture)));

        #endregion
    }
}
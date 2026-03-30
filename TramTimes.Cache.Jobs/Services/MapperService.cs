using System.Globalization;
using AutoMapper;
using GTFS.Entities;
using NextDepartures.Standard.Models;
using TramTimes.Cache.Jobs.Models;

namespace TramTimes.Cache.Jobs.Services;

public class MapperService : Profile
{
    private readonly TimeZoneInfo _timezone = TimeZoneInfo.FindSystemTimeZoneById(id: "Europe/London");

    public MapperService()
    {
        #region service -> cache stop point

        CreateMap<Service, CacheStopPoint>()
            .ForMember(
                destinationMember: point => point.DepartureDateTime,
                memberOptions: member => member.MapFrom(mapExpression: service =>
                    TimeZoneInfo.ConvertTimeToUtc(
                        dateTime: service.DepartureDateTime,
                        sourceTimeZone: _timezone)));

        CreateMap<CacheStopPoint, Service>()
            .ForMember(
                destinationMember: service => service.DepartureDateTime,
                memberOptions: member => member.MapFrom(mapExpression: point =>
                    TimeZoneInfo.ConvertTimeFromUtc(
                        dateTime: point.DepartureDateTime!.Value,
                        destinationTimeZone: _timezone)));

        #endregion

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

        CreateMap<WorkerStopPoint, Service>()
            .ForMember(
                destinationMember: service => service.DepartureDateTime,
                memberOptions: member => member.MapFrom(mapExpression: point =>
                    TimeZoneInfo.ConvertTimeFromUtc(
                        dateTime: DateTime.Parse(
                            s: point.DepartureDateTime ?? string.Empty,
                            provider: CultureInfo.InvariantCulture),
                        destinationTimeZone: _timezone)));

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
                    point.DepartureDateTime!.Value.ToString(provider: CultureInfo.InvariantCulture)));

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
using System.Globalization;
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

        CreateMap<SearchStopPoint, Service>()
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

        #region search stop -> stop

        CreateMap<SearchStop, Stop>()
            .ForMember(
                destinationMember: stop => stop.PlatformCode,
                memberOptions: member => member.MapFrom(mapExpression: stop => stop.Platform));

        CreateMap<Stop, SearchStop>()
            .ForMember(
                destinationMember: stop => stop.Platform,
                memberOptions: member => member.MapFrom(mapExpression: stop => stop.PlatformCode));

        #endregion

        #region search stop point -> worker stop point

        CreateMap<SearchStopPoint, WorkerStopPoint>()
            .ForMember(
                destinationMember: point => point.DepartureDateTime,
                memberOptions: member => member.MapFrom(mapExpression: point =>
                    point.DepartureDateTime!.Value.ToString(provider: CultureInfo.InvariantCulture)));

        CreateMap<WorkerStopPoint, SearchStopPoint>()
            .ForMember(
                destinationMember: point => point.DepartureDateTime,
                memberOptions: member => member.MapFrom(mapExpression: point =>
                    DateTime.Parse(
                        s: point.DepartureDateTime ?? string.Empty,
                        provider: CultureInfo.InvariantCulture)));

        #endregion
    }
}
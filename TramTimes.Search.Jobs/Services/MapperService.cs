using System.Globalization;
using AutoMapper;
using GTFS.Entities;
using NextDepartures.Standard.Models;
using TramTimes.Search.Jobs.Models;

namespace TramTimes.Search.Jobs.Services;

public class MapperService : Profile
{
    public MapperService()
    {
        #region service -> search stop point

        CreateMap<Service, SearchStopPoint>();
        CreateMap<SearchStopPoint, Service>();

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
                    point.DepartureDateTime!.Value.ToString(CultureInfo.InvariantCulture)));

        CreateMap<WorkerStopPoint, SearchStopPoint>()
            .ForMember(
                destinationMember: point => point.DepartureDateTime,
                memberOptions: member => member.MapFrom(mapExpression: point =>
                    DateTime.Parse(point.DepartureDateTime ?? string.Empty, CultureInfo.InvariantCulture)));

        #endregion
    }
}
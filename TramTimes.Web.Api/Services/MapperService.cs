using System.Globalization;
using AutoMapper;
using GTFS.Entities;
using NextDepartures.Standard.Models;
using TramTimes.Web.Api.Models;
using TramTimes.Web.Utilities.Models;

namespace TramTimes.Web.Api.Services;

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

        #region cache stop -> web stop

        CreateMap<CacheStop, WebStop>();
        CreateMap<WebStop, CacheStop>();

        #endregion

        #region cache stop point -> web stop point

        CreateMap<CacheStopPoint, WebStopPoint>()
            .ForMember(
                destinationMember: point => point.DepartureDateTime,
                memberOptions: member => member.MapFrom(mapExpression: point =>
                    point.DepartureDateTime!.Value.ToString(CultureInfo.InvariantCulture)));

        CreateMap<WebStopPoint, CacheStopPoint>()
            .ForMember(
                destinationMember: point => point.DepartureDateTime,
                memberOptions: member => member.MapFrom(mapExpression: point =>
                    DateTime.Parse(point.DepartureDateTime ?? string.Empty, CultureInfo.InvariantCulture)));

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

        #region database stop -> stop

        CreateMap<DatabaseStop, Stop>()
            .ForMember(
                destinationMember: stop => stop.PlatformCode,
                memberOptions: member => member.MapFrom(mapExpression: stop => stop.Platform));

        CreateMap<Stop, DatabaseStop>()
            .ForMember(
                destinationMember: stop => stop.Platform,
                memberOptions: member => member.MapFrom(mapExpression: stop => stop.PlatformCode));

        #endregion

        #region database stop -> web stop

        CreateMap<DatabaseStop, WebStop>();
        CreateMap<WebStop, DatabaseStop>();

        #endregion

        #region database stop point -> web stop point

        CreateMap<DatabaseStopPoint, WebStopPoint>()
            .ForMember(
                destinationMember: point => point.DepartureDateTime,
                memberOptions: member => member.MapFrom(mapExpression: point =>
                    point.DepartureDateTime!.Value.ToString(CultureInfo.InvariantCulture)));

        CreateMap<WebStopPoint, DatabaseStopPoint>()
            .ForMember(
                destinationMember: point => point.DepartureDateTime,
                memberOptions: member => member.MapFrom(mapExpression: point =>
                    DateTime.Parse(point.DepartureDateTime ?? string.Empty, CultureInfo.InvariantCulture)));

        #endregion

        #region database stop point -> worker stop point

        CreateMap<DatabaseStopPoint, WorkerStopPoint>()
            .ForMember(
                destinationMember: point => point.DepartureDateTime,
                memberOptions: member => member.MapFrom(mapExpression: point =>
                    point.DepartureDateTime!.Value.ToString(CultureInfo.InvariantCulture)));

        CreateMap<WorkerStopPoint, DatabaseStopPoint>()
            .ForMember(
                destinationMember: point => point.DepartureDateTime,
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

        #region search stop -> web stop

        CreateMap<SearchStop, WebStop>();
        CreateMap<WebStop, SearchStop>();

        #endregion

        #region search stop point -> web stop point

        CreateMap<SearchStopPoint, WebStopPoint>()
            .ForMember(
                destinationMember: point => point.DepartureDateTime,
                memberOptions: member => member.MapFrom(mapExpression: point =>
                    point.DepartureDateTime!.Value.ToString(CultureInfo.InvariantCulture)));

        CreateMap<WebStopPoint, SearchStopPoint>()
            .ForMember(
                destinationMember: point => point.DepartureDateTime,
                memberOptions: member => member.MapFrom(mapExpression: point =>
                    DateTime.Parse(point.DepartureDateTime ?? string.Empty, CultureInfo.InvariantCulture)));

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
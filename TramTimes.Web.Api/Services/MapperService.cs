using System.Globalization;
using AutoMapper;
using GTFS.Entities;
using NextDepartures.Standard.Models;
using TramTimes.Web.Api.Models;
using TramTimes.Web.Utilities.Models;

namespace TramTimes.Web.Api.Services;

public static class MapperService
{
    public static WebApplicationBuilder AddMapperDefaults(this WebApplicationBuilder builder)
    {
        var configuration = new MapperConfiguration(configure: expression =>
        {
            #region service -> search stop point
            
            expression.CreateMap<Service, SearchStopPoint>();
            expression.CreateMap<SearchStopPoint, Service>();
            
            #endregion
            
            #region service -> worker stop point
            
            expression
                .CreateMap<Service, WorkerStopPoint>()
                .ForMember(
                    destinationMember: point => point.DepartureDateTime,
                    memberOptions: member => member.MapFrom(mapExpression: service =>
                        service.DepartureDateTime.ToString(CultureInfo.InvariantCulture)));
            
            expression
                .CreateMap<WorkerStopPoint, Service>()
                .ForMember(
                    destinationMember: service => service.DepartureDateTime,
                    memberOptions: member => member.MapFrom(mapExpression: point =>
                        DateTime.Parse(point.DepartureDateTime ?? string.Empty, CultureInfo.InvariantCulture)));
            
            #endregion
            
            #region cache stop -> stop
            
            expression.CreateMap<CacheStop, Stop>()
                .ForMember(
                    destinationMember: stop => stop.PlatformCode,
                    memberOptions: member => member.MapFrom(mapExpression: stop => stop.Platform));
            
            expression.CreateMap<Stop, CacheStop>()
                .ForMember(
                    destinationMember: stop => stop.Platform,
                    memberOptions: member => member.MapFrom(mapExpression: stop => stop.PlatformCode));
            
            #endregion
            
            #region cache stop -> web stop
            
            expression.CreateMap<CacheStop, WebStop>();
            expression.CreateMap<WebStop, CacheStop>();
            
            #endregion
            
            #region cache stop point -> web stop point
            
            expression
                .CreateMap<CacheStopPoint, WebStopPoint>()
                .ForMember(
                    destinationMember: point => point.DepartureDateTime,
                    memberOptions: member => member.MapFrom(mapExpression: point =>
                        point.DepartureDateTime!.Value.ToString(CultureInfo.InvariantCulture)));
            
            expression
                .CreateMap<WebStopPoint, CacheStopPoint>()
                .ForMember(
                    destinationMember: point => point.DepartureDateTime,
                    memberOptions: member => member.MapFrom(mapExpression: point =>
                        DateTime.Parse(point.DepartureDateTime ?? string.Empty, CultureInfo.InvariantCulture)));
            
            #endregion
            
            #region cache stop point -> worker stop point
            
            expression
                .CreateMap<CacheStopPoint, WorkerStopPoint>()
                .ForMember(
                    destinationMember: point => point.DepartureDateTime,
                    memberOptions: member => member.MapFrom(mapExpression: point =>
                        point.DepartureDateTime!.Value.ToString(CultureInfo.InvariantCulture)));
            
            expression
                .CreateMap<WorkerStopPoint, CacheStopPoint>()
                .ForMember(
                    destinationMember: point => point.DepartureDateTime,
                    memberOptions: member => member.MapFrom(mapExpression: point =>
                        DateTime.Parse(point.DepartureDateTime ?? string.Empty, CultureInfo.InvariantCulture)));
            
            #endregion
            
            #region database stop -> stop
            
            expression.CreateMap<DatabaseStop, Stop>()
                .ForMember(
                    destinationMember: stop => stop.PlatformCode,
                    memberOptions: member => member.MapFrom(mapExpression: stop => stop.Platform));
            
            expression.CreateMap<Stop, DatabaseStop>()
                .ForMember(
                    destinationMember: stop => stop.Platform,
                    memberOptions: member => member.MapFrom(mapExpression: stop => stop.PlatformCode));
            
            #endregion
            
            #region database stop -> web stop
            
            expression.CreateMap<DatabaseStop, WebStop>();
            expression.CreateMap<WebStop, DatabaseStop>();
            
            #endregion
            
            #region database stop point -> web stop point
            
            expression
                .CreateMap<DatabaseStopPoint, WebStopPoint>()
                .ForMember(
                    destinationMember: point => point.DepartureDateTime,
                    memberOptions: member => member.MapFrom(mapExpression: point =>
                        point.DepartureDateTime!.Value.ToString(CultureInfo.InvariantCulture)));
            
            expression
                .CreateMap<WebStopPoint, DatabaseStopPoint>()
                .ForMember(
                    destinationMember: point => point.DepartureDateTime,
                    memberOptions: member => member.MapFrom(mapExpression: point =>
                        DateTime.Parse(point.DepartureDateTime ?? string.Empty, CultureInfo.InvariantCulture)));
            
            #endregion
            
            #region database stop point -> worker stop point
            
            expression
                .CreateMap<DatabaseStopPoint, WorkerStopPoint>()
                .ForMember(
                    destinationMember: point => point.DepartureDateTime,
                    memberOptions: member => member.MapFrom(mapExpression: point =>
                        point.DepartureDateTime!.Value.ToString(CultureInfo.InvariantCulture)));
            
            expression
                .CreateMap<WorkerStopPoint, DatabaseStopPoint>()
                .ForMember(
                    destinationMember: point => point.DepartureDateTime,
                    memberOptions: member => member.MapFrom(mapExpression: point =>
                        DateTime.Parse(point.DepartureDateTime ?? string.Empty, CultureInfo.InvariantCulture)));
            
            #endregion
            
            #region search stop -> stop
            
            expression.CreateMap<SearchStop, Stop>()
                .ForMember(
                    destinationMember: stop => stop.PlatformCode,
                    memberOptions: member => member.MapFrom(mapExpression: stop => stop.Platform));
            
            expression.CreateMap<Stop, SearchStop>()
                .ForMember(
                    destinationMember: stop => stop.Platform,
                    memberOptions: member => member.MapFrom(mapExpression: stop => stop.PlatformCode));
            
            #endregion
            
            #region search stop -> web stop
            
            expression.CreateMap<SearchStop, WebStop>();
            expression.CreateMap<WebStop, SearchStop>();
            
            #endregion
            
            #region search stop point -> web stop point
            
            expression
                .CreateMap<SearchStopPoint, WebStopPoint>()
                .ForMember(
                    destinationMember: point => point.DepartureDateTime,
                    memberOptions: member => member.MapFrom(mapExpression: point =>
                        point.DepartureDateTime!.Value.ToString(CultureInfo.InvariantCulture)));
            
            expression
                .CreateMap<WebStopPoint, SearchStopPoint>()
                .ForMember(
                    destinationMember: point => point.DepartureDateTime,
                    memberOptions: member => member.MapFrom(mapExpression: point =>
                        DateTime.Parse(point.DepartureDateTime ?? string.Empty, CultureInfo.InvariantCulture)));
            
            #endregion
            
            #region search stop point -> worker stop point
            
            expression
                .CreateMap<SearchStopPoint, WorkerStopPoint>()
                .ForMember(
                    destinationMember: point => point.DepartureDateTime,
                    memberOptions: member => member.MapFrom(mapExpression: point =>
                        point.DepartureDateTime!.Value.ToString(CultureInfo.InvariantCulture)));
            
            expression
                .CreateMap<WorkerStopPoint, SearchStopPoint>()
                .ForMember(
                    destinationMember: point => point.DepartureDateTime,
                    memberOptions: member => member.MapFrom(mapExpression: point =>
                        DateTime.Parse(point.DepartureDateTime ?? string.Empty, CultureInfo.InvariantCulture)));
            
            #endregion
        });
        
        builder.Services.AddSingleton(implementationInstance: configuration.CreateMapper());
        
        return builder;
    }
}
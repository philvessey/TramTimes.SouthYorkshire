using System.Globalization;
using AutoMapper;
using GTFS.Entities;
using NextDepartures.Standard.Models;
using TramTimes.Cache.Jobs.Models;

namespace TramTimes.Cache.Jobs.Services;

public static class MapperService
{
    public static HostApplicationBuilder AddMapperDefaults(this HostApplicationBuilder builder)
    {
        var configuration = new MapperConfiguration(configure: expression =>
        {
            #region service -> cache stop point
            
            expression.CreateMap<Service, CacheStopPoint>();
            expression.CreateMap<CacheStopPoint, Service>();
            
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
            
            expression
                .CreateMap<CacheStop, Stop>()
                .ForMember(
                    destinationMember: stop => stop.PlatformCode,
                    memberOptions: member => member.MapFrom(mapExpression: stop => stop.Platform));
            
            expression
                .CreateMap<Stop, CacheStop>()
                .ForMember(
                    destinationMember: stop => stop.Platform,
                    memberOptions: member => member.MapFrom(mapExpression: stop => stop.PlatformCode));
            
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
        });
        
        builder.Services.AddSingleton(implementationInstance: configuration.CreateMapper());
        
        return builder;
    }
}
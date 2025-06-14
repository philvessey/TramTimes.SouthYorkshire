using System.Globalization;
using AutoMapper;
using GTFS.Entities;
using NextDepartures.Standard.Models;
using TramTimes.Search.Jobs.Models;

namespace TramTimes.Search.Jobs.Services;

public static class MapperService
{
    public static HostApplicationBuilder AddMapperDefaults(this HostApplicationBuilder builder)
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
            
            #region search stop -> stop
            
            expression
                .CreateMap<SearchStop, Stop>()
                .ForMember(
                    destinationMember: stop => stop.PlatformCode,
                    memberOptions: member => member.MapFrom(mapExpression: stop => stop.Platform));
            
            expression
                .CreateMap<Stop, SearchStop>()
                .ForMember(
                    destinationMember: stop => stop.Platform,
                    memberOptions: member => member.MapFrom(mapExpression: stop => stop.PlatformCode));
            
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
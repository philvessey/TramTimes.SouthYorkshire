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
            expression.CreateMap<Service, WorkerStopPoint>()
                .ForMember(
                    destinationMember: point => point.DepartureDateTime,
                    memberOptions: member => member.MapFrom(mapExpression: service =>
                        service.DepartureDateTime.ToString(CultureInfo.InvariantCulture)));
            
            expression.CreateMap<WorkerStopPoint, DatabaseStopPoint>()
                .ForMember(
                    destinationMember: point => point.DepartureDateTime,
                    memberOptions: member => member.MapFrom(mapExpression: point =>
                        DateTime.Parse(point.DepartureDateTime ?? string.Empty, CultureInfo.InvariantCulture)));
            
            expression.CreateMap<DatabaseStopPoint, WebStopPoint>()
                .ForMember(
                    destinationMember: point => point.DepartureDateTime,
                    memberOptions: member => member.MapFrom(mapExpression: point =>
                        point.DepartureDateTime!.Value.ToString(CultureInfo.InvariantCulture)));
            
            expression.CreateMap<WorkerStopPoint, CacheStopPoint>()
                .ForMember(
                    destinationMember: point => point.DepartureDateTime,
                    memberOptions: member => member.MapFrom(mapExpression: point =>
                        DateTime.Parse(point.DepartureDateTime ?? string.Empty, CultureInfo.InvariantCulture)));
            
            expression.CreateMap<CacheStopPoint, WebStopPoint>()
                .ForMember(
                    destinationMember: point => point.DepartureDateTime,
                    memberOptions: member => member.MapFrom(mapExpression: point =>
                        point.DepartureDateTime!.Value.ToString(CultureInfo.InvariantCulture)));
            
            expression.CreateMap<WorkerStopPoint, SearchStopPoint>()
                .ForMember(
                    destinationMember: point => point.DepartureDateTime,
                    memberOptions: member => member.MapFrom(mapExpression: point =>
                        DateTime.Parse(point.DepartureDateTime ?? string.Empty, CultureInfo.InvariantCulture)));
            
            expression.CreateMap<SearchStopPoint, WebStopPoint>()
                .ForMember(
                    destinationMember: point => point.DepartureDateTime,
                    memberOptions: member => member.MapFrom(mapExpression: point =>
                        point.DepartureDateTime!.Value.ToString(CultureInfo.InvariantCulture)));
            
            expression.CreateMap<Stop, DatabaseStop>();
            expression.CreateMap<DatabaseStop, WebStop>();
            
            expression.CreateMap<Stop, CacheStop>();
            expression.CreateMap<CacheStop, WebStop>();
            
            expression.CreateMap<Stop, SearchStop>();
            expression.CreateMap<SearchStop, WebStop>();
        });
        
        builder.Services.AddSingleton(implementationInstance: configuration.CreateMapper());
        
        return builder;
    }
}
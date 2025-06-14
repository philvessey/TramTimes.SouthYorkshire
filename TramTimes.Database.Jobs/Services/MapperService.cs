using AutoMapper;
using GTFS.Entities;
using NextDepartures.Standard.Models;
using TramTimes.Database.Jobs.Models;

namespace TramTimes.Database.Jobs.Services;

public static class MapperService
{
    public static HostApplicationBuilder AddMapperDefaults(this HostApplicationBuilder builder)
    {
        var configuration = new MapperConfiguration(configure: expression =>
        {
            #region service -> worker stop point
            
            expression
                .CreateMap<Service, WorkerStopPoint>()
                .ForMember(
                    destinationMember: point => point.DepartureTime,
                    memberOptions: member => member.MapFrom(mapExpression: service =>
                        service.DepartureTime.ToString()));
            
            expression
                .CreateMap<WorkerStopPoint, Service>()
                .ForMember(
                    destinationMember: service => service.DepartureTime,
                    memberOptions: member => member.MapFrom(mapExpression: point =>
                        TimeOfDay.FromString(point.DepartureTime)));
            
            #endregion
        });
        
        builder.Services.AddSingleton(implementationInstance: configuration.CreateMapper());
        
        return builder;
    }
}
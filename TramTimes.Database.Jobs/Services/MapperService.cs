using AutoMapper;
using NextDepartures.Standard.Models;
using TramTimes.Database.Jobs.Models;

namespace TramTimes.Database.Jobs.Services;

public static class MapperService
{
    public static HostApplicationBuilder AddMapperDefaults(this HostApplicationBuilder builder)
    {
        var configuration = new MapperConfiguration(configure: expression =>
        {
            expression.CreateMap<Service, WorkerStopPoint>()
                .ForMember(
                    destinationMember: point => point.DepartureTime,
                    memberOptions: member => member.MapFrom(mapExpression: service =>
                        service.DepartureTime.ToString()));
        });
        
        builder.Services.AddSingleton(implementationInstance: configuration.CreateMapper());
        
        return builder;
    }
}
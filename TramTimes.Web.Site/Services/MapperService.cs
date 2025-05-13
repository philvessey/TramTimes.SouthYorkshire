using System.Globalization;
using AutoMapper;
using TramTimes.Web.Site.Models;
using TramTimes.Web.Utilities.Models;
using TramTimes.Web.Utilities.Tools;

namespace TramTimes.Web.Site.Services;

public static class MapperService
{
    public static WebApplicationBuilder AddMapperDefaults(this WebApplicationBuilder builder)
    {
        var configuration = new MapperConfiguration(configure: expression =>
        {
            expression.CreateMap<WebStopPoint, TelerikStopPoint>()
                .ForMember(
                    destinationMember: point => point.DepartureDateTime,
                    memberOptions: member => member.MapFrom(mapExpression: point =>
                        DateTime.Parse(point.DepartureDateTime ?? string.Empty, CultureInfo.InvariantCulture)))
                .ForMember(
                    destinationMember: point => point.DestinationName,
                    memberOptions: member => member.MapFrom(mapExpression: point =>
                        RegexTools.RemoveDirection(point.DestinationName ?? string.Empty)))
                .ForMember(
                    destinationMember: point => point.StopName,
                    memberOptions: member => member.MapFrom(mapExpression: point =>
                        RegexTools.RemoveDirection(point.StopName ?? string.Empty)));
            
            expression.CreateMap<WebStop, TelerikStop>()
                .ForMember(
                    destinationMember: stop => stop.Name,
                    memberOptions: member => member.MapFrom(mapExpression: stop =>
                        RegexTools.RemoveDirection(stop.Name ?? string.Empty)))
                .ForMember(
                    destinationMember: stop => stop.Direction,
                    memberOptions: member => member.MapFrom(mapExpression: stop =>
                        RegexTools.RemoveName(stop.Name ?? string.Empty)))
                .ForMember(
                    destinationMember: stop => stop.Location,
                    memberOptions: member => member.MapFrom(mapExpression: stop =>
                        new[] { stop.Latitude ?? 0, stop.Longitude ?? 0 }));
        });
        
        builder.Services.AddSingleton(implementationInstance: configuration.CreateMapper());
        
        return builder;
    }
}
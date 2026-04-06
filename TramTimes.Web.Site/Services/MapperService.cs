using System.Globalization;
using AutoMapper;
using TramTimes.Web.Site.Models;
using TramTimes.Web.Utilities.Models;
using TramTimes.Web.Utilities.Tools;

namespace TramTimes.Web.Site.Services;

public class MapperService : Profile
{
    public MapperService()
    {
        #region web stop -> telerik stop

        CreateMap<WebStop, TelerikStop>()
            .ForMember(
                destinationMember: stop => stop.Name,
                memberOptions: member => member.MapFrom(mapExpression: stop =>
                    RegexTools.RemoveDirection(input: stop.Name ?? string.Empty)))
            .ForMember(
                destinationMember: stop => stop.Direction,
                memberOptions: member => member.MapFrom(mapExpression: stop =>
                    RegexTools.RemoveName(input: stop.Name ?? string.Empty)))
            .ForMember(
                destinationMember: stop => stop.Location,
                memberOptions: member => member.MapFrom(mapExpression: stop =>
                    new[] { stop.Latitude ?? 0, stop.Longitude ?? 0 }));

        #endregion

        #region web stop point -> telerik stop point

        CreateMap<WebStopPoint, TelerikStopPoint>()
            .ForMember(
                destinationMember: point => point.DepartureDateTime,
                memberOptions: member => member.MapFrom(mapExpression: point =>
                    DateTime.Parse(
                        s: point.DepartureDateTime ?? string.Empty,
                        provider: CultureInfo.InvariantCulture)))
            .ForMember(
                destinationMember: point => point.DestinationName,
                memberOptions: member => member.MapFrom(mapExpression: point =>
                    RegexTools.RemoveDirection(input: point.DestinationName ?? string.Empty)))
            .ForMember(
                destinationMember: point => point.DestinationDirection,
                memberOptions: member => member.MapFrom(mapExpression: point =>
                    RegexTools.RemoveName(input: point.DestinationName ?? string.Empty)))
            .ForMember(
                destinationMember: point => point.StopName,
                memberOptions: member => member.MapFrom(mapExpression: point =>
                    RegexTools.RemoveDirection(input: point.StopName ?? string.Empty)))
            .ForMember(
                destinationMember: point => point.StopDirection,
                memberOptions: member => member.MapFrom(mapExpression: point =>
                    RegexTools.RemoveName(input: point.StopName ?? string.Empty)));

        #endregion
    }
}
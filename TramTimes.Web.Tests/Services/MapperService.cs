using System.Globalization;
using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;
using TramTimes.Web.Tests.Models;
using TramTimes.Web.Utilities.Models;
using TramTimes.Web.Utilities.Tools;

namespace TramTimes.Web.Tests.Services;

public static class MapperService
{
    public static IMapper CreateMapper()
    {
        var loggerFactory = new NullLoggerFactory();

        var configuration = new MapperConfiguration(loggerFactory: loggerFactory, configure: expression =>
        {
            expression.LicenseKey = Environment.GetEnvironmentVariable(variable: "AUTOMAPPER_LICENSE") ?? string.Empty;

            #region web stop point -> telerik stop point

            expression.CreateMap<WebStopPoint, TelerikStopPoint>()
                .ForMember(
                    destinationMember: point => point.DepartureDateTime,
                    memberOptions: member => member.MapFrom(mapExpression: point =>
                        DateTime.Parse(
                            s: string.IsNullOrEmpty(value: point.DepartureDateTime)
                                ? DateTime.UtcNow.ToString(provider: CultureInfo.InvariantCulture)
                                : point.DepartureDateTime,
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
        });

        return configuration.CreateMapper();
    }
}
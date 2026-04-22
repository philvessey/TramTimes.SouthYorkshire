using System.Globalization;
using AutoMapper;
using GTFS.Entities;
using NextDepartures.Standard.Models;
using TramTimes.Web.Site.Defaults;
using TramTimes.Web.Site.Models;
using TramTimes.Web.Utilities.Models;
using TramTimes.Web.Utilities.Tools;

namespace TramTimes.Web.Site.Services;

public class MapperService : Profile
{
    private readonly TimeZoneInfo _timezone = TimeZoneInfo.FindSystemTimeZoneById(id: "Europe/London");

    public MapperService()
    {
        #region cache stop point -> web stop point

        CreateMap<CacheStopPoint, WebStopPoint>()
            .ForMember(
                destinationMember: point => point.DepartureDateTime,
                memberOptions: member => member.MapFrom(mapExpression: point =>
                    point.DepartureDateTime.HasValue
                        ? point.DepartureDateTime.Value.ToString(provider: CultureInfo.InvariantCulture)
                        : DateTime.UtcNow.ToString(provider: CultureInfo.InvariantCulture)));

        #endregion

        #region database stop -> web stop

        CreateMap<DatabaseStop, WebStop>();

        #endregion

        #region database stop point -> web stop point

        CreateMap<DatabaseStopPoint, WebStopPoint>()
            .ForMember(
                destinationMember: point => point.DepartureDateTime,
                memberOptions: member => member.MapFrom(mapExpression: point =>
                    point.DepartureDateTime.HasValue
                        ? point.DepartureDateTime.Value.ToString(provider: CultureInfo.InvariantCulture)
                        : DateTime.UtcNow.ToString(provider: CultureInfo.InvariantCulture)));

        #endregion

        #region search stop -> web stop

        CreateMap<SearchStop, WebStop>();

        #endregion

        #region search stop point -> web stop point

        CreateMap<SearchStopPoint, WebStopPoint>()
            .ForMember(
                destinationMember: point => point.DepartureDateTime,
                memberOptions: member => member.MapFrom(mapExpression: point =>
                    point.DepartureDateTime.HasValue
                        ? point.DepartureDateTime.Value.ToString(provider: CultureInfo.InvariantCulture)
                        : DateTime.UtcNow.ToString(provider: CultureInfo.InvariantCulture)));

        #endregion

        #region service -> search stop point

        CreateMap<Service, SearchStopPoint>()
            .ForMember(
                destinationMember: point => point.DepartureDateTime,
                memberOptions: member => member.MapFrom(mapExpression: service =>
                    TimeZoneInfo.ConvertTimeToUtc(
                        dateTime: service.DepartureDateTime ?? DateTime.UtcNow,
                        sourceTimeZone: _timezone)));

        #endregion

        #region service -> web stop point

        CreateMap<Service, WebStopPoint>()
            .ForMember(
                destinationMember: point => point.DepartureDateTime,
                memberOptions: member => member.MapFrom(mapExpression: service =>
                    TimeZoneInfo
                        .ConvertTimeToUtc(
                            dateTime: service.DepartureDateTime ?? DateTime.UtcNow,
                            sourceTimeZone: _timezone)
                        .ToString(provider: CultureInfo.InvariantCulture)));

        #endregion

        #region stop -> database stop

        CreateMap<Stop, DatabaseStop>()
            .ForMember(
                destinationMember: stop => stop.Platform,
                memberOptions: member => member.MapFrom(mapExpression: stop => stop.PlatformCode));

        #endregion

        #region stop -> search stop

        CreateMap<Stop, SearchStop>()
            .ForMember(
                destinationMember: stop => stop.Platform,
                memberOptions: member => member.MapFrom(mapExpression: stop => stop.PlatformCode));

        #endregion

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
                memberOptions: member => member.MapFrom(mapExpression: stop => new[]
                {
                    stop.Latitude ?? TelerikMapDefaults.Center[0],
                    stop.Longitude ?? TelerikMapDefaults.Center[1]
                }));

        #endregion

        #region web stop point -> cache stop point

        CreateMap<WebStopPoint, CacheStopPoint>()
            .ForMember(
                destinationMember: point => point.DepartureDateTime,
                memberOptions: member => member.MapFrom(mapExpression: point =>
                    DateTime.Parse(
                        s: string.IsNullOrEmpty(value: point.DepartureDateTime)
                            ? DateTime.UtcNow.ToString(provider: CultureInfo.InvariantCulture)
                            : point.DepartureDateTime,
                        provider: CultureInfo.InvariantCulture)));

        #endregion

        #region web stop point -> database stop point

        CreateMap<WebStopPoint, DatabaseStopPoint>()
            .ForMember(
                destinationMember: point => point.DepartureDateTime,
                memberOptions: member => member.MapFrom(mapExpression: point =>
                    DateTime.Parse(
                        s: string.IsNullOrEmpty(value: point.DepartureDateTime)
                            ? DateTime.UtcNow.ToString(provider: CultureInfo.InvariantCulture)
                            : point.DepartureDateTime,
                        provider: CultureInfo.InvariantCulture)));

        #endregion

        #region web stop point -> telerik stop point

        CreateMap<WebStopPoint, TelerikStopPoint>()
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
    }
}
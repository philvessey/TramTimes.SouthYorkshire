using System.Globalization;
using GeoUK;
using GeoUK.Coordinates;
using GeoUK.Ellipsoids;
using GeoUK.Projections;
using TramTimes.Database.Jobs.Models;

namespace TramTimes.Database.Jobs.Builders;

public static class NaptanStopBuilder
{
    public static NaptanStop Build(
        Dictionary<string, NaptanStop> stops,
        string? reference) {

        #region build unknown

        var unknown = new NaptanStop { AtcoCode = reference ?? "unknown" };

        #endregion

        #region build result

        stops.TryGetValue(
            key: reference ?? "unknown",
            value: out var stop);

        if (stop is null)
            return unknown;

        var result = new NaptanStop
        {
            AtcoCode = reference ?? "unknown",
            CommonName = stop.CommonName,
            ShortCommonName = stop.ShortCommonName,
            Landmark = stop.Landmark,
            Street = stop.Street,
            Crossing = stop.Crossing,
            Indicator = stop.Indicator,
            NptgLocalityCode = stop.NptgLocalityCode,
            LocalityName = stop.LocalityName,
            ParentLocalityName = stop.ParentLocalityName,
            GridType = stop.GridType,
            Easting = stop.Easting,
            Northing = stop.Northing,
            StopType = stop.StopType,
            AdministrativeAreaCode = stop.AdministrativeAreaCode
        };

        var commonName = result.CommonName ?? string.Empty;

        commonName = commonName.Replace(
            oldValue: "(South Yorkshire Supertram)",
            newValue: string.Empty);

        commonName = commonName.Trim();

        commonName = GeneratedRegexBuilder.GetPlatform().Replace(
            input: commonName,
            replacement: string.Empty);

        commonName = commonName.Trim();

        var from = GeneratedRegexBuilder.GetFrom().Match(input: commonName);
        var to = GeneratedRegexBuilder.GetTo().Match(input: commonName);

        var direction = from.Success ? from.Value : to.Success ? to.Value : result.Indicator;

        commonName = GeneratedRegexBuilder.GetFrom().Replace(
            input: commonName,
            replacement: string.Empty);

        commonName = commonName.Trim();

        commonName = GeneratedRegexBuilder.GetTo().Replace(
            input: commonName,
            replacement: string.Empty);

        commonName = commonName.Trim();

        commonName = $"{commonName} {direction ?? string.Empty}";
        commonName = commonName.Trim();

        result.CommonName = commonName;

        if (result.Easting is null || result.Northing is null)
            return result;

        var latitudeLongitude = GeoUK.Convert.ToLatitudeLongitude(
            ellipsoid: new Wgs84(),
            coordinates: Transform.Osgb36ToEtrs89(coordinates: GeoUK.Convert.ToCartesian(
                ellipsoid: new Airy1830(),
                projection: new BritishNationalGrid(),
                coordinates: new EastingNorthing(
                    easting: double.Parse(s: result.Easting),
                    northing: double.Parse(s: result.Northing)))));

        result.Latitude = latitudeLongitude.Latitude.ToString(provider: CultureInfo.InvariantCulture);
        result.Longitude = latitudeLongitude.Longitude.ToString(provider: CultureInfo.InvariantCulture);

        #endregion

        return result;
    }
}
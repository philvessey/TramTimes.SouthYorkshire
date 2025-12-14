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
            value: out var result);

        if (result is null)
            return unknown;

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
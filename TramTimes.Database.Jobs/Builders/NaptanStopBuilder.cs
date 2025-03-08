using System.Globalization;
using GeoUK;
using GeoUK.Coordinates;
using GeoUK.Ellipsoids;
using GeoUK.Projections;
using TramTimes.Database.Jobs.Models;

namespace TramTimes.Database.Jobs.Builders;

public static class NaptanStopBuilder
{
    public static async Task<NaptanStop> BuildAsync(
        Dictionary<string, NaptanStop> stops,
        string? reference) {
        
        stops.TryGetValue(
            key: reference ?? "unknown",
            value: out var value);
        
        if (value == null)
        {
            return await Task.FromResult(result: new NaptanStop
            {
                AtcoCode = reference ?? "unknown"
            });
        }
        
        if (value.Easting == null || value.Northing == null)
            return await Task.FromResult(result: value);
        
        var eastingNorthing = new EastingNorthing(
            easting: double.Parse(s: value.Easting),
            northing: double.Parse(s: value.Northing));
        
        var cartesian = GeoUK.Convert.ToCartesian(
            ellipsoid: new Airy1830(),
            projection: new BritishNationalGrid(),
            coordinates: eastingNorthing);
        
        var latitudeLongitude = GeoUK.Convert.ToLatitudeLongitude(
            ellipsoid: new Wgs84(),
            coordinates: Transform.Osgb36ToEtrs89(coordinates: cartesian));
        
        value.Latitude = latitudeLongitude.Latitude.ToString(provider: CultureInfo.InvariantCulture);
        value.Longitude = latitudeLongitude.Longitude.ToString(provider: CultureInfo.InvariantCulture);
        
        return await Task.FromResult(result: value);
    }
}
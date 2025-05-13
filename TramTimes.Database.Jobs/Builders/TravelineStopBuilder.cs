using System.Globalization;
using GeoUK;
using GeoUK.Coordinates;
using GeoUK.Ellipsoids;
using GeoUK.Projections;
using TramTimes.Database.Jobs.Models;

namespace TramTimes.Database.Jobs.Builders;

public static class TravelineStopBuilder
{
    public static async Task<TravelineStop> BuildAsync(
        Dictionary<string, NaptanLocality> localities,
        TransXChangeStopPoints? stopPoints,
        string? reference) {
        
        var stopPoint = stopPoints?.StopPoint?.FirstOrDefault(predicate: point => point.AtcoCode == reference);
        
        localities.TryGetValue(
            key: stopPoint?.Place?.NptgLocalityRef ?? "unknown",
            value: out var locality);
        
        if (locality is null)
        {
            return await Task.FromResult(result: new TravelineStop
            {
                AtcoCode = reference ?? "unknown"
            });
        }
        
        var value = new TravelineStop
        {
            AtcoCode = reference ?? "unknown",
            CommonName = stopPoint?.Descriptor?.CommonName,
            ShortCommonName = stopPoint?.Descriptor?.ShortCommonName,
            Landmark = stopPoint?.Descriptor?.Landmark,
            Street = stopPoint?.Descriptor?.Street,
            Crossing = stopPoint?.Descriptor?.Crossing,
            Indicator = stopPoint?.Descriptor?.Indicator,
            NptgLocalityCode = stopPoint?.Place?.NptgLocalityRef,
            LocalityName = locality.LocalityName,
            ParentLocalityName = locality.ParentLocalityName,
            GridType = locality.GridType,
            Easting = stopPoint?.Place?.Location?.Easting,
            Northing = stopPoint?.Place?.Location?.Northing,
            StopType = stopPoint?.StopClassification?.StopType,
            AdministrativeAreaCode = stopPoint?.AdministrativeAreaRef
        };
        
        if (value.Easting is null || value.Northing is null)
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
using System.Globalization;
using GeoUK;
using GeoUK.Coordinates;
using GeoUK.Ellipsoids;
using GeoUK.Projections;
using TramTimes.Database.Jobs.Models;

namespace TramTimes.Database.Jobs.Builders;

public static class TravelineStopBuilder
{
    public static TravelineStop Build(
        Dictionary<string, NaptanLocality> localities,
        TransXChangeStopPoints? stopPoints,
        string? reference) {
        
        #region build unknown
        
        var unknown = new TravelineStop
        {
            AtcoCode = reference ?? "unknown"
        };
        
        #endregion
        
        #region build result
        
        var stopPoint = stopPoints?.StopPoint?.FirstOrDefault(predicate: point => point.AtcoCode == reference);
        
        localities.TryGetValue(
            key: stopPoint?.Place?.NptgLocalityRef ?? "unknown",
            value: out var locality);
        
        if (locality is null)
            return unknown;
        
        var result = new TravelineStop
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
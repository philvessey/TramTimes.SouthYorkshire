using Sylvan.Data.Csv;
using TramTimes.Database.Jobs.Models;

namespace TramTimes.Database.Jobs.Tools;

public static class NaptanStopTools
{
    public static Dictionary<string, NaptanStop> GetFromFile(string path)
    {
        var results = new Dictionary<string, NaptanStop>();

        #region get reader

        using var streamReader = new StreamReader(path: path);
        using var dataReader = CsvDataReader.Create(reader: streamReader);

        #endregion

        #region get ordinals

        var atcoCodeOrdinal = dataReader.GetOrdinal(name: "ATCOCode");
        var naptanCodeOrdinal = dataReader.GetOrdinal(name: "NaptanCode");
        var plateCodeOrdinal = dataReader.GetOrdinal(name: "PlateCode");
        var cleardownCodeOrdinal = dataReader.GetOrdinal(name: "CleardownCode");
        var commonNameOrdinal = dataReader.GetOrdinal(name: "CommonName");
        var commonNameLangOrdinal = dataReader.GetOrdinal(name: "CommonNameLang");
        var shortCommonNameOrdinal = dataReader.GetOrdinal(name: "ShortCommonName");
        var shortCommonNameLangOrdinal = dataReader.GetOrdinal(name: "ShortCommonNameLang");
        var landmarkOrdinal = dataReader.GetOrdinal(name: "Landmark");
        var landmarkLangOrdinal = dataReader.GetOrdinal(name: "LandmarkLang");
        var streetOrdinal = dataReader.GetOrdinal(name: "Street");
        var streetLangOrdinal = dataReader.GetOrdinal(name: "StreetLang");
        var crossingOrdinal = dataReader.GetOrdinal(name: "Crossing");
        var crossingLangOrdinal = dataReader.GetOrdinal(name: "CrossingLang");
        var indicatorOrdinal = dataReader.GetOrdinal(name: "Indicator");
        var indicatorLangOrdinal = dataReader.GetOrdinal(name: "IndicatorLang");
        var bearingOrdinal = dataReader.GetOrdinal(name: "Bearing");
        var nptgLocalityCodeOrdinal = dataReader.GetOrdinal(name: "NptgLocalityCode");
        var localityNameOrdinal = dataReader.GetOrdinal(name: "LocalityName");
        var parentLocalityNameOrdinal = dataReader.GetOrdinal(name: "ParentLocalityName");
        var grandParentLocalityNameOrdinal = dataReader.GetOrdinal(name: "GrandParentLocalityName");
        var townOrdinal = dataReader.GetOrdinal(name: "Town");
        var townLangOrdinal = dataReader.GetOrdinal(name: "TownLang");
        var suburbOrdinal = dataReader.GetOrdinal(name: "Suburb");
        var suburbLangOrdinal = dataReader.GetOrdinal(name: "SuburbLang");
        var localityCentreOrdinal = dataReader.GetOrdinal(name: "LocalityCentre");
        var gridTypeOrdinal = dataReader.GetOrdinal(name: "GridType");
        var eastingOrdinal = dataReader.GetOrdinal(name: "Easting");
        var northingOrdinal = dataReader.GetOrdinal(name: "Northing");
        var longitudeOrdinal = dataReader.GetOrdinal(name: "Longitude");
        var latitudeOrdinal = dataReader.GetOrdinal(name: "Latitude");
        var stopTypeOrdinal = dataReader.GetOrdinal(name: "StopType");
        var busStopTypeOrdinal = dataReader.GetOrdinal(name: "BusStopType");
        var timingStatusOrdinal = dataReader.GetOrdinal(name: "TimingStatus");
        var defaultWaitTimeOrdinal = dataReader.GetOrdinal(name: "DefaultWaitTime");
        var notesOrdinal = dataReader.GetOrdinal(name: "Notes");
        var notesLangOrdinal = dataReader.GetOrdinal(name: "NotesLang");
        var administrativeAreaCodeOrdinal = dataReader.GetOrdinal(name: "AdministrativeAreaCode");

        #endregion

        #region build results

        while (dataReader.Read())
        {
            var key = !dataReader.IsDBNull(ordinal: atcoCodeOrdinal)
                ? dataReader.GetString(ordinal: atcoCodeOrdinal)
                : null;

            if (key is not null)
                results.TryAdd(
                    key: key,
                    value: new NaptanStop
                    {
                        AtcoCode = !dataReader.IsDBNull(ordinal: atcoCodeOrdinal)
                            ? dataReader.GetString(ordinal: atcoCodeOrdinal)
                            : null,

                        NaptanCode = !dataReader.IsDBNull(ordinal: naptanCodeOrdinal)
                            ? dataReader.GetString(ordinal: naptanCodeOrdinal)
                            : null,

                        PlateCode = !dataReader.IsDBNull(ordinal: plateCodeOrdinal)
                            ? dataReader.GetString(ordinal: plateCodeOrdinal)
                            : null,

                        CleardownCode = !dataReader.IsDBNull(ordinal: cleardownCodeOrdinal)
                            ? dataReader.GetString(ordinal: cleardownCodeOrdinal)
                            : null,

                        CommonName = !dataReader.IsDBNull(ordinal: commonNameOrdinal)
                            ? dataReader.GetString(ordinal: commonNameOrdinal)
                            : null,

                        CommonNameLang = !dataReader.IsDBNull(ordinal: commonNameLangOrdinal)
                            ? dataReader.GetString(ordinal: commonNameLangOrdinal)
                            : null,

                        ShortCommonName = !dataReader.IsDBNull(ordinal: shortCommonNameOrdinal)
                            ? dataReader.GetString(ordinal: shortCommonNameOrdinal)
                            : null,

                        ShortCommonNameLang = !dataReader.IsDBNull(ordinal: shortCommonNameLangOrdinal)
                            ? dataReader.GetString(ordinal: shortCommonNameLangOrdinal)
                            : null,

                        Landmark = !dataReader.IsDBNull(ordinal: landmarkOrdinal)
                            ? dataReader.GetString(ordinal: landmarkOrdinal)
                            : null,

                        LandmarkLang = !dataReader.IsDBNull(ordinal: landmarkLangOrdinal)
                            ? dataReader.GetString(ordinal: landmarkLangOrdinal)
                            : null,

                        Street = !dataReader.IsDBNull(ordinal: streetOrdinal)
                            ? dataReader.GetString(ordinal: streetOrdinal)
                            : null,

                        StreetLang = !dataReader.IsDBNull(ordinal: streetLangOrdinal)
                            ? dataReader.GetString(ordinal: streetLangOrdinal)
                            : null,

                        Crossing = !dataReader.IsDBNull(ordinal: crossingOrdinal)
                            ? dataReader.GetString(ordinal: crossingOrdinal)
                            : null,

                        CrossingLang = !dataReader.IsDBNull(ordinal: crossingLangOrdinal)
                            ? dataReader.GetString(ordinal: crossingLangOrdinal)
                            : null,

                        Indicator = !dataReader.IsDBNull(ordinal: indicatorOrdinal)
                            ? dataReader.GetString(ordinal: indicatorOrdinal)
                            : null,

                        IndicatorLang = !dataReader.IsDBNull(ordinal: indicatorLangOrdinal)
                            ? dataReader.GetString(ordinal: indicatorLangOrdinal)
                            : null,

                        Bearing = !dataReader.IsDBNull(ordinal: bearingOrdinal)
                            ? dataReader.GetString(ordinal: bearingOrdinal)
                            : null,

                        NptgLocalityCode = !dataReader.IsDBNull(ordinal: nptgLocalityCodeOrdinal)
                            ? dataReader.GetString(ordinal: nptgLocalityCodeOrdinal)
                            : null,

                        LocalityName = !dataReader.IsDBNull(ordinal: localityNameOrdinal)
                            ? dataReader.GetString(ordinal: localityNameOrdinal)
                            : null,

                        ParentLocalityName = !dataReader.IsDBNull(ordinal: parentLocalityNameOrdinal)
                            ? dataReader.GetString(ordinal: parentLocalityNameOrdinal)
                            : null,

                        GrandParentLocalityName = !dataReader.IsDBNull(ordinal: grandParentLocalityNameOrdinal)
                            ? dataReader.GetString(ordinal: grandParentLocalityNameOrdinal)
                            : null,

                        Town = !dataReader.IsDBNull(ordinal: townOrdinal)
                            ? dataReader.GetString(ordinal: townOrdinal)
                            : null,

                        TownLang = !dataReader.IsDBNull(ordinal: townLangOrdinal)
                            ? dataReader.GetString(ordinal: townLangOrdinal)
                            : null,

                        Suburb = !dataReader.IsDBNull(ordinal: suburbOrdinal)
                            ? dataReader.GetString(ordinal: suburbOrdinal)
                            : null,

                        SuburbLang = !dataReader.IsDBNull(ordinal: suburbLangOrdinal)
                            ? dataReader.GetString(ordinal: suburbLangOrdinal)
                            : null,

                        LocalityCentre = !dataReader.IsDBNull(ordinal: localityCentreOrdinal)
                            ? dataReader.GetString(ordinal: localityCentreOrdinal)
                            : null,

                        GridType = !dataReader.IsDBNull(ordinal: gridTypeOrdinal)
                            ? dataReader.GetString(ordinal: gridTypeOrdinal)
                            : null,

                        Easting = !dataReader.IsDBNull(ordinal: eastingOrdinal)
                            ? dataReader.GetString(ordinal: eastingOrdinal)
                            : null,

                        Northing = !dataReader.IsDBNull(ordinal: northingOrdinal)
                            ? dataReader.GetString(ordinal: northingOrdinal)
                            : null,

                        Longitude = !dataReader.IsDBNull(ordinal: longitudeOrdinal)
                            ? dataReader.GetString(ordinal: longitudeOrdinal)
                            : null,

                        Latitude = !dataReader.IsDBNull(ordinal: latitudeOrdinal)
                            ? dataReader.GetString(ordinal: latitudeOrdinal)
                            : null,

                        StopType = !dataReader.IsDBNull(ordinal: stopTypeOrdinal)
                            ? dataReader.GetString(ordinal: stopTypeOrdinal)
                            : null,

                        BusStopType = !dataReader.IsDBNull(ordinal: busStopTypeOrdinal)
                            ? dataReader.GetString(ordinal: busStopTypeOrdinal)
                            : null,

                        TimingStatus = !dataReader.IsDBNull(ordinal: timingStatusOrdinal)
                            ? dataReader.GetString(ordinal: timingStatusOrdinal)
                            : null,

                        DefaultWaitTime = !dataReader.IsDBNull(ordinal: defaultWaitTimeOrdinal)
                            ? dataReader.GetString(ordinal: defaultWaitTimeOrdinal)
                            : null,

                        Notes = !dataReader.IsDBNull(ordinal: notesOrdinal)
                            ? dataReader.GetString(ordinal: notesOrdinal)
                            : null,

                        NotesLang = !dataReader.IsDBNull(ordinal: notesLangOrdinal)
                            ? dataReader.GetString(ordinal: notesLangOrdinal)
                            : null,

                        AdministrativeAreaCode = !dataReader.IsDBNull(ordinal: administrativeAreaCodeOrdinal)
                            ? dataReader.GetString(ordinal: administrativeAreaCodeOrdinal)
                            : null
                    });
        }

        #endregion

        return results;
    }
}
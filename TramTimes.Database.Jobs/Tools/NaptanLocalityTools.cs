using Sylvan.Data.Csv;
using TramTimes.Database.Jobs.Models;

namespace TramTimes.Database.Jobs.Tools;

public static class NaptanLocalityTools
{
    public static Dictionary<string, NaptanLocality> GetFromFile(string path)
    {
        var results = new Dictionary<string, NaptanLocality>();

        #region get reader

        using var streamReader = new StreamReader(path: path);
        using var dataReader = CsvDataReader.Create(reader: streamReader);

        #endregion

        #region get ordinals

        var nptgLocalityCodeOrdinal = dataReader.GetOrdinal(name: "NptgLocalityCode");
        var localityNameOrdinal = dataReader.GetOrdinal(name: "LocalityName");
        var localityNameLangOrdinal = dataReader.GetOrdinal(name: "LocalityNameLang");
        var shortNameOrdinal = dataReader.GetOrdinal(name: "ShortName");
        var shortNameLangOrdinal = dataReader.GetOrdinal(name: "ShortNameLang");
        var qualifierNameOrdinal = dataReader.GetOrdinal(name: "QualifierName");
        var qualifierNameLangOrdinal = dataReader.GetOrdinal(name: "QualifierNameLang");
        var qualifierLocalityRefOrdinal = dataReader.GetOrdinal(name: "QualifierLocalityRef");
        var qualifierDistrictRefOrdinal = dataReader.GetOrdinal(name: "QualifierDistrictRef");
        var parentLocalityNameOrdinal = dataReader.GetOrdinal(name: "ParentLocalityName");
        var parentLocalityNameLangOrdinal = dataReader.GetOrdinal(name: "ParentLocalityNameLang");
        var administrativeAreaCodeOrdinal = dataReader.GetOrdinal(name: "AdministrativeAreaCode");
        var nptgDistrictCodeOrdinal = dataReader.GetOrdinal(name: "NptgDistrictCode");
        var sourceLocalityTypeOrdinal = dataReader.GetOrdinal(name: "SourceLocalityType");
        var gridTypeOrdinal = dataReader.GetOrdinal(name: "GridType");
        var eastingOrdinal = dataReader.GetOrdinal(name: "Easting");
        var northingOrdinal = dataReader.GetOrdinal(name: "Northing");

        #endregion

        #region build results

        while (dataReader.Read())
        {
            var key = !dataReader.IsDBNull(ordinal: nptgLocalityCodeOrdinal)
                ? dataReader.GetString(ordinal: nptgLocalityCodeOrdinal)
                : null;

            if (key is not null)
                results.TryAdd(
                    key: key,
                    value: new NaptanLocality
                    {
                        NptgLocalityCode = !dataReader.IsDBNull(ordinal: nptgLocalityCodeOrdinal)
                            ? dataReader.GetString(ordinal: nptgLocalityCodeOrdinal)
                            : null,

                        LocalityName = !dataReader.IsDBNull(ordinal: localityNameOrdinal)
                            ? dataReader.GetString(ordinal: localityNameOrdinal)
                            : null,

                        LocalityNameLang = !dataReader.IsDBNull(ordinal: localityNameLangOrdinal)
                            ? dataReader.GetString(ordinal: localityNameLangOrdinal)
                            : null,

                        ShortName = !dataReader.IsDBNull(ordinal: shortNameOrdinal)
                            ? dataReader.GetString(ordinal: shortNameOrdinal)
                            : null,

                        ShortNameLang = !dataReader.IsDBNull(ordinal: shortNameLangOrdinal)
                            ? dataReader.GetString(ordinal: shortNameLangOrdinal)
                            : null,

                        QualifierName = !dataReader.IsDBNull(ordinal: qualifierNameOrdinal)
                            ? dataReader.GetString(ordinal: qualifierNameOrdinal)
                            : null,

                        QualifierNameLang = !dataReader.IsDBNull(ordinal: qualifierNameLangOrdinal)
                            ? dataReader.GetString(ordinal: qualifierNameLangOrdinal)
                            : null,

                        QualifierLocalityRef = !dataReader.IsDBNull(ordinal: qualifierLocalityRefOrdinal)
                            ? dataReader.GetString(ordinal: qualifierLocalityRefOrdinal)
                            : null,

                        QualifierDistrictRef = !dataReader.IsDBNull(ordinal: qualifierDistrictRefOrdinal)
                            ? dataReader.GetString(ordinal: qualifierDistrictRefOrdinal)
                            : null,

                        ParentLocalityName = !dataReader.IsDBNull(ordinal: parentLocalityNameOrdinal)
                            ? dataReader.GetString(ordinal: parentLocalityNameOrdinal)
                            : null,

                        ParentLocalityNameLang = !dataReader.IsDBNull(ordinal: parentLocalityNameLangOrdinal)
                            ? dataReader.GetString(ordinal: parentLocalityNameLangOrdinal)
                            : null,

                        AdministrativeAreaCode = !dataReader.IsDBNull(ordinal: administrativeAreaCodeOrdinal)
                            ? dataReader.GetString(ordinal: administrativeAreaCodeOrdinal)
                            : null,

                        NptgDistrictCode = !dataReader.IsDBNull(ordinal: nptgDistrictCodeOrdinal)
                            ? dataReader.GetString(ordinal: nptgDistrictCodeOrdinal)
                            : null,

                        SourceLocalityType = !dataReader.IsDBNull(ordinal: sourceLocalityTypeOrdinal)
                            ? dataReader.GetString(ordinal: sourceLocalityTypeOrdinal)
                            : null,

                        GridType = !dataReader.IsDBNull(ordinal: gridTypeOrdinal)
                            ? dataReader.GetString(ordinal: gridTypeOrdinal)
                            : null,

                        Easting = !dataReader.IsDBNull(ordinal: eastingOrdinal)
                            ? dataReader.GetString(ordinal: eastingOrdinal)
                            : null,

                        Northing = !dataReader.IsDBNull(ordinal: northingOrdinal)
                            ? dataReader.GetString(ordinal: northingOrdinal)
                            : null
                    });
        }

        #endregion

        return results;
    }
}
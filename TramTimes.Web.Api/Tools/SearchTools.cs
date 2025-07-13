using Elastic.Clients.Elasticsearch;

namespace TramTimes.Web.Api.Tools;

public static class SearchTools
{
    public static List<SortOptions> SortByDistance(LatLonGeoLocation location)
    {
        #region build results
        
        var results = new List<SortOptions>
        {
            SortOptions.GeoDistance(geoDistanceSort: new GeoDistanceSort
            {
                Field = new Field(name: "location"),
                Order = SortOrder.Asc,
                Unit = DistanceUnit.Meters,
                Location = new List<GeoLocation>
                {
                    GeoLocation.LatitudeLongitude(latitudeLongitude: location)
                }
            })
        };
        
        #endregion
        
        return results;
    }
    
    public static List<SortOptions> SortByNameThenById()
    {
        #region build results
        
        var results = new List<SortOptions>
        {
            SortOptions.Field(field: new Field(name: "name")),
            SortOptions.Field(field: new Field(name: "id"))
        };
        
        #endregion
        
        return results;
    }
}
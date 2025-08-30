using Elastic.Clients.Elasticsearch;

namespace TramTimes.Web.Api.Tools;

public static class SearchTools
{
    public static List<SortOptions> SortByDistance(LatLonGeoLocation location)
    {
        #region build results
        
        var results = new List<SortOptions>
        {
            new()
            {
                GeoDistance = new GeoDistanceSort
                {
                    Field = "location",
                    Location = new List<GeoLocation>
                    {
                        GeoLocation.LatitudeLongitude(latitudeLongitude: location)
                    },
                    Order = SortOrder.Asc,
                    Unit = DistanceUnit.Meters
                }
            }
        };
        
        #endregion
        
        return results;
    }
    
    public static List<SortOptions> SortByNameThenById()
    {
        #region build results
        
        var results = new List<SortOptions>
        {
            new()
            {
                Field = new FieldSort
                {
                    Field = "name",
                    Order = SortOrder.Asc
                }
            },
            new()
            {
                Field = new FieldSort
                {
                    Field = "id",
                    Order = SortOrder.Asc
                }
            }
        };
        
        #endregion
        
        return results;
    }
}
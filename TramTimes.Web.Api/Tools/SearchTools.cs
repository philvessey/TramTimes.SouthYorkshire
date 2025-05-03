using Elastic.Clients.Elasticsearch;

namespace TramTimes.Web.Api.Tools;

public static class SearchTools
{
    public static List<SortOptions> SortDistance(LatLonGeoLocation location)
    {
        var options = new List<SortOptions>
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
        
        return options;
    }
}
namespace TramTimes.Web.Site.Defaults;

public static class TelerikMapDefaults
{
    public static string Attribution => "&copy; <a href='https://openstreetmap.org/copyright' target='_blank'>OpenStreetMap</a> contributors";
    public static double[] Center => [53.381129, -1.470085];
    public static double[] Extent => [53.381129 + ExtentOffset, -1.470085 - ExtentOffset, 53.381129 - ExtentOffset, -1.470085 + ExtentOffset];
    public static double ExtentOffset => 0.025;
    public static double MaxZoom => 18;
    public static double MinZoom => 14;
    public static string[] Subdomains => ["a", "b", "c"];
    public static string UrlTemplate => "https://#= subdomain #.tile.openstreetmap.org/#= zoom #/#= x #/#= y #.png";
    public static double Zoom => 16;
}
namespace TramTimes.Web.Site.Defaults;

public static class TelerikMediaQueryDefaults
{
    public static string MouseExtraSmall => "(min-width: 0px) and (max-width: 575.98px) and (pointer:fine)";
    public static string MouseSmall => "(min-width: 576px) and (max-width: 767.98px) and (pointer:fine)";
    public static string MouseMedium => "(min-width: 768px) and (max-width: 991.98px) and (pointer:fine)";
    public static string MouseLarge => "(min-width: 992px) and (max-width: 1199.98px) and (pointer:fine)";
    public static string MouseExtraLarge => "(min-width: 1200px) and (max-width: 1399.98px) and (pointer:fine)";
    public static string MouseExtraExtraLarge => "(min-width: 1400px) and (pointer:fine)";
    public static string TouchExtraSmall => "(min-width: 0px) and (max-width: 575.98px) and (pointer:coarse)";
    public static string TouchSmall => "(min-width: 576px) and (max-width: 767.98px) and (pointer:coarse)";
    public static string TouchMedium => "(min-width: 768px) and (max-width: 991.98px) and (pointer:coarse)";
    public static string TouchLarge => "(min-width: 992px) and (max-width: 1199.98px) and (pointer:coarse)";
    public static string TouchExtraLarge => "(min-width: 1200px) and (max-width: 1399.98px) and (pointer:coarse)";
    public static string TouchExtraExtraLarge => "(min-width: 1400px) and (pointer:coarse)";
}
using JetBrains.Annotations;

namespace TramTimes.Web.Site.Models;

public static class WindowSize
{
    [UsedImplicitly]
    public static bool? Tiny { get; set; }
    
    [UsedImplicitly]
    public static bool? Small { get; set; }
    
    [UsedImplicitly]
    public static bool? Medium { get; set; }
    
    [UsedImplicitly]
    public static bool? Large { get; set; }
    
    [UsedImplicitly]
    public static bool? Ultra { get; set; }
}
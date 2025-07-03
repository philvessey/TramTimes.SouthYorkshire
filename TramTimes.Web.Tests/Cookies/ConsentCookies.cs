using Microsoft.Playwright;

namespace TramTimes.Web.Tests.Cookies;

public static class ConsentCookies
{
    public static Cookie True => new()
    {
        Domain = "localhost",
        Name = ".AspNet.Consent",
        Path = "/",
        SameSite = SameSiteAttribute.Strict,
        Secure = true,
        Value = "true"
    };
    
    public static Cookie False => new()
    {
        Domain = "localhost",
        Name = ".AspNet.Consent",
        Path = "/",
        SameSite = SameSiteAttribute.Strict,
        Secure = true,
        Value = "false"
    };
}
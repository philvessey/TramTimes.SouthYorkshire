// ReSharper disable all

using System.Text.Json;
using Microsoft.Playwright;

namespace TramTimes.Web.Tests.Cookies;

public static class ConsentCookies
{
    private sealed record Metadata(string Timestamp, string Version)
    {
        public const string CurrentVersion = "2026-01";
    }

    public static List<Cookie> Unknown => [];

    public static List<Cookie> Accepted =>
    [
        new()
        {
            Domain = "localhost",
            Name = ".AspNet.Consent.Metadata",
            Path = "/",
            SameSite = SameSiteAttribute.Strict,
            Secure = true,
            Value = JsonSerializer.Serialize(value: new Metadata(
                Timestamp: DateTime.UtcNow.ToString(format: "yyyy-MM-dd"),
                Version: Metadata.CurrentVersion))
        },
        new()
        {
            Domain = "localhost",
            Name = ".AspNet.Consent",
            Path = "/",
            SameSite = SameSiteAttribute.Strict,
            Secure = true,
            Value = "true"
        }
    ];

    public static List<Cookie> Rejected =>
    [
        new()
        {
            Domain = "localhost",
            Name = ".AspNet.Consent.Metadata",
            Path = "/",
            SameSite = SameSiteAttribute.Strict,
            Secure = true,
            Value = JsonSerializer.Serialize(value: new Metadata(
                Timestamp: DateTime.UtcNow.ToString(format: "yyyy-MM-dd"),
                Version: Metadata.CurrentVersion))
        },
        new()
        {
            Domain = "localhost",
            Name = ".AspNet.Consent",
            Path = "/",
            SameSite = SameSiteAttribute.Strict,
            Secure = true,
            Value = "false"
        }
    ];
}
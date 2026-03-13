// ReSharper disable all

using System.Text.Json;
using Microsoft.Playwright;

namespace TramTimes.Web.Tests.Cookies;

public static class ConsentCookies
{
    private sealed record Metadata(string Timestamp, string Version)
    {
        public const string CurrentVersion = "2026-02";
    }

    public static List<Cookie> Unknown => [];

    public static List<Cookie> Accepted =>
    [
        new()
        {
            Domain = "localhost",
            Name = ".AspNet.Consent.Metadata",
            Value = JsonSerializer.Serialize(value: new Metadata(
                Timestamp: DateTime.UtcNow.ToString(format: "yyyy-MM-dd"),
                Version: Metadata.CurrentVersion)),
            Expires = DateTimeOffset.UtcNow
                .AddDays(days: 365)
                .ToUnixTimeSeconds(),
            Path = "/",
            SameSite = SameSiteAttribute.Strict,
            Secure = true
        },
        new()
        {
            Domain = "localhost",
            Name = ".AspNet.Consent",
            Value = "true",
            Expires = DateTimeOffset.UtcNow
                .AddDays(days: 365)
                .ToUnixTimeSeconds(),
            Path = "/",
            SameSite = SameSiteAttribute.Strict,
            Secure = true
        }
    ];

    public static List<Cookie> Rejected =>
    [
        new()
        {
            Domain = "localhost",
            Name = ".AspNet.Consent.Metadata",
            Value = JsonSerializer.Serialize(value: new Metadata(
                Timestamp: DateTime.UtcNow.ToString(format: "yyyy-MM-dd"),
                Version: Metadata.CurrentVersion)),
            Expires = DateTimeOffset.UtcNow
                .AddDays(days: 365)
                .ToUnixTimeSeconds(),
            Path = "/",
            SameSite = SameSiteAttribute.Strict,
            Secure = true
        },
        new()
        {
            Domain = "localhost",
            Name = ".AspNet.Consent",
            Value = "false",
            Expires = DateTimeOffset.UtcNow
                .AddDays(days: 365)
                .ToUnixTimeSeconds(),
            Path = "/",
            SameSite = SameSiteAttribute.Strict,
            Secure = true
        }
    ];
}
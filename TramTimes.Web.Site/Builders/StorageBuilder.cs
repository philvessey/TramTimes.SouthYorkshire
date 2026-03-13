namespace TramTimes.Web.Site.Builders;

public static class StorageBuilder
{
    public static string Build(
        string name,
        string value,
        DateTime? expires = null,
        string path = "/",
        string sameSite = "Strict",
        bool secure = false) {

        #region build result

        var result = $"{name}={value}";

        if (expires.HasValue)
            result += $"; expires={expires:R}";

        if (!string.IsNullOrWhiteSpace(value: path))
            result += $"; path={path}";

        if (!string.IsNullOrWhiteSpace(value: sameSite))
            result += $"; samesite={sameSite}";

        if (secure)
            result += "; secure";

        #endregion

        return result;
    }
}
namespace TramTimes.Web.Site.Services;

public class SchemeService
{
    public event Action? OnSchemeChanged;
    public string? Scheme { get; private set; }

    public void Set(string scheme)
    {
        #region Set value

        Scheme = scheme;

        #endregion

        OnSchemeChanged?.Invoke();
    }

    public string? Get()
    {
        #region Get value

        Scheme ??= "system";

        #endregion

        return Scheme;
    }
}
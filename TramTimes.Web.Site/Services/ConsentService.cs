using Microsoft.AspNetCore.Http.Features;

namespace TramTimes.Web.Site.Services;

public class ConsentService(IHttpContextAccessor accessor)
{
    public event Action? OnConsentChanged;
    public bool? Consent { get; private set; }

    public void Set(bool consent)
    {
        #region Set value

        Consent = consent;

        #endregion

        OnConsentChanged?.Invoke();
    }

    public bool? Get()
    {
        #region Get value

        Consent ??= accessor.HttpContext?.Features.Get<ITrackingConsentFeature>()?.CanTrack;

        #endregion

        return Consent;
    }
}
// ReSharper disable all

using System.Globalization;
using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.JSInterop;
using TramTimes.Web.Utilities.Extensions;

namespace TramTimes.Web.Site.Components.Shared;

public partial class LocalStorageConsent : ComponentBase, IAsyncDisposable
{
    private IJSObjectReference? Manager { get; set; }
    private string? ConsentCookie { get; set; }
    private bool ShowOutline { get; set; }
    private bool ShowPolicy { get; set; }
    private bool Disposed { get; set; }

    private sealed record Metadata(string Timestamp, string Version)
    {
        public const string CurrentVersion = "2026-01";
    }

    protected override void OnInitialized()
    {
        #region get feature

        var feature = AccessorService.HttpContext?.Features.Get<ITrackingConsentFeature>();

        #endregion

        #region create consent

        ConsentCookie = feature?.CreateConsentCookie();

        #endregion

        #region set toggles

        ShowOutline = false;
        ShowPolicy = false;

        #endregion
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        #region check disposed

        if (Disposed)
            return;

        #endregion

        #region create manager

        if (firstRender)
        {
            Manager = await JavascriptService.InvokeAsync<IJSObjectReference>(
                identifier: "import",
                args: "./Components/Shared/LocalStorageConsent.razor.js");

            var showPrompt = false;

            var consent = string.Empty;
            var metadata = string.Empty;

            if (!showPrompt)
                consent = await Manager.InvokeAsync<string>(
                    identifier: "getCookie",
                    args: ".AspNet.Consent");

            if (!showPrompt)
                if (string.IsNullOrEmpty(value: consent))
                    showPrompt = true;

            if (!showPrompt)
                metadata = await Manager.InvokeAsync<string>(
                    identifier: "getCookie",
                    args: ".AspNet.Consent.Metadata");

            if (!showPrompt)
                if (string.IsNullOrEmpty(value: metadata))
                    showPrompt = true;

            if (!showPrompt)
                if (!metadata.Contains(value: Metadata.CurrentVersion))
                    showPrompt = true;

            if (showPrompt)
                ShowPrivacyOutline();
        }

        #endregion
    }

    private void ShowPrivacyOutline()
    {
        #region set toggles

        ShowOutline = true;
        ShowPolicy = false;

        #endregion

        #region change state

        StateHasChanged();

        #endregion
    }

    private void ShowPrivacyPolicy()
    {
        #region set toggles

        ShowOutline = false;
        ShowPolicy = true;

        #endregion

        #region change state

        StateHasChanged();

        #endregion
    }

    private async Task AcceptPrivacyPolicyAsync()
    {
        #region check disposed

        if (Disposed)
            return;

        #endregion

        #region set cookies

        ConsentCookie = ConsentCookie?.Replace(
            oldValue: "false",
            newValue: "true");

        if (Manager is not null)
            await Manager.InvokeVoidAsync(
                identifier: "setCookie",
                args: [ConsentCookie, DateTime.UtcNow
                    .AddDays(value: 365)
                    .ToString(provider: CultureInfo.InvariantCulture)]);

        var metadata = JsonSerializer.Serialize(value: new Metadata(
            Timestamp: DateTime.UtcNow.ToString(format: "yyyy-MM-dd"),
            Version: Metadata.CurrentVersion));

        var cookie = NavigationService.Uri.StartsWithIgnoreCase(value: "https")
            ? $".AspNet.Consent.Metadata={metadata}; SameSite=Strict; Secure"
            : $".AspNet.Consent.Metadata={metadata}; SameSite=Strict";

        if (Manager is not null)
            await Manager.InvokeVoidAsync(
                identifier: "setCookie",
                args: [cookie, DateTime.UtcNow
                    .AddDays(value: 365)
                    .ToString(provider: CultureInfo.InvariantCulture)]);

        #endregion

        #region navigate page

        NavigationService.NavigateTo(
            uri: NavigationService.Uri,
            forceLoad: true,
            replace: true);

        #endregion
    }

    private async Task RejectPrivacyPolicyAsync()
    {
        #region check disposed

        if (Disposed)
            return;

        #endregion

        #region set cookies

        ConsentCookie = ConsentCookie?.Replace(
            oldValue: "true",
            newValue: "false");

        if (Manager is not null)
            await Manager.InvokeVoidAsync(
                identifier: "setCookie",
                args: [ConsentCookie, DateTime.UtcNow
                    .AddDays(value: 365)
                    .ToString(provider: CultureInfo.InvariantCulture)]);

        var metadata = JsonSerializer.Serialize(value: new Metadata(
            Timestamp: DateTime.UtcNow.ToString(format: "yyyy-MM-dd"),
            Version: Metadata.CurrentVersion));

        var cookie = NavigationService.Uri.StartsWithIgnoreCase(value: "https")
            ? $".AspNet.Consent.Metadata={metadata}; SameSite=Strict; Secure"
            : $".AspNet.Consent.Metadata={metadata}; SameSite=Strict";

        if (Manager is not null)
            await Manager.InvokeVoidAsync(
                identifier: "setCookie",
                args: [cookie, DateTime.UtcNow
                    .AddDays(value: 365)
                    .ToString(provider: CultureInfo.InvariantCulture)]);

        #endregion

        #region delete cookies

        if (Manager is not null)
            await Manager.InvokeVoidAsync(identifier: "deleteCookie");

        #endregion

        #region navigate page

        NavigationService.NavigateTo(
            uri: NavigationService.Uri,
            forceLoad: true,
            replace: true);

        #endregion
    }

    async ValueTask IAsyncDisposable.DisposeAsync()
    {
        #region set disposed

        Disposed = true;

        #endregion

        #region suppress finalizer

        GC.SuppressFinalize(obj: this);

        #endregion

        #region dispose manager

        if (Manager is not null)
            await Manager.DisposeAsync();

        #endregion
    }
}
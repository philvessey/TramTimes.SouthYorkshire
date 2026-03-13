// ReSharper disable all

using System.Globalization;
using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.JSInterop;
using TramTimes.Web.Site.Builders;
using TramTimes.Web.Site.Types;
using TramTimes.Web.Utilities.Extensions;

namespace TramTimes.Web.Site.Components.Shared;

public partial class LocalStorageConsent : ComponentBase, IAsyncDisposable
{
    private IJSObjectReference? Manager { get; set; }
    private ConsentType? ConsentState { get; set; }
    private bool Disposed { get; set; }

    private sealed record Metadata(string Timestamp, string Version)
    {
        public const string CurrentVersion = "2026-02";
    }

    protected override void OnParametersSet()
    {
        #region get state

        var consent = ConsentService.Get();

        #endregion

        #region set state

        ConsentState = consent switch
        {
            true => ConsentType.Hidden,
            false => ConsentType.Hidden,
            _ => ConsentType.Outline
        };

        #endregion
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        #region check disposed

        if (Disposed)
            return;

        #endregion

        #region import javascript

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
                if (!metadata.ContainsIgnoreCase(value: Metadata.CurrentVersion))
                    showPrompt = true;

            if (showPrompt)
                ShowPrivacyOutline();
        }

        #endregion
    }

    private void ShowPrivacyOutline()
    {
        #region set state

        ConsentState = ConsentType.Outline;

        #endregion

        #region change state

        StateHasChanged();

        #endregion
    }

    private void ShowPrivacyPolicy()
    {
        #region set state

        ConsentState = ConsentType.Policy;

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

        #region get state

        var consent = true;

        #endregion

        #region set cookie

        if (Manager is not null)
            await Manager.InvokeVoidAsync(
                identifier: "setCookie",
                args: StorageBuilder.Build(
                    name: ".AspNet.Consent",
                    value: "true",
                    expires: DateTime.UtcNow.AddDays(value: 365),
                    secure: NavigationService.Uri.StartsWithIgnoreCase(value: "https")));

        #endregion

        #region set cookie

        if (Manager is not null)
            await Manager.InvokeVoidAsync(
                identifier: "setCookie",
                args: StorageBuilder.Build(
                    name: ".AspNet.Consent.Metadata",
                    value: JsonSerializer.Serialize(value: new Metadata(
                        Timestamp: DateTime.UtcNow.ToString(format: "yyyy-MM-dd"),
                        Version: Metadata.CurrentVersion)),
                    expires: DateTime.UtcNow.AddDays(value: 365),
                    secure: NavigationService.Uri.StartsWithIgnoreCase(value: "https")));

        #endregion

        #region set state

        ConsentState = ConsentType.Hidden;

        #endregion

        #region change state

        StateHasChanged();

        #endregion

        #region set state

        ConsentService.Set(consent: consent);

        #endregion
    }

    private async Task RejectPrivacyPolicyAsync()
    {
        #region check disposed

        if (Disposed)
            return;

        #endregion

        #region delete cookie

        if (Manager is not null)
            await Manager.InvokeVoidAsync(identifier: "deleteCookie");

        #endregion

        #region delete storage

        await StorageService.DeleteAsync(key: "cache");
        await StorageService.DeleteAsync(key: "location");

        #endregion

        #region get state

        var consent = false;

        #endregion

        #region set cookie

        if (Manager is not null)
            await Manager.InvokeVoidAsync(
                identifier: "setCookie",
                args: StorageBuilder.Build(
                    name: ".AspNet.Consent",
                    value: "false",
                    expires: DateTime.UtcNow.AddDays(value: 365),
                    secure: NavigationService.Uri.StartsWithIgnoreCase(value: "https")));

        #endregion

        #region set cookie

        if (Manager is not null)
            await Manager.InvokeVoidAsync(
                identifier: "setCookie",
                args: StorageBuilder.Build(
                    name: ".AspNet.Consent.Metadata",
                    value: JsonSerializer.Serialize(value: new Metadata(
                        Timestamp: DateTime.UtcNow.ToString(format: "yyyy-MM-dd"),
                        Version: Metadata.CurrentVersion)),
                    expires: DateTime.UtcNow.AddDays(value: 365),
                    secure: NavigationService.Uri.StartsWithIgnoreCase(value: "https")));

        #endregion

        #region set state

        ConsentState = ConsentType.Hidden;

        #endregion

        #region change state

        StateHasChanged();

        #endregion

        #region set state

        ConsentService.Set(consent: consent);

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

        #region dispose javascript

        if (Manager is not null)
            await Manager.DisposeAsync();

        #endregion
    }
}
// noinspection all

let _timeout;

export function focusElement(element) {
    element?.focus();
}

export function registerBanner(_160x300, _160x600, _320x50, _468x60, _728x90, consent) {
    const innerWidth = window.innerWidth;
    const innerHeight = window.innerHeight;

    const keys = {
        _key160x300: _160x300,
        _key160x600: _160x600,
        _key320x50: _320x50,
        _key468x60: _468x60,
        _key728x90: _728x90,
        _consent: consent
    };

    Object.assign(globalThis, keys);
    if (Object.values(keys).some(key => !key)) return;

    let bannerTarget;
    let bannerKey;
    let bannerWidth;
    let bannerHeight;

    if (innerWidth >= 769) {
        if (innerHeight >= 600) {
            bannerTarget = document.querySelector("#privacy-banner-vertical ._160x600");
            bannerKey = _key160x600;
            bannerWidth = 160;
            bannerHeight = 600;
        } else {
            bannerTarget = document.querySelector("#privacy-banner-vertical ._160x300");
            bannerKey = _key160x300;
            bannerWidth = 160;
            bannerHeight = 300;
        }
    } else {
        if (innerWidth >= 728) {
            bannerTarget = document.querySelector("#privacy-banner-horizontal ._728x90");
            bannerKey = _key728x90;
            bannerWidth = 728;
            bannerHeight = 90;
        } else if (innerWidth >= 468) {
            bannerTarget = document.querySelector("#privacy-banner-horizontal ._468x60");
            bannerKey = _key468x60;
            bannerWidth = 468;
            bannerHeight = 60;
        } else {
            bannerTarget = document.querySelector("#privacy-banner-horizontal ._320x50");
            bannerKey = _key320x50;
            bannerWidth = 320;
            bannerHeight = 50;
        }
    }

    if (!bannerTarget || !bannerKey || !bannerWidth || !bannerHeight || !consent) return;

    const config = document.createElement("script");
    config.innerHTML = `
        atOptions = {
            "key" : "${bannerKey}",
            "format" : "iframe",
            "height" : ${bannerHeight},
            "width" : ${bannerWidth},
            "params" : {}
        };
    `;

    const invoke = document.createElement("script");
    invoke.src = `https://www.highperformanceformat.com/${bannerKey}/invoke.js`;

    bannerTarget.appendChild(config);
    bannerTarget.appendChild(invoke);
    bannerTarget.style.display = "block";
}

export function registerResize(helper) {
    const handleResize = () => {
        clearTimeout(_timeout);

        _timeout = setTimeout(() => {
            helper.invokeMethodAsync("OnScreenResizedAsync");

            if (!_key160x300 || !_key160x600 || !_key320x50 || !_key468x60 || !_key728x90 || !_consent) return;

            ["#privacy-banner-horizontal", "#privacy-banner-vertical"].forEach(selector => {
                ["_160x300", "_160x600", "_320x50", "_468x60", "_728x90"].forEach(className => {
                    const element = document.querySelector(`${selector} .${className}`);

                    if (element) {
                        element.innerHTML = "";
                        element.style.display = "none";
                    }
                });
            });

            registerBanner(_key160x300, _key160x600, _key320x50, _key468x60, _key728x90, _consent);
        }, 500);
    };

    window.addEventListener("resize", handleResize);
}

export function registerStorage(helper) {
    const handleStorage = (event) => {
        if (event.key === "theme") {
            helper.invokeMethodAsync("OnStorageChangedAsync");
        }
    };

    window.addEventListener("storage", handleStorage);
}

export function writeConsole(message) {
    console.log(message);
}
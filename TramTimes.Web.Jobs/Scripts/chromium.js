// noinspection all

Object.defineProperty(navigator, 'webdriver', {
    get: () => false,
});

Object.defineProperty(navigator, 'plugins', {
    get: () => [
        { name: "Chrome PDF Plugin" },
        { name: "Chrome PDF Viewer" }
    ]
});

Object.defineProperty(navigator, 'languages', {
    get: () => ['en-GB', 'en']
});

window.chrome = {
    app: { isInstalled: false },
    csi: () => {},
    loadTimes: () => {},
    runtime: {},
    webstore: {}
};

const originalQuery = navigator.permissions.query;

navigator.permissions.query = (parameters) => {
    if (parameters?.name === 'notifications') {
        return Promise.resolve({ state: 'prompt' });
    }

    return originalQuery(parameters);
};

const getParameter = WebGLRenderingContext.prototype.getParameter;

WebGLRenderingContext.prototype.getParameter = function (parameter) {
    if (parameter === 37445) return "Google Inc.";
    if (parameter === 37446) return "ANGLE (Google, Vulkan backend)";

    return getParameter.apply(this, [parameter]);
};

const iframeDescriptor = Object.getOwnPropertyDescriptor(
    HTMLIFrameElement.prototype,
    'contentWindow'
);

Object.defineProperty(HTMLIFrameElement.prototype, 'contentWindow', {
    get() {
        return iframeDescriptor.get.apply(this) || window;
    }
});

if (navigator.userAgentData) {
    try {
        Object.defineProperty(navigator.userAgentData, 'brands', {
            get: () => [
                { brand: "Chromium", version: "122" },
                { brand: "Google Chrome", version: "122" }
            ]
        });

        Object.defineProperty(navigator.userAgentData, 'mobile', {
            get: () => false
        });

        Object.defineProperty(navigator.userAgentData, 'platform', {
            get: () => "Linux"
        });
    } catch (e) {
        console.warn('Failed to set properties:', e);
    }
}
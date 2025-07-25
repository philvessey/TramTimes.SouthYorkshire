// noinspection JSUnusedGlobalSymbols

let resizeTimeout;

export function registerResize(dotNetHelper) {
    window.addEventListener('resize', () => {
        clearTimeout(resizeTimeout);
        
        resizeTimeout = setTimeout(() => {
            dotNetHelper.invokeMethodAsync('OnScreenResized');
        }, 500);
    });
}

export function writeConsole(message) {
    console.log(message);
}
// noinspection all

let resizeTimeout;

export function focusElement(element) {
    element?.focus();
}

export function registerResize(dotNetHelper) {
    window.addEventListener('resize', () => {
        clearTimeout(resizeTimeout);

        resizeTimeout = setTimeout(() => {
            dotNetHelper.invokeMethodAsync('OnScreenResizedAsync');
        }, 500);
    });
}

export function writeConsole(message) {
    console.log(message);
}
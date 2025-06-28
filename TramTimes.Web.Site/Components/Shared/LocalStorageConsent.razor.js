// noinspection JSUnusedGlobalSymbols

export function setCookie(cookieValue, expires) {
    document.cookie = cookieValue + "; expires=" + expires + "; path=/";
}
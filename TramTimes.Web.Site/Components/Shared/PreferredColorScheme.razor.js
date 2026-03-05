// noinspection all

export function setCookie(value, expires) {
    document.cookie = value + "; expires=" + expires + "; path=/";
}
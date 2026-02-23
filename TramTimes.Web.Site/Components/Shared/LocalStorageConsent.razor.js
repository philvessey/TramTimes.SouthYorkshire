// noinspection all

export function setCookie(value, expires) {
    document.cookie = value + "; expires=" + expires + "; path=/";
}

export function getCookie(value) {
    const cookie = decodeURIComponent(document.cookie);
    const cookies = cookie.split(";");

    for (let i = 0; i < cookies.length; i++) {
        let item = cookies[i].trim();

        if (item.startsWith(value + "=")) {
            return item.substring(value.length + 1);
        }
    }

    return null;
}

export function deleteCookie() {
    const cookie = decodeURIComponent(document.cookie);
    const cookies = cookie.split(";");

    for (let i = 0; i < cookies.length; i++) {
        let item = cookies[i].trim();
        let name = item.split("=")[0];

        if (!name.startsWith(".AspNet")) {
            document.cookie = name + "=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/";
        }
    }
}
// noinspection JSUnusedGlobalSymbols

export function getCookie(value) {
    const name = value + "=";
    
    const cookie = decodeURIComponent(document.cookie);
    const cookies = cookie.split(';');
    
    for (let i = 0; i < cookies.length; i++) {
        let cookie = cookies[i].trim();
        
        if (cookie.indexOf(name) === 0) {
            return cookie.substring(name.length, cookie.length);
        }
    }
    
    return null;
}

export function setCookie(value, expires) {
    document.cookie = value + "; expires=" + expires + "; path=/";
}
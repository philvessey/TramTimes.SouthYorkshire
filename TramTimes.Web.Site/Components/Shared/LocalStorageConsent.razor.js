// noinspection JSUnusedGlobalSymbols

export function getCookie(cookieName) {
    const name = cookieName + "=";
    
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

export function setCookie(cookieValue, expires) {
    document.cookie = cookieValue + "; expires=" + expires + "; path=/";
}
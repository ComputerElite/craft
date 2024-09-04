<script context="module">
export async function hashStringSHA256(input) {
    const encoder = new TextEncoder();
    const data = encoder.encode(input);
    const hashBuffer = await crypto.subtle.digest('SHA-256', data);
    const hashArray = Array.from(new Uint8Array(hashBuffer));
    const hashHex = hashArray.map(b => b.toString(16).padStart(2, '0')).join('');
    return hashHex;
}

export function saveSession(sessionId) {
    setCookie("session", sessionId)
}

export function setCookie(name, value) {
    // Set cookie to expire in 1 day
    let date = new Date(Date.now() + 86400e3);
    date = date.toUTCString();
    document.cookie = `${name}=${value};`
}

export function getCookie(name) {
    let cookies = document.cookie.split(';');
    let userCookie = cookies.find(cookie => cookie.startsWith(`${name}=`));
    if (userCookie) {
        let userInfo = userCookie.substring(name.length + 1); // remove 'user=' prefix
        return userInfo
    }
    return undefined
}

export function getRandomString() {
    const minLength = 4;
    const maxLength = 6;
    const characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
    let result = '';
    const charactersLength = characters.length;

    // Generate a random length between minLength and maxLength (inclusive)
    const length = Math.floor(Math.random() * (maxLength - minLength + 1)) + minLength;

    for (let i = 0; i < length; i++) {
        result += characters.charAt(Math.floor(Math.random() * charactersLength));
    }

    return result;
}
    
export function fetchJson(path, params) {
    if(!params) {
        params = {};
    }
    if(path.startsWith("/")) {
        path = path.substring(1);
    }
    path = "http://localhost:8383/" + path;
  return fetch(path, params).then(response => response.json());
}
</script>
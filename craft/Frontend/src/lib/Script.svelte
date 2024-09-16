<script context="module">
export async function hashStringSHA256(input) {
    const encoder = new TextEncoder();
    const data = encoder.encode(input);
    const hashBuffer = await crypto.subtle.digest('SHA-256', data);
    const hashArray = Array.from(new Uint8Array(hashBuffer));
    const hashHex = hashArray.map(b => b.toString(16).padStart(2, '0')).join('');
    return hashHex;
}

export function saveSession(sessionId, localStorage) {
    localStorage.session = sessionId;
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

export function openFileDownload(craftFile, localStorage) {

    let path = "http://localhost:8383" + `/api/v1/file?path=${craftFile.path}&session=${localStorage.session}`;
    window.open(path, '_blank');
}

export function GetNameOfFileFromPath(path) {
    if(path.endsWith("/")) {
        path = path.substring(0, path.length - 1);
    }
    return path.substring(path.lastIndexOf("/") + 1);
}

export function GetParentPath(path) {
    if(path.endsWith("/")) {
        path = path.substring(0, path.length - 1);
    }
    console.log(path)
    return path.substring(0, path.lastIndexOf("/")) + "/";
}

export function fetchJson(path, params, localStorage) {
    if(!params) {
        params = {};
    }
    if(!params["headers"]) {
        params["headers"] = {};
    }
    params["credentials"] = "include";
    if(localStorage) {
        if(localStorage.session) {
            console.log("session cookie is " + localStorage.getItem("session"))
            params["headers"]["Authorization"] = `Bearer ${localStorage.session}`;
        }
    }
    if(path.startsWith("/")) {
        path = path.substring(1);
    }
    path = "http://localhost:8383/" + path;
  return fetch(path, params).then(response => {
    if(!response.ok) {
        location = "/login?sessionExpired=true"
        return null;
    }
    return response.json()
  });
}
</script>
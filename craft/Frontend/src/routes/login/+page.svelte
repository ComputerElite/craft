<style>
    label {
        display: flex;
        align-items: center;
        justify-content: space-between;
    }
</style>
<CenteredBox>
    {#if state === "pwd"}
        <h1>Login</h1>
        <label for="username">Username<input bind:this={username} on:keydown={onKeyDownUsername} id="username" type="text" placeholder="Username" /></label><br>
        <label for="password">Password<input bind:this={password} on:keydown={onKeyDownPassword} id="password" type="password" placeholder="Password" /></label><br>
        <button on:click={startLogin}>Login</button>
        {#if loginError}
            <ErrorBox>{loginError}</ErrorBox>
        {/if}
    {:else if state == "totp"}
        <h1>Two-factor authentication</h1>
        <label for="totp">TOTP<input id="totp" type="text" placeholder="TOTP" /></label><br>
        <button>Submit</button>
        <ErrorBox>2 FA is not implemented yet</ErrorBox>
    {/if}
</CenteredBox>
<script>
    import '$lib/style.css'
    import CenteredBox from "$lib/CenteredBox.svelte";
    import {fetchJson, hashStringSHA256, getRandomString, saveSession} from '$lib/Script.svelte';
    import ErrorBox from "$lib/ErrorBox.svelte";
    import {goto} from "$app/navigation";
    import { page } from '$app/stores';
    import {onMount} from "svelte";

    function onKeyDownUsername(e) {
        if(e.key == "Enter")password.focus()
    }
    function onKeyDownPassword(e) {
        if(e.key == "Enter") startLogin()
    }
    
   
    onMount(() => {
        const sessionExpired = $page.url.searchParams.get('sessionExpired') == "true" || false;
        loginError = "Session expired. Please log in agian"
    });

    let state = "pwd"
    let username;
    let password;
    let loginError = "";
    let twoFAChallengeId = "";
    console.log(hashStringSHA256("test").then(res => {
        console.log(res)
    }))
    function startLogin() {
        fetchJson("/api/v1/user/start_login", {
            method: "POST",
            body: JSON.stringify({
                username: username.value
            })
        }).then((res) => {
            if(!res.success) {
                // user doesn't exist or something like that
                loginError = res.error;
                return;
            }
            console.log("server nonce is " + res.nonce)
            const clientNonce = getRandomString();
            console.log("client nonce is " + clientNonce)
            // salted password hash to server
            hashStringSHA256(password.value + res.nonce + clientNonce).then((passwordHash) => {
                fetchJson("/api/v1/user/login", {
                    method: "POST",
                    body: JSON.stringify({
                        username: username.value,
                        passwordHash: passwordHash,
                        cnonce: clientNonce,
                        challengeId: res.challengeId
                    })
                }).then((res) => {
                    if(!res.success) {
                        // some error occurred, display it
                        loginError = res.error;
                        return;
                    }
                    if (res.requires2fa) {
                        // userrequires 2fa, ask for TOTP token
                        twoFAChallengeId = res.challengeId
                        state = "totp"
                        return;
                    }
                    // user should be logged in, save session
                    saveSession(res.sessionId, localStorage)
                    goto("/home")
                })
            })
            
        })

    }
</script>
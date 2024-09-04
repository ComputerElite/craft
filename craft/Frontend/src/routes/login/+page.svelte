
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
        <label for="username">Username<input bind:this={username} id="username" type="text" placeholder="Username" /></label><br>
        <label for="password">Password<input bind:this={password} id="password" type="password" placeholder="Password" /></label><br>
        <button on:click={startLogin}>Login</button>
        {#if loginError}
            <ErrorBox>{loginError}</ErrorBox>
        {/if}
    {:else if state == "totp"}
        <h1>Two-factor authentication</h1>
        <label for="totp">TOTP<input id="totp" type="text" placeholder="TOTP" /></label><br>
        <button>Submit</button>
    {/if}
            
</CenteredBox>
<script>
    import '../../style.css'
    import CenteredBox from "$lib/CenteredBox.svelte";
    import {fetchJson, hashStringSHA256, getRandomString} from '$lib/Script.svelte';
    import ErrorBox from "$lib/ErrorBox.svelte";

    let state = "pwd"
    let username;
    let password;
    let loginError = "";
    let sessionToken = "";
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
                loginError = res.error;
                return;
            }
            console.log("server nonce is " + res.nonce)
            const clientNonce = getRandomString();
            console.log("client nonce is " + clientNonce)
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
                        loginError = res.error;
                        return;
                    }
                    sessionToken = res.session;
                    if (res.requires2fa) {
                        state = "totp"
                    }
                })
            })
            
        })

    }
</script>
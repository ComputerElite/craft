<div class="main_container">
    {#each files as file}
        <p>{file.path}</p>    
    {/each}
</div>
<script>
    import { page } from '$app/stores';
    import {fetchJson} from '$lib/Script.svelte';
    import {onMount} from "svelte";
    let currentPath = "";
    
    onMount(() => {
        const path = $page.url.searchParams.get('path') || '/';
        onPathChange(path);
    });
        
    
    let files = []
    
    function onPathChange(path) {
        //history.pushState({}, '', `?path=${path}`);
        
        // load current directory from api
        console.log("fetching files for path " + path)
        fetchJson(`/api/v1/list_dir?path=${path}`, {}, localStorage).then((res) => {
            files = res
        });
    }
</script>
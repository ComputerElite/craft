<script>
    import { page } from '$app/stores';
    import {fetchJson, openFileDownload} from '$lib/Script.svelte';
    import {onMount} from "svelte";
    import ListFile from "$lib/FileBrowser/ListFile.svelte";
    let currentPath = "";

    onMount(() => {
        const path = $page.url.searchParams.get('path') || '/';
        onPathChange(path);
    });

    let files = []
    let display = "list"

    function onFileClick(craftFile) {
        //history.pushState({}, '', `?path=${path}`);

        // load current directory from api
        if(craftFile.isDirectory) {
            onPathChange(craftFile.path);
            return;
        }
        openFileDownload(craftFile, localStorage);
    }
    
    function onPathChange(path) {
        console.log("fetching files for path " + path)
        fetchJson(`/api/v1/list_dir?path=${path}`, {}, localStorage).then((res) => {
            files = res
        });
    }
</script>

<div class="main_container">
    {#each files as file}
        
        <ListFile craftFile={file} onfileClick={onFileClick} />
    {/each}
</div>

<script>
    import {GetParentPath, GetNameOfFileFromPath} from "$lib/Script.svelte";
    
    export let path;
    export let onPathChange;
    $: path, updatePaths()
    
    let paths = []
    function updatePaths() {
        let cumulatedPath = ""
        paths = path.split("/").map((part) => {
            if(part.trim() === "") {
                return "/"
            }
            cumulatedPath += "/" + part
            return cumulatedPath + "/"
        }).filter(x => x !== "/")
        paths.forEach((p) => console.log(p))
    }
</script>
<style>
    div {
        display: inline;
    }
</style>
<div class="container">
{#each paths as path}
    <div on:click={onPathChange(GetParentPath(path))} style="padding-left: 5px;">&gt;</div>
    <div on:click={onPathChange(path)}>{GetNameOfFileFromPath(path)}</div>
{/each}
</div>
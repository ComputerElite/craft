<script>
    import MountPoint from "$lib/MountPoints/MountPoint.svelte";
    import {fetchJson} from "$lib/Script.svelte";
    import AutoComplete from 'simple-svelte-autocomplete';
    import "$lib/style.css"

    let mountpoints = []
    
    let paths = ["test", "test2", "/die/def", "/dev/dee"]
    let rootPath = ""
    let mountPoint = ""
    $: rootPath, updateSuggestions(rootPath)
    
    
    function updateSuggestions(path) {
        fetchJson(`/api/v1/admin/filesystem/list_dir?path=${path}`, {}, localStorage).then((res) => {
            paths = res
        });
    }
    
    function updateMountPoints() {
        fetchJson(`/api/v1/admin/mountpoints`, {}, localStorage).then((res) => {
            mountpoints = res
        });
    }
    updateMountPoints()
    
    function createMountPoint() {
        fetchJson(`/api/v1/admin/mountpoints`, {
            method: "POST",
            body: JSON.stringify({
                rootPath: rootPath,
                mountPoint: mountPoint
            })
        }, localStorage).then((res) => {
            mountpoints = res
        });
    }
</script>

<div class="main_container">
    <h2>Mountpoints</h2>
    {#each mountpoints as mountpoint}
        <MountPoint mountpoint={mountpoint}/>
    {:else}
        <h4>No Mountpoints exist atm or you do not have access to view them</h4>
    {/each}
    <h2>Create Mountpoints</h2>

    <table>
        <tr>
            <td>Root path</td>
            <AutoComplete items={paths} bind:text={rootPath} required={true} />
        </tr>
        <tr>
            <td>Mount point</td>
            <AutoComplete bind:text={mountPoint} required={true} />
        </tr>
    </table>
    <button on:click={createMountPoint}>Create Mountpoint</button>
</div>

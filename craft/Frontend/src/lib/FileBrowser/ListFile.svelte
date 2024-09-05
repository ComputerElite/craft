<style>
    .listFile {
        margin: 2px;
        padding: 5px;
        background-color: #FFF2;
        border-radius: 10px;
        align-items: center;
    }
</style>
<div class="flex listFile" on:click={handleClick}>
    {#if craftFile.isDirectory}
        <Icon src={folderIcon} />
    {/if}
    <div>{craftFile.name}</div>
</div>
<script>
    import folderIcon from "$lib/icons/folder2-open.svg"
    import Icon from "$lib/Icon.svelte";
    export let craftFile;
    export let onfileClick;
    function handleClick() {
        onfileClick(craftFile);
    }
</script>
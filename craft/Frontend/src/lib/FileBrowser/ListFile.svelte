<style>
    .listFile {
        margin: 2px;
        padding: 5px;
        background-color: #FFF2;
        border-radius: 10px;
        align-items: center;
    }
</style>
<div class="flex listFile" on:click={handleDoubleClick} ondblclick={handleDoubleClick()}>
    <CraftFileIcon craftFile={craftFile}></CraftFileIcon>
    <div>{craftFile.name}</div>
</div>
<script>
    import folderIcon from "$lib/icons/folder2-open.svg"
    import Icon from "$lib/Icon.svelte";
    import CraftFileIcon from "$lib/FileBrowser/CraftFileIcon.svelte";
    export let craftFile;
    export let onfileClick;
    function handleClick() {
        console.log("lol")
    }
    
    function handleDoubleClick() {
        onfileClick(craftFile);
    }
</script>
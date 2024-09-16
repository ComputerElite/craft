<script>
    import NavbarVertical from "$lib/Navbar/NavbarVertical.svelte";
    import FileBrowser from "$lib/FileBrowser/FileBrowser.svelte";
    import MountPointsEditor from "$lib/MountPoints/MountPointsEditor.svelte";
    
    let currentSection = "files"
    
    function updateCurrentSection(newSection) {
        currentSection = newSection
    }
</script>
<div class="root_container">
    <NavbarVertical currentSection={currentSection} updateSectionFunction={updateCurrentSection}></NavbarVertical>
    {#if currentSection == "files"}
        <FileBrowser>
            
        </FileBrowser>
    {:else if currentSection == "mountpoints"}
        <MountPointsEditor/>
    {/if}
</div>
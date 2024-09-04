<script>
    import NavbarVertical from "$lib/Navbar/NavbarVertical.svelte";
    import FileBrowser from "$lib/FileBrowser/FileBrowser.svelte";
    
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
    {/if}
</div>
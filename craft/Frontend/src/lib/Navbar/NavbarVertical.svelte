<style>
    .navbar {
        height: 100%;
        background-color: #FFF0;
        margin: var(--content-padding);
        border-radius: 20px;
    }
</style>
<div class="flex vertical navbar">
    <NavBarElement icon={folderOpen} currentSection={currentSection} section="files" updateSection={updateSectionFunction}>Files</NavBarElement>
    <NavBarElement icon={bg} currentSection={currentSection} section="ipsum" updateSection={updateSectionFunction}>ipsum</NavBarElement>
    <NavBarElement icon={bg} currentSection={currentSection} section="dolor" updateSection={updateSectionFunction}>dolor</NavBarElement>
    <NavBarElement icon={bg} currentSection={currentSection} section="sit" updateSection={updateSectionFunction}>sit</NavBarElement>

</div>
<script>
    import bg from "$lib/images/bg.png"
    import folderOpen from "$lib/icons/folder2-open.svg"
    import '$lib/style.css'
    import NavBarElement from "$lib/Navbar/NavBarElement.svelte";
    
    export let currentSection
    export let updateSectionFunction
</script>
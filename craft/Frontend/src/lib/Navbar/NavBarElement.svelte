<style>
    img {
        height: 100%;
        width: auto;
        margin-right: 20px;
        user-select: none;
    }
    
    .navbar_element {
        padding: 20px;
        animation: .5s ease-in-out;
        height: 1.5em;
        display: flex;
        align-items: center;
        border-radius: 10px;
        margin-bottom: 10px;
    }
    
    .highlighted {
        background-color: var(--accent-color) !important;
    }
    
    .navbar_element:hover {
        background-color: var(--accent-color-hover);
    }
</style>
<div class={"flex navbar_element animatebg" + (highlighted ? " highlighted": "")} on:click={updateSection(section)}>
    <img src={icon} >
    <slot>
        
    </slot>
</div>
<script>
    import "$lib/style.css"
    export let icon;
    export let currentSection;
    export let section;
    export let updateSection;
    $: highlighted = section === currentSection;
</script>
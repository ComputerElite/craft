<script>
    import folderIcon from "$lib/icons/folder2-open.svg";
    import imageIcon from "$lib/icons/image.svg"
    import audioIcon from "$lib/icons/music-note-beamed.svg"
    import videoIcon from "$lib/icons/film.svg"
    import diskImageIcon from "$lib/icons/device-hdd.svg"
    import textFileIcon from "$lib/icons/text-left.svg"
    import archiveIcon from "$lib/icons/file-earmark-zip.svg"
    import documentIcon from "$lib/icons/file-earmark-pdf.svg"
    import websiteIcon from "$lib/icons/cloud.svg"
    import markdownIcon from "$lib/icons/filetype-md.svg"
    import Icon from "$lib/Icon.svelte";

    export let craftFile;
</script>
{#if craftFile.fileType =="directory"}
    <Icon src={folderIcon} />
{:else if craftFile.fileType == "image"}
    <Icon src={imageIcon}/>
{:else if craftFile.fileType == "audio"}
    <Icon src={audioIcon}/>
{:else if craftFile.fileType == "video"}
    <Icon src={videoIcon}/>
{:else if craftFile.fileType == "diskImage"}
    <Icon src={diskImageIcon}/>
{:else if craftFile.fileType == "textFile"}
    <Icon src={textFileIcon}/>
{:else if craftFile.fileType == "archive"}
    <Icon src={archiveIcon}/>
{:else if craftFile.fileType == "document"}
    <Icon src={documentIcon}/>
{:else if craftFile.fileType == "website"}
    <Icon src={websiteIcon}/>
{:else if craftFile.fileType == "markdown"}
    <Icon src={markdownIcon}/>
{/if}
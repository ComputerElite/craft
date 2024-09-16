<script>
    export let mountpoint;
    import "$lib/style.css"
</script>
<div class="content_bg">
    <table>
        <tr>
            <td>Root path</td>
            <td>{mountpoint.rootPath}</td>
        </tr>
        <tr>
            <td>Mount point</td>
            <td>{mountpoint.mountPoint}</td>
        </tr>
    </table>
</div>
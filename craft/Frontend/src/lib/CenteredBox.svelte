<style>
    .centered_box_wrapper {
        display: flex;
        justify-content: center;
        align-items: center;
        height: 100%;
        width: 100%;
    }
    .centered_box {
        justify-content: center;
        align-items: center;
        padding: 10px;
        border-radius: 20px;
        background-color: #000A;
    }
</style>
<div class="centered_box_wrapper">
    <div class="centered_box">
        <slot>No content provided</slot>
    </div>
</div>
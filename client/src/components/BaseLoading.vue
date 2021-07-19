<template>
    <div>
        <div class="hiden-load"></div>
        <div class="load">
            <div class="load__gif"></div>
            <p class="load__text">{{this.message}}</p>
        </div>
    </div>
</template>

<script>
    export default {
        props: {
            message: {
                type: String,
                default: '...'
            },
        },
    }
</script>

<style scoped>
@import '../css/components/baseloading.css';
</style>
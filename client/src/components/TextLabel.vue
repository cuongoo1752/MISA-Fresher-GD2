<template>
    <div class="textlabel">
        <p class="textlable__text"><slot/></p>
        <div v-if="hasInformation" class="textlable__icon" :title="title"></div>
    </div>
</template>

<script>
    export default {
        props: {
            hasInformation: {
                type: Boolean,
                default: false
            },
            title:{
                type: String,
                default: ''
            }
        },
    }
</script>

<style scoped>
@import '../css/components/textlabel.css';
</style>
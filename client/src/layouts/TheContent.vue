<template>
    <div class="content">
        <pagination/>
    </div>
</template>

<script>
import pagination from '../components/Pagination.vue'
    export default {
        components:{
            pagination,
        }
    }
</script>

<style scoped>
.content{
    padding: 50px;
}
</style>
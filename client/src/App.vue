<template>
  <div id="app">
    <the-header/>
    <the-content/>
    <the-navbar/>
  </div>
</template>

<script>
import TheContent from './layouts/TheContent.vue';
import TheHeader from './layouts/TheHeader.vue';
import TheNavbar from './layouts/TheNavbar.vue';
export default {
  name: 'App',
  components: {
    TheContent, TheHeader, TheNavbar
  }
}
</script>

<style>
@import './css/common.css';

</style>

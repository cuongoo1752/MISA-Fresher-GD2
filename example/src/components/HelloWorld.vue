<template>
	<div class="box-msg">
		<button @click="isShow = !isShow">Click me!</button>
		<div>
			<transition name="fade">
				<div v-if="isShow">
					<h1>Lorem ipsum, dolor sit amet consectetur adipisicing elit. Maiores ea minima vitae a, consectetur magni? Beatae animi eius expedita vitae nam error reiciendis, dolores, culpa esse tempora laborum quasi maiores.</h1>
				</div>
				<slot-component>hihihihihih</slot-component>
				
			</transition>
		</div>
    </div>
	
</template>

<script>
import SlotComponent from './SlotComponent.vue';


export default {
	components: { SlotComponent },
	name: "HelloWorld",
  data() {
    return {
      isShow : false
    }
  },
	methods: {
	},
};
</script>

<style scoped>
.box-msg {
	padding: 30px;
	background-color: aqua;
}

.msg {
	padding: 10px;
}
.fade-enter-active, .fade-leave-active {
  transition: opacity 1s;
}
.fade-enter, .fade-leave-to /* .fade-leave-active below version 2.1.8 */ {
  opacity: 0;
}
</style>

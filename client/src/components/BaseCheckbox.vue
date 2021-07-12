<template>
	<div class="base-checkbox" @click="handleClick">
		<input type="checkbox"
			class="base-checkbox__element"
			:class="{ 'checkbox-true': value, 'checkbox-false': !value }"
            
		/>
	</div>
</template>

<script>
export default {
	props: {
		value: {
			type: Boolean,
			default: false,
		},
	},
    methods: {
        handleClick() {
            this.$emit('input:update', !this.value);
            console.log(this.value)
        }
    },
};
</script>

<style scoped>
@import "../css/components/basecheckbox.css";
</style>
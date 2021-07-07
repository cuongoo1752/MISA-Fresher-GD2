<template>
	<div class="pagination">
        <div class="pagination__group-button">
		<!-- Button -->
		<button
			class="pagination__start button-small"
			:style="{
				backgroundPosition: active.isStart
					? backgroundPositionActive.start
					: backgroundPositionNotActive.start,
			}"
			:class="{
				'button-active': active.isStart,
				'button-not-active': !active.isStart,
			}"
		></button>
		<button
			class="pagination__prev button-small"
			:style="{
				backgroundPosition: active.isPrev
					? backgroundPositionActive.prev
					: backgroundPositionNotActive.prev,
			}"
            :class="{
				'button-active': active.isPrev,
				'button-not-active': !active.isPrev,
			}"
		></button>
		<!-- Text -->
		<p class="pagination__page-text">Trang</p>
		<!-- Input -->
		<input class="pagination__input" type="number" />
		<!-- Text -->
		<p class="pagination__length-page">trên 1</p>
		<!-- Button -->
		<button
			class="pagination__next button-small"
			:style="{
				backgroundPosition: active.isNext
					? backgroundPositionActive.next
					: backgroundPositionNotActive.next,
			}"
            :class="{
				'button-active': active.isNext,
				'button-not-active': !active.isNext,
			}"
		></button>
		<button
			class="pagination__end button-small"
			:style="{
				backgroundPosition: active.isEnd
					? backgroundPositionActive.end
					: backgroundPositionNotActive.end,
			}"
            :class="{
				'button-active': active.isEnd,
				'button-not-active': !active.isEnd,
			}"
		></button>
		<button class="pagination__refresh"></button>
		<!-- Select -->
		<select class="pagination__page-size" name="" id="">
			<option value="15">15</option>
			<option value="25">50</option>
			<option value="50" selected>50</option>
			<option value="100">100</option>
		</select>
        </div>
		<!-- Text -->
		<p class="pagination__end-text">Hiển thị 1 - 15 trên 33 kết quả</p>
	</div>
</template>

<script>
export default {
    props: {

    },
	data() {
		return {
			active: {
				isStart: false,
				isPrev: false,
				isNext: false,
				isEnd: false,
			},
			backgroundPositionActive: {
				start: "-650px -125px",
				prev: "-700px -125px",
				next: "-749px -125px",
				end: "-798px -125px",
			},
			backgroundPositionNotActive: {
				start: "-650px -150px",
				prev: "-700px -150px",
				next: "-749px -150px",
				end: "-798px -150px",
			},
		};
	},
};
</script>

<style scoped>
@import "../css/components/pagination.css";
</style>
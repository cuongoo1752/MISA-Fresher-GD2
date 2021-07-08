<template>
	<div class="pagination">
		<div class="pagination__group-button">
			<!-- Button -->
			<button
				class="pagination__start button-small"
				@click="startPage()"
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
				@click="prevPage()"
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
			<input
				class="pagination__input"
				type="text"
				v-model="temptIndex"
				@keypress="changeTextInput($event)"
				@keyup.enter="handleTextInput()"
			/>
			<!-- Text -->
			<p class="pagination__length-page">trên {{ lengthPage }}</p>
			<!-- Button -->
			<button
				class="pagination__next button-small"
				@click="nextPage()"
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
				@click="endPage()"
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
			<button class="pagination__refresh" @click="refreshPage()"></button>
			<!-- Select -->
			<select
				class="pagination__page-size"
				v-model="pageSize"
				name=""
				id=""
				@change="changeSelectBox"
			>
				<option value="15">15</option>
				<option value="25">25</option>
				<option value="50" selected>50</option>
				<option value="100">100</option>
			</select>
		</div>
		<!-- Text -->
		<p class="pagination__end-text">
			Hiển thị {{ startRow }} - {{ endRow }} trên {{ lengRow }} kết quả
		</p>
	</div>
</template>

<script>
export default {
	watch: {
		lengthPage() {
			this.changeStyteButton();
		},
		currIndex() {
			this.pageIndex = this.currIndex;
			this.changeStyteButton();
		},
	},
	props: {
		lengthPage: {
			type: Number,
			default: 10,
		},
		currIndex: {
			type: Number,
			default: 1,
		},
		startRow: {
			type: Number,
			default: 1,
		},
		endRow: {
			type: Number,
			default: 10,
		},
		lengRow: {
			type: Number,
			default: 20,
		},
	},
	data() {
		return {
			// State style active của các nút start, prev, next, end
			active: {
				isStart: false,
				isPrev: false,
				isNext: false,
				isEnd: false,
			},
			// Style active của các nút start, prev, next, end
			backgroundPositionActive: {
				start: "-650px -125px",
				prev: "-700px -125px",
				next: "-749px -125px",
				end: "-798px -125px",
			},
			// Style not active của các nút start, prev, next, end
			backgroundPositionNotActive: {
				start: "-650px -150px",
				prev: "-700px -150px",
				next: "-749px -150px",
				end: "-798px -150px",
			},
			// Thứ tự trang
			pageIndex: 1,
			temptIndex: 1,
			pageSize: 50,
			refresh: false,
		};
	},
	methods: {
		/**
		 * Trở về trang page đầu
		 * Created By: LMCUONG(08/07/2021)
		 */
		startPage() {
			if (this.pageIndex != 1) {
				this.pageIndex = 1;
				this.changeStyteButton();
				this.returnParentValue();
			}
		},
		/**
		 * Đi đến trang page cuối cùng
		 * Created By: LMCUONG(08/07/2021)
		 */
		endPage() {
			if (this.pageIndex != this.lengthPage) {
				this.pageIndex = this.lengthPage;
				this.changeStyteButton();
				this.returnParentValue();
			}
		},
		prevPage() {
			if (this.pageIndex > 1) {
				this.pageIndex--;
				this.changeStyteButton();
				this.returnParentValue();
			}
		},
		nextPage() {
			if (this.pageIndex < this.lengthPage) {
				this.pageIndex++;
				this.changeStyteButton();
				this.returnParentValue();
			}
		},
		/**
		 * Thay đổi style hiển thị theo giá trị pageIndex
		 * Created By: LMCUONG(08/07/2021)
		 */
		changeStyteButton() {
			this.temptIndex = this.pageIndex;
			if (this.pageIndex == 1) {
				this.active.isStart = false;
				this.active.isPrev = false;
			} else {
				this.active.isStart = true;
				this.active.isPrev = true;
			}

			if (this.pageIndex == this.lengthPage) {
				this.active.isEnd = false;
				this.active.isNext = false;
			} else {
				this.active.isEnd = true;
				this.active.isNext = true;
			}
		},
		changeTextInput(evt) {
			evt = evt ? evt : window.event;
			var charCode = evt.which ? evt.which : evt.keyCode;
			if (
				charCode > 31 &&
				(charCode < 48 || charCode > 57) &&
				charCode !== 46
			) {
				evt.preventDefault();
			} else {
				return true;
			}
		},
		handleTextInput() {
			if (this.temptIndex <= this.pageIndex && this.temptIndex >= 1) {
				this.pageIndex = this.temptIndex;
				this.changeStyteButton();
				this.returnParentValue();
			} else {
				this.temptIndex = this.lengthPage;
				this.pageIndex = this.temptIndex;
				this.returnParentValue();
			}
		},
		returnParentValue() {
			this.refresh = false;
			this.$emit("change", this.pageIndex, this.pageSize, this.refresh);
		},
		refreshPage() {
			this.refresh = true;
			this.$emit("change", this.pageIndex, this.pageSize, this.refresh);
		},
		changeSelectBox() {
			this.returnParentValue();
		},
	},
};
</script>

<style scoped>
@import "../css/components/pagination.css";
</style>
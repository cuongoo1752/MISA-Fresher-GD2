<template>
	<div class="headtb" :style="{ width: width }" tabindex="0">
		<div class="headtb__title" @click="handleClickTitle">
			<p class="headtb__text">{{ title }}</p>
			<div
				class="headtb__sort"
				:class="{
					'arrow-up': stateSort == 'desc',
					'arrow-down': stateSort == 'asc',
				}"
			></div>
		</div>
		<div v-if="type == 'string'" class="headtb__input string">
			<input
				@click="isShowSelect = !isShowSelect"
				type="button"
				:value="valueDisplay"
				class="headtb__btn"
			/>
			<input
				v-model="valueFieldInput"
				@blur="returnValueParent"
				type="text"
				class="headtb__input-text"
			/>
			<ul @blur="isShowSelect = false" v-if="isShowSelect" class="list">
				<li class="element" @click="handleClick('*', 1)">*:Chứa</li>
				<li class="element" @click="handleClick('=', 2)">=:Bằng</li>
				<li class="element" @click="handleClick('+', 3)">
					+:Bắt đầu bằng
				</li>
				<li class="element" @click="handleClick('-', 4)">
					-:Kết thúc bằng
				</li>
				<li class="element" @click="handleClick('!', 5)">
					!:Không chứa
				</li>
			</ul>
		</div>
		<div v-else-if="type == 'number'" class="headtb__input number">
			<input
				@click="isShowSelect = !isShowSelect"
				type="button"
				:value="valueDisplay"
				class="headtb__btn"
			/>
			<input
				v-model="valueFieldInput"
				@blur="returnValueParent"
				type="text"
				class="headtb__input-text"
			/>
			<ul
				@blur="isShowSelect = false"
				v-if="isShowSelect"
				class="list number-list"
			>
				<li class="element" @click="handleClick('=', 1)">= :Bằng</li>
				<!-- eslint-disable-next-line vue/no-parsing-error -->
				<li class="element" @click="handleClick('<', 2)">< :Nhỏ hơn</li>
				<!-- eslint-disable-next-line vue/no-parsing-error -->
				<li class="element" @click="handleClick('<=', 3)">
					<!-- eslint-disable-next-line vue/no-parsing-error -->
					<=:Nhỏ hơn hoặc bằng
				</li>
				<li class="element" @click="handleClick('>', 4)">> :Lớn hơn</li>
				<li class="element" @click="handleClick('>=', 5)">
					>=:Lớn hơn hoặc bằng
				</li>
			</ul>
		</div>
		<div v-else-if="type == 'select'" class="headtb__input select">
			<select
				name=""
				id=""
				v-model="valueSelect"
				@change="returnValueParent"
				class="headtb__select"
			>
				<option
					v-for="(opt, index) in selectArray"
					:key="index"
					:value="opt.key"
					class="select-element"
				>
					{{opt.value}}
				</option>
			</select>
		</div>
	</div>
</template>

<script>
export default {
	props: {
		// Các loại column table
		// string - chuổi
		// number - số
		// select - chọn các kiểu
		type: {
			type: String,
			default: "number",
		},
		// Chiều rộng của trường input
		width: {
			type: String,
			default: "200px",
		},
		// id của trường input
		id: {
			type: String,
			default: "",
		},
		// Mảng hiển thị với type là input
		selectArray: {
			type: Array,
		},
		title: {
			type: String,
			default: "Ma SKU",
		},
	},
	created() {
		if (this.type == "select") {
			this.valueSelect = this.selectArray[0];
		} else if (this.type == "number") {
			this.valueDisplay = "=";
		} else if (this.type == "string") {
			this.valueDisplay = "*";
		}
	},
	data() {
		return {
			//Sort có 3 trạng thái
			// '' ẩn
			// 'desc'
			// 'asc'
			stateSort: "",
			isShowSelect: false,
			valueSelect: {},
			stateInput: 0,
			valueFieldInput: "",
			valueDisplay: "",
		};
	},
	methods: {
		handleClickTitle() {
			if (this.stateSort == "") {
				this.stateSort = "desc";
			} else if (this.stateSort == "desc") {
				this.stateSort = "asc";
			} else if (this.stateSort == "asc") {
				this.stateSort = "";
			}
			this.returnValueParent();
		},
		handleClick(display, value) {
			this.valueDisplay = display;
			this.stateInput = value;
			this.isShowSelect = false;
			this.returnValueParent();
		},
		returnValueParent() {
			if (this.type == "select") {
				this.$emit("change", this.id, this.stateSort, this.valueSelect);
			} else {
				this.$emit(
					"change",
					this.id,
					this.stateSort,
					this.stateInput,
					this.valueFieldInput
				);
			}
		},
	},
};
</script>

<style scoped>
@import "../css/components/columnheadertable.css";
</style>
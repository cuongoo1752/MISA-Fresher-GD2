<template>
	<div class="groupbtn">
		<div class=" groupbtn__btndir" @click="handleClickButton(1, 1)">
			<div class="add-icon groupbtn__icon"></div>
			<p class="groupbtn__text">Thêm mới</p>
			<div class="add-select" @click.stop="isShowAddSelelct = !isShowAddSelelct"></div>
			<ul class="add-list" v-if="isShowAddSelelct" tabindex="0" @blur="isShowAddSelelct = false">
				<li class="add-element"  @click.stop="handleClickButton(1, 1)">Thêm mới hàng hóa</li>
				<li class="add-element" @click.stop="handleClickButton(1, 2)">Thêm mới Combo</li>
				<li class="add-element" @click.stop="handleClickButton(1, 3)">Thêm mới dịch vụ</li>
			</ul>
		</div>
		<div class=" groupbtn__btndir" @click="handleClickButton(2, 0)">
			<div class="dup-icon groupbtn__icon"></div>
			<p class="groupbtn__text">Nhân bản</p>
		</div>
		<div class=" groupbtn__btndir" @click="handleClickButton(3, 0)">
			<div class="fix-icon groupbtn__icon"></div>
			<p class="groupbtn__text">Sửa</p>
		</div>
		<div class=" groupbtn__btndir" @click="handleClickButton(4, 0)">
			<div class="del-icon groupbtn__icon"></div>
			<p class="groupbtn__text">Xóa</p>
		</div>
		<div class=" groupbtn__btndir" @click="handleClickButton(5, 0)">
			<div class="refresh-icon groupbtn__icon"></div>
			<p class="groupbtn__text">Nạp</p>
		</div>
		<!-- <div class=" groupbtn__btndir" @click="handleClickButton(6, 0)">
			<div class="export-icon groupbtn__icon"></div>
			<p class="groupbtn__text">Xuất khẩu</p>
		</div> -->
		
	</div>
</template>

<script>
export default {
	data() {
		return {
			isShowAddSelelct: false,
		}
	},
	methods: {
		/** 
		* Trả lại dữ liệu cho Paren
		* @param {buttonType} Loại nút đang được Click
		* - 1: Thêm mới
		* - 2: Nhân bản
		* - 3: Sửa
		* - 4: Xóa
		* - 5: Nạp
		* - 6: Xuất khẩu
		* @param {buttonElementType} Loại nút đang được Click
		* Thêm mới
		* - 0: Mặc định
		* - 1: Thêm mới hàng hóa
		* - 2: Thêm mới Combo
		* - 3: Thêm mới dịch vụ
		* @returns {}  
		* Created By: LMCUONG(08/07/2021) 
		*/
		handleClickButton(buttonType, buttonElementType) {
			
			this.isShowAddSelelct = false;
			this.$emit("click", buttonType, buttonElementType)
		}
	},
};
</script>

<style  scoped>
@import "../css/components/groupbuttondirection.css";
</style>
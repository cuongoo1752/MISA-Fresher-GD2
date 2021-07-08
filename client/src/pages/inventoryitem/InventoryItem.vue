<template>
	<div class="item">
		<groupbuttondirection @click="handleClickButtonGroup" />
		<div class="table-box">
			<table class="table-items" style="width: 100%">
				<thead class="items-thead">
					<tr class="items-tr-title">
						<th class="items-element-title">X</th>
						<th class="items-element-title">
							<colum-header-table
								:title="'Mã SKU'"
								:type="'string'"
								:width="'123'"
								:id="'sku'"
								@change="handleChangeColumnHeaderTable"
							/>
						</th>
						<th class="items-element-title">
							<colum-header-table
								:type="'string'"
								:width="'123'"
								:id="'name'"
								:title="'Tên hàng hóa'"
								@change="handleChangeColumnHeaderTable"
							/>
						</th>
						<th class="items-element-title">
							<colum-header-table
								:type="'string'"
								:width="'123'"
								:id="'firstColum'"
								:title="'Nhóm hàng hóa'"
								@change="handleChangeColumnHeaderTable"
							/>
						</th>
						<th class="items-element-title">
							<colum-header-table
								:type="'string'"
								:width="'123'"
								:id="'unit'"
								:title="'Đơn vị tính'"
								@change="handleChangeColumnHeaderTable"
							/>
						</th>
						<th class="items-element-title">
							<colum-header-table
								:type="'number'"
								:width="'123'"
								:id="''"
								:title="'Giá bán TB'"
								@change="handleChangeColumnHeaderTable"
							/>
						</th>
						<th class="items-element-title">
							<colum-header-table
								:type="'select'"
								:width="'123'"
								:id="''"
								:selectArray="dataColumnTable"
								:title="'Hiển thị trên MH hàng hóa'"
								@change="handleChangeColumnHeaderTable"
							/>
						</th>
						<th class="items-element-title">
							<colum-header-table
								:type="'select'"
								:width="'123'"
								:id="'firstColum'"
								:selectArray="dataColumnTable"
								:title="'jlfasjfks'"
								@change="handleChangeColumnHeaderTable"
							/>
						</th>
						<th class="items-element-title">
							<colum-header-table
								:type="'select'"
								:width="'123'"
								:id="''"
								:selectArray="dataColumnTable"
								:title="'Loại hàng hóa'"
								@change="handleChangeColumnHeaderTable"
							/>
						</th>
						<th class="items-element-title">
							<colum-header-table
								:type="'select'"
								:width="'123'"
								:id="''"
								:selectArray="dataColumnTable"
								:title="'Quản lý theo'"
								@change="handleChangeColumnHeaderTable"
							/>
						</th>
						<th class="items-element-title">
							<colum-header-table
								:type="'select'"
								:width="'123'"
								:id="''"
								:selectArray="dataColumnTable"
								:title="'Trạng thái'"
								@change="handleChangeColumnHeaderTable"
							/>
						</th>
					</tr>
				</thead>

				<tbody class="items-tbody">
					<tr
						v-for="index in 30"
						:key="index"
						class="items-tr-element"
					>
						<td class="items-element">
							<p class="item-text">X</p>
						</td>
						<td class="items-element">
							<p class="item-text">John</p>
						</td>
						<td class="items-element">
							<p class="item-text">John</p>
						</td>
						<td class="items-element">
							<p class="item-text">John</p>
						</td>
						<td class="items-element">
							<p class="item-text">John</p>
						</td>
						<td class="items-element">
							<p class="item-text">John</p>
						</td>
						<td class="items-element">
							<p class="item-text">John</p>
						</td>
						<td class="items-element">
							<p class="item-text">John</p>
						</td>
						<td class="items-element">
							<p class="item-text">John</p>
						</td>
						<td class="items-element">
							<p class="item-text">John</p>
						</td>
						<td class="items-element">
							<p class="item-text">John</p>
						</td>
					</tr>
				</tbody>
			</table>
		</div>

		<div class="pagination-items">
			<pagination />
		</div>
	</div>
</template>

<script>
import ColumHeaderTable from "../../components/ColumHeaderTable.vue";
import groupbuttondirection from "../../components/GroupButtonDirection.vue";
import Pagination from "../../components/Pagination.vue";
export default {
	components: {
		groupbuttondirection,
		ColumHeaderTable,
		Pagination,
	},
	data() {
		return {
			dataColumnTable: [
				{ key: "key1", value: "Tất cả" },
				{ key: "key2", value: "13212312142" },
				{ key: "key3", value: "112142" },
			],
		};
	},
	methods: {
		/**
		 * Sự kiện Click từ nhóm các nút điều hướng
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
		handleClickButtonGroup() {},
		handleChangeColumnHeaderTable(id, stateSort, stateInput, valueSelect) {
			console.log(
				">" +
					id +
					" " +
					stateSort +
					" " +
					stateInput +
					" " +
					valueSelect
			);
		},
	},
};
</script>

<style scoped>
@import "../../css/pages/inventoryitem/inventoryitem.css";
</style>
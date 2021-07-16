<template>
  <div class="item">
    <detail-item
      v-if="isShowDetailItem"
      @change="handleDetailItem"
      :detailItem="detailItem"
      :baseUrl="baseUrl"
    />
    <div v-else-if="!isShowDetailItem" class="item__detail">
      <groupbuttondirection @click="handleClickButtonGroup" />
      <div class="table-box">
        <table class="table-items" style="width: 100%">
          <thead class="items-thead" style="z-index: 100;">
            <tr class="items-tr-title">
              <th class="items-element-title" style="background: white">
                <input type="checkbox" class="checkbox" />
              </th>
              <th class="items-element-title">
                <colum-header-table
                  :title="'Mã SKU'"
                  :type="'string'"
                  :width="'99px'"
                  :id="'sku'"
                  @change="handleChangeColumnHeaderTable"
                />
              </th>
              <th class="items-element-title">
                <colum-header-table
                  :type="'string'"
                  :width="'710px'"
                  :id="'name'"
                  :title="'Tên hàng hóa'"
                  @change="handleChangeColumnHeaderTable"
                />
              </th>
              <th class="items-element-title">
                <colum-header-table
                  :type="'string'"
                  :width="'129px'"
                  :id="'firstColum'"
                  :title="'Nhóm hàng hóa'"
                  @change="handleChangeColumnHeaderTable"
                />
              </th>
              <th class="items-element-title">
                <colum-header-table
                  :type="'string'"
                  :width="'99px'"
                  :id="'unit'"
                  :title="'Đơn vị tính'"
                  @change="handleChangeColumnHeaderTable"
                />
              </th>
              <th class="items-element-title">
                <colum-header-table
                  :type="'number'"
                  :width="'104px'"
                  :id="''"
                  :title="'Giá bán TB'"
                  @change="handleChangeColumnHeaderTable"
                />
              </th>
              <th class="items-element-title">
                <colum-header-table
                  :type="'select'"
                  :width="'174px'"
                  :id="''"
                  :selectArray="dataColumnTable"
                  :title="'Hiển thị trên MH hàng hóa'"
                  @change="handleChangeColumnHeaderTable"
                />
              </th>

              <th class="items-element-title">
                <colum-header-table
                  :type="'select'"
                  :width="'119px'"
                  :id="''"
                  :selectArray="dataColumnTable"
                  :title="'Loại hàng hóa'"
                  @change="handleChangeColumnHeaderTable"
                />
              </th>
              <th class="items-element-title">
                <colum-header-table
                  :type="'select'"
                  :width="'119px'"
                  :id="''"
                  :selectArray="dataColumnTable"
                  :title="'Quản lý theo'"
                  @change="handleChangeColumnHeaderTable"
                />
              </th>
              <th class="items-element-title">
                <colum-header-table
                  :type="'select'"
                  :width="'134px'"
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
              v-for="(inventoryItem, index) in listInventoryItem"
              :key="index"
              class="items-tr-element"
              @dblclick.stop="
                UpdateInventoryItem(
                  inventoryItem.inventoryItemID,
                  inventoryItem.inventoryItemType
                )
              "
            >
              <td class="items-element">
                <input
                  type="checkbox"
                  class="checkbox"
                  :value="inventoryItem.inventoryItemID"
                  v-model="listIDCheckbox"
                />
              </td>
              <td class="items-element">
                <p class="item-text">{{ inventoryItem.skuCode }}</p>
              </td>
              <td class="items-element">
                <p class="item-text">{{ inventoryItem.inventoryItemName }}</p>
              </td>
              <td class="items-element">
                <p class="item-text">{{ inventoryItem.inventoryItemTypeName }}</p>
              </td>
              <td class="items-element">
                <p class="item-text">Chiếc</p>
              </td>
              <td class="items-element">
                <p class="item-text">{{ inventoryItem.buyPrice }}</p>
              </td>
              <td class="items-element">
                <p class="item-text">{{ inventoryItem.showInMenu }}</p>
              </td>
              <td class="items-element">
                <p class="item-text">{{ inventoryItem.inventoryItemTypeName }}</p>
              </td>
              <td class="items-element">
                <p class="item-text">{{ inventoryItem.ManageType }}</p>
              </td>
              <td class="items-element">
                <p class="item-text">Đang kinh doanh</p>
              </td>
            </tr>
          </tbody>
        </table>
      </div>

      <div class="pagination-items">
        <pagination
          :lengthPage="page.lengthPage"
          :currIndex="page.currIndex"
          :startRow="page.startRow"
          :endRow="page.endRow"
          :lengRow="page.lengRow"
          @change="handlePagination"
        />
      </div>
    </div>
  </div>
</template>

<script>
import ColumHeaderTable from "../../components/ColumHeaderTable.vue";
import groupbuttondirection from "../../components/GroupButtonDirection.vue";
import Pagination from "../../components/Pagination.vue";
import DetailItem from "./DetailItem.vue";
import Vue from "vue";
import axios from "axios";
import VueAxios from "vue-axios";
Vue.use(VueAxios, axios);
export default {
  components: {
    groupbuttondirection,
    ColumHeaderTable,
    Pagination,
    DetailItem,
  },
  created() {
    this.refreshPage();
  },
  data() {
    return {
      // Data tạm cho combobox
      dataColumnTable: [
        { key: "key1", value: "Tất cả" },
        { key: "key2", value: "13212312142" },
        { key: "key3", value: "112142" },
      ],
      // Biến trạng thái hiện thị cửa sổ Detail
      isShowDetailItem: false,
      // Mảng dữ liệu sẽ hiển thị
      listInventoryItem: [],
      // URL của Server
      baseUrl: "https://localhost:44371/api/v1/InventoryItems",
      // Đối tượng trạng thái phân trang, tìm kiếm sắp xếp
      filterPage: {
        page: 1,
        start: 0,
        limit: 50,
        sort: [
            {
              property: "CreatedDate",
              direction: "desc",
            },
            {
              property: "ModifiedDate",
              direction: "desc",
            },
        ],
        filter: [
          //   {
          //     isFilterRow: true,
          //     value: "a",
          //     stateFilter: 1,
          //     property: "ItemCategoryName",
          //     type: 1,
          //     tableReference: "ItemCategory",
          //   },
        ],
      },
      // Đối tượng trả về Pagination
      page: {
        lengthPage: 1,
        currIndex: 1,
        startRow: 1,
        endRow: 10,
        lengRow: 20,
      },
      // mảng chưa ID cần xóa
      listIDCheckbox: [],
      // Dữ liệu trả về cho DetailItem
      detailItem: {
        inventoryItem: {},
        inventoryItemsColor: [],
        colors: [],
      },
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
    handleClickButtonGroup(buttonType, buttonElementType) {
      //Thêm mới hàng hóa
      if (
        buttonType == 1 &&
        (buttonElementType == 0 || buttonElementType == 1)
      ) {
        this.openDetailItem();
      }

      //Xóa hàng hóa
      if (buttonType == 4) {
        this.DeleteInventoryItems();
      }

      // Nạp lại hàng hóa
      if (buttonType == 5) {
        this.refreshPage();
      }
    },
    /**
     * Trả lại gái trị của các sắp xếp trong cột
     * Created By: LMCUONG(15/07/2021)
     */
    handleChangeColumnHeaderTable(id, stateSort, stateInput, valueSelect) {
      console.log(
        ">" + id + " " + stateSort + " " + stateInput + " " + valueSelect
      );
    },
    /**
     * Trả lại giá trị của sổ DetailItem
     * Created By: LMCUONG(15/07/2021)
     */
    handleDetailItem(value) {
      if (value == 1) {
        this.closeDetailItem();
      }
    },
    /** 
    * Đóng cửa sổ DetailItem reset lại các biến
    * Created By: LMCUONG(16/07/2021) 
    */
    closeDetailItem(){
      this.isShowDetailItem = false;
      this.detailItem = {
        inventoryItem: {},
        inventoryItemsColor: [],
        colors: [],
      };
      this.refreshPage();
    },
    /**
     * Mở cửa sổ DetailItem
     * Created By: LMCUONG(15/07/2021)
     */
    openDetailItem() {
      this.isShowDetailItem = true;
    },
    /**
     * Tải lại dữ liệu trang
     * Created By: LMCUONG(15/07/2021)
     */
    refreshPage() {
      axios({
        method: "post",
        url: this.baseUrl,
        headers: {},
        data: this.filterPage,
      })
        .then((res) => {
          // Load dữ liệu lên trang
          this.listInventoryItem = res.data.data;
          // Đặt các trường trong pagination
          this.page.lengthPage = Math.ceil(
            res.data.total / this.filterPage.limit
          );
          this.page.currIndex = this.filterPage.page;
          this.page.startRow =
            (this.filterPage.page - 1) * this.filterPage.limit;
          this.page.endRow = res.data.data.length + this.page.startRow;
          this.page.lengRow = res.data.total;
        })
        .catch((error) => {
          console.log(error);
        });
    },
    handlePagination(pageIndex, pageSize) {
      this.filterPage.page = pageIndex;
      this.filterPage.limit = pageSize;
      this.refreshPage();
    },
    /**
     * Xóa đối tượng theo danh sách ID
     * @param {}
     * @returns {}
     * Created By: LMCUONG(15/07/2021)
     */
    DeleteInventoryItems() {
      // Gửi API xóa bản ghi
      axios({
        method: "post",
        url: this.baseUrl + "/Delete",
        headers: {},
        // Danh sách Id cần xóa
        data: this.listIDCheckbox,
      })
        .then((res) => {
          alert("Xóa thành công " + res.data.data.rowAffect + " bản ghi!");
        })
        .catch((error) => {
          alert(error.response.data.devMsg);
        });

      this.refreshPage();
    },

    UpdateInventoryItem(id, type) {
      axios({
        method: "post",
        url: this.baseUrl + "/" + id + "?type=" + type,
        headers: {},
      })
        .then((res) => {
          // Gán giá trị cho From Detail Item
          this.detailItem = res.data.data;
          // Mở cửa số Detail
          this.openDetailItem();
          //Format dữ liệu nhận vào
        
        })
        .catch((error) => {
          console.log(error);
        });
    },
  },
};
</script>

<style scoped>
@import "../../css/pages/inventoryitem/inventoryitem.css";
</style>

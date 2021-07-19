<template>
  <div class="item">
    <detail-item
      v-if="isShowDetailItem"
      @change="handleDetailItem"
      :detailItem="detailItem"
      :baseUrl="baseUrl"
      :dataUnits="dataUnits"
      :dataItemCategorys="dataItemCategorys"
      :state="stateDetailItem"
    />
    <div v-else-if="!isShowDetailItem" class="item__detail">
      <groupbuttondirection @click="handleClickButtonGroup" />
      <div class="table-box">
        <table class="table-items" style="width: 100%">
          <thead class="items-thead" style="z-index: 2;">
            <tr class="items-tr-title">
              <th class="items-element-title table-items__checkbox">
                <input
                  type="checkbox"
                  class="checkbox "
                  v-model="isAllCheckBox"
                  @change.stop="changeCheckBoxAll"
                />
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
                  :width="'707px'"
                  :id="'name'"
                  :title="'Tên hàng hóa'"
                  @change="handleChangeColumnHeaderTable"
                />
              </th>
              <th class="items-element-title">
                <colum-header-table
                  :type="'select'"
                  :width="'129px'"
                  :id="'firstColum'"
                  :title="'Nhóm hàng hóa'"
                  :selectArray="dataItemCategorys"
                  @change="handleChangeColumnHeaderTable"
                />
              </th>
              <th class="items-element-title">
                <colum-header-table
                  :type="'select'"
                  :width="'99px'"
                  :id="'unit'"
                  :title="'Đơn vị tính'"
                  :selectArray="dataUnits"
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
                  :selectArray="dataShowInMenu"
                  :title="'Hiển thị trên MH hàng hóa'"
                  @change="handleChangeColumnHeaderTable"
                />
              </th>

              <th class="items-element-title">
                <colum-header-table
                  :type="'select'"
                  :width="'119px'"
                  :id="''"
                  :selectArray="dataInventoryItemType"
                  :title="'Loại hàng hóa'"
                  @change="handleChangeColumnHeaderTable"
                />
              </th>
              <th class="items-element-title">
                <colum-header-table
                  :type="'select'"
                  :width="'119px'"
                  :id="''"
                  :selectArray="dataManageType"
                  :title="'Quản lý theo'"
                  @change="handleChangeColumnHeaderTable"
                />
              </th>
              <th class="items-element-title">
                <colum-header-table
                  :type="'select'"
                  :width="'134px'"
                  :id="''"
                  :selectArray="dataState"
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
              :class="{
                'item-tr-element-click': isStyleOnClickRow(
                  inventoryItem.inventoryItemID
                ),
              }"
              @dblclick.stop="
                UpdateInventoryItem(
                  inventoryItem.inventoryItemID,
                  inventoryItem.inventoryItemType
                )
              "
              @click.stop="
                onClickRow($event, inventoryItem.inventoryItemID, inventoryItem)
              "
            >
              <td class="items-element" style="text-align:center">
                <input
                  type="checkbox"
                  class="checkbox"
                  :value="inventoryItem.inventoryItemID"
                  v-model="listIDCheckbox"
                  @change.stop="
                    changeCheckBox(inventoryItem.inventoryItemID, inventoryItem)
                  "
                  @click.stop=""
                  @dblclick.stop=""
                />
              </td>
              <td class="items-element">
                <p class="item-text" style="width:78px">
                  {{ inventoryItem.skuCode }}
                </p>
              </td>
              <td class="items-element">
                <p class="item-text">{{ inventoryItem.inventoryItemName }}</p>
              </td>
              <td class="items-element">
                <p class="item-text" style="width:108px">
                  {{ displayItemCategoryName(inventoryItem.itemCategoryID) }}
                </p>
              </td>
              <td class="items-element">
                <p class="item-text">
                  {{ displayUnitName(inventoryItem.unitID) }}
                </p>
              </td>
              <td class="items-element">
                <p class="item-text">{{ inventoryItem.buyPrice }}</p>
              </td>
              <td class="items-element">
                <p class="item-text">{{ inventoryItem.showInMenuName }}</p>
              </td>
              <td class="items-element">
                <p class="item-text">
                  {{ inventoryItem.inventoryItemTypeName }}
                </p>
              </td>
              <td class="items-element">
                <p class="item-text">{{ inventoryItem.manageTypeName }}</p>
              </td>
              <td class="items-element">
                <p class="item-text">{{ inventoryItem.stateName }}</p>
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

    <base-alert
      @change="handleAlert"
      v-if="messageAlert.isShowAlert"
      :id="messageAlert.id"
      :type="messageAlert.type"
      :title="messageAlert.title"
      :numberOfButton="messageAlert.numberOfButton"
    >
      <span>{{ messageAlert.message.textNormal1 }}</span>
      <span style="font-family: var(--font-main-bold)">{{
        messageAlert.message.textBold
      }}</span>
      <span style="font-family: var(--font-main-bold);color: red">{{
        messageAlert.message.textRed
      }}</span>
      <span>{{ messageAlert.message.textNormal2 }}</span>
      <a
        style="color: blue"
        href="https://help.mshopkeeper.vn/vi/kb/lam_the_nao_de_xoa_duoc_hang_hoa_khi_da_co_phat_sinh"
        >{{ messageAlert.message.textLink }}</a
      >
    </base-alert>
    <base-loading v-if="load.isShowLoad" :message="load.message" />
  </div>
</template>

<script>
import ColumHeaderTable from "../../components/ColumHeaderTable.vue";
import groupbuttondirection from "../../components/GroupButtonDirection.vue";
import Pagination from "../../components/Pagination.vue";
import DetailItem from "./DetailItem.vue";
import BaseLoading from "../../components/BaseLoading.vue";
import Vue from "vue";
import axios from "axios";
import VueAxios from "vue-axios";
import BaseAlert from "../../components/BaseAlert.vue";
Vue.use(VueAxios, axios);
export default {
  components: {
    groupbuttondirection,
    ColumHeaderTable,
    Pagination,
    DetailItem,
    BaseAlert,
    BaseLoading,
  },
  created() {
    this.refreshPage();
    this.getDataUnitAndCategory();
  },

  data() {
    return {
      // Data tạm cho combobox
      dataItemCategorys: [],
      dataUnits: [],
      dataShowInMenu: [
        { key: "true", value: "Có" },
        { key: "false", value: "Không" },
      ],
      dataInventoryItemType: [
        { key: "0", value: "Tất cả" },
        { key: "1", value: "Hàng hóa" },
        { key: "2", value: "Combo" },
        { key: "3", value: "Dịch vụ" },
      ],
      dataManageType: [
        { key: "1", value: "Tất cả" },
        { key: "2", value: "Lô/Hạn sử dụng" },
        { key: "3", value: "Serial IMEI" },
        { key: "4", value: "Khác" },
      ],
      dataState: [
        { key: "1", value: "Tất cả" },
        { key: "1", value: "Đang kinh doanh" },
        { key: "1", value: "Ngừng kinh doanh" },
      ],

      // Biến trạng thái hiện thị cửa sổ Detail
      isShowDetailItem: false,
      // Mảng dữ liệu sẽ hiển thị
      listInventoryItem: [],
      // mảng style hiện thị click
      listStyleRowByID: [],
      // URL của Server
      baseUrl: "https://localhost:44371/api/v1/InventoryItems",
      unitUrl: "https://localhost:44371/api/v1/Units",
      itemCategoryUrl: "https://localhost:44371/api/v1/ItemCategorys",

      // Trạng thái của DetailItem
      // - add thêm hàng hóa mới
      // - update sửa hàng hóa
      stateDetailItem: "",
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
      isAllCheckBox: false,
      // Dữ liệu trả về cho DetailItem
      detailItem: {
        inventoryItem: {},
        inventoryItemsColor: [],
        colors: [],
      },
      // Các biến của Alert cảnh báo
      messageAlert: {
        isShowAlert: false,
        id: "",
        title: "",
        type: "",
        numberOfButton: 2,
        message: {
          textNormal1: "",
          textBold: "",
          textRed: "",
          textNormal2: "",
          textLink: "",
        },
      },

      // Đối tượng vừa được chọn
      itemSelect: {},

      // Loading
      load: {
        isShowLoad: false,
        message: "...",
      },
      // Kết thúc Data
    };
  },
  methods: {
    /**
     * Lấy dữ liệu Đơn vị và Nhóm hàng hóa
     * Created By: LMCUONG(17/07/2021)
     */
    getDataUnitAndCategory() {
      // Lấy dữ liệu đơn vị
      axios({
        method: "get",
        url: this.unitUrl,
      })
        .then((res) => {
          for (let index in res.data) {
            this.dataUnits.push({
              key: res.data[index].unitID,
              value: res.data[index].unitName,
            });
          }
        })
        .catch((error) => {
          console.log(error);
        });
      // Láy dữ liệu nhóm hàng hóa
      axios({
        method: "get",
        url: this.itemCategoryUrl,
      })
        .then((res) => {
          for (let index in res.data) {
            this.dataItemCategorys.push({
              key: res.data[index].itemCategoryId,
              value: res.data[index].itemCategoryName,
            });
          }
        })
        .catch((error) => {
          console.log(error);
        });
    },
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
      if (buttonType == 1) {
        if (buttonElementType == 0 || buttonElementType == 1) {
          this.openDetailItem(1, "add");
        } else {
          this.openDetailItem(buttonElementType, "add");
        }
      }

      //Xóa hàng hóa
      if (buttonType == 4) {
        this.handleDeleteInventoryItems();
      }

      // Nạp lại hàng hóa
      if (buttonType == 5) {
        this.refreshPage();
      }

      // Nhân bản - Test
      if (buttonType == 2) {
        console.log(this.listStyleRowByID);
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
    closeDetailItem() {
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
    openDetailItem(type, state) {
      this.detailItem.inventoryItem.inventoryItemType = type;
      this.stateDetailItem = state;
      this.isShowDetailItem = true;
    },
    /**
     * Tải lại dữ liệu trang
     * Created By: LMCUONG(15/07/2021)
     */
    refreshPage() {
      this.load.isShowLoad = true;
      this.load.message = "Đang lấy dữ liệu...";

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

          // Tắt loading
          this.load.isShowLoad = false;
          this.load.message = "...";
        })
        .catch((error) => {
          console.log(error);
          // Tắt loading
          this.load.isShowLoad = false;
          this.load.message = "...";
        });
    },
    /**
     * Giá trị trả về từ pagination
     * @param {Number} pageIndex Giá trị page hiện tại
     * @param {Number} pageSize Số phần tử có thể tối đa của page
     * Created By: LMCUONG(19/07/2021)
     */
    handlePagination(pageIndex, pageSize) {
      this.filterPage.page = pageIndex;
      this.filterPage.limit = pageSize;
      this.refreshPage();
    },

    handleDeleteInventoryItems() {
      // Nếu đamg chọn một phần tử
      if (this.listIDCheckbox.length == 1) {
        this.openAlert(
          "checkdelete",
          "Xóa dữ liệu",
          "confuse",
          2,
          "Bạn có chắc muốn xóa hàng hóa ",
          "",
          this.itemSelect.inventoryItemName +
            " - (" +
            this.itemSelect.skuCode +
            ")",
          " không?",
          ""
        );
      } else if (this.listStyleRowByID.length == 1) {
        this.openAlert(
          "checkdelete",
          "Xóa dữ liệu",
          "confuse",
          2,
          "Bạn có chắc muốn xóa hàng hóa ",
          "",
          this.itemSelect.inventoryItemName +
            " - (" +
            this.itemSelect.skuCode +
            ")",
          " không?",
          ""
        );
      }
      // Nếu đang chọn nhiều phần tử
      else if (
        this.listIDCheckbox.length > 0 ||
        this.listStyleRowByID.length > 0
      ) {
        this.openAlert(
          "checkdelete",
          "Xóa dữ liệu",
          "confuse",
          2,
          "Bạn có chắc chắn muốn xóa các hàng hóa đã chọn không?",
          "",
          "",
          ""
        );
      }
    },
    /**
     * Xóa đối tượng theo danh sách ID
     * Created By: LMCUONG(15/07/2021)
     */
    DeleteInventoryItems() {
      if (this.listStyleRowByID.length > 0 || this.listIDCheckbox > 0) {
        // Gửi API xóa bản ghi

        let listDelete = [];
        if (this.listIDCheckbox.length > 0) {
          listDelete = this.listIDCheckbox;
        } else {
          listDelete = this.listStyleRowByID;
        }

        // Mở loading
        this.load.isShowLoad = true;
        this.load.message = "Đang xóa...";

        axios({
          method: "post",
          url: this.baseUrl + "/Delete",
          headers: {},
          // Danh sách Id cần xóa
          data: listDelete,
        })
          .then(() => {
            // Tắt loading
            this.load.isShowLoad = false;
            this.load.message = "..";
            this.refreshPage();
          })
          .catch(() => {
            this.load.isShowLoad = false;
            this.load.message = "..";
            this.openAlert(
              "",
              "MISAEshop",
              "danger",
              1,
              "Không xóa được hàng hóa do hàng hóa đã có phát sinh. Xem các tình huống phát sinh và cách xử lý ",
              "",
              "",
              "",
              "tại đây"
            );
            this.refreshPage();
          });
      }
    },
    /**
     * Lấy dữ liệu chi tiết đối tượng và mở bảng Detail Item
     * @param {String} id Mã đối tượng cần lấy
     * @param {Number} type Loại đối tượng
     * Created By: LMCUONG(17/07/2021)
     */
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
          this.openDetailItem(
            res.data.data.inventoryItem.inventoryItemType,
            "update"
          );
          //Format dữ liệu nhận vào
        })
        .catch((error) => {
          console.log(error);
        });
    },
    /**
     * Hiển thị Nhóm hàng hóa theo tên từ Id
     * @param {string} itemCategoryID ID Nhóm hàng hóa
     * Created By: LMCUONG(17/07/2021)
     */
    displayItemCategoryName(itemCategoryID) {
      for (let index in this.dataItemCategorys) {
        if (itemCategoryID == this.dataItemCategorys[index].key) {
          return this.dataItemCategorys[index].value;
        }
      }
    },
    /**
     * Hiển thị tên đơn vị
     * @param {String} unitID mã đơn vị
     * Created By: LMCUONG(17/07/2021)
     */
    displayUnitName(unitID) {
      for (let index in this.dataUnits) {
        if (unitID == this.dataUnits[index].key) {
          return this.dataUnits[index].value;
        }
      }
    },
    /**
     * Khi click vào checkbox all sẽ click vào tất cả checkbox trong bảng
     * Created By: LMCUONG(17/07/2021)
     */
    changeCheckBoxAll() {
      // Xóa tất cả các checkbox đang check hiệu ứng check box
      this.listIDCheckbox = [];
      this.listStyleRowByID = [];
      // Nếu trạng thái checkbox all = true
      if (this.isAllCheckBox) {
        // Chuyển tất cả các checkbox trên màn hình = true và hiệu ứng hiển thị Click
        for (let index in this.listInventoryItem) {
          this.listIDCheckbox.push(
            this.listInventoryItem[index].inventoryItemID
          );
          this.listStyleRowByID.push(
            this.listInventoryItem[index].inventoryItemID
          );
        }
      }
    },
    /**
     * Khi Click vào checkbox sẽ thêm style cho ô đó
     * @param {String} inventoryItemID mã id của hàng vừa click
     * Created By: LMCUONG(17/07/2021)
     */
    changeCheckBox(inventoryItemID, inventoryItem) {
      // Nếu đẫ có trong mảng style hiện thị, xóa phần tử đó
      for (let index in this.listStyleRowByID) {
        if (this.listStyleRowByID[index] == inventoryItemID) {
          this.listStyleRowByID.splice(index, 1);
          return;
        }
      }

      // Nếu chưa có thêm phần tử đó vào mảng hiển thị
      this.listStyleRowByID.push(inventoryItemID);
      this.itemSelect = inventoryItem;
    },
    /**
     * Kiểm tra hàng có trong mảng Style hiện thị, true - nếu có, false - không có
     * @param {String} inventoryItemID id của hàng cần kiểm tra
     * Created By: LMCUONG(17/07/2021)
     */
    isStyleOnClickRow(inventoryItemID) {
      return this.listStyleRowByID.includes(inventoryItemID);
    },
    /**
     * Xử lý sự kiện Click vào một hàng trong bảng
     * @param {event} event sự kiện hiện tại
     * @param {String} inventoryItemID id hàng được click
     * Created By: LMCUONG(17/07/2021)
     */
    onClickRow(event, inventoryItemID, inventoryItem) {
      // Nếu có phím Ctrl đang bật, cho phép thêm nhiền bản ghi
      if (event.ctrlKey) {
        // Nếu có phần tử đang có hiệu ứng rồi
        for (let index in this.listStyleRowByID) {
          if (this.listStyleRowByID[index] == inventoryItemID) {
            //Thì không làm gì
            return;
          }
        }
        // Nếu phần tử chưa có hiệu ứng, thì thêm vào mảng hiện ứng
        this.listStyleRowByID.push(inventoryItemID);
      }
      // Nếu phím Ctrl đang tắt chỉ cho phép 1 giá trị
      else {
        // Nếu có phần tử đang có hiệu ứng rồi
        for (let index in this.listStyleRowByID) {
          if (this.listStyleRowByID[index] == inventoryItemID) {
            //Thì không làm gì
            return;
          }
        }
        // Nếu phần tử đang không có hiệu ứng
        // Xóa hết phần tử mảng hiệu ứng
        this.listStyleRowByID = [];
        // Thêm phần tử mới
        this.listStyleRowByID.push(inventoryItemID);
        this.itemSelect = inventoryItem;
      }
    },

    openAlert(
      id,
      title,
      type,
      numberOfButton,
      textNormal1,
      textBold,
      textRed,
      textNormal2,
      textLink
    ) {
      // Khởi tại giá trị
      this.messageAlert.id = id;
      this.messageAlert.title = title;
      this.messageAlert.type = type;
      this.messageAlert.numberOfButton = numberOfButton;
      this.messageAlert.message.textNormal1 = textNormal1;
      this.messageAlert.message.textBold = textBold;
      this.messageAlert.message.textRed = textRed;
      this.messageAlert.message.textNormal2 = textNormal2;
      this.messageAlert.message.textLink = textLink;

      // Mở cửa sổ Popup
      this.messageAlert.isShowAlert = true;
    },
    /**
     * Trả lại giá trị từ Alert
     * @param {string} id mã của đối tượng gọi Alert
     * @returns {Number} value nút thứ mấy đang được bấm
     * Created By: LMCUONG(19/07/2021)
     */
    handleAlert(id, value) {
      // Cho phép xóa phần tử
      if (id == "checkdelete" && value == 1) {
        this.DeleteInventoryItems();
      }

      this.messageAlert.isShowAlert = false;
    },

    // Kết hàm
  },
};
</script>

<style scoped>
@import "../../css/pages/inventoryitem/inventoryitem.css";
</style>

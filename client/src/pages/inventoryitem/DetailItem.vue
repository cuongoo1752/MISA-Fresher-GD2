<template>
  <div>
    <!-- Group button -->
    <div class="groupbtndetail">
      <div @click="saveDetailItem" class="groupbtndetail__btn btn-blue">
        <div class="groupbtndetail__icon detail-icon-save"></div>
        <p class="groupbtndetail__text detail-text-save">
          Lưu
        </p>
      </div>

      <div
        @click="returnValueParent(1)"
        class="groupbtndetail__btn btn-outline"
      >
        <div class="groupbtndetail__icon detail-icon-cancel"></div>
        <p class="groupbtndetail__text detail-text-cancel">
          Hủy bỏ
        </p>
      </div>
    </div>
    <div class="table-add-item-box">
      <table class="table-add-item" style="width: 100%">
        <tbody class="table-add-item__tbody">
          <!-- THÔNG TIN CƠ BẢN -->
          <tr class="table-add-item__body-tr">
            <td class="table-add-item__td" colspan="2">
              <h1 class="table-add-item__title">THÔNG TIN CƠ BẢN</h1>
            </td>
          </tr>
          <!-- Tên hàng hóa -->
          <tr class="table-add-item__body-tr">
            <td class="table-add-item__td" style="width: 168px">
              <text-label>Tên hàng hóa</text-label>
            </td>
            <td class="table-add-item__td">
              <BaseInput
                tabindex="0"
                :type="'text'"
                v-model="inventoryItem.inventoryItemName"
                @blur="outInputSKUCode"
              />
            </td>
          </tr>
          <!-- Nhóm hàng hóa -->
          <tr class="table-add-item__body-tr">
            <td class="table-add-item__td">
              <text-label>Nhóm hàng hóa</text-label>
            </td>

            <td class="table-add-item__td">
              <select
                name=""
                id=""
                class="headtb__select"
                style="width: 238px"
                v-model="inventoryItem.itemCategoryID"
              >
                <option
                  v-for="(opt, index) in dataItemCategorys"
                  :key="index"
                  :value="opt.key"
                  class="select-element"
                >
                  {{ opt.value }}
                </option>
              </select>
            </td>
          </tr>
          <!-- Mã SKUCode -->
          <tr class="table-add-item__body-tr">
            <td class="table-add-item__td">
              <text-label>Mã SKU</text-label>
            </td>
            <td class="table-add-item__td">
              <BaseInput
                tabindex="0"
                :type="'text'"
                :placeholder="'Hệ thống tự sinh khi bỏ trống'"
                v-model="inventoryItem.skuCode"
                :watchState="watchStateSKUCode"
                @blur="outInputSKUCode"
              />
            </td>
          </tr>
          <!-- Mã vạch -->
          <tr class="table-add-item__body-tr">
            <td class="table-add-item__td">
              <text-label>Mã vạch</text-label>
            </td>
            <td class="table-add-item__td">
              <BaseInput
                :type="'text'"
                :placeholder="'Hệ thống tự sinh khi bỏ trống'"
                v-model="inventoryItem.barCode"
              />
            </td>
          </tr>
          <!-- Thành phần Combo -->
          <tr v-if="type == 2" class="table-add-item__body-tr">
            <td class="table-add-item__td">
              <text-label>Thành phần Combo</text-label>
            </td>
            <td class="table-add-item__td">
              <PartCombo
                v-for="(part, index) in dataParts"
                :key="index"
                :part="index"
                :list="dataParts[index]"
                :dataUnits="dataUnits"
                :baseUrl="baseUrl"
                @change="handlePart"
              />

              <div
                class="partcombo__btn table-add-item-box__btn-add-part "
                style=""
                @click="addPart"
              >
                <div
                  class="partcombo__icon table-add-item-box__icon-add-part"
                ></div>
                <p class="partcombo__text ">
                  Thêm thành phần
                </p>
              </div>
            </td>
          </tr>
          <!-- Giá bán của Combo -->
          <tr v-if="type == 2" class="table-add-item__body-tr">
            <td class="table-add-item__td">
              <text-label>Giá bán của Combo</text-label>
            </td>
            <td class="table-add-item__td">
              <div style="display:flex">
                <BaseInputNumber
                  :type="'number'"
                  :placeholder="'Tổng giá mua: 0'"
                  v-model="inventoryItem.buyPrice"
                /><BaseInputNumber
                  :type="'number'"
                  :placeholder="'0'"
                  v-model="inventoryItem.costPrice"
                />
              </div>
            </td>
          </tr>
          <!-- Giá mua -->
          <tr v-if="type == 1" class="table-add-item__body-tr">
            <td class="table-add-item__td">
              <text-label
                :hasInformation="true"
                :title="
                  'Giá mua gần nhất, phần mềm tự động cập nhật mỗi lần nhập hàng.'
                "
                >Giá mua</text-label
              >
            </td>
            <td class="table-add-item__td">
              <BaseInputNumber
                :width="'153px'"
                :type="'number'"
                :textAlign="'right'"
                v-model="inventoryItem.buyPrice"
              />
            </td>
          </tr>
          <!-- Giá bán -->
          <tr v-if="type == 1 || type == 3" class="table-add-item__body-tr">
            <td class="table-add-item__td">
              <text-label>Giá bán</text-label>
            </td>
            <td class="table-add-item__td">
              <BaseInputNumber
                :width="'153px'"
                :type="'number'"
                :textAlign="'right'"
                v-model="inventoryItem.costPrice"
              />
            </td>
          </tr>
          <!-- Đơn vị tính -->
          <tr class="table-add-item__body-tr">
            <td class="table-add-item__td">
              <text-label>Đơn vị tính</text-label>
            </td>

            <td class="table-add-item__td">
              <select
                name=""
                id=""
                class="headtb__select"
                style="width: 238px"
                v-model="inventoryItem.unitID"
              >
                <option
                  v-for="(opt, index) in dataUnits"
                  :key="index"
                  :value="opt.key"
                  class="select-element"
                >
                  {{ opt.value }}
                </option>
              </select>
            </td>
          </tr>
          <!-- Tồn kho ban đầu -->
          <tr v-if="type == 1" class="table-add-item__body-tr">
            <td class="table-add-item__td">
              <text-label>Tồn kho ban đầu</text-label>
            </td>
            <td class="table-add-item__td">
              <BaseInputNumber
                :width="'111px'"
                :type="'number'"
                :textAlign="'right'"
                v-model="inventoryItem.initStock"
              />
            </td>
          </tr>
          <!-- Đinh mức tồn kho -->
          <tr v-if="type == 1" class="table-add-item__body-tr">
            <td class="table-add-item__td">
              <text-label>Định mức tồn kho</text-label>
            </td>
            <td class="table-add-item__td">
              <div style="display:flex">
                <BaseInputNumber
                  :width="'142px'"
                  :hasFront="true"
                  :textFront="'Tối thiểu'"
                  :type="'number'"
                  :textAlign="'right'"
                  v-model="inventoryItem.minimumStock"
                />
                <div style="width:16px"></div>
                <BaseInputNumber
                  :width="'142px'"
                  :hasFront="true"
                  :textFront="'Tối đa'"
                  :type="'number'"
                  :textAlign="'right'"
                  v-model="inventoryItem.maximumStock"
                />
              </div>
            </td>
          </tr>
          <!-- Vị trí lưu trữ trong kho -->
          <tr v-if="type == 1 || type == 2" class="table-add-item__body-tr">
            <td class="table-add-item__td">
              <text-label>Vị trí lưu trữ trong kho</text-label>
            </td>
            <td class="table-add-item__td">
              <BaseInput :type="'text'" v-model="inventoryItem.stockLocation" />
            </td>
          </tr>
          <!-- Vị trí trưng bày -->
          <tr v-if="type == 1 || type == 2" class="table-add-item__body-tr">
            <td class="table-add-item__td">
              <text-label>Vị trí trưng bày</text-label>
            </td>
            <td class="table-add-item__td">
              <BaseInput :type="'text'" v-model="inventoryItem.showLocation" />
            </td>
          </tr>
          <!-- HIện thị trên màn hình bán hàng -->
          <tr class="table-add-item__body-tr">
            <td class="table-add-item__td" colspan="2">
              <div style="display:flex;padding-left: 12px">
                <input
                  type="checkbox"
                  class="checkbox"
                  v-model="inventoryItem.showInMenu"
                />
                <text-label
                  :hasInformation="true"
                  :title="
                    'Hàng hóa được bán trực tiếp, bỏ tích nếu chỉ bán hàng hóa trong combo hoặc tặng quà'
                  "
                  >Hiển thị màn hình bán hàng</text-label
                >
              </div>
            </td>
          </tr>
          <!-- THÔNG TIN THUỘC TÍNH -->
          <tr v-if="type == 1 || type == 3" class="table-add-item__body-tr">
            <td class="table-add-item__td" colspan="2">
              <h1 class="table-add-item__title">THÔNG TIN THUỘC TÍNH</h1>
            </td>
          </tr>
          <!-- Thuộc tính -->
          <tr v-if="type == 1 || type == 3" class="table-add-item__body-tr">
            <td class="table-add-item__td">
              <text-label>Thuộc tính</text-label>
            </td>
            <td class="table-add-item__td">
              <BaseInputSizeAndColor
                :type="'color'"
                :id="'color'"
                :list="listColor"
                @change="handleInputSizeOrColor"
              />
            </td>
          </tr>
          <!-- Chi tiết thuộc tính -->
          <tr
            v-if="listTableItems.length > 0 && (type == 1 || type == 3)"
            class="table-add-item__body-tr"
          >
            <td class="table-add-item__td">
              <text-label>Chi tiết thuộc tính</text-label>
            </td>
            <td class="table-add-item__td">
              <div style="padding-top: 5px;padding-bottom: 5px">
                <TableItems @change="handleTableItem" :list="listTableItems" />
              </div>
            </td>
          </tr>
          <!-- THÔNG TIN BỔ XUNG -->
          <tr class="table-add-item__body-tr">
            <td class="table-add-item__td" colspan="2">
              <h1 class="table-add-item__title">THÔNG TIN BỔ XUNG</h1>
            </td>
          </tr>
          <!-- Trọng lượng gói hàng(g) -->
          <tr v-if="type == 1 || type == 2" class="table-add-item__body-tr">
            <td class="table-add-item__td">
              <text-label>Trọng lượng gói hàng(g)</text-label>
            </td>
            <td class="table-add-item__td">
              <BaseInputNumber
                :type="'number'"
                :textAlign="'right'"
                v-model="inventoryItem.weight"
              />
            </td>
          </tr>
          <!-- Kích thước gói hàng(cm) -->
          <tr v-if="type == 1 || type == 2" class="table-add-item__body-tr">
            <td class="table-add-item__td">
              <text-label>Kích thước gói hàng(cm)</text-label>
            </td>
            <td class="table-add-item__td">
              <div style="display:flex">
                <!-- Chiều dài -->
                <BaseInputNumber
                  :width="'76.3px'"
                  :type="'number'"
                  :textAlign="'right'"
                  :placeholder="'Chiều dài'"
                  v-model="inventoryItem.length"
                />
                <!-- Chiều rộng -->
                <BaseInputNumber
                  :width="'83.3px'"
                  :type="'number'"
                  :textAlign="'right'"
                  :placeholder="'Chiều rộng'"
                  v-model="inventoryItem.width"
                />
                <!-- Chiều cao -->
                <BaseInputNumber
                  :width="'78.3px'"
                  :type="'number'"
                  :textAlign="'right'"
                  :placeholder="'Chiều cao'"
                  v-model="inventoryItem.height"
                />
              </div>
            </td>
          </tr>
          <!-- Mô tả -->
          <tr class="table-add-item__body-tr">
            <td class="table-add-item__td">
              <text-label>Mô tả</text-label>
            </td>
            <td class="table-add-item__td">
              <BaseInput :type="'text'" v-model="inventoryItem.description" />
            </td>
          </tr>
          <!-- Ảnh hàng hóa -->
          <tr class="table-add-item__body-tr">
            <td class="table-add-item__td">
              <text-label>Ảnh hàng hóa</text-label>
            </td>
            <td class="table-add-item__td">
              <div class="inputimage">
                <div
                  @click="$refs.inputlogo.click()"
                  class="inputimage__logg-box"
                >
                  <div class="inputimage__icon"></div>
                  <p class="inputimage__text">Biểu tượng</p>
                </div>

                <div class="inputimage__image"></div>
                <div @click="$refs.inputimage.click()" class="inputimage__file">
                  ...
                </div>

                <input ref="inputlogo" style="display:none" type="file" />
                <input ref="inputimage" style="display:none" type="file" />
              </div>
            </td>
          </tr>
        </tbody>
      </table>
    </div>

    <!-- Group button -->
    <div class="groupbtndetail">
      <div @click="saveDetailItem" class="groupbtndetail__btn btn-blue">
        <div class="groupbtndetail__icon detail-icon-save"></div>
        <p class="groupbtndetail__text detail-text-save">
          Lưu
        </p>
      </div>

      <div
        @click="returnValueParent(1)"
        class="groupbtndetail__btn btn-outline"
      >
        <div class="groupbtndetail__icon detail-icon-cancel"></div>
        <p class="groupbtndetail__text detail-text-cancel">
          Hủy bỏ
        </p>
      </div>
    </div>

    <base-loading v-if="load.isShowLoad" :message="load.message" />
  </div>
</template>

<script>
import BaseInput from "../../components/BaseInput.vue";
import BaseInputNumber from "../../components/BaseInputNumber.vue";
import BaseInputSizeAndColor from "../../components/BaseInputSizeAndColor.vue";
import TableItems from "../../components/TableItems.vue";
import TextLabel from "../../components/TextLabel.vue";
import PartCombo from "../../components/PartCombo.vue";
import BaseLoading from "../../components/BaseLoading.vue";
import Vue from "vue";
import axios from "axios";
import VueAxios from "vue-axios";
Vue.use(VueAxios, axios);
export default {
  components: {
    BaseInput,
    BaseInputSizeAndColor,
    TableItems,
    TextLabel,
    BaseInputNumber,
    PartCombo,
    BaseLoading,
  },
  props: {
    detailItem: {
      type: Object,
      default: function() {
        return {};
      },
    },
    dataUnits: {
      type: Array,
      default: function() {
        return [];
      },
    },
    dataItemCategorys: {
      type: Array,
      default: function() {
        return [];
      },
    },
    baseUrl: {
      type: String,
      default: "",
    },
    state: {
      type: String,
      default: "add",
    },
  },
  created() {
    this.inventoryItem = this.detailItem.inventoryItem;
    this.listTableItems = this.detailItem.inventoryItemsColor;
    this.listColor = this.detailItem.colors;
    this.type = this.detailItem.inventoryItem.inventoryItemType;
    this.createDataParts();
  },
  data() {
    return {
      msg: "msg",
      isChecked: [],
      // Đối tượng Item hiện tại
      inventoryItem: {},
      // Danh sách phần tử tổ hợp màu sắc hiển thị trong bảng
      listTableItems: [],
      // Danh sách màu hiện tại
      listColor: [],
      // Trạng thái của Form DetailItem
      // 1 - Hàng hóa
      // 2 - Combo
      // 3 - Dịch vụ
      type: 1,
      dataParts: [[]],
      watchStateSKUCode: 0,

      // Loading
      load: {
        isShowLoad: false,
        message: "...",
      },
    };
  },
  methods: {
    createDataParts() {
      for (let index in this.listTableItems) {
        if (this.listTableItems[index].part > this.dataParts.length) {
          let lengPartsAdd =
            this.listTableItems[index].part - this.dataParts.length;
          for (let a = 0; a < lengPartsAdd; a++) {
            this.dataParts.push([]);
          }
        }

        this.dataParts[this.listTableItems[index].part - 1].push(
          this.listTableItems[index]
        );
      }
    },
    addPart() {
      this.dataParts.push([]);
    },
    handlePart(state, list, part) {
      if (state == "update") {
        this.dataParts[part] = list;
      } else if (state == "delete") {
        this.dataParts.splice(part, 1);
      }
    },
    /**
     * Giá trị màu trả về khi thêm, xóa
     * @param {string}  id Mã của input thực hiện đối tượng
     * @param {string}  state trang thái delete - xóa phần tử, add thêm mới phần tử
     * @param {Array}  list mảng màu hiện tại
     * @param {object}  element đối tượng bị tác động
     * Created By: LMCUONG(15/07/2021)
     */
    handleInputSizeOrColor(id, state, list, element) {
      // Thêm mới Color
      if (state == "add") {
        this.addItemColor(element);
      }

      // Xóa Color
      if (state == "delete") {
        this.deleteItemColor(element);
      }
      this.listColor = list;
    },

    returnValueParent(value) {
      this.$emit("change", value);
    },
    /**
     * Thêm phân tử vào bảng hiện thị tổ hợp màu
     * @param {object} color màu cần thêm
     * Created By: LMCUONG(15/07/2021)
     */
    addItemColor(color) {
      this.listTableItems.push({
        barCode: Date.now().toString(),
        skuCode: this.inventoryItem.skuCode + " - " + color.color.toUpperCase(),
        inventoryItemName:
          this.inventoryItem.inventoryItemName + " (" + color.color + ")",
        inventoryItemType: this.inventoryItem.inventoryItemType,
        itemCategoryID: this.inventoryItem.itemCategoryID,
        buyPrice: this.inventoryItem.buyPrice,
        costPrice: this.inventoryItem.costPrice,
        unitID: this.inventoryItem.unitID,
        initStock: this.inventoryItem.initStock,
        minimumStock: this.inventoryItem.minimumStock,
        maximumStock: this.inventoryItem.maximumStock,
        showInMenu: this.inventoryItem.showInMenu,
        showLocation: this.inventoryItem.showLocation,
        stockLocation: this.inventoryItem.stockLocation,
        color: color.color,
        colorCode: color.colorCode,
        description: this.inventoryItem.description,
        size: this.inventoryItem.size,
        sizeCode: this.inventoryItem.sizeCode,
        editMode: color.editMode,
        weight: this.inventoryItem.weight,
        length: this.inventoryItem.length,
        width: this.inventoryItem.width,
        height: this.inventoryItem.height,
        parentID: null,
        createdDate: null,
        createdBy: null,
        modifiedDate: null,
        modifiedBy: null,
      });
    },
    /**
     * Xóa đôi tượng màu sắc trong danh sách màu sắc
     * @param {object} element đối tượng vừa xóa
     * Created By: LMCUONG(15/07/2021)
     */
    deleteItemColor(element) {
      for (var index in this.listTableItems) {
        if (element.colorCode == this.listTableItems[index].colorCode) {
          this.listTableItems.splice(index, 1);
          break;
        }
      }
    },
    /**
     * Xử lý khi một hàng của Table bị xóa
     * @param {string} state :delete - một hàng vừa hóa, update - bảng vừa được sửa
     * Created By: LMCUONG(15/07/2021)
     */
    handleTableItem(state, list, element) {
      // Xóa một hàng
      if (state == "delete") {
        for (let index in this.listColor) {
          if (element.colorCode == this.listColor[index].colorCode) {
            this.listColor.splice(index, 1);
            break;
          }
        }
      }

      // Sửa bảng
      if (state == "update") {
        this.listTableItems = list;
      }
    },
    preSendDataAPI() {
      if (this.type == 2) {
        this.listTableItems = [];

        for (let index in this.dataParts) {
          for (let i in this.dataParts[index]) {
            this.listTableItems.push(this.dataParts[index][i]);
          }
        }
      }
    },
    /**
     * Thêm các Item mới
     * Created By: LMCUONG(16/07/2021)
     */
    addDetailItem() {
      this.preSendDataAPI();
      // Danh sách tối tượng cần thêm
      var detailItem = JSON.stringify({
        InventoryItem: this.inventoryItem,
        InventoryItemsColor: this.listTableItems,
      });
      let subUrl;
      if (this.type == 2) {
        subUrl = "InsertCombo";
      } else {
        subUrl = "InsertMerchandise";
      }
      // Mở loading
      this.load.isShowLoad = true;

      // Gọi API thêm đối tượng
      axios({
        method: "post",
        url: this.baseUrl + "/" + subUrl,
        headers: { "Content-Type": "application/json" },
        data: detailItem,
      })
        .then(() => {
          // Tắt loading
          this.load.isShowLoad = false;
          // Trả lại giá trị
          this.returnValueParent(1);
        })
        .catch((error) => {
          // Tắt loading
          this.load.isShowLoad = false;
          console.log(error);
        });
    },
    updateDetailItem() {
      this.preSendDataAPI();
      // Mở loading
      this.load.isShowLoad = true;
      // Danh sách tối tượng cần thêm
      var detailItem = JSON.stringify({
        InventoryItem: this.inventoryItem,
        InventoryItemsColor: this.listTableItems,
      });

      let subUrl;
      if (this.type == 2) {
        subUrl = "UpdateCombo";
      } else {
        subUrl = "UpdateMerchandise";
      }
      // Gọi API thêm đối tượng
      axios({
        method: "put",
        url: this.baseUrl + "/" + subUrl,
        headers: { "Content-Type": "application/json" },
        data: detailItem,
      })
        .then(() => {
          // Tắt loading
          this.load.isShowLoad = false;
          this.returnValueParent(1);
        })
        .catch((error) => {
          // Tắt loading
          this.load.isShowLoad = false;
          // Hiện thị lỗi
          console.log(error.response.data.userMsg);
        });
    },
    /**
     * Lưu đối tượng Detail Item tới Server
     * Created By: LMCUONG(18/07/2021)
     */
    saveDetailItem() {
      if (this.state == "add") {
        this.addDetailItem();
      } else if (this.state == "update") {
        this.updateDetailItem();
      }
    },
    outInputSKUCode() {
      if (
        this.inventoryItem.inventoryItemName != "" &&
        this.inventoryItem.inventoryItemName != null &&
        (this.inventoryItem.skuCode == "" ||
          this.inventoryItem.skuCode == null ||
          this.inventoryItem.skuCode == undefined)
      ) {
        axios({
          method: "get",
          url: this.baseUrl + "/" + "SKUCodeMax",
        })
          .then((res) => {
            this.inventoryItem.skuCode = res.data.data;
            this.watchStateSKUCode++;
          })
          .catch((error) => {
            // Hiện thị lỗi
            console.log(error.response.data.userMsg);
          });
      }
    },

    //Kết hàm
  },
};
</script>
<style scoped>
@import "../../css/pages/inventoryitem/groupbtndetail.css";
@import "../../css/components/basecheckbox.css";
@import "../../css/pages/inventoryitem/detailitem.css";
.test {
  padding: 20px;
  padding-top: 100px;
  background: white;
}
</style>

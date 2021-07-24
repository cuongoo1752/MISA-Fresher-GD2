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
            <td class="table-add-item__td" colspan="2" style="padding-top: 0">
              <h1 class="table-add-item__title">THÔNG TIN CƠ BẢN</h1>
            </td>
          </tr>

          <!-- Trạng thái kinh doanh -->
          <tr v-if="state == 'update'" class="table-add-item__body-tr">
            <td class="table-add-item__td" style="width: 168px">
              <text-label>Trạng thái kinh doanh</text-label>
            </td>
            <td class="table-add-item__td" style="height: 29px;">
              <div style="display: flex; align-item: center;">
                <input
                  type="radio"
                  class="radio"
                  v-model="inventoryItem.state"
                  value="2"
                  style="margin-left: 2px;margin-right: 5px;"
                />
                Đang kinh doanh
                <input
                  type="radio"
                  class="radio"
                  v-model="inventoryItem.state"
                  style="margin-left: 5px;margin-right: 5px;"
                  value="3"
                />
                Ngừng kinh doanh
              </div>
            </td>
          </tr>
          <!-- Tên hàng hóa -->
          <tr class="table-add-item__body-tr">
            <td class="table-add-item__td" style="width: 168px">
              <text-label>Tên hàng hóa</text-label>
            </td>
            <td class="table-add-item__td">
              <BaseInput
                id="inventoryItemName"
                tabindex="0"
                :type="'text'"
                v-model="inventoryItem.inventoryItemName"
                @blur="outInputSKUCode"
                :required="isRequiredItemName"
              />
            </td>
          </tr>
          <!-- Nhóm hàng hóa -->
          <tr class="table-add-item__body-tr">
            <td class="table-add-item__td">
              <text-label>Nhóm hàng hóa</text-label>
            </td>

            <td class="table-add-item__td">
              <BaseAutocomplete
                :defaultKey="inventoryItem.itemCategoryID"
                :id="'itemCategoryID'"
                @change="handleAutoComplete"
                :width="'238px'"
                :selections="dataItemCategorys"
              />
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
                :disabled="listColor.length > 0"
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
                <money
                  v-model="inventoryItem.buyPrice"
                  v-bind="money"
                  style="width: 153px; text-align: right;"
                  class="baseinput__input"
                />

                <money
                  v-model="inventoryItem.costPrice"
                  v-bind="money"
                  style="width: 153px; text-align: right;"
                  class="baseinput__input"
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
              <money
                :disabled="listColor.length > 0"
                v-model="inventoryItem.buyPrice"
                v-bind="money"
                style="width: 153px; text-align: right;"
                class="baseinput__input"
              />
            </td>
          </tr>
          <!-- Giá bán -->
          <tr v-if="type == 1 || type == 3" class="table-add-item__body-tr">
            <td class="table-add-item__td">
              <text-label>Giá bán</text-label>
            </td>
            <td class="table-add-item__td">
              <money
                :disabled="listColor.length > 0"
                v-model="inventoryItem.costPrice"
                v-bind="money"
                style="width: 153px; text-align: right;"
                class="baseinput__input"
              />
            </td>
          </tr>
          <!-- Đơn vị tính -->
          <tr class="table-add-item__body-tr">
            <td class="table-add-item__td">
              <text-label>Đơn vị tính</text-label>
            </td>

            <td class="table-add-item__td">
              <BaseAutocomplete
                :defaultKey="inventoryItem.unitID"
                :id="'unitID'"
                @change="handleAutoComplete"
                :width="'238px'"
                :selections="dataUnits"
              />
            </td>
          </tr>
          <!-- Tồn kho ban đầu -->
          <tr v-if="type == 1" class="table-add-item__body-tr">
            <td class="table-add-item__td">
              <text-label>Tồn kho ban đầu</text-label>
            </td>
            <td class="table-add-item__td">
              <money
                :disabled="listColor.length > 0"
                v-model="inventoryItem.initStock"
                v-bind="money"
                style="width:111px; text-align: right;"
                class="baseinput__input"
              />
              <span class="text-initstock"
                >Tồn kho ban đầu chỉ được nhập khi thêm mới hàng hóa.</span
              >
            </td>
          </tr>
          <!-- Đinh mức tồn kho -->
          <tr v-if="type == 1" class="table-add-item__body-tr">
            <td class="table-add-item__td">
              <text-label>Định mức tồn kho</text-label>
            </td>
            <td class="table-add-item__td">
              <div style="display:flex">
                <!-- Tồn kho tối thiểu -->
                <div class="baseinput__front">Tối thiểu</div>
                <money
                  v-model="inventoryItem.minimumStock"
                  v-bind="money"
                  style="width: 75px; text-align: right;"
                  class="baseinput__input has-front"
                />
                <!-- Tồn kho tối đa -->
                <div style="margin-left:14px" class="baseinput__front">
                  Tối đa
                </div>
                <money
                  v-model="inventoryItem.maximumStock"
                  v-bind="money"
                  style="width: 75px; text-align: right;"
                  class="baseinput__input has-front"
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
                  tabindex="0"
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
            <td
              class="table-add-item__td"
              colspan="2"
              style="padding-top: 16px"
            >
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
              <TableItems @change="handleTableItem" :list="listTableItems" />
            </td>
          </tr>
          <!-- THÔNG TIN BỔ XUNG -->
          <tr class="table-add-item__body-tr">
            <td
              class="table-add-item__td"
              colspan="2"
              style="padding-top: 16px"
            >
              <h1 class="table-add-item__title">THÔNG TIN BỔ XUNG</h1>
            </td>
          </tr>
          <!-- Trọng lượng gói hàng(g) -->
          <tr v-if="type == 1 || type == 2" class="table-add-item__body-tr">
            <td class="table-add-item__td">
              <text-label>Trọng lượng gói hàng(g)</text-label>
            </td>
            <td class="table-add-item__td">
              <money
                v-model="inventoryItem.weight"
                v-bind="money"
                style="width: 153px; text-align: right;"
                class="baseinput__input"
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
                <!-- Ciều dài -->
                <input
                  :type="typeInputLength"
                  @focus="typeInputLength = 'number'"
                  v-model="inventoryItem.length"
                  v-bind="money"
                  class="baseinput__input baseinput-length"
                  placeholder="Chiều dài"
                />
                <!-- Chiều rộng -->
                <input
                  :type="typeInputLength"
                  @focus="typeInputLength = 'number'"
                  v-model="inventoryItem.width"
                  v-bind="money"
                  class="baseinput__input baseinput-width"
                  placeholder="Chiều rộng"
                />

                <!-- Chiều cao -->
                <input
                  :type="typeInputLength"
                  @focus="typeInputLength = 'number'"
                  v-model="inventoryItem.height"
                  v-bind="money"
                  class="baseinput__input baseinput-height"
                  placeholder="Chiều cao"
                />
              </div>
            </td>
          </tr>
          <!-- Mô tả -->
          <tr class="table-add-item__body-tr">
            <td class="table-add-item__td">
              <text-label>Mô tả</text-label>
            </td>
            <td class="table-add-item__td td-description">
              <textarea
                rows="4"
                cols="67"
                style="height: auto;width: auto"
                type="text"
                v-model="inventoryItem.description"
                class="baseinput__input"
              />
            </td>
          </tr>
          <!-- Ảnh hàng hóa -->
          <tr class="table-add-item__body-tr">
            <td class="table-add-item__td">
              <text-label>Ảnh hàng hóa</text-label>
            </td>
            <td class="table-add-item__td">
              <div class="inputimage-box">
                <div class="inputimage">
                <div style="display:flex; justify-content: center;">
                  <img :src="srcImage" class="inputimage__image" />
                </div>
                <div
                  tabindex="0"
                  @click="$refs.inputimage.click()"
                  class="inputimage__file"
                >
                  ...
                </div>

                <input
                  @change="convertImageToBase64($event)"
                  ref="inputimage"
                  style="display:none"
                  type="file"
                />
              </div>

              <div class="text-note-image">
                <p class="text-note-image__text">
                  - Lựa chọn biểu tượng để thay thế nếu không có ảnh
                </p>
                 
                <p class="text-note-image__text">
                  <!-- eslint-disable-next-line -->
                  - Định dạng ảnh (.jpg, .jpeg, .png, .gif) và dung lượng < 5MB
                </p>
              </div>
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
  </div>
</template>

<script>
import BaseInput from "../../components/BaseInput.vue";
import BaseInputSizeAndColor from "../../components/BaseInputSizeAndColor.vue";
import TableItems from "../../components/TableItems.vue";
import TextLabel from "../../components/TextLabel.vue";
import PartCombo from "../../components/PartCombo.vue";
import BaseLoading from "../../components/BaseLoading.vue";
import BaseAutocomplete from "../../components/BaseAutocomplete.vue";
import BaseAlert from "../../components/BaseAlert.vue";
import Vue from "vue";
import axios from "axios";
import VueAxios from "vue-axios";
import { Money } from "v-money";
Vue.use(VueAxios, axios);
export default {
  components: {
    BaseInput,
    BaseInputSizeAndColor,
    TableItems,
    TextLabel,
    PartCombo,
    BaseLoading,
    BaseAutocomplete,
    BaseAlert,
    Money,
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
    if (this.type == 2) {
      this.createDataParts();
    }

    if (this.state == "add") {
      this.inventoryItem.state = 2;
    }

    if (
      this.inventoryItem.pictureLocation != null &&
      this.inventoryItem.pictureLocation.length > 0
    ) {
      this.srcImage =
        "https://localhost:44371" + this.inventoryItem.pictureLocation;
    }
  },
  mounted() {
    document.getElementById("inventoryItemName").focus();
    this.isRequiredItemName = true;
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
      prise: 0,
      // confix thư viện money
      money: {
        decimal: "",
        thousands: ".",
        prefix: "",
        suffix: "",
        precision: 0,
        masked: false,
      },

      // Biến required cho trường tên
      isRequiredItemName: false,

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

      // Nguồn ảnh hiển thị
      srcImage: "http://localhost:8080/img/item.e1e109ea.png",
      srcImageBase64: null,

      //
      typeInputLength: "text",
    };
  },
  methods: {
    /**
     * Tạo phần mới
     * Created By: LMCUONG(20/07/2021)
     */
    createDataParts() {
      console.log(this.listTableItems[index])
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
    /**
     * Thêm mảng mới
     * Created By: LMCUONG(20/07/2021)
     */
    addPart() {
      this.dataParts.push([]);
    },
    /**
     * Dữ liệu tử Part trả về
     * Created By: LMCUONG(20/07/2021)
     */
    handlePart(state, list, part) {
      if (state == "update") {
        this.dataParts[part] = list;
      } else if (state == "delete") {
        this.dataParts.splice(part, 1);

        for (let index = 0; index < this.dataParts.length; index++) {
          for (let a = 0; a < this.dataParts[index].length; a++) {
            this.dataParts[index][a].part = index + 1;
          }
        }
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
      let name = "";
      let sku = "";
      if (
        this.inventoryItem.inventoryItemName != undefined ||
        this.inventoryItem.inventoryItemName != null
      ) {
        name = this.inventoryItem.inventoryItemName;
      }

      if (
        this.inventoryItem.skuCode != undefined ||
        this.inventoryItem.skuCode != null
      ) {
        sku = this.inventoryItem.skuCode;
      }

      this.listTableItems.push({
        barCode: Date.now().toString(),
        skuCode: sku + " - " + this.removeAccents(color.color).toUpperCase(),
        inventoryItemName: name + " (" + color.color + ")",
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
     * Chuyển chữ có dấu thành chữ không dấu
     * (str) chuỗi có dấu
     * return: chuỗi không dấu
     */
    removeAccents(str) {
      var AccentsMap = [
        "aàảãáạăằẳẵắặâầẩẫấậ",
        "AÀẢÃÁẠĂẰẲẴẮẶÂẦẨẪẤẬ",
        "dđ",
        "DĐ",
        "eèẻẽéẹêềểễếệ",
        "EÈẺẼÉẸÊỀỂỄẾỆ",
        "iìỉĩíị",
        "IÌỈĨÍỊ",
        "oòỏõóọôồổỗốộơờởỡớợ",
        "OÒỎÕÓỌÔỒỔỖỐỘƠỜỞỠỚỢ",
        "uùủũúụưừửữứự",
        "UÙỦŨÚỤƯỪỬỮỨỰ",
        "yỳỷỹýỵ",
        "YỲỶỸÝỴ",
      ];
      for (var i = 0; i < AccentsMap.length; i++) {
        var re = new RegExp("[" + AccentsMap[i].substr(1) + "]", "g");
        var char = AccentsMap[i][0];
        str = str.replace(re, char);
      }
      return str;
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
          this.openAlert(
            "add",
            "MISA eShop",
            "warning",
            1,
            error.response.data.userMsg,
            "",
            "",
            "",
            ""
          );
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
          this.openAlert(
            "add",
            "MISA eShop",
            "warning",
            1,
            error.response.data.userMsg,
            "",
            "",
            "",
            ""
          );
          // Hiện thị lỗi
        });
    },
    /**
     * Vadidate dữ liệu trước khi gửi đi
     * Created By: LMCUONG(20/07/2021)
     */
    vadidateDetailItem() {
      let isValid = true;

      // Kiểm tra trường tên
      if (
        this.inventoryItem.inventoryItemName == null ||
        this.inventoryItem.inventoryItemName == "" ||
        this.inventoryItem.inventoryItemName == undefined
      ) {
        isValid = false;
        document.getElementById("inventoryItemName").focus();
      }

      return isValid;
    },
    /**
     * Lưu đối tượng Detail Item tới Server
     * Created By: LMCUONG(18/07/2021)
     */
    saveDetailItem() {
      if (this.vadidateDetailItem()) {
        if (this.state == "add") {
          this.addDetailItem();
        } else if (this.state == "update") {
          this.updateDetailItem();
        }
      }
    },
    /**
     * Khi thoát khỏi input tên
     * Created By: LMCUONG(20/07/2021)
     */
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
    /**
     * Mở cửa sổ cảnh báo
     * Created By: LMCUONG(22/07/2021)
     */
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
      this.messageAlert.isShowAlert = false;
    },
    handleAutoComplete(id, selectResult) {
      if (id == "itemCategoryID") {
        this.inventoryItem.itemCategoryID = selectResult;
      }

      if (id == "unitID") {
        this.inventoryItem.unitID = selectResult;
      }
    },

    convertImageToBase64(event) {
      var thisDetailItem = this;

      let file = event.target.files[0];
      this.getBase64(file)
        .then(function(data) {
          thisDetailItem.srcImage = data;

          if (file.type != "image/jpg" && file.type != "image/jpeg") {
            data = data.slice(22);
          }

          if (file.type == "image/jpg" || file.type == "image/jpeg") {
            data = data.slice(23);
          }

          thisDetailItem.inventoryItem.pictureType = file.type;
          thisDetailItem.inventoryItem.pictureBase64 = data;
          thisDetailItem.inventoryItem.pictureName = file.name;
          console.log(thisDetailItem.inventoryItem);
        })
        .catch(function(err) {
          console.log(err);
        });
    },

    getBase64(file) {
      return new Promise((resolve, reject) => {
        const reader = new FileReader();
        reader.readAsDataURL(file);
        reader.onload = () => resolve(reader.result);
        reader.onerror = (error) => reject(error);
      });
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

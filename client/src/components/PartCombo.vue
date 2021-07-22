<template>
  <div class="partcombo">
    <div class="partcombo__title">
      <div>
        <span class="partcombo__part">Thành phần {{ part + 1 }}: </span>
        <span class="partcombo__part-detail">Gồm các hàng hóa dưới dây</span>
      </div>

      <div
        v-if="part > 0"
        class="partcombo__btn-cancel"
        @click="returnParentValue('delete')"
      >
        <div class="partcombo__icon-cancel"></div>
        <p class="partcombo__text-cancel">
          Xóa thành phần
        </p>
      </div>
    </div>
    <div class="partcombo__find-item">

      <base-autocomplete
        @change="handleAutocomplete"
        :selections="dataItems"
        :width="'284px'"
        :numberOfTableColumn="2"
      />

      <div class="partcombo__btn" @click="addItemToTable">
        <div class="partcombo__icon"></div>
        <p class="partcombo__text ">
          Đồng ý
        </p>
      </div>
    </div>

    <div class="table-detail-box">
      <table class="table-detail" style="width: 100%">
        <thead class=" table-detail__thead">
          <tr class="table-detail__tr" style="z-index:2">
            <th class="table-detail__th">
              Mã SKU
            </th>
            <th class="table-detail__th">
              Tên hàng hóa
            </th>

            <th class="table-detail__th">
              Đơn vị tính
            </th>

            <th class="table-detail__th">
              Giá mua
            </th>

            <th class="table-detail__th">
              Giá bán
            </th>

            <th class="table-detail__th">
              Số lượng
            </th>

            <th class="table-detail__th"></th>
            <th class="table-detail__th">
              Sử dụng
            </th>
          </tr>
        </thead>

        <tbody class="table-detail__tbody">
          <tr
            tabindex="0"
            v-for="(item, index) in listItemsOnPart"
            :key="index"
            class="table-detail__body-tr"
            @click.stop=""
          >
            <td class="table-detail__td">
              <input
                type="text"
                class="table-detail__input"
                v-model="item.skuCode"
                disabled
              />
            </td>
            <td class="table-detail__td">
              <input
                type="text"
                class="table-detail__input"
                v-model="item.inventoryItemName"
                disabled
              />
            </td>

            <td class="table-detail__td">
              <input
                type="text"
                class="table-detail__input"
                :value="displayUnitName(item.unitID)"
                disabled
              />
            </td>
            <td class="table-detail__td">
              <input
                type="number"
                class="table-detail__input"
                v-model="item.buyPrice"
                disabled
              />
            </td>
            <td class="table-detail__td">
              <input
                type="number"
                class="table-detail__input"
                v-model="item.costPrice"
                disabled
              />
            </td>
            <td class="table-detail__td">
              <input
                type="number"
                class="table-detail__input"
                v-model="item.quantity"
              />
            </td>
            <td @click.stop="modifiedRow(index)" class="table-detail__td">
              <div style="display: flex; justify-content: center">
                <div class="table-detail__dup"></div>
              </div>
            </td>
            <td class="table-detail__td">
              <div style="display: flex; justify-content: center">
                <input type="checkbox" class="checkbox" v-model="item.isUse" />
              </div>
            </td>
          </tr>
        </tbody>
      </table>
      <BaseAlert :id="'warningItem'" v-if="isShowAlert" @change="handleAlert"
        >{{ message }}
      </BaseAlert>
    </div>
    <div class="partcombo__padding"></div>
  </div>
</template>

<script>
import BaseAlert from "../components/BaseAlert.vue";
import BaseAutocomplete from "../components/BaseAutocomplete.vue";
import Vue from "vue";
import axios from "axios";
import VueAxios from "vue-axios";
Vue.use(VueAxios, axios);
export default {
  components: {
    BaseAlert,
    BaseAutocomplete,
  },
  props: {
    part: {
      type: Number,
      default: 2,
    },
    list: {
      type: Array,
      default: function() {
        return [{}];
      },
    },
    dataUnits: {
      type: Array,
      default: function() {
        return [];
      },
    },
    baseUrl: {
      type: String,
      default: "",
    },
  },
  created() {
    this.listItemsOnPart = this.list;
    this.getItems();
  },
  watch: {
    list: {
      handler() {
        this.listItemsOnPart = this.list;
      },
      deep: true,
    },
    part(){
      console.log(1)
      console.log(this.part)
      for (const key in this.listItemsOnPart) {
        if (Object.hasOwnProperty.call(this.listItemsOnPart, key)) {
          const element = this.listItemsOnPart[key];
          element.part = this.part + 1;
        }
      }
    }
  },
  data() {
    return {
      // Các đối tượng trong bảng
      listItemsOnPart: [],
      // Các biết cho kiểu Alert
      isShowAlert: false,
      message: "",
      // Dữ liệu tìm kiếm hàng hóa
      dataItems: [],
      selectInventoryItemID: "",
    };
  },
  methods: {
    /**
     * Lấy dữ liệu hàng hóa
     * Created By: LMCUONG(18/07/2021)
     */
    async getItems() {
      axios({
        method: "post",
        url: this.baseUrl,
        headers: {},
        data: {
          page: 1,
          start: 0,
          limit: 1000,
          sort: [],
          filter: [
            {
              isFilterRow: true,
              value: "1",
              stateFilter: 1,
              property: "InventoryItemType",
              type: 2,
            },
          ],
        },
      })
        .then((res) => {
          for (const key in res.data.data) {
            if (Object.hasOwnProperty.call(res.data.data, key)) {
              const element = res.data.data[key];
              this.dataItems.push({
                key: element.inventoryItemID,
                value: element.inventoryItemName,
                code: element.skuCode,
              });
            }
          }
        })
        .catch((error) => {
          console.log(error);
        });
      axios({
        method: "post",
        url: this.baseUrl,
        headers: {},
        data: {
          page: 1,
          start: 0,
          limit: 1000,
          sort: [],
          filter: [
            {
              isFilterRow: true,
              value: "3",
              stateFilter: 1,
              property: "InventoryItemType",
              type: 2,
            },
          ],
        },
      })
        .then((res) => {
          for (const key in res.data.data) {
            if (Object.hasOwnProperty.call(res.data.data, key)) {
              const element = res.data.data[key];
              this.dataItems.push({
                key: element.inventoryItemID,
                value: element.inventoryItemName,
                code: element.skuCode,
              });
            }
          }
        })
        .catch((error) => {
          console.log(error);
        });
    },
    /**
     * Từ dữ liệu hàng hóa thêm các hàng vào bảng
     * @param {}
     * @returns {}
     * Created By: LMCUONG(18/07/2021)
     */
    addItemToTable() {
      if (
        this.selectInventoryItemID != null ||
        this.selectInventoryItemID != ""
      ) {
        axios({
          method: "post",
          url: this.baseUrl + "/" + this.selectInventoryItemID + "?type=" + "1",
          headers: {},
        })
          .then((res) => {
            if (res.data.data.inventoryItemsColor.length > 0) {
              this.listItemsOnPart = res.data.data.inventoryItemsColor;
              for (let index in this.listItemsOnPart) {
                this.listItemsOnPart[index].part = this.part + 1;
                this.listItemsOnPart[index].isUse = true;
              }
            } else {
              this.listItemsOnPart = [];
              this.listItemsOnPart.push(res.data.data.inventoryItem);
              this.listItemsOnPart[0].part = this.part + 1;
              this.listItemsOnPart[0].isUse = true;
            }

            this.returnParentValue("update");
          })
          .catch((error) => {
            console.log(error);
          });
      }
    },
    modifiedRow(index) {
      if (typeof index == typeof 0) {
        let tempElement = this.listItemsOnPart[index];
        index++;
        for (; index < this.listItemsOnPart.length; index++) {
          this.listItemsOnPart[index].quantity = tempElement.quantity;
        }
        this.returnParentValue("update");
      }
    },
    handleAlert(id, value) {
      if (id == "warningItem") {
        if (value == 2) {
          this.isShowAlert = false;
        }
      }
    },
    returnParentValue(state) {
      this.$emit("change", state, this.listItemsOnPart, this.part);
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
    handleAutocomplete(id, result){
      this.selectInventoryItemID = result;
    }

    // Kết hàm
  },
};
</script>

<style scoped>
@import "../css/components/tableitems.css";
@import "../css/components/partcombo.css";
</style>

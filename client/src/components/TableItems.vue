<template>
  <div class="table-detail-box" @click="deleteRow">
    <table class="table-detail" style="width: 100%">
      <thead class=" table-detail__thead">
        <tr class="table-detail__tr">
          <th class="table-detail__th">
            Tên hàng hóa
          </th>
          <th class="table-detail__th">
            Mã SKU
          </th>

          <th class="table-detail__th">
            Mã vạch
          </th>

          <th class="table-detail__th">
            Giá mua
          </th>

          <th class="table-detail__th">
            Giá bán
          </th>

          <th class="table-detail__th">
            Tồn kho ban đầu
          </th>

          <th class="table-detail__th"></th>
          <th class="table-detail__th"></th>
        </tr>
      </thead>

      <tbody class="table-detail__tbody">
        <tr
          tabindex="0"
          v-for="(item, index) in listTableItems"
          :key="index"
          class="table-detail__body-tr"
          @click.stop=""
        >
          <td class="table-detail__td">
            <input
              type="text"
              class="table-detail__input"
              v-model="item.inventoryItemName"
            />
          </td>
          <td class="table-detail__td">
            <input
              type="text"
              class="table-detail__input"
              v-model="item.skuCode"
            />
          </td>
          <td class="table-detail__td">
            <input
              type="text"
              class="table-detail__input"
              v-model="item.barCode"
            />
          </td>
          <td class="table-detail__td">
            <input
              type="text"
              class="table-detail__input"
              v-model="item.buyPrice"
            />
          </td>
          <td class="table-detail__td">
            <input
              type="text"
              class="table-detail__input"
              v-model="item.costPrice"
            />
          </td>
          <td class="table-detail__td">
            <input
              type="text"
              class="table-detail__input"
              v-model="item.initStock"
            />
          </td>
          <td @click.stop="modifiedRow(index)" class="table-detail__td">
            <div class="table-detail__dup"></div>
          </td>
          <td @click.stop="deleteRow(index)" class="table-detail__td">
            <div class="table-detail__delete"></div>
          </td>
        </tr>
      </tbody>
    </table>
    <BaseAlert :id="'warningItem'" v-if="isShowAlert" @change="handleAlert"
      >{{ message }}
    </BaseAlert>
  </div>
</template>

<script>
import BaseAlert from "../components/BaseAlert.vue";
export default {
  components: {
    BaseAlert,
  },
  props: {
    list: {
      type: Array,
      default: function() {
        return [{}];
      },
    },
  },
  created() {
    this.listTableItems = this.list;
  },
  watch: {
    list: {
      handler() {
        this.listTableItems = this.list;
      },
      deep: true,
    },
  },
  data() {
    return {
      listTableItems: [],
      isShowAlert: false,
      message: "",
      elementDelette:{}
    };
  },
  methods: {
    deleteRow(index) {
      if (typeof index == typeof 0) {
        if (this.listTableItems[index].editMode == 1) {
          this.elementDelette = this.listTableItems[index];
          this.listTableItems.splice(index, 1);
          this.returnParentValue('delete')
        }
        // Nếu không cho phép xóa
        else {
          this.message =
            "Hàng hóa " +
            this.listTableItems[index].inventoryItemName +
            " đã phát sinh. Bạn không thể xóa. Vui lòng kiểm tra lại.";
          this.isShowAlert = true;
        }
      }
      // Cho phép xóa
    },
    modifiedRow(index){
      
      if (typeof index == typeof 0) {
        let tempElement = this.listTableItems[index];
        index++;
        for(;index<this.listTableItems.length;index++){
          this.listTableItems[index].buyPrice = tempElement.buyPrice;
          this.listTableItems[index].costPrice = tempElement.costPrice;
          this.listTableItems[index].initStock = tempElement.initStock;
        }
        this.returnParentValue('update');
      }
    },
    handleAlert(id, value) {
      if (id == "warningItem") {
        if (value == 2) {
          this.isShowAlert = false;
        }
      }
    },
    returnParentValue(state){
      this.$emit('change', state,this.listTableItems, this.elementDelette)
    }
  },
};
</script>

<style scoped>
@import "../css/components/tableitems.css";
</style>

<template>
  <div class="headtb" :style="{ minWidth: width }" tabindex="0">
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
        @blur="setTimeOutShowSelete"
        @click="isShowSelect = !isShowSelect"
        type="button"
        :value="valueDisplay"
        class="headtb__btn"
      />
      <input
        v-model="valueFieldInput"
        @blur="returnValueParent(type)"
        @keyup.enter.stop="returnValueParent(type)"
        type="text"
        class="headtb__input-text"
      />
      <ul v-if="isShowSelect" class="list">
        <li class="element" @click="handleClick('*', 1)">*: Chứa</li>
        <li class="element" @click="handleClick('=', 2)">=: Bằng</li>
        <li class="element" @click="handleClick('+', 3)">
          +: Bắt đầu bằng
        </li>
        <li class="element" @click="handleClick('-', 4)">
          -: Kết thúc bằng
        </li>
        <li class="element" @click="handleClick('!', 5)">
          !: Không chứa
        </li>
      </ul>
    </div>
    <div v-else-if="type == 'number'" class="headtb__input number">
      <input
        @blur="setTimeOutShowSelete"
        @click="isShowSelect = !isShowSelect"
        type="button"
        :value="valueDisplay"
        class="headtb__btn"
      />
      <input
        v-model="valueFieldInput"
        @blur="returnValueParent(type)"
        type="text"
        class="headtb__input-text"
      />
      <ul v-if="isShowSelect" class="list number-list">
        <li class="element" @click="handleClick('=', 1)">= : Bằng</li>
        <!-- eslint-disable-next-line vue/no-parsing-error -->
        <li class="element" @click="handleClick('<', 2)">< : Nhỏ hơn</li>
        <!-- eslint-disable-next-line vue/no-parsing-error -->
        <li class="element" @click="handleClick('<=', 3)">
          <!-- eslint-disable-next-line vue/no-parsing-error -->
          <=: Nhỏ hơn hoặc bằng
        </li>
        <li class="element" @click="handleClick('>', 4)">> : Lớn hơn</li>
        <li class="element" @click="handleClick('>=', 5)">
          >=: Lớn hơn hoặc bằng
        </li>
      </ul>
    </div>
    <div v-else-if="type == 'select'" class="headtb__input select">
      <BaseAutocomplete
        :selections="selectArray"
        :width="width"
        :styleInput="{
          borderRadius: '1px',
          border: ' #e1e1e1 solid 1px !important',
          fontSize: '12px',
          height: '32px',
        }"
        :heightInput="'32px'"
        @change="handleAutocomplete"
      />
    </div>
  </div>
</template>

<script>
import BaseAutocomplete from "../components/BaseAutocomplete.vue";
export default {
  components: {
    BaseAutocomplete,
  },

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
      stateInput: 1,
      valueFieldInput: "",
      // Biến hiển thị giá trị select
      valueDisplay: "",
    };
  },
  methods: {
    /**
     * Xử lý Click vào phần title
     * Created By: LMCUONG(20/07/2021)
     */
    handleClickTitle() {
      if (this.type != "select") {
        if (this.stateSort == "") {
          this.stateSort = "desc";
        } else if (this.stateSort == "desc") {
          this.stateSort = "asc";
        } else if (this.stateSort == "asc") {
          this.stateSort = "";
        }
        this.returnValueParent("sort");
      }
    },
    /**
     * Xử lý sư kiện click vào select box
     * @param {string} display chuỗi sẽ hiển thị
     * @param {number} value giá trị trả về
     * Created By: LMCUONG(21/07/2021)
     */
    handleClick(display, value) {
      this.valueDisplay = display;
      this.stateInput = value;
      this.isShowSelect = false;
      this.returnValueParent(this.type);
    },
    /**
     * Trả giá trị về phần tử gọi nó
     * Created By: LMCUONG(20/07/2021)
     */
    returnValueParent(type) {
      if (this.type == "select") {
        this.$emit(
          "change",
          this.id,
          type,
          this.stateSort,
          1,
          this.valueSelect
        );
      } else {
        this.$emit(
          "change",
          this.id,
          type,
          this.stateSort,
          this.stateInput,
          this.valueFieldInput
        );
      }
    },
    /**
     * Đặt thời gian để tắt select hiển thị
     * Created By: LMCUONG(21/07/2021)
     */
    setTimeOutShowSelete() {
      let that = this;
      setTimeout(function() {
        that.isShowSelect = false;
      }, 100);
    },

    handleAutocomplete(id, result) {
      this.valueSelect = result;
      this.returnValueParent(this.type);
    },
  },
};
</script>

<style scoped>
@import "../css/components/columnheadertable.css";
</style>

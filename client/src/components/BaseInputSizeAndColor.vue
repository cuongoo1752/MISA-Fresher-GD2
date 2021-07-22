<template>
  <div class="sizecolor">
    <div v-if="type == 'color'" class="sizecolor__front">Màu sắc</div>
    <div v-else-if="type == 'size'" class="sizecolor__front">Size</div>
    <div
      class="sizecolor__input-box"
      tabindex="0"
      :class="{ sizecolor__boder: isBorder }"
      @click="focusInput"
    >
      <!-- Color -->
      <ul v-if="type == 'color'" class="sizecolor__list">
        <li
          v-for="(element, index) in listColorOrSize"
          :key="index"
          class="sizecolor__element"
        >
          {{ element.color }}
          <div
            @click="deleteElementByID(element.colorID)"
            class="sizecolor__element-cancel"
          ></div>
        </li>
      </ul>
      <!-- Size -->
      <ul v-else-if="type == 'size'" class="sizecolor__list">
        <li
          v-for="(element, index) in listColorOrSize"
          :key="index"
          class="sizecolor__element"
        >
          {{ element.size }}
          <div
            @click="deleteElementByID(element.sizeID)"
            class="sizecolor__element-cancel"
          ></div>
        </li>
      </ul>
      <!-- input -->
      <input
        tabindex="0"
        ref="sizecolorinput"
        @focus="isBorder = true"
        @blur="
          isBorder = false;
          addElementFromValue();
        "
        v-model="value"
        @keyup.enter="addElementFromValue()"
        
        type="text"
        class="sizecolor__input"
      />
      <!-- @keyup.delete="popElementFromtValue()" -->
    </div>
    <!-- Các component khác -->
    <base-alert
      :id="'checkSizeOrColor'"
      v-if="isShowAlert"
      @change="handleAlert"
      >{{ message }}</base-alert
    >
  </div>
</template>

<script>
import BaseAlert from "../components/BaseAlert.vue";
export default {
  components: {
    BaseAlert,
  },
  watch: {
    list: {
      handler() {
        this.listColorOrSize = this.list;
      },
      deep: true,
    },
  },
  created() {
    this.listColorOrSize = this.list;
  },
  props: {
    type: {
      type: String,
      default: "size",
    },
    id: {
      type: String,
      default: null,
    },
    list: {
      type: Array,
      default: function() {
        return [];
      },
    },
  },
  data() {
    return {
      // Trạng thái hiện border khi focus vào input
      isBorder: false,
      // Giá trị của thanh input
      value: "",
      // Mảng chưa giá trị color hoặc size tùy vào type
      listColorOrSize: [],
      // Trạng thái hiện/ẩn thông báo
      isShowAlert: false,
      // Giá trị đệm chuẩn bị xóa
      index: 0,
      // Thông điện gửi cho thông báo Alert
      message: "",
      // Đối tượng vừa thêm hoặc vừa hóa
      element: {},
    };
  },
  methods: {
    /**
     * Hàm khi gọi sẽ hover vào đối tượng
     * Created By: LMCUONG(13/07/2021)
     */
    focusInput() {
      this.$refs.sizecolorinput.focus();
    },
    /**
     * Hàm tạo ra một đối tượng mới từ giá trị input
     * Created By: LMCUONG(13/07/2021)
     */
    addElementFromValue() {
      if (this.value != "") {
        //validate dữ liệu trước khi tạo mới
        if (this.isVadidateInput()) {
          if (this.type == "color") {
            this.listColorOrSize.push({
              colorCode: Date.now().toString(),
              color: this.value,
              editMode: 1,
            });
          } else if (this.type == "size") {
            this.listColorOrSize.push({
              sizeCode: Date.now().toString(),
              size: this.value,
              editMode: 1,
            });
          }
          this.element = this.listColorOrSize[this.listColorOrSize.length - 1];
          this.returnValueParent("add");
        }
        // Reset lại dữ liệu thanh input
        this.value = "";
      }
    },
    /**
     * Vadidate dữ liệu cần tạo mới
     * @returns {true} nếu dữ liệu hợp lệ, false nếu dữ liệu không hợp lệ
     * Created By: LMCUONG(13/07/2021)
     */
    isVadidateInput() {
      let isVadidate = true;
      // Vadidate trống
      if (
        this.value == null ||
        this.value == undefined ||
        this.value.trim() == ""
      ) {
        isVadidate = false;
      }

      //Vadidate trùng
      if (this.listColorOrSize.length > 0 && isVadidate) {
        for (let element in this.listColorOrSize) {
          let tempElement = "";
          if (this.type == "color") {
            tempElement = this.listColorOrSize[element].color;
          } else if (this.type == "size") {
            tempElement = this.listColorOrSize[element].size;
          }

          if (tempElement == this.value) {
            isVadidate = false;
            break;
          }
        }
      }

      return isVadidate;
    },
    /**
     * Xóa phân tử theo ID nhận vào
     * @param {ID} ID đối tượng muốn xóa
     * Created By: LMCUONG(13/07/2021)
     */
    deleteElementByID(ID) {
      let temptID = "";
      // Tìm phần tử muốn xóa
      for (let index in this.listColorOrSize) {
        // Lưu ID phần từ muốn xóa theo từng kiểu vào biết temptID
        if (this.type == "color") {
          temptID = this.listColorOrSize[index].colorID;
        } else if (this.type == "size") {
          temptID = this.listColorOrSize[index].sizeID;
        }

        //Nếu tìm thấy phần tử cần xóa
        if (ID == temptID) {
          // Nếu cho phép sửa: editMode = 1
          if (this.listColorOrSize[index].editMode == 1) {
            this.element = this.listColorOrSize[index];
            this.listColorOrSize.splice(index, 1);
            // Trả lại giá trị phần tử cha
            this.returnValueParent("delete");
          }
          // Nếu không cho phếp sửa
          else {
            // Hiện cảnh bảo
            this.warningDeleteElement(index);
          }
          break;
        }
      }
    },
    /**
     * Bật cảnh báo xóa đối tượng?
     * @param {index} thử tự phần tử cần xóa
     * Created By: LMCUONG(13/07/2021)
     */
    warningDeleteElement(index) {
      // Lưu tạm thứ tự phần tử
      this.index = index;
      // Tạo ra câu message thông báo
      if (this.type == "color") {
        this.message =
          "Bạn có muốn xóa các hàng hóa có size " +
          this.listColorOrSize[index].color +
          " không?";
      } else if (this.type == "size") {
        this.message =
          "Bạn có muốn xóa các hàng hóa có size " +
          this.listColorOrSize[index].size +
          " không?";
      }
      // Mở thông báo
      this.openAlert();
    },
    /**
     * Xóa phần tử cuối cùng trong input
     * Created By: LMCUONG(13/07/2021)
     */
    popElementFromtValue() {
      if (this.listColorOrSize.length > 0 && this.value == "") {
        // Nếu cho phép sửa: editMode = 1
        if (
          this.listColorOrSize[this.listColorOrSize.length - 1].editMode == 1
        ) {
          this.element = this.listColorOrSize[this.listColorOrSize.length - 1];
          this.listColorOrSize.pop();
          // Trả lại giá trị phần tử cha
          this.returnValueParent("delete");
        }
        // Nếu không cho phép sửa
        else {
          this.warningDeleteElement(this.listColorOrSize.length - 1);
        }
      }
    },
    /**
     * Mở cửa sổ cảnh báo
     * Created By: LMCUONG(13/07/2021)
     */
    openAlert() {
      this.isShowAlert = true;
    },
    /**
     * Xử lý tin hiện nhập từ Alert thông báo
     * @param {id} mã đối tượng gửi thông báo
     * @returns {value} giá trị thông báo gửi
     * Created By: LMCUONG(13/07/2021)
     */
    handleAlert(id, value) {
      if (id == "checkSizeOrColor") {
        // Gửi về tắt cửa sổ
        if (value == 2) {
          this.isShowAlert = false;
          // Gửi vể chấp nhận
        } else if (value == 1) {
          this.isShowAlert = false;
          this.element = this.listColorOrSize[this.index];
          this.listColorOrSize.splice(this.index, 1);
          //Trả lại giá trị cho phần tử cha
          this.returnValueParent("delete");
        }
      }
    },
    /**
     * Trả lại giá trị cho phần tử cha
     * @param {state} Trạng thái chỉnh sửa add - thêm, delete - xóa
     * Created By: LMCUONG(13/07/2021)
     */
    returnValueParent(state) {
      this.$emit("change", this.id, state, this.listColorOrSize, this.element);
    },
  },
};
</script>

<style scoped>
@import "../css/components/baseinputsizeandcolor.css";
</style>

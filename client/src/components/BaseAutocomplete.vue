<template>
  <div >
    <!-- Phần chính -->
    <div
      class="combobox"
      :style="{ width: width }"
      v-on:keyup.up="nextElement()"
      v-on:keyup.down="prepElement()"
      v-on:keyup.enter="selectCurrent(selectIndex)"
      v-on:keydown.tab.stop="closeAutoComplete"
      @click.stop=""
      :title="stateVadidateValueInput == 1 ? 'Đơn vị không được để trống.' : ''"
    >
      <div
        :style="styleInput"
        class="combobox__input"
        :class="{
          valid: stateVadidateValueInput == 2,
          error: stateVadidateValueInput == 1,
        }"
      >
        <input
         tabindex="0"
          @blur.stop="blurInput"
          v-on:keydown.tab="closeAutoComplete"
          v-on:keyup="findSelectForInput($event)"
          @focus="focusInput"
          v-model="valueInput"
          type="text"
          class="combobox__text"
          :style="{height:heightInput }"
        />
        <div @blur="blurIconList" tabindex="10" @click="showSelections(stateComboboxIcon)" class="combobox__icon">
          <div class="icon-dropdown"></div>
        </div>
      </div>

      <!-- Phần hiển thị các option -->
      <div class="box-box-table-autocomplete">
        <div
          class="box-table-autocomplete"
          :style="{ width: width }"
          v-if="displayData.length > 0"
        >
          <table class="combobox__list-result" style="width: 100%">
            <tr class="combobox__title" v-if="numberOfTableColumn == 2">
              <th class="title-code">Mã SKU</th>
              <th class="title-name">Tên hàng hóa</th>
            </tr>
            <tr
              class="combobox__element-result"
              v-for="(item, index) in displayData"
              :key="item.key"
              @click.stop="selectCurrent(index)"
              :class="{ 'select': index == selectIndex && (typeof index) == (typeof selectIndex)}"
              @mouseover="selectIndex = index"
            >
              <td v-if="numberOfTableColumn == 2" class="element-code">
                {{ item.code }}
              </td>
              <td class="element-name">{{ item.value }}</td>
            </tr>
          </table>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  props: {
    id: {
      type: String,
      default: null,
    },
    selections: {
      type: Array,
      default: function() {
        return [];
      },
    },
    defaultKey: {
      type: String,
      default: null,
    },
    modeVadidate: {
      type: Boolean,
      default: false,
    },
    width: {
      type: String,
      default: "200px",
    },
    heightInput:{
      type: String,
      default: '34px'
    },
    numberOfTableColumn: {
      type: Number,
      default: 1,
    },
    styleInput:{
      type: Object,
      default: function(){
        return {};
      }
    }
  },
  created() {
    if (this.defaultKey != null) {
      var selectDefault = this.selections.filter((selection) => {
        return selection.key === this.defaultKey;
      });
      if (selectDefault.length > 0) {
        this.valueInput = selectDefault[0].value;
        this.selection = selectDefault[0];
      }
    }
  },
  watch: {
    modeVadidate() {
      if (this.modeVadidate == true) {
        this.vadidateValueInput(this.valueVadidate);
      }
    },
  },
  data() {
    return {
      // Giá trị trong thanh input
      valueInput: "",
      // Dữ liệu hiển thị dạng mảng: key - value
      displayData: [],
      // Trạng thái của combobox__icon: true - hiển thị tất cả, false - đóng tất cả
      stateComboboxIcon: true,
      // Giá trị đối tượng đang được chọn
      selection: {},
      // Đối tượng combobox__list-result đang được chọn
      selectIndex: 0,
      // Trạng thái vaditadate dữ liệu trong input: 0 mặc định, 1 - có lỗi, 2 - giá trị chính xác
      stateVadidateValueInput: 0,
      // Biến kiểm tra trạng thái hộ trợ cho hàm Blur input
      stateIsClickOpenList: false,
      // Biến kiểm tra trạng thái hộ trợ cho hàm Focus cho input
      stateIsFocusInput: false,
      // Giá trị cũ trước khi return
      oldValue: ''
    };
  },
  methods: {
    /**
     * Tìm kiếm dữ liệu dựa vào dữ liệu thanh input
     */
    findSelectForInput(event) {
      setTimeout(() => {
        if (
          !(
            event.code == "ArrowDown" ||
            event.code == "ArrowUp" ||
            event.code == "Enter" ||
            event.code == "Tab"
          )
        ) {
          // Lọc ra các dữ liệu thỏa mãn để hiển thị
          this.displayData = this.selections.filter((element) => {
            // Lọc bỏ dấu, khoảng trắng, cùng in hoa, để tiến hành tìm kiếm
            return this.removeAccents(
              element.value.toLowerCase().trim()
            ).includes(
              this.removeAccents(this.valueInput.toLowerCase().trim())
            );
          });
          // Gán lại index nếu mảng hiện thị có dữ liệu
          if (this.displayData.length >= 0) {
            this.selectIndex = 0;
          }
          // Vadidate dữ liệu trong input
          this.vadidateValueInput(this.valueInput);
        }

        if (event.code == "Tab") {
          this.closeAutoComplete();
        }
      }, 500);
    },
    /**
     * Kiểm tra dữ liệu trong input
     * (value) dữ liệu nhập vào
     */
    vadidateValueInput(valueVadidate) {
      var select;
      // if(this.modeVadidate && valueVadidate == null || valueVadidate.trim() == ""){
      // 	this.stateVadidateValueInput = 1;
      // }

      // Nếu dữ liệu nhập khác null hoặc rỗng
      if (valueVadidate != null || valueVadidate == "") {
        //Lọc lấy phần tử có value bằng dữ liệu nhập vào
        select = this.selections.filter((selection) => {
          return (
            this.removeAccents(selection.value.trim().toLowerCase()) ==
            this.removeAccents(valueVadidate.trim().toLowerCase())
          );
        });

        // Nếu dữ liệu hợp lệ
        if (select.length > 0) {
          // Hiện thị hiệu ứng  hợp lệ
          this.stateVadidateValueInput = 2;
          this.returnValueInput(select[0].key);
        }
        // Nếu dữ liệu không hợp lệ
        else {
          this.stateVadidateValueInput = 1;
        }
      } else {
        select = {};
        this.stateVadidateValueInput = 1;
      }
    },
    returnValueInput(selectResult) {
      if(selectResult != this.oldValue){
        this.oldValue = selectResult;
        this.$emit("change", this.id, selectResult);

      }
      
    },
    /**
     * Xử lý khi ấn phím xuống
     */
    nextElement() {
      if (this.selectIndex > 0) {
        this.selectIndex--;
      }
    },
    /**
     * Xử lý khi ấn phím lên
     */
    prepElement() {
      if (this.selectIndex < this.displayData.length - 1) {
        this.selectIndex++;
      }
    },
    /**
     * Hiện thị dữ liệu dựa vào trạng thái của biến stateComboboxIcon
     * (state) true - hiển thị tất cả, false: không hiển thị dữ liệu
     */
    showSelections(state) {
      this.stateIsClickOpenList = true;

      // Nếu dữ liệu chưa hiển thị hết thì sẽ hiển thị full
      if (this.displayData.length != this.selections.length && state == false) {
        state = true;
      }

      // Hiển thị tất cả
      if (state) {
        this.displayData = this.selections;
      }
      // Không hiển thị dữ liệu
      else {
        this.displayData = [];
      }

      // Thay đổi trạng thái
      this.stateComboboxIcon = !this.stateComboboxIcon;
    },
    /**
     * Xử lý khi chọn vào đối tượng hiên tại
     * (index) vị trị đối tượng hiện tại trong mảng
     */
    selectCurrent(index) {
      // Nếu mảng hiển thị đang có giá trị
      if (this.displayData[index] != null) {
        this.valueInput = this.displayData[index].value;
      }
      this.selectIndex = index;
      // Vadidate dữ liệu trong input
      this.vadidateValueInput(this.valueInput);
      // Đóng danh sách lựa chọn
      this.displayData = [];
      // Cập nhật biến index
      this.selectIndex = index;
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
     * Đóng cửa sổ có các lựa chọn
     * Created By: LMCUONG(22/07/2021)
     */
    closeAutoComplete() {
      this.displayData = [];
    },
    /**
     * Thoát khỏi thanh Input
     * Created By: LMCUONG(22/07/2021)
     */
    blurInput() {
      this.stateIsClickOpenList = false;

      // Đợi xem người dùng có click vào button mở các lựa chọn hay không?

      let thisAutocomplete = this;
      setTimeout(function() {
        // Sau 0.2s đợi
        // Nếu người dùng có click, gán lại giá trị không làm gì
        if (thisAutocomplete.stateIsClickOpenList) {
          thisAutocomplete.stateIsClickOpenList = false;
        }
        // Nếu click ra ngoài đóng cừa sổ, thoát ra ngoài
        else {
          thisAutocomplete.vadidateValueInput(thisAutocomplete.valueInput);
          if (thisAutocomplete.stateVadidateValueInput == 1) {
            thisAutocomplete.valueInput = "";
            thisAutocomplete.stateVadidateValueInput = 0;
            thisAutocomplete.returnValueInput(null);
          } else if (thisAutocomplete.stateVadidateValueInput == 2) {
            thisAutocomplete.stateVadidateValueInput = 0;
          }

          thisAutocomplete.closeAutoComplete();
        }
      }, 200);
    },
    blurIconList() {
      let thisAutocomplete = this;
      this.stateIsFocusInput = false;

      setTimeout(function() {
        if (thisAutocomplete.stateIsFocusInput) {
          thisAutocomplete.stateIsFocusInput = false;
        } else {
          thisAutocomplete.vadidateValueInput(thisAutocomplete.valueInput);
          if (thisAutocomplete.stateVadidateValueInput == 1) {
            thisAutocomplete.valueInput = "";
            thisAutocomplete.stateVadidateValueInput = 0;
            thisAutocomplete.returnValueInput(null);
          } else if (thisAutocomplete.stateVadidateValueInput == 2) {
            thisAutocomplete.stateVadidateValueInput = 0;
          }

          thisAutocomplete.closeAutoComplete();
        }
      }, 200);
    },
    focusInput() {
      this.stateIsFocusInput = true;
    },
  },
};
</script>

<style scoped>
@import "../css/components/baseautocomplete.css";
</style>
